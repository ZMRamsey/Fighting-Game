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
    [Header("Settings")]
    [SerializeField] protected FighterSettings _settings;

    [Header("Controller Components")]
    [SerializeField] Hitbox _hitboxes;
    [SerializeField] Hurtbox _hurtBox;
    [SerializeField] Transform _hitboxFlipper;

    [Header("Aesthetic")]
    [SerializeField] Transform _controllerScaler;
    [SerializeField] FighterEffects _effects;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected SpriteRenderer _rgbRenderer;
    [SerializeField] float _stretchSpeed;
    [SerializeField] protected AudioSource _source;
    [SerializeField] AudioClip _jumpUpSFX, _jumpDownSFX;
    [SerializeField] AudioClip[] _hitSounds;
    [SerializeField] AudioClip[] _damageSounds;
    [SerializeField] AudioClip[] _swingSounds;
    [SerializeField] AudioClip[] _leftFootSounds;
    [SerializeField] AudioClip[] _rightFootSounds;
    [SerializeField] AudioClip[] _grabSound;
    [SerializeField] AudioClip[] _squeakSound;

    [SerializeField] ParticleSystem _hitSmashVFX;
    [SerializeField] ParticleSystem _hitChipVFX;
    [SerializeField] ParticleSystem _hitDriveVFX;
    [SerializeField] ParticleSystem _hitLiftVFX;
    [SerializeField] ParticleSystem _hitImpactBig;
    [SerializeField] ParticleSystem _runningLeftVFX;
    [SerializeField] ParticleSystem _runningRightVFX;
    [SerializeField] ParticleSystem _jumpDust;
    [SerializeField] ParticleSystem _jumpLand;

    [Header("Controller Values")]
    [SerializeField] protected Vector3 _controllerVelocity;
    [SerializeField] protected Vector3 _extraVelocity;
        Vector3 _lastTapAxis;
    protected FighterMove _currentMove;
    protected bool _canAttack;
    float _commandMeter;
    float _yVelocity;
    float _grabCoolDown;
    float _dashCoolDown;
    float _successHitsCoolDown;
    float _failSafeAttack;
    int _currentJumps;
    int _successfulHits;
    bool _canJump;
    bool _freeze;
    bool _isDashing;
    bool _hasBounced;
    bool _grounded;
    bool _inSuper;
    RaycastHit _groundHit;

    protected FighterAction _myAction;
    protected FighterStance _myStance;
    protected FighterState _myState;

    [Header("Components")]
    [SerializeField] protected Animator _animator;
    FighterUI _fighterUI;
    protected InputHandler _inputHandler;
    protected Rigidbody _rigidbody;

    bool test = false;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        _rigidbody.isKinematic = true;
        FighterAwake();
    }

    public bool HurtBoxActive() {
        return _hurtBox.isActiveAndEnabled;
    }

    public virtual void FighterAwake() {

    }

    public virtual void SetPallette(bool active, Material mat) {

    }

    public FighterMove GetSmashMove() {
        return _settings.GetSmashMove();
    }

    public FighterMove GetChipMove() {
        return _settings.GetChipMove();
    }

    public FighterMove GetDriveMove() {
        return _settings.GetDriveMove();
    }

    public FighterMove GetLiftMove() {
        return _settings.GetLiftMove();
    }

    public virtual Transform GetFocusTransform() {
        return transform;
    }

    void Start() {
        _fighterUI = GameManager.Get().GetFighterTab(_filter).GetUI();
    }

    public void PlayLeftFoot() {
        if (_leftFootSounds == null || _leftFootSounds.Length == 0) {
            return;
        }
        _source.PlayOneShot(_leftFootSounds[UnityEngine.Random.Range(0, _leftFootSounds.Length)], 1f);
    }

    public void PlaySound(AudioClip clip) {
        _source.PlayOneShot(clip);
    }

    public void PlayRightFoot() {
        if (_rightFootSounds == null || _rightFootSounds.Length == 0) {
            return;
        }

        _source.PlayOneShot(_rightFootSounds[UnityEngine.Random.Range(0, _rightFootSounds.Length)], 1f);
    }

    public virtual void SetFilter(FighterFilter filter) {
        _filter = filter;
    }

    public FighterFilter GetFilter() {
        return _filter;
    }

    public InfluenceType GetHitType() {
        return _settings.GetHitType();
    }

    public virtual void InitializeFighter() {
        _settings.InitializeSettings();
        _renderer.flipX = _filter == FighterFilter.one;
        if (_rgbRenderer) {
            _rgbRenderer.flipX = _filter == FighterFilter.one;
        }

        if (_filter == FighterFilter.one) {
            _hitboxFlipper.localScale = new Vector3(1, 1, 1);
            _renderer.sortingOrder = -1;
            if (_rgbRenderer) {
                _rgbRenderer.sortingOrder = 0;
            }
        }
        else {
            _hitboxFlipper.localScale = new Vector3(-1, 1, 1);
            _renderer.sortingOrder = -3;
            if (_rgbRenderer) {
                _rgbRenderer.sortingOrder = -2;
            }
        }

        if (_filter == FighterFilter.one) {
            _source.panStereo = -0.5f;
        }
        else {
            _source.panStereo = 0.5f;
        }

        ResetFighter();

    }

    public void ResetGrab() {
        _grabCoolDown = 1;
    }

    public bool CanGrab() {
        return _grabCoolDown <= 0;
    }

    public bool IsKOed() {
        return _myState == FighterState.dead;
    }

    public virtual void ResetFighter() {
        OnSuperEnd(true);

        _animator.SetLayerWeight(1, 0);

        _myState = FighterState.inControl;

        ResetAttack();
        ResetSuper();

        _commandMeter = 0.0f;
        _currentJumps = 0;
        _successfulHits = 0;
        _canJump = true;
        _canAttack = true;
        _hasBounced = false;

        AdjustControllerHeight();
        _rigidbody.isKinematic = false;
    }

    void Update() {
        OnFighterUpdate();

        _grounded = IsGrounded();

        _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, Vector3.one, Time.fixedDeltaTime * _stretchSpeed);

        ProcessInput();

        if (_grabCoolDown > 0) {
            _grabCoolDown -= Time.fixedDeltaTime;
        }

        if (_dashCoolDown > 0) {
            _dashCoolDown -= Time.fixedDeltaTime;
        }

        if (_successHitsCoolDown > 0) {
            _successHitsCoolDown -= Time.deltaTime;
            if (_successHitsCoolDown <= 0) {
                _successHitsCoolDown = 0.0f;
                _successfulHits = 0;
            }
        }

        if (_settings.HasDash() && _dashCoolDown <= 0 && _inputHandler.GetDash() && !_isDashing && _myState == FighterState.inControl) {
            _lastTapAxis.x = _inputHandler.GetInputX();
            _lastTapAxis.y = 0;
            if (_inputHandler.GetJumpHeld()) {
                _lastTapAxis.y = 1;
            }

            if (_inputHandler.GetCrouch()) {
                _lastTapAxis.y = -1;
            }

            _lastTapAxis.Normalize();

            OnDash();
        }

        _animator.SetBool("grounded", _myStance == FighterStance.standing);


        Vector3 controllerVel = _controllerVelocity.normalized;
        var xAnim = controllerVel.x;

        if (_renderer.flipX) {
            xAnim = -xAnim;
        }

        _animator.SetFloat("xInput", xAnim);
        _animator.SetFloat("yInput", _rigidbody.velocity.y);

        if (GameManager.Get().KOCoroutine != null && !IsKOed()) {
            if (_inputHandler._crouchInput) {
                _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, new Vector3(1f, 0.7f, 1f), Time.fixedDeltaTime * 10);
            }
            else {
                _controllerScaler.localScale = Vector3.Lerp(_controllerScaler.localScale, Vector3.one, Time.fixedDeltaTime * 5); ;
            }
        }

    }

    void FixedUpdate() {
        OnFixedFighterUpdate();
        var canRun = (int)Mathf.Abs(_rigidbody.velocity.magnitude) > _settings.GetSpeed() / 2;
        var isRunning = _myState == FighterState.inControl && _myStance == FighterStance.standing && canRun && !_inputHandler.GetCrouch();

        _animator.SetBool("running", isRunning);

        _animator.SetBool("falling", _myStance == FighterStance.air && _rigidbody.velocity.y < 0);

        if (!_freeze) {
            if (_myStance == FighterStance.standing) {
                OnGroundMovement();
            }

            if ((_myStance == FighterStance.air || _myAction == FighterAction.jumping)) {
                OnAirMovement();
            }

            if (_settings.HasJump() && _inputHandler.GetDoubleJump(!_canJump && !_grounded && _currentJumps < _settings.GetMaxJumps() && _canAttack && _myState == FighterState.inControl)) {
                OnJump();
                return;
            }

            if (_settings.HasJump() && _inputHandler.GetJump(_canJump) && _canJump && _myAction != FighterAction.jumping && _canAttack && _myState == FighterState.inControl) {
                OnJump();
                return;
            }

            if (_isDashing) {
                _rigidbody.AddForce(new Vector3(_lastTapAxis.x * 20, _lastTapAxis.y * 4, 0), ForceMode.Impulse);
            }

            if (_animator.speed == 1) {
                _extraVelocity *= 0.9f;
            }

            _rigidbody.velocity = _controllerVelocity + _extraVelocity;
        }
    }

    public void KO(Vector3 velocity) {
        _myState = FighterState.dead;
        _animator.Rebind();
        _animator.SetLayerWeight(1, 1);
        _animator.SetTrigger("KO");
        _grounded = false;


        if (_damageSounds.Length > 0) {
            _source.PlayOneShot(_damageSounds[UnityEngine.Random.Range(0, _damageSounds.Length)], 1.5f);
        }

        OnJump();
        _controllerVelocity = velocity;
    }

    public virtual void OnFighterUpdate() {

    }

    public virtual void OnFixedFighterUpdate() {

    }

    float xCalculation = 0.0f;

    float lastXTest;
    public virtual void OnGroundMovement() {
        if (lastXTest > 0) {
            if (_inputHandler.GetInputX() < 0) {
                OnChangeDirection(false);
            }
        }

        if (lastXTest < 0) {
            if (_inputHandler.GetInputX() > 0) {
                OnChangeDirection(true);
            }
        }

        lastXTest = _inputHandler.GetInputX();

        if (_canAttack && !_inSuper && _myState == FighterState.inControl) {
            xCalculation = _inputHandler.GetInputX();
            xCalculation *= _settings.GetSpeed();
        }
        else {
            if (!_isDashing) {
                xCalculation = xCalculation * 0.8f;
            }
        }

        if (_inputHandler.GetCrouch()) {
            xCalculation = 0;
        }

        if (CanHahaFunny() && _inputHandler.GetCrouch()) {
            //print("nice");
        }

        AdjustControllerHeight();

        xCalculation = Mathf.Clamp(xCalculation, -_settings.GetSpeed(), _settings.GetSpeed());

        _controllerVelocity = new Vector3(xCalculation, _yVelocity, 0);
    }

    public void ResetHitbox() {
        _hitboxes.ResetCD();
    }

    public void ResetSuper() {
        _animator.SetLayerWeight(2, 0);
        _inSuper = false;
    }

    public void ResetAttack() {
        _canAttack = true;
    }

    public void RemoveControl() {
        if (_myState == FighterState.dead) {
            return;
        }
        _myState = FighterState.restricted;
    }

    public virtual void PlayWin() {
        _animator.SetTrigger("win");
    }

    public virtual void PlayLose() {
        if (_myState == FighterState.dead) {
            return;
        }
        _animator.SetTrigger("lose");
    }

    void AddMeter(float value) {
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

    }

    public virtual void OnSuperEvent() {

    }

    public virtual void OnAfterSuperScreen() {

    }

    public virtual void OnSuperEnd(bool instant) {

    }

    void AdjustControllerHeight() {
        var rayDirection = transform.TransformDirection(Vector3.down);

        float targetDirVel = Vector3.Dot(rayDirection, _controllerVelocity);
        float groundDirVel = Vector3.Dot(rayDirection, Vector3.zero);

        float relativeVel = targetDirVel - groundDirVel;

        float x = _groundHit.distance - _settings.GetHeight();
        float springForce = (x * 1) - (relativeVel * 0.01f);

        Vector3 result = rayDirection * springForce;
        _yVelocity = result.y;
    }

    private void OnCollisionEnter(Collision collision) {
        if (_myState == FighterState.dead && !_hasBounced && !_grounded) {
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
        _dashCoolDown = 0;

        if (_jumpLand != null) {
            _jumpLand.transform.position = _groundHit.point;
            _jumpLand.Play();
            PlayLeftFoot();
            PlayRightFoot();
        }

        _animator.SetTrigger("land");

        _canJump = true;
        _currentJumps = 0;

        ResetAttack();
    }

    public virtual void OnJump() {
        _canJump = false;
        _currentJumps++;

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
        _rigidbody.AddForce(Vector3.up * GetComponent<Rigidbody>().mass * _settings.GetJumpForce(), ForceMode.Impulse);
    }

    //Plays a swing sound for the attack
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
                _rigidbody.AddForce(Physics.gravity * GetComponent<Rigidbody>().mass * (_settings.GetFallMultiplier() - 1));
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
                velocityY *= _settings.GetFallOff();
            }
            else {
                _myAction = FighterAction.none;
            }
        }

        if (CanFastFall() && _inputHandler.GetCrouch()) {
            _rigidbody.AddForce(Physics.gravity * GetComponent<Rigidbody>().mass * _settings.GetFastFallSpeed());
        }

        var xCalculation = _inputHandler.GetInputX();

        if (_myState == FighterState.inControl) {
            _rigidbody.AddForce(new Vector3(xCalculation, 0, 0) * _settings.GetSpeed() * _settings.GetAirSpeed());
            velocityX = Mathf.Clamp(velocityX, -_settings.GetAirSpeed(), _settings.GetAirSpeed());
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
        _canJump = false;
        _effects.SetAfterImage();
        _isDashing = true;
        _hurtBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _effects.DisableAfterImage();
        _isDashing = false;
        yield return new WaitForSeconds(0.1f);
        _hurtBox.gameObject.SetActive(true);

        if (_grounded) {
            _currentJumps = 0;
            _canJump = true;
        }
        _dashCoolDown = 1;
    }

    //Checks all input commands from InputHandler
    public virtual void ProcessInput() {
        if (IsGrounded()) {
            _myStance = FighterStance.standing;
        }
        else {
            _myStance = FighterStance.air;
        }

        if (_freeze || _inSuper || _myState != FighterState.inControl || GameManager.Get().IsInKO()) {
            return;
        }

        if (_inputHandler.GetSmash() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _settings.GetSmashMove();
            UpdateMove();
        }

        if (_inputHandler.GetDrive() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _settings.GetDriveMove();
            UpdateMove();
        }

        if (_inputHandler.GetLift() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _settings.GetLiftMove();
            UpdateMove();
        }


        if (_inputHandler.GetChip() && _canAttack) {
            _canAttack = false;
            ResetHitbox();

            _currentMove = _settings.GetChipMove();
            UpdateMove();
        }

        if (_inputHandler.GetSuper() && _canAttack && _commandMeter >= 100) {
            _canAttack = false;
            _inSuper = true;
            ResetHitbox();

            _animator.SetLayerWeight(2, 1);

            _currentMove = _settings.GetSuperMove();
            OnSuperMechanic();

            UpdateMove();
            ResetMeter();
        }

        if (_failSafeAttack > 0 && _animator.speed == 1) {
            _failSafeAttack -= Time.deltaTime;
            if (_failSafeAttack <= 0) {
                ResetAttack();
            }
        }

    }

    public virtual void UpdateMove() {
        OnAttack();
        _hitboxes.SetMove(_currentMove);
        _animator.SetTrigger(_currentMove.GetPath());
        _failSafeAttack = _currentMove.GetClip().length;
    }

    public virtual void OnSuccessfulHit(Vector3 point, Vector3 dir, bool big, ShotType shot, bool isGrab) {
        _successfulHits++;
        _successHitsCoolDown = 2;

        if (isGrab) {
            _failSafeAttack = 0;
            ResetAttack();
        }


        if (_successfulHits > 1) {
            GameManager.Get().GetFighterTab(GetFilter()).UpdateRallyScore(_successfulHits);
        }

        //GameManager.Get().GetFighterTab(GetFilter()).UpdateMessage("beans");

        _isDashing = false;
        AddMeter(_settings.GetMeterIncreaseValue() / GameManager.Get().GetSuccessive());

        if (!isGrab && _successfulHits > 2) {
            _extraVelocity.x = 8;
            if (GetFilter() == FighterFilter.two) {
                _extraVelocity.x = -8;
            }
        }

        _animator.Play(_currentMove.GetClipName(), 0, (1f / _currentMove.GetFrames()) * _currentMove.GetHitFrame());

        ParticleSystem useVFX = null;
        if (shot == ShotType.smash) {
            useVFX = _hitSmashVFX;
        }

        if (shot == ShotType.chip) {
            useVFX = _hitChipVFX;
        }

        if (shot == ShotType.drive) {
            useVFX = _hitDriveVFX;
        }

        if (shot == ShotType.lift) {
            useVFX = _hitLiftVFX;
        }

        useVFX.transform.position = point;
        useVFX.transform.right = -dir;
        useVFX.Play();

        if (big) {
            _hitImpactBig.transform.position = point;
            _hitImpactBig.Play();
        }

        if (!isGrab) {
            if (_hitSounds.Length > 0) {
                _source.PlayOneShot(_hitSounds[UnityEngine.Random.Range(0, _hitSounds.Length)], 0.5f);
            }
        }
        else {
            if (_grabSound.Length > 0) {
                _source.PlayOneShot(_grabSound[UnityEngine.Random.Range(0, _grabSound.Length)], 1f);
            }
        }

    }

    public void OnChangeDirection(bool isLeft) {
        if (_runningLeftVFX == null || _runningRightVFX == null) {
            return;
        }

        if (!_canAttack) {
            return;
        }

        if (isLeft) {
            _runningLeftVFX.Play();
        }
        else {
            _runningRightVFX.Play();
        }

        _source.PlayOneShot(_squeakSound[UnityEngine.Random.Range(0, _squeakSound.Length)], 0.5f);
    }

    public bool IsDashing() {
        return _isDashing;
    }

    public virtual bool IsGrounded() {
        if (_myAction == FighterAction.jumping) {
            return false;
        }

        var groundCheck = Physics.Raycast(transform.position, Vector3.down, out _groundHit, _settings.GetHeight(), _settings.GetGroundMask());

        if (groundCheck && _myStance == FighterStance.air) {
            OnLand();
        }

        return groundCheck;
    }

    public bool GetGrounded() {
        return _grounded;
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
        if (_grounded) {
            _currentJumps = 0;
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

    public bool CanFastFall() {
        return _myStance == FighterStance.air && _rigidbody.velocity.y < 0.1;
    }

    public bool CanHahaFunny() {
        return _myState != FighterState.dead && GameManager.Get().KOCoroutine != null;
    }

    public InputHandler GetHandler() {
        return _inputHandler;
    }
}
