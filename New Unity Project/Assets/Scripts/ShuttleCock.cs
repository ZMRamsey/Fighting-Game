using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShuttleCock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _maxSpeed;
    [Range(0.0f, 1.0f)] [SerializeField] float _trailActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _windActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _killActiveOnPercent;
    [SerializeField] float _maximumJail;
    [SerializeField] int _bouncesBeforeSpeedLoss;
    [SerializeField] bool _canKillOnPercentSpeed;
    [SerializeField] float _gainPerHit = 0.25f;
    [SerializeField] bool _canImpactFrame;
    [SerializeField] bool _useGravity = true;
    [SerializeField] bool _flipGravity = false;
    [SerializeField] bool _canTimeOut;
    float _timeOutValue;

    [Header("Aesthetic")]
    [SerializeField] protected Transform _ballHolder;
    [SerializeField] protected float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;
    [SerializeField] GameObject _wind;
    [SerializeField] Transform _circle;
    [SerializeField] Image _timeOutVisual;

    [Header("Particles")]
    [SerializeField] ParticleSystem _hit;
    [SerializeField] ParticleSystem _wallHit;
    [SerializeField] ParticleSystem _impactFrameEffect;
    [SerializeField] ParticleSystem _trailRed;
    [SerializeField] ParticleSystem _trailBlue;
    [SerializeField] ParticleSystem _trailYellow;
    [SerializeField] ParticleSystem _trailParticle;

    [Header("Audio")]
    [SerializeField] AudioSource _windSource;
    [SerializeField] AudioSource _killSource;
    [SerializeField] ShuttleCockSound _soundManager;
    [SerializeField] AudioClip[] _ricochets;

    [Header("Componenets")]
    protected AudioSource _source;
    protected Rigidbody _rb;
    FighterController _owner;

    [Header("Stats")]
    FighterFilter _filter;
    bool _frozen;
    bool _windTrailActive;
    bool _redKillActive;
    bool _blueKillActive;
    bool _yellowKillActive;
    protected float _squishTimer;
    float _speed;
    float _windVolume;
    float _killVolume;
    Vector3 _lastVelocity;
    float grabbedTimer;
    int _bouncesSinceShoot;
    FighterController _grabber;
    Vector3 _storedHitVelocity;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        _speed = 1;
        _rb.useGravity = false;
        _timeOutValue = 0;
    }

    void Start() {
        ResetShuttle(true);
    }

    void FixedUpdate() {
        if (_useGravity) {
            var gravity = Physics.gravity * _rb.mass;

            if (_flipGravity) {
                gravity = -Physics.gravity * _rb.mass;
            }

            if (!CanKill()) {
                _rb.AddForce(gravity, ForceMode.Acceleration);
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        _rb.velocity *= 0.9f;

        if(_rb.velocity.magnitude < 0.01f && _canTimeOut && GameManager.Get().NoActiveCoroutines()) {
            _timeOutValue += Time.deltaTime;
        }
        else {
            _timeOutValue = 0;
        }
    }

    public virtual void ResetShuttle(bool freeze) {
        _speed = 1;
        _timeOutValue = 0;

        UnboundFromPlayer(false);

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        if (freeze) {
            _rb.isKinematic = true;
        }
        else {
            _rb.isKinematic = false;
        }

        _frozen = false;

        _rb.velocity = Vector3.zero;
    }

    void ProcessForce(HitMessage message) {
        if (_grabber != null) {
            UnboundFromPlayer(false);
        }

        float processedSpeed = _speed;

        if (message.muteVelocity) {
            processedSpeed = 2f;
        }

        if(message.overrideSpeed != 0) {
            processedSpeed = message.overrideSpeed;
        }

        _hit.Play();

        SquishBall();

        _rb.velocity = Vector3.zero;

        _storedHitVelocity = Vector3.zero;

        Vector3 targetVelocity = message.direction * processedSpeed;

        _rb.isKinematic = false;
        _rb.velocity = targetVelocity;

        if (message.shot != ShotType.chip) {
            _speed += _gainPerHit;
        }

        _bouncesSinceShoot = 0;
    }

    public FighterFilter GetFilter() {
        return _filter;
    }

    public bool IsFalling() {
        return GetVelocity().y < 0;
    }

    public bool CanTimeOut() {
        return _timeOutValue >= 1;
    }

    Coroutine shootCoroutine;
    public virtual void Shoot(HitMessage message) {
        transform.right = message.direction.normalized;

        _storedHitVelocity = message.direction * _speed;

        var hitStun = 0.3f;

        if (GetSpeedPercent() > _windActiveOnPercent) {
            hitStun = 0.4f;
        }

        if (CanKill()) {
            hitStun = 0.6f;
        }

        if (message.isPlayer) {
            GameManager.Get().StunFrames(hitStun, message.sender);
            GameManager.Get().GetCameraShaker().SetShake(hitStun / 2, 2f, true);
        }

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        if (CanKill() && _canImpactFrame && _filter != message.sender) {
            GameManager.Get().OnImpactFrame(0.1f);
            if (_impactFrameEffect) {
                _impactFrameEffect.Play();
            }
        }

        shootCoroutine = StartCoroutine(OnFreezeProcess(message, hitStun));
    }

    public virtual void Shoot(HitMessage message, FighterController owner) {
        Shoot(message);
        SetOwner(owner);
    }

    public FighterController GetOwner() {
        return _owner;
    }

    public bool IsFrozen() {
        return _frozen;
    }

    public bool CanKill() {
        return _canKillOnPercentSpeed && GetSpeedPercent() >= _killActiveOnPercent && _rb.velocity.magnitude > 35f;
    }

    void Update() {
        _speed = Mathf.Clamp(_speed, 1, _maximumJail);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);

        if (_canTimeOut) {
            _timeOutVisual.fillAmount = Mathf.Clamp(_timeOutValue, 0, 1) / 1;
        }

        if (_wind) {
            _wind.SetActive(GetSpeedPercent() > _killActiveOnPercent);
        }

        if (GetSpeedPercent() > _trailActiveOnPercent && _rb.velocity.magnitude > 10 && !CanKill()) {
            if (!_windTrailActive && _trailParticle) {
                _trailParticle.Play();
                _windTrailActive = true;
            }
        }
        else {
            if (_windTrailActive && _trailParticle) {
                _trailParticle.Stop();
                _windTrailActive = false;
            }
        }

        if (_trailRed != null) {
            if (CanKill() && _filter == FighterFilter.one) {
                if (!_redKillActive && _trailRed) {
                    _trailRed.Play();
                    _redKillActive = true;
                }
            }
            else {
                if (_redKillActive && _trailRed) {
                    _trailRed.Stop();
                    _redKillActive = false;
                }
            }
        }

        if (_trailBlue != null) {
            if (CanKill() && _filter == FighterFilter.two) {
                if (!_blueKillActive && _trailBlue) {
                    _trailBlue.Play();
                    _blueKillActive = true;
                }
            }
            else {
                if (_blueKillActive && _trailBlue) {
                    _trailBlue.Stop();
                    _blueKillActive = false;
                }
            }
        }

        if (_trailYellow != null) {
            if (CanKill() && _filter != FighterFilter.one && _filter != FighterFilter.two) {
                if (!_yellowKillActive && _trailYellow) {
                    _trailYellow.Play();
                    _yellowKillActive = true;
                }
            }
            else {
                if (_yellowKillActive && _trailYellow) {
                    _trailYellow.Stop();
                    _yellowKillActive = false;
                }
            }
        }

        float pos = Mathf.Clamp(transform.position.x, -5, 5);
        float vol = pos / 5;
        vol = Mathf.Clamp(vol, -0.5f, 0.5f);

        _source.panStereo = -vol;

        if (_windSource != null) {

            _windSource.panStereo = -vol;


            if (GetSpeedPercent() > _windActiveOnPercent && _rb.velocity.magnitude > 10 && GetSpeedPercent() < _killActiveOnPercent) {
                _windVolume = Mathf.Lerp(_windVolume, 1, Time.fixedDeltaTime * 2);
            }
            else {
                _windVolume = Mathf.Lerp(_windVolume, 0, Time.fixedDeltaTime * 10);
            }

            _windSource.volume = _windVolume;
        }

        if (_killSource != null) {

            _killSource.panStereo = -vol;


            if (CanKill()) {
                _killVolume = 1;
                GameManager.Get().GetCameraShaker().SetShake(0.2f, 2f, true);
            }
            else {
                _killVolume = Mathf.Lerp(_killVolume, 0, Time.fixedDeltaTime * 2);
            }

            _killSource.volume = _killVolume;
        }

        Vector3 velocity = _rb.velocity;


        if (_bouncesSinceShoot > _bouncesBeforeSpeedLoss) {
            velocity.x = velocity.x * 0.9f;
        }


        if (_frozen) {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        else {
            UpdateShuttleApperance(velocity);
        }

        _circle.gameObject.SetActive(_grabber != null);

        if (_grabber != null) {
            grabbedTimer -= Time.fixedDeltaTime * 0.5f;
            _circle.localScale = Vector3.one * grabbedTimer;
            _rb.velocity = Vector3.zero;
            transform.position = _grabber.transform.position;

            if (grabbedTimer <= 0 || (_grabber.GetComponent<InputHandler>().GetCrouch() && _grabber.GetGrounded()) || _grabber.IsDashing()) {
                UnboundFromPlayer(true);
            }
        }

        ShuttleUpdate();
    }

    public void SetVelocity(Vector3 dir) {
        _rb.velocity = dir * 4;
    }

    public virtual void UpdateShuttleApperance(Vector3 vel) {
        Vector3 scale = Vector3.one;
        scale.x = Mathf.Clamp(vel.magnitude * 0.15f, 1, 3);

        if (vel.magnitude < 0) {
            scale.x = Mathf.Clamp(vel.magnitude * 0.15f, -1, -3);
        }

        transform.right = Vector3.Lerp(transform.right, vel, Time.fixedDeltaTime * _smoothing);

        if (_squishTimer <= 0) {
            _ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.fixedDeltaTime;
        }
    }

    void SquishBall() {
        _squishTimer = _squishThreshold;

        float x = 0.8f;
        if (_ballHolder.localScale.x < 0) {
            x = -0.8f;
        }
        _ballHolder.localScale = new Vector3(x, 1, 1);
    }

    public virtual void ShuttleUpdate() {

    }


    void OnWallHit(ContactPoint point, float force, string tag) {
        _bouncesSinceShoot++;

        if (_bouncesSinceShoot > _bouncesBeforeSpeedLoss) {
            _speed -= _gainPerHit;
        }

        _windVolume = 0;
        if (force > 80) {
            GameManager.Get().GetStageShaker().SetShake(0.1f, 2.5f, true);
        }
        else if (force > 50) {
            GameManager.Get().GetStageShaker().SetShake(0.1f, 2f, true);
        }
        else if (force > 20) {
            GameManager.Get().GetStageShaker().SetShake(0.1f, 1.5f, true);
        }
        else {
            GameManager.Get().GetStageShaker().SetShake(0.1f, 0.5f, true);
        }

        _source.PlayOneShot(_soundManager.GetClip(tag), 2f);

        if (force > 20) {
            _source.PlayOneShot(_ricochets[Random.Range(0, _ricochets.Length)]);
        }

        var hit = Instantiate(_wallHit.gameObject, point.point, Quaternion.identity);
        hit.transform.rotation = Quaternion.FromToRotation(Vector3.forward, point.normal);
        hit.GetComponent<ParticleSystem>().Play();
        hit.transform.position += hit.transform.forward * 0.1f;
        Destroy(hit, 1);
    }

    public float GetSpeedPercent() {
        return _speed / _maximumJail;
    }

    public float GetSpeed() {
        return _speed;
    }

    public float GetMaxJailSpeed() {
        return _maximumJail;
    }

    public bool IsHeadingRight() {
        return _rb.velocity.x > 0;
    }

    public bool IsHeadingLeft() {
        return _rb.velocity.x < 0;
    }

    public Vector3 GetVelocity() {
        return _rb.velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        SquishBall();
        OnWallHit(collision.contacts[0], collision.relativeVelocity.magnitude, collision.transform.tag);

        if (CanKill()) {
            _lastVelocity = _rb.velocity;
            FreezeShuttle(0.15f);
        }

        if (collision.gameObject.GetComponent<StageNet>()) {
            if (collision.gameObject.layer == 14) {
                collision.gameObject.GetComponent<StageNet>().NetHit(collision.relativeVelocity);
            }
        }

        if (collision.gameObject.GetComponent<MagicNet>()) {
            if (collision.gameObject.layer == 14) {
                collision.gameObject.GetComponent<MagicNet>().OnHit();
            }
        }
    }

    public void FreezeShuttle(float timer) {
        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        shootCoroutine = StartCoroutine(OnFreezeProcess(null, timer));
    }

    public void ForceFreeze() {
        _lastVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _frozen = true;
    }

    public void ForceUnfreeze() {
        _rb.isKinematic = false;
        _frozen = false;

        if (_storedHitVelocity != Vector3.zero) {
            _lastVelocity = _storedHitVelocity;
        }

        _rb.velocity = _lastVelocity;
        _storedHitVelocity = Vector3.zero;
    }

    IEnumerator OnFreezeProcess(HitMessage message, float time) {
        ForceFreeze();

        yield return new WaitForSeconds(time);

        ForceUnfreeze();

        if (message != null) {
            ProcessForce(message);
        }

    }

    public void SetOwner(FighterController owner) {
        _owner = owner;

        SetOwner(owner.GetFilter());
    }

    public void SetOwner(FighterFilter owner) {
        _filter = owner;

        if (_filter == FighterFilter.one) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material = GameManager.Get().GetRedOutline();
        }
        else if (_filter == FighterFilter.two) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material = GameManager.Get().GetBlueOutline();
        }
        else {
            transform.root.GetComponentInChildren<SpriteRenderer>().material = GameManager.Get().GetYellowOutline();
        }
    }

    public void SetBounciness(float value) {
        if (transform.root.GetComponent<SphereCollider>()) {
            transform.root.GetComponent<SphereCollider>().material.bounciness = value;
        }
    }

    public void OnBounce() {
        _bouncesBeforeSpeedLoss++;
    }
    public void Bounce(float axis) {
        _speed = 1;
        var hitMes = new HitMessage(new Vector3(axis, 1, 0), new VelocityInfluence(), false, FighterFilter.both, ShotType.drive);
        ProcessForce(hitMes);
    }

    public void ResetBounce() {
        _bouncesBeforeSpeedLoss = 2;
    }

    public void BoundToPlayer(FighterController player) {
        if(shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        grabbedTimer = 1;
        _grabber = player;
    }

    public void UnboundFromPlayer(bool inheritVel) {
        if (_grabber == null) {
            return;
        }

        _frozen = false;
        _rb.isKinematic = false;

        if (inheritVel) {
            var own = _grabber.GetChipMove().GetHitDirection();
            if (_grabber.GetFilter() == FighterFilter.two) {
                own.x *= -1;
            }
            _rb.velocity = own;
        }

        _grabber.ResetGrab();
        _grabber = null;
        grabbedTimer = 0;
    }

    public bool IsGrabbed(FighterController me) {
        if (_grabber == null) {
            return false;
        }
        return _grabber.GetInstanceID() == me.GetInstanceID();
    }

    public void ReverseVelocity(FighterFilter filter) {
        SetOwner(filter);

        Vector3 vel = _rb.velocity;
        vel.x = -vel.x;
        vel = vel.normalized * 6;
        var hitMes = new HitMessage(vel, new VelocityInfluence(), false, FighterFilter.none, ShotType.drive);
        Shoot(hitMes);
    }

}

