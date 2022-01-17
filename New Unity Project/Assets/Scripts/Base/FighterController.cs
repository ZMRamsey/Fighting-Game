using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterAction { none, attacking, jumping, dead };
public enum FighterStance { standing, air, blow };
public enum FighterState { inControl, restricted };
public abstract class FighterController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth;
    float _health;

    [Header("Base Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _speed;
    [SerializeField] float _height;

    [Header("Air Settings")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _fallMultiplier;
    [SerializeField] float _jumpFalloff;

    [Header("Aesthetic")]
    [SerializeField] Transform _controllerScaler;
    [SerializeField] float _stretchSpeed;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _jumpUpSFX, _jumpDownSFX;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _commandMeter;
    float _yVelocity;
    bool _canJump;
    RaycastHit _groundHit;

    FighterAction _myAction;
    FighterStance _myStance;
    FighterState _myState;

    [Header("Components")]
    [SerializeField] Animator _animator;
    InputHandler _inputHandler;
    Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    public void InitializeFighter() {
        _health = _maxHealth;
        _canJump = true;
    }

    void Update() {
        _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, Vector3.one, Time.deltaTime * _stretchSpeed);
        ProcessInput();

        _animator.SetBool("grounded", _myStance == FighterStance.standing);
        _animator.SetBool("falling", _myStance == FighterStance.air && _rigidbody.velocity.y < 0);

        if (_myStance == FighterStance.standing) {
            OnGroundMovement();
        }

        if (_myStance == FighterStance.air || _myAction == FighterAction.jumping) {
            OnAirMovement();
        }

        if (_inputHandler.GetSmash()) {
            _animator.SetTrigger("smash");
        }
    }

    void FixedUpdate() {

        //_controllerVelocity = Vector3.ClampMagnitude(_controllerVelocity, 20);

        if (_inputHandler.GetJump(_canJump) && _canJump && _myAction != FighterAction.jumping) {
            OnJump();
            return;
        }

        _rigidbody.velocity = _controllerVelocity;
    }

    void ResetAction() {
        _myAction = FighterAction.none;
    }


    public virtual void OnGroundMovement() {
        var xCalculation = _inputHandler.GetInputX();
        
        AdjustControllerHeight();

        xCalculation *= _speed;

        _controllerVelocity = new Vector3(xCalculation, _yVelocity, 0);
    }

    void AdjustControllerHeight() {
        var rayDirection = transform.TransformDirection(Vector3.down);

        float targetDirVel = Vector3.Dot(rayDirection, _controllerVelocity);
        float groundDirVel = Vector3.Dot(rayDirection, Vector3.zero);

        float relativeVel = targetDirVel - groundDirVel;

        float x = _groundHit.distance - _height;
        float springForce = (x * 20) - (relativeVel * 0.01f);

        Vector3 result = rayDirection * springForce;
        _yVelocity = result.y;
    }

    public virtual void OnLand() {
        _controllerScaler.localScale = new Vector3(1.2f, 0.8f, 1);
        GameManager.Get().GetCameraShaker().SetShake(0.1f, 1.5f, true);
        _source.PlayOneShot(_jumpDownSFX);
        _animator.SetTrigger("land");
        _canJump = true;
    }

    public virtual void OnJump() {
        _canJump = false;

        _animator.SetTrigger("jump");
        _source.PlayOneShot(_jumpUpSFX);

        _myAction = FighterAction.jumping;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector3.up * GetComponent<Rigidbody>().mass * _jumpForce, ForceMode.Impulse);
    }

    public virtual void OnAirMovement() {
        var velocityX = _rigidbody.velocity.x;
        var velocityY = _rigidbody.velocity.y;

        if (_myAction != FighterAction.jumping) {

            if (_rigidbody.velocity.y < 0) {
                _rigidbody.AddForce(Physics.gravity * GetComponent<Rigidbody>().mass * (_fallMultiplier - 1));
            }
            else {
                _rigidbody.AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);
            }
        }
        else {
            if (_rigidbody.velocity.y > 0) {
                velocityY *= _jumpFalloff;
            }
            else {
                ResetAction();
            }
        }

        var xCalculation = _inputHandler.GetInputX();

        _rigidbody.AddForce(new Vector3(xCalculation, 0, 0) * _speed * 0.6f);

        velocityX = Mathf.Clamp(velocityX, -5f, 5f);

        _controllerVelocity = new Vector3(velocityX, velocityY, 0);
    }

    public virtual void OnDash() {

    }

    public virtual void ProcessInput() {
        if (IsGrounded()) {
            _myStance = FighterStance.standing;
        }
        else {
            _myStance = FighterStance.air;
        }

    }

    public virtual void ProcessHitRegister(HitRegister register) {
        ApplyDamage(register.GetDamage());
        ProcessKnockback(register.GetKnockbackDirection());
    }

    void ApplyDamage(float damage) {
        _health -= Mathf.Abs(damage);

        if (_health < 0) {
            _health = 0;
        }
    }

    public void ProcessKnockback(Vector3 knockbackDirection) {
        //ProcessKnockback
    }

    bool _wasGrounded;
    public bool IsGrounded() {
        if (_myAction == FighterAction.jumping) {
            return false;
        }

        var groundCheck = Physics.Raycast(transform.position, Vector3.down, out _groundHit, _height, _groundLayers);

        if (groundCheck && !_wasGrounded) {
            OnLand();
        }

        _wasGrounded = groundCheck;

        Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.red, _height * 2);
        return groundCheck;
    }

    public float GetHealth() {
        return _health;
    }

    public FighterStance GetFighterStance() {
        return _myStance;
    }

    public FighterAction GetFighterAction() {
        return _myAction;
    }
}
