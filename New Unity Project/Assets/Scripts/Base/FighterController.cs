using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterAction { none, attacking, jumping, dead };
public enum FighterStance { standing, air, blow };
public enum FighterState { inControl, restricted };
public abstract class FighterController : MonoBehaviour
{
    [SerializeField] FighterFilter _filter;
    [Header("Moves")]
    [SerializeField] FighterMove _smashMove;
    [SerializeField] FighterMove _chipMove;
    [SerializeField] FighterMove _driveMove;
    [SerializeField] FighterMove _dropMove;
    //[SerializeField] FighterMove _specialMove;
    FighterMove _currentMove;
    [SerializeField] Hitbox _hitboxes;

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
    [SerializeField] ParticleSystem _impact;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _commandMeter;
    float _yVelocity;
    bool _canJump;
    bool _canAttack;
    bool _freeze;
    RaycastHit _groundHit;

    FighterAction _myAction;
    FighterStance _myStance;
    FighterState _myState;

    [Header("Components")]
    [SerializeField] Animator _animator;
    [SerializeField] FighterUI _fighterUI;
    InputHandler _inputHandler;
    Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    void Start() {
        InitializeFighter();
    }

    public FighterFilter GetFilter() {
        return _filter;
    }

    public virtual void InitializeFighter() {
        _commandMeter = 0.0f;
        _canJump = true;
        _canAttack = true;
    }

    void Update() {
        _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, Vector3.one, Time.deltaTime * _stretchSpeed);
        ProcessInput();

        _animator.SetBool("grounded", _myStance == FighterStance.standing);
        _animator.SetBool("falling", _myStance == FighterStance.air && _rigidbody.velocity.y < 0);

    }

    void FixedUpdate() {

        if (!_freeze) {
            if (_myStance == FighterStance.standing) {
                OnGroundMovement();
            }

            if (_myStance == FighterStance.air || _myAction == FighterAction.jumping) {
                OnAirMovement();
            }

            if (_inputHandler.GetJump(_canJump) && _canJump && _myAction != FighterAction.jumping) {
                OnJump();
                return;
            }

            _rigidbody.velocity = _controllerVelocity;
        }
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

    public void ResetHitbox() {
        _hitboxes.ResetCD();
        _hitboxes.ResetSCD();
    }

    public void ResetAttack() {
        if (_canAttack) {
            return;
        }

        _canAttack = true;

        ResetAction();
    }

    void AddMeter(float value) {
        _commandMeter += value;

        if (_commandMeter > 100) {
            _commandMeter = 100;
        }

        _fighterUI.SetBarValue(GetMeter());
    }

    void ResetMeter() {
        _commandMeter = 0;
        _fighterUI.SetBarValue(GetMeter());
    }

    public float GetMeter() {
        return _commandMeter / 100;
    }

    public virtual void OnSuperMechanic() {
        _impact.transform.position = transform.position;
        _impact.Play();
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
        _canAttack = true;

        ResetAttack();
    }

    public virtual void OnJump() {
        _canJump = false;

        if (_canAttack) {
            _animator.SetTrigger("jump");
        }

        _source.PlayOneShot(_jumpUpSFX);

        _myAction = FighterAction.jumping;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector3.up * GetComponent<Rigidbody>().mass * _jumpForce, ForceMode.Impulse);
    }

    public virtual void OnAttack() {

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
            if (_rigidbody.velocity.y > 0 && !_inputHandler.GetJumpHeld()) {
                velocityY *= 0.6f;
            }
            else if (_rigidbody.velocity.y > 0) {
                velocityY *= _jumpFalloff;
            }
            else {
                ResetAction();
            }
        }

        var xCalculation = _inputHandler.GetInputX();

        _rigidbody.AddForce(new Vector3(xCalculation, 0, 0) * _speed * 5);

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

        if (_inputHandler.GetSmash() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _smashMove;
            UpdateMove();
        }

        if (_inputHandler.GetDrive() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _driveMove;
            UpdateMove();
        }

        if (_inputHandler.GetDrop() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _dropMove;
            UpdateMove();
        }

        if (_inputHandler.GetChip() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _chipMove;
            UpdateMove();
        }


        if (_inputHandler.GetSpecial() && _canAttack && _commandMeter >= 100) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _smashMove;
            UpdateMove();
            ResetMeter();
            OnSuperMechanic();
            GameManager.Get().OnSpecial();
        }
    }

    void UpdateMove() {
        _hitboxes.SetType(_currentMove.GetType());
        _animator.SetTrigger(_currentMove.GetPath());
    }

    public virtual void ProcessHitRegister(HitRegister register) {
        KnockOut();
    }

    public virtual void OnSuccessfulHit(Vector3 point) {
        AddMeter(12);
        _animator.Play(_smashMove.GetClipName(), 0, (1f / _smashMove.GetFrames()) * _smashMove.GetHitFrame());
        _impact.transform.position = point;
        _impact.Play();
    }


    void KnockOut() {
    }


    public bool IsGrounded() {
        if (_myAction == FighterAction.jumping) {
            return false;
        }

        var groundCheck = Physics.Raycast(transform.position, Vector3.down, out _groundHit, _height, _groundLayers);

        if (groundCheck && _myStance == FighterStance.air) {
            OnLand();
        }

        return groundCheck;
    }

    public FighterStance GetFighterStance() {
        return _myStance;
    }

    public FighterAction GetFighterAction() {
        return _myAction;
    }

    public void StunController(float time) {
        if (Stun != null) {
            StopCoroutine(Stun);
        }
        Stun = StartCoroutine(StunFrame(time));
    }


    Coroutine Stun;
    Vector3 _tempVelocity;
    IEnumerator StunFrame(float time) {
        _tempVelocity = _controllerVelocity;
        _freeze = true;
        _animator.speed = 0;
        _rigidbody.isKinematic = true;
        yield return new WaitForSeconds(time);
        _freeze = false;
        _rigidbody.isKinematic = false;
        _animator.speed = 1;
        _rigidbody.velocity = _controllerVelocity;
        if (IsGrounded()) {
            _canJump = true;
            _canAttack = true;
        }
        StopCoroutine(Stun);
    }
}
