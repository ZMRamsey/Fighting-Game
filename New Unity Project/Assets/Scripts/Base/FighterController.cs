using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FighterAction { none, attacking, jumping, dead };
public enum FighterStance { standing, air, blow };
public enum FighterState { inControl, restricted };
public abstract class FighterController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth;
    float _health;

    [Header("Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _height;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _fallMultiplier;
    [SerializeField] float _jumpFalloff;
    [SerializeField] bool _canAirDash;
    [SerializeField] bool _canGroundDash;
    [SerializeField] bool _onJump;
    [SerializeField] bool _canJump;

    [Header("Aesthetic")]
    [SerializeField] Transform _controllerScaler;
    [SerializeField] float _stretchSpeed;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _commandMeter;
    float _yVelocity;
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

        if (Keyboard.current.wKey.wasPressedThisFrame) {
            _onJump = true;
        }

        if (_myStance == FighterStance.standing) {
            OnGroundMovement();
        }

        if (_myStance == FighterStance.air || _myAction == FighterAction.jumping) {
            OnAirMovement();
        }
    }

    void FixedUpdate() {

        //_controllerVelocity = Vector3.ClampMagnitude(_controllerVelocity, 20);

        if (_onJump && _canJump && _myAction != FighterAction.jumping) {
            OnJump();
            return;
        }

        _rigidbody.velocity = _controllerVelocity;
    }

    void ResetAction() {
        _myAction = FighterAction.none;
    }


    public virtual void OnGroundMovement() {
        var xCalculation = 0.0f;
        if (Keyboard.current.aKey.IsPressed()) {
            xCalculation = 1;
        }
        if (Keyboard.current.dKey.IsPressed()) {
            xCalculation = -1;
        }

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
        _canJump = true;
    }

    public virtual void OnJump() {
        _onJump = false;
        _canJump = false;

        _myAction = FighterAction.jumping;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector3.up * GetComponent<Rigidbody>().mass * _jumpForce, ForceMode.Impulse);
    }

    public virtual void OnAirMovement() {
        var x = _rigidbody.velocity.x;
        var y = _rigidbody.velocity.y;

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
                y *= _jumpFalloff;
            }
            else {
                ResetAction();
            }
        }

        var xCalculation = 0.0f;
        if (Keyboard.current.aKey.IsPressed()) {
            xCalculation = 1;
        }
        if (Keyboard.current.dKey.IsPressed()) {
            xCalculation = -1;
        }

        _rigidbody.AddForce(new Vector3(xCalculation, 0, 0) * _speed * 0.6f);

        x = Mathf.Clamp(x, -5f, 5f);

        _controllerVelocity = new Vector3(x, y, 0);
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
