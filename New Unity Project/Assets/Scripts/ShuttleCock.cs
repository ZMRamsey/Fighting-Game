using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShuttleCock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _maxSpeed = 40;
    [Range(0.0f, 1.0f)] [SerializeField] float _trailActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _windActiveOnPercent;
    [Range(0.0f, 1.0f)] [SerializeField] float _killActiveOnPercent;
    [SerializeField] int _bouncesBeforeSpeedLoss;
    [SerializeField] bool _canKillOnPercentSpeed;

    [Header("Aesthetic")]
    [SerializeField] protected Transform _ballHolder;
    [SerializeField] protected float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;

    [Header("Particles")]
    [SerializeField] ParticleSystem _hit;
    [SerializeField] ParticleSystem _wallHit;
    [SerializeField] ParticleSystem _trailParticle;

    [Header("Audio")]
    [SerializeField] AudioSource _windSource;
    [SerializeField] AudioClip[] _initialImpact;
    [SerializeField] AudioClip[] _ricochets;

    [Header("Componenets")]
    protected AudioSource _source;
    protected Rigidbody _rb;
    FighterController _owner;

    [Header("Stats")]
    bool _frozen;
    bool _waitForHit;
    protected float _squishTimer;
    float _speed;
    float _magnitude;
    float jail;
    int _bouncesSinceShoot;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();

        _speed = 1;
    }

    private void Start() {
        ResetShuttle();
    }

    [ContextMenu("Reset Ball")]
    public virtual void ResetShuttle() {
        if(shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }
        _rb.isKinematic = true;
        _waitForHit = true;
        _rb.velocity = Vector3.zero;
        _speed = 1;
    }


    void ProcessForce(Vector3 direction, Vector3 movementInfluence, bool slowDown) {
        float processedSpeed = _speed;

        if (slowDown) {
            processedSpeed = 2;
        }

        _hit.Play();

        SquishBall();

        _rb.velocity = Vector3.zero;

        Vector3 targetVelocity = (movementInfluence + direction) * processedSpeed;

        _rb.velocity = targetVelocity;

        _speed += 0.5f;

        _bouncesSinceShoot = 0;
    }

    Coroutine shootCoroutine;
    public virtual void Shoot(Vector3 distance, Vector3 movementInfluence, bool player, bool slowDown, FighterFilter filter) {
        if (player) {
            GameManager.Get().StunFrames(0.3f, filter);
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2f, true);
        }

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        if (_rb.velocity.magnitude > (_maxSpeed / 2)) {
            GameManager.Get().OnImpactFrame(0.1f);
        }

        shootCoroutine = StartCoroutine(ShootProccess(distance, movementInfluence, slowDown, false, 0.3f));
    }

    public virtual void Shoot(Vector3 distance, Vector3 movementInfluence, bool player, bool slowDown, FighterFilter filter, FighterController owner) {
        SetOwner(owner);
        Shoot(distance, movementInfluence, player, slowDown, filter);
    }

    public void Bounce(float axis) {
        _speed = 1;
        ProcessForce(new Vector3(axis,1,0), Vector3.one, false);
    }

    public FighterController GetOwner() {
        return _owner;
    }

    public bool IsFrozen() {
        return _frozen;
    }

    public bool CanKill() {
        return _canKillOnPercentSpeed && GetSpeedPercent() > _killActiveOnPercent;
    }

    bool isPlaying;
    float volume;
    void Update() {

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

        _source.spatialBlend = vol;

        if (_windSource != null) {

            _windSource.spatialBlend = vol;
            

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

        var tempVel = _rb.velocity;
        tempVel = Vector3.ClampMagnitude(tempVel, _maxSpeed);

        _rb.velocity = tempVel;

        if (_frozen) {
            _rb.velocity = Vector3.zero;
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

 
    void OnWallHit(ContactPoint point, float force) {
        _bouncesSinceShoot++;

        if(_bouncesSinceShoot > _bouncesBeforeSpeedLoss) {
            _speed -= 0.5f;
        }

        volume =0;
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

        float calc = _magnitude / _initialImpact.Length;


        int con = (int)calc;
        con = Mathf.Clamp(con, 0, _initialImpact.Length - 1);

        _source.PlayOneShot(_initialImpact[con]);

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
        SquishBall();
        OnWallHit(collision.contacts[0], collision.relativeVelocity.magnitude);
    }

    public void FreezeShuttle(float timer) {
        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        shootCoroutine = StartCoroutine(ShootProccess(Vector3.zero, Vector3.zero, false, true, timer));
    }

    void OnCollisionStay(Collision collision) {
        _rb.velocity *= 0.9f;
    }

    public void Reverse() {
        SetOwner(FighterFilter.both);

        Vector3 vel = _rb.velocity;
        vel.x = -vel.x;
        _rb.velocity = vel;
    }

    IEnumerator ShootProccess(Vector3 distance, Vector3 movementInfluence, bool slowDown, bool resetVelocity, float time) {
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
            ProcessForce(distance, movementInfluence, slowDown);
        }
    }

    public void SetOwner(FighterController owner)
    {
        _owner = owner;

        SetOwner(owner.GetFilter());
    }

    void SetOwner(FighterFilter owner) {
        if (owner == FighterFilter.one) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.red);
        }
        else if (owner == FighterFilter.two) {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.blue);
        }
        else {
            transform.root.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.yellow);
            _owner = null;
        }
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
