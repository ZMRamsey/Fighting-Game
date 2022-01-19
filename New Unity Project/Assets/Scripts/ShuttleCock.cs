using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShuttleCock : MonoBehaviour
{
    //[SerializeField] Vector3 _ballVelocity;
    [Header("Settings")]
    [SerializeField] float _maxSpeed = 40;

    [Header("Aesthetic")]
    [SerializeField] Transform _ballHolder;
    [SerializeField] float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;
    [SerializeField] ParticleSystem _hit;
    [SerializeField] ParticleSystem _wallHit;
    [SerializeField] TrailRenderer _trail;

    [Header("Audio")]
    [SerializeField] AudioClip[] _initialImpact;
    [SerializeField] AudioClip[] _ricochets;
    [SerializeField] AudioClip _testHit;

    [Header("Componenets")]
    [SerializeField] AudioSource _source;
    [SerializeField] Rigidbody _rb;
    bool _freeze;
    float _squishTimer;
    float _speed;
    float _magnitude;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        spawn = transform.position;

        _speed = 1;
    }

    [ContextMenu("Reset Ball")]
    public void ResetBall() {
        transform.position = spawn;
    }


    void ProcessForce(Vector3 direction) {
        _source.PlayOneShot(_testHit);
        _hit.Play();

        SquishBall();

        _rb.velocity = Vector3.zero;

        Vector3 targetVelocity = direction * _speed;

        _rb.velocity = targetVelocity;

        _speed++;
    }

    Coroutine shootCoroutine;
    public void Shoot(Vector3 distance, bool player) {
        if (player) {
            GameManager.Get().StunFrames(0.3f);
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2f, true);
        }

        if (shootCoroutine != null) {
            StopCoroutine(shootCoroutine);
        }

        shootCoroutine = StartCoroutine(ShootProccess(distance));
    }

    Vector3 spawn;
    void Update() {
        _trail.emitting = _rb.velocity.magnitude > 40;

        if (Keyboard.current.rKey.wasPressedThisFrame) {
            _rb.velocity = Vector3.zero;
            //transform.position = _fighterDebug.transform.position + Vector3.right * 0.2f;
            //Shoot(new Vector3(1f, 12f));
            //Shoot(new Vector3(10f, 8f));
            Shoot(new Vector3(-12f, 4f), false);
            //Shoot(new Vector3(2f, 8f));
        }

        _magnitude = _rb.velocity.magnitude;

        Vector3 velocity = _rb.velocity;

        Vector3 scale = Vector3.one;
        scale.x = Mathf.Clamp(velocity.magnitude * 0.15f, 1, 3);

        //Negative Check
        if (velocity.magnitude < 0) {
            scale.x = Mathf.Clamp(scale.x, -1, -10);
        }

        transform.right = Vector3.Lerp(transform.right, velocity, Time.deltaTime * _smoothing);

        if (_squishTimer <= 0) {
            _ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
        }

        velocity.x = velocity.x * 0.9f;

        var tempVel = _rb.velocity;
        tempVel = Vector3.ClampMagnitude(tempVel, _maxSpeed);

        _rb.velocity = tempVel;

        if (_freeze) {
            _rb.velocity = Vector3.zero;
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

    void OnWallHit(ContactPoint point, float force) {
        if (force > 80) {
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2.5f, true);
        }
        else if (force > 50) {
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2f, true);
        }
        else if (force > 20) {
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 1.5f, true);
        }
        else {
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 0.5f, true);
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

    public float getSpeed() {
        return _magnitude;
    }

    private void OnCollisionEnter(Collision collision) {
        SquishBall();
        OnWallHit(collision.contacts[0], collision.relativeVelocity.magnitude);
    }

    IEnumerator ShootProccess(Vector3 distance) {
        _freeze = true;
        yield return new WaitForSeconds(0.3f);
        _freeze = false;
        ProcessForce(distance);
    }
}
