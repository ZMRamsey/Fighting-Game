using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FighterAction { none, attacking, dashing, jumping };
public enum FighterStance { standing, air, blow };
public enum FighterState { inControl, restricted, dead };
public abstract class FighterController : MonoBehaviour
{
    [SerializeField] protected FighterFilter _filter;
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
    float _failSafeAttack;

    [Header("Aesthetic")]
    [SerializeField] Transform _controllerScaler;
    [SerializeField] FighterEffects _effects;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _stretchSpeed;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _jumpUpSFX, _jumpDownSFX;
    [SerializeField] AudioClip[] _hitSounds;
    [SerializeField] AudioClip[] _damageSounds;
    [SerializeField] AudioClip[] _swingSounds;
    [SerializeField] AudioClip[] _leftFootSounds;
    [SerializeField] AudioClip[] _rightFootSounds;

    [SerializeField] ParticleSystem _impact;
    [SerializeField] ParticleSystem _impactFrame;
    [SerializeField] ParticleSystem _jumpDust;
    [SerializeField] ParticleSystem _jumpLand;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _commandMeter;
    float _yVelocity;
    bool _canJump;
    bool _canAttack;
    bool _freeze;
    bool _isDashing;
    bool _hasBounced;
    float _lastTapAxis;
    float _meterPenaltyTimer;
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
        _fighterUI = GameManager.Get().GetFighterTab(_filter).GetUI();
    }

    public void PlayLeftFoot() {
        _source.PlayOneShot(_leftFootSounds[UnityEngine.Random.Range(0, _leftFootSounds.Length)], 1f);
    }

    public void PlaySound(AudioClip clip) {
        _source.PlayOneShot(clip);
    }

    public void PlayRightFoot() {
        _source.PlayOneShot(_rightFootSounds[UnityEngine.Random.Range(0, _rightFootSounds.Length)], 1f);
    }

    public void SetFilter(FighterFilter filter) {
        _filter = filter;
    }

    public FighterFilter GetFilter() {
        return _filter;
    }

    public virtual void InitializeFighter() {
        _renderer.flipX = _filter == FighterFilter.one;

        if(_filter == FighterFilter.one) {
            _source.panStereo = -0.5f;
        }
        else {
            _source.panStereo = 0.5f;
        }

        _animator.SetLayerWeight(1, 0);

        _myState = FighterState.inControl;

        _commandMeter = 0.0f;
        _canJump = true;
        _canAttack = true;
        _hasBounced = false;
    }

    void Update() {

        if (IsGrounded()) {
            if (_filter == FighterFilter.one && transform.position.x < 0 || _filter == FighterFilter.two && transform.position.x > 0) {
                _meterPenaltyTimer += Time.deltaTime;
                if (_meterPenaltyTimer > 0.1f && GameManager.Get().KOCoroutine == null) {
                    ReduceMeter(1);
                    _meterPenaltyTimer = 0;
                }
            }
        }

        _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, Vector3.one, Time.deltaTime * _stretchSpeed);
        ProcessInput();

        if (_inputHandler.GetDash() && !_isDashing && _myState == FighterState.inControl) {
            _lastTapAxis = _inputHandler.GetInputX();
            OnDash();
        }

        _animator.SetBool("grounded", _myStance == FighterStance.standing);
        _animator.SetBool("running", IsGrounded() && _inputHandler.GetInputX() != 0 && Mathf.Abs(_rigidbody.velocity.magnitude) > 1);
        _animator.SetBool("falling", _myStance == FighterStance.air && _rigidbody.velocity.y < 0);


        Vector3 controllerVel = _controllerVelocity.normalized;
        var xAnim = controllerVel.x;

        if (_renderer.flipX) {
            xAnim = -xAnim;
        }

        _animator.SetFloat("xInput", xAnim);
        _animator.SetFloat("yInput", _rigidbody.velocity.y);

    }

    void FixedUpdate() {

        if (!_freeze) {
            if (_myStance == FighterStance.standing) {
                OnGroundMovement();
            }

            if ((_myStance == FighterStance.air || _myAction == FighterAction.jumping)) {
                OnAirMovement();
            }

            if (_inputHandler.GetJump(_canJump) && _canJump && _myAction != FighterAction.jumping && _canAttack && _myState == FighterState.inControl) {
                OnJump();
                return;
            }

            if (_isDashing) {
                _rigidbody.AddForce(new Vector3(_lastTapAxis, 0, 0) * 25, ForceMode.Impulse);
            }

            _rigidbody.velocity = _controllerVelocity;
        }
    }

    public void KO(Vector3 velocity) {
        _myState = FighterState.dead;
        _animator.Rebind();
        _animator.SetLayerWeight(1, 1);
       
        _animator.SetTrigger("KO");

        if (IsGrounded()) {
            _animator.SetTrigger("land");
        }

        if(_damageSounds.Length > 0) {
            _source.PlayOneShot(_damageSounds[UnityEngine.Random.Range(0, _damageSounds.Length)], 1.5f);
        }


        _controllerVelocity = velocity;
        //if (_filter == FighterFilter.one) {
        //    _controllerVelocity = new Vector3(30, 3, 0);
        //}
        //else {
        //    _controllerVelocity = new Vector3(-30, 3, 0);
        //}
        print("SET");
    }

    float xCalculation = 0.0f;
    public virtual void OnGroundMovement() {

        if (_canAttack && _myState == FighterState.inControl) {
            xCalculation = _inputHandler.GetInputX();
            xCalculation *= _speed;
        }
        else {
            xCalculation = xCalculation * 0.8f;
        }

        AdjustControllerHeight();

        xCalculation = Mathf.Clamp(xCalculation, -_speed, _speed);

        _controllerVelocity = new Vector3(xCalculation, _yVelocity, 0);
    }

    public void ResetHitbox() {
        _hitboxes.ResetCD();
    }

    public void ResetAttack() {
        _canAttack = true;
    }


    void AddMeter(float value)
    {
        //Debug.Log("Meter Gain: " + value);
        _commandMeter += value;

        if (_commandMeter > 100) {
            _commandMeter = 100;
        }

        _fighterUI.SetBarValue(GetMeter());
    }

    public void ReduceMeter(float value) {
        _commandMeter -= value;

        if (_commandMeter < 0) {
            _commandMeter = 0;
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

    private void OnCollisionEnter(Collision collision) {
        if(_myState == FighterState.dead && !_hasBounced) {
            _hasBounced = true;
            GameManager.Get().StunFrames(0.1f, _filter);
            GameManager.Get().GetCameraShaker().SetShake(0.2f, 5f, true);
            GameManager.Get().GetStageShaker().SetShake(0.2f, 5f, true);

            if (_filter == FighterFilter.one) {
                _controllerVelocity = new Vector3(-15, 3, 0);
            }
            else {
                _controllerVelocity = new Vector3(15, 3, 0);
            }
            _rigidbody.velocity = _controllerVelocity;
            _animator.SetTrigger("wallBounce");
        }
    }

    public virtual void OnLand() {
        _controllerScaler.localScale = new Vector3(1.2f, 0.8f, 1);
        GameManager.Get().GetCameraShaker().SetShake(0.1f, 1.5f, true);
        _source.PlayOneShot(_jumpDownSFX);

        if (_jumpLand != null) {
            _jumpLand.transform.position = _groundHit.point;
            _jumpLand.Play();
            PlayLeftFoot();
            PlayRightFoot();
        }

        if (_myAction == FighterAction.dashing) {
            _myAction = FighterAction.none;
        }

        _animator.SetTrigger("land");
        _canJump = true;

        ResetAttack();
    }

    public virtual void OnJump() {
        _canJump = false;

        if (_canAttack) {
            _animator.SetTrigger("jump");
        }

        if (_jumpDust != null) {
            _jumpDust.transform.position = _groundHit.point;
            if (_controllerVelocity.x > 0) {
                _jumpDust.transform.eulerAngles = new Vector3(0, 0, 330);
            }
            else if (_controllerVelocity.x < 0) {
                _jumpDust.transform.eulerAngles = new Vector3(0, 0, 30);
            }
            else {
                _jumpDust.transform.eulerAngles = Vector3.zero;
            }

            _jumpDust.Play();
        }

        _source.PlayOneShot(_jumpUpSFX);

        _myAction = FighterAction.jumping;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
        _rigidbody.AddForce(Vector3.up * GetComponent<Rigidbody>().mass * _jumpForce, ForceMode.Impulse);
    }

    public virtual void OnAttack() {
        if (_swingSounds.Length > 0) {
            _source.PlayOneShot(_swingSounds[UnityEngine.Random.Range(0, _swingSounds.Length)], 1.75f);
        }
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
                _myAction = FighterAction.none;
            }
        }

        var xCalculation = _inputHandler.GetInputX();

        if (_myState == FighterState.inControl) {
            _rigidbody.AddForce(new Vector3(xCalculation, 0, 0) * _speed * 5);
            velocityX = Mathf.Clamp(velocityX, -5f, 5f);
        }

        _controllerVelocity = new Vector3(velocityX, velocityY, 0);
    }

    public virtual void OnDash() {
        if (_myAction == FighterAction.dashing) {
            return;
        }
        StartCoroutine(DashProcess());
    }

    IEnumerator DashProcess() {
        // _myAction = FighterAction.dashing;
        _canJump = false;
        _effects.SetAfterImage();
        _isDashing = true;
        yield return new WaitForSeconds(0.1f);
        _effects.DisableAfterImage();
        _isDashing = false;
        //if (_myAction == FighterAction.dashing) {
        //    _myAction = FighterAction.none;
        //}

        if (IsGrounded()) {
            _canJump = true;
        }
    }



    public virtual void ProcessInput() {
        if (IsGrounded()) {
            _myStance = FighterStance.standing;
        }
        else {
            _myStance = FighterStance.air;
        }

        if (_freeze || _myState == FighterState.dead) {
            return;
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
        }

        if (_failSafeAttack > 0) {
            _failSafeAttack -= Time.deltaTime;
            if (_failSafeAttack <= 0) {
                ResetAttack();
            }
        }
    }

    void UpdateMove() {
        OnAttack();
        _hitboxes.SetMove(_currentMove);
        _animator.SetTrigger(_currentMove.GetPath());
        _failSafeAttack = _currentMove.GetClip().length;
    }

    public virtual void OnSuccessfulHit(Vector3 point) {
        //AddMeter(12);
        AddMeter(12 / GameManager.Get().GetSuccessive());
        //Debug.Log("Successive: " + GameManager.Get().GetSuccessive() + " - Meter Gain: " + 12 / GameManager.Get().GetSuccessive());
        _animator.Play(_currentMove.GetClipName(), 0, (1f / _currentMove.GetFrames()) * _currentMove.GetHitFrame());
        _impact.transform.position = point;
        _impact.Play();

        if (_hitSounds.Length > 0) {
            _source.PlayOneShot(_hitSounds[UnityEngine.Random.Range(0, _hitSounds.Length)], 0.5f);
        }
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
            ResetAttack();
        }
        StopCoroutine(Stun);
    }

    public bool CanAttack() {
        return _canAttack;
    }

    public void SetFighterAction(FighterAction action) {
        _myAction = action;
    }

    public void SetFighterStance(FighterStance stance) {
        _myStance = stance;
    }
}
