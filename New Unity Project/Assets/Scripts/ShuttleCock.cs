using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShuttleCock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _maxSpeed;
    [Range(0.0f, 1.0f)] [SerializeField] float _trailActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _windActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _killActiveOnPercent;
    [SerializeField] int _bouncesBeforeSpeedLoss;
    [SerializeField] bool _canKillOnPercentSpeed;
    [SerializeField] float _gainPerHit = 0.25f;
    [SerializeField] bool _canImpactFrame;

    [Header("Aesthetic")]
    [SerializeField] protected Transform _ballHolder;
    [SerializeField] protected float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;
    [SerializeField] GameObject _wind;

    [Header("Particles")]
    [SerializeField] ParticleSystem _hit;
    [SerializeField] ParticleSystem _wallHit;
    [SerializeField] ParticleSystem _impactFrameEffect;
    [SerializeField] ParticleSystem _trailParticle;

    [Header("Audio")]
    [SerializeField] AudioSource _windSource;
    [SerializeField] ShuttleCockSound _soundManager;
    [SerializeField] AudioClip[] _ricochets;

    [Header("Componenets")]
    protected AudioSource _source;
    protected Rigidbody _rb;
    FighterController _owner;

    [Header("Stats")]
    FighterFilter _filter;
    bool _canGimic;
    VelocityInfluence _influence;
    bool _frozen;
    bool _waitForHit;
    protected float _squishTimer;
    float _speed;
    float _magnitude;
    float chargedForce;
    float chargeTimer;
    int _bouncesSinceShoot;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        chargedForce = 0.1f;
        _speed = 1;
    }

    private void Start() {
        ResetShuttle(true);
    }

    [ContextMenu("Reset Ball")]
    public virtual void ResetShuttle(bool freeze) {
        _speed = 1;

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        if (freeze) {
            _rb.isKinematic = true;
            _waitForHit = true;
        }
        else {
            _rb.isKinematic = false;
        }

        _frozen = false;

        _rb.velocity = Vector3.zero;
    }

    Vector3 _lastShootForce;
    Vector3 _lastShootDirection;
    public void ProcessForce(HitMessage message, float charge) {
        _canGimic = !message.muteVelocity && charge <= 0.4f;

        float processedSpeed = _speed;
        _acceleration = 0;
        _accelerationTimer = 0;
        _lastShootDirection = message.direction;

        if (message.muteVelocity) {
            processedSpeed = 2f;
        }

        if (charge <= 0.4f) {
            processedSpeed = 1.5f;
        }

        if (message.influence.type == InfluenceType.overtime) {
            _influence = message.influence;
        }
        else {
            _influence = new VelocityInfluence();
        }

        _hit.Play();

        SquishBall();

        _rb.velocity = Vector3.zero;

        Vector3 proceDir = message.direction;

        Vector3 targetVelocity = proceDir * processedSpeed;

        if (message.influence.type == InfluenceType.instant) {
            targetVelocity = (message.influence.velocity + proceDir) * processedSpeed;
        }

        _rb.isKinematic = false;

        _rb.velocity = targetVelocity;

        _speed += _gainPerHit;

        _lastShootForce = targetVelocity;

        _bouncesSinceShoot = 0;
    }

    public FighterFilter GetFilter() {
        return _filter;
    }

    Coroutine shootCoroutine;
    public virtual void Shoot(HitMessage message) {
        transform.right = message.direction.normalized;

        if (message.isPlayer) {
            GameManager.Get().StunFrames(0.3f, message.sender);
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2f, true);
        }

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        if (GetSpeedPercent() >= _killActiveOnPercent && _canImpactFrame) {
            GameManager.Get().OnImpactFrame(0.1f);
            if (_impactFrameEffect) {
                _impactFrameEffect.Play();
            }
        }

        shootCoroutine = StartCoroutine(ShootProccess(message, false, 0.3f));
    }

    public virtual void Shoot(HitMessage message, FighterController owner) {
        SetOwner(owner);
        Shoot(message);
    }

    public void Bounce(float axis) {
        _speed = 1;
        var hitMes = new HitMessage(new Vector3(axis, 1, 0), new VelocityInfluence(), false, FighterFilter.both);
        ProcessForce(hitMes, 1);
    }

    public FighterController GetOwner() {
        return _owner;
    }

    public bool IsFrozen() {
        return _frozen;
    }

    public bool CanKill() {
        return _canKillOnPercentSpeed && GetSpeedPercent() >= _killActiveOnPercent;
    }

    bool isPlaying;
    float volume;
    void Update() {

        if (_frozen) {
            if (_owner && _owner.GetComponent<InputHandler>().GetCharge()) {
                chargeTimer += Time.deltaTime;
                chargedForce = Mathf.Clamp(chargeTimer, 0f, 0.3f) / 0.3f;
            }
        }

        if (_wind) {
            _wind.SetActive(GetSpeedPercent() > _killActiveOnPercent);
        }

        if (GetSpeedPercent() > _trailActiveOnPercent) {
            if (!isPlaying && _trailParticle) {
                _trailParticle.Play();
                isPlaying = true;
            }
        }
        else {
            if (isPlaying && _trailParticle) {
                _trailParticle.Stop();
                isPlaying = false;
            }
        }

        float pos = Mathf.Clamp(transform.position.x, -5, 5);
        float vol = pos / 5;
        vol = Mathf.Clamp(vol, -0.5f, 0.5f);

        _source.panStereo = -vol;

        if (_windSource != null) {

            _windSource.panStereo = -vol;


            if (GetSpeedPercent() > _windActiveOnPercent) {
                volume = Mathf.Lerp(volume, 1, Time.deltaTime * 2);
            }
            else {
                volume = Mathf.Lerp(volume, 0, Time.deltaTime * 10);
            }

            _windSource.volume = volume;
        }

        _magnitude = _rb.velocity.magnitude;

        Vector3 velocity = _rb.velocity;

        UpdateShuttleApperance(velocity);

        velocity.x = velocity.x * 0.9f;

        //var tempVel = _rb.velocity;
        //tempVel = Vector3.ClampMagnitude(tempVel, _maxSpeed);

        //_rb.velocity = tempVel;

        if (_frozen) {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        ShuttleUpdate();
    }

    public virtual void UpdateShuttleApperance(Vector3 vel) {
        Vector3 scale = Vector3.one;
        scale.x = Mathf.Clamp(vel.magnitude * 0.15f, 1, 3);

        //Negative Check
        if (vel.magnitude < 0) {
            scale.x = Mathf.Clamp(vel.magnitude * 0.15f, -1, -3);
        }

        transform.right = Vector3.Lerp(transform.right, vel, Time.deltaTime * _smoothing);

        if (_squishTimer <= 0) {
            _ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
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

        volume = 0;
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

        //float calc = _magnitude / _initialImpact.Length;


        //int con = (int)calc;
        //con = Mathf.Clamp(con, 0, _initialImpact.Length - 1);

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
        return _rb.velocity.magnitude / _maxSpeed;
    }

    public bool IsBallActive() {
        return _waitForHit == false;
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
        _acceleration = 0;
        _influence = new VelocityInfluence();

        SquishBall();
        OnWallHit(collision.contacts[0], collision.relativeVelocity.magnitude, collision.transform.tag);

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

        var hitMes = new HitMessage(Vector3.zero, new VelocityInfluence(), false, FighterFilter.current);
        shootCoroutine = StartCoroutine(ShootProccess(hitMes, true, timer));
    }

    public void ForceFreeze() {
        _rb.isKinematic = true;
        _frozen = true;
    }

    public void ForceUnfreeze() {
        _rb.isKinematic = false;
        _frozen = false;
        _rb.velocity = _lastShootForce;
    }

    float _acceleration;
    float _accelerationTimer;
    void FixedUpdate() {
        //if (_bouncesSinceShoot < _bouncesBeforeSpeedLoss) {
        //    _rb.velocity = _speed * 10 * (_rb.velocity.normalized);
        //}

        if (_influence != null && _canGimic && _influence.type == InfluenceType.overtime && !_frozen && _accelerationTimer < 1f) {
            _accelerationTimer += Time.deltaTime;
            _acceleration += Time.deltaTime;
            _rb.velocity += _influence.velocity * _acceleration;
        }
    }

    void OnCollisionStay(Collision collision) {
        _rb.velocity *= 0.9f;
    }

    public void Reverse(FighterFilter filter) {
        SetOwner(filter);

        Vector3 vel = _rb.velocity;
        vel.x = -vel.x;
        vel= vel.normalized * 6;
        var hitMes = new HitMessage(vel, new VelocityInfluence(), false, FighterFilter.none);
        Shoot(hitMes);
    }

    IEnumerator ShootProccess(HitMessage message, bool resetVelocity, float time) {
        var vel = _rb.velocity;
        _frozen = true;
        yield return new WaitForSeconds(time);
        _frozen = false;
        if (_waitForHit) {
            _rb.isKinematic = false;
        }
        if (resetVelocity) {
            _rb.velocity = vel;
        }
        else {
            ProcessForce(message, chargedForce);
        }

        chargedForce = 0.1f;
        chargeTimer = 0f;
    }

    public void SetOwner(FighterController owner) {
        _owner = owner;

        SetOwner(owner.GetFilter());
    }

    public void SetOwner(FighterFilter owner) {
        _filter = owner;

        if (_filter == FighterFilter.one) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.red);
        }
        else if (_filter == FighterFilter.two) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.blue);
        }
        else {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.yellow);
        }
    }

    public void SetBounciness(float value) {
        if (transform.root.GetComponent<SphereCollider>()) {
            transform.root.GetComponent<SphereCollider>().material.bounciness = value;
        }
    }

    public void increaseBounces() {
        _bouncesBeforeSpeedLoss++;
        print("Bounces: " + _bouncesBeforeSpeedLoss);
    }

    public void resetBounces() {
        _bouncesBeforeSpeedLoss = 2;
    }
    //public void JailSpeed()
    //{
    //    if(jail == 0)
    //    {
    //        jail = _rb.velocity.magnitude;
    //        Debug.Log("Jailed at: " + jail);
    //    }
    //}

    //public void UnJailSpeed()
    //{
    //    if(jail != 0)
    //    {
    //        _rb.velocity *= jail;
    //        jail = 0;
    //        Debug.Log("UnJailed to " + _rb.velocity.magnitude);
    //    }
    //}
}

