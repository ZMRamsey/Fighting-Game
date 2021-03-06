using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWoofer : ShuttleCock
{
    [Header("Subwoofer Settings")]
    [SerializeField] float _subWooferThreshold;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] ParticleSystem _beepRadius;
    [SerializeField] AudioClip _explosionSND, _beepSND;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] AnimationCurve _beepCurve;
    float subWooferTimer;
    float _timeBetweenBeeps;
    bool _isActive;

    void Start() {
        //ResetShuttle(false);
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
        if (IsFrozen() || GameManager.Get().IsSpecialScreen()) {
            return;
        }

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
            _beepRadius.Play();
            _timeBetweenBeeps = 0;
        }

        if (subWooferTimer > _subWooferThreshold) {
            Explode();
        }
    }

    public float GetTimer(){
        return subWooferTimer;
    }

    void Explode() {
        Collider[] collidersPlayers = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider hit in collidersPlayers) {
            FighterController controller = hit.GetComponent<FighterController>();

            //We need to decide who dies in the event of double explosion KO
            if(controller != null && controller.HurtBoxActive() && GameManager.Get().KOCoroutine == null && GameManager.Get().EndGameCoroutine == null && !controller.GetComponent<TekaFighter>()) {
                Vector3 direction = controller.transform.position - transform.position;
                direction.Normalize();

                GameManager.Get().GetCameraShaker().SetShake(0.1f, 5.0f, true);
                controller.KO(direction * 20);
                ScoreManager.Get().UpdateScore(controller.GetFilter().ToString(), "KO");
                GameManager.Get().KOEvent(controller.GetFilter());
            }
        }

        Collider[] collidersShuttle = Physics.OverlapSphere(transform.position, 12f);
        foreach (Collider hit in collidersShuttle) {
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();
            MrHandy handy = hit.GetComponent<MrHandy>();

            if (shuttle != null && !shuttle.GetComponent<SubWoofer>()) {
                Vector3 direction = shuttle.transform.position - transform.position;
                direction.Normalize();

                shuttle.GetComponent<Rigidbody>().velocity = direction * 30;
                shuttle.SetOwner(FighterFilter.both);
            }

            if(handy != null) {
                handy.transform.eulerAngles = new Vector3(180, 0, 0);
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
