using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWoofer : ShuttleCock
{
    [Header("Subwoofer Settings")]
    [SerializeField] float _subWooferThreshold;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _explosionSND, _beepSND;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] AnimationCurve _beepCurve;
    float subWooferTimer;
    float _timeBetweenBeeps;
    bool _isActive;

    void Start() {
        ResetShuttle(false);
    }

    public override void UpdateShuttleApperance(Vector3 vel) {
        //transform.right = Vector3.Lerp(transform.right, vel, Time.deltaTime * _smoothing);
        if (!_isActive) {
            return;
        }

        if (_squishTimer <= 0) {
            //_ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
        }
    }

    public override void ResetShuttle(bool freeze) {
        base.ResetShuttle(false);
        _isActive = true;
        subWooferTimer = 0.0f;
        //_rb.velocity = Vector3.zero;
    }

    public override void Shoot(HitMessage message) {
        base.Shoot(message);
        _rb.AddTorque(message.direction, ForceMode.Impulse);
    }

    bool _tick;
    public override void ShuttleUpdate() {
        subWooferTimer += Time.deltaTime;
        _timeBetweenBeeps += Time.deltaTime;
        var thres = _beepCurve.Evaluate(subWooferTimer / _subWooferThreshold);

        if (_timeBetweenBeeps > thres) {
            _tick = !_tick;

            if (_tick) {
                _renderer.sprite = _sprites[0];
            }
            else {
                _renderer.sprite = _sprites[1];
            }

            _source.PlayOneShot(_beepSND);
            _timeBetweenBeeps = 0;
        }

        if (subWooferTimer > _subWooferThreshold) {
            Explode();
        }
    }

    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
        foreach (Collider hit in colliders) {
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();

            if (shuttle != null && !shuttle.GetComponent<SubWoofer>()) {
                Vector3 direction = shuttle.transform.position - transform.position;
                direction.Normalize();

                //shuttle.ProcessForce(direction * 100, Vector3.zero, 1, false);
                shuttle.GetComponent<Rigidbody>().velocity = direction * 20;
                shuttle.SetOwner(FighterFilter.both);
            }
        }

        _isActive = false;
        _explosion.Play();
        _explosion.transform.SetParent(null);
        _explosion.transform.position = transform.position;
        _explosion.GetComponent<AudioSource>().PlayOneShot(_explosionSND, 1.5f);
        GameManager.Get().GetCameraShaker().SetShake(1.1f, 4, false);
        ResetShuttle(false);
        gameObject.SetActive(false);
    }
}
