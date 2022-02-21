using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWoofer : ShuttleCock
{
    [Header("Subwoofer Settings")]
    [SerializeField] float _subWooferThreshold;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _explosionSND;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] SpriteRenderer _renderer;
    float _timeBetweenBeeps;
    float subWooferTimer;
    bool _isActive;
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

    public override void ResetShuttle() {
        _isActive = true;
        subWooferTimer = 0.0f;
        base.ResetShuttle();
    }

    public override void Shoot(Vector3 distance, Vector3 movementInfluence, bool player, bool slowDown, FighterFilter filter) {
        base.Shoot(distance, movementInfluence, player, slowDown, filter);
        _rb.AddTorque(distance, ForceMode.Impulse);
    }

    bool _tick;
    public override void ShuttleUpdate() {
        subWooferTimer += Time.deltaTime;
        _timeBetweenBeeps += Time.deltaTime;

        if (_timeBetweenBeeps > 0.25f) {
            _tick = !_tick;

            if (_tick) {
                _renderer.sprite = _sprites[0];
            }
            else {
                _renderer.sprite = _sprites[1];
            }
            _timeBetweenBeeps = 0;
        }

        if (subWooferTimer > _subWooferThreshold) {
            Explode();
        }
    }

    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100);
        foreach (Collider hit in colliders) {
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();

            if (shuttle != null) {
                shuttle.Reverse();
            }
        }

        _isActive = false;
        _explosion.Play();
        _explosion.transform.SetParent(null);
        _explosion.transform.position = transform.position;
        GetComponent<AudioSource>().PlayOneShot(_explosionSND, 1.5f);
        GameManager.Get().GetCameraShaker().SetShake(1.1f, 4, false);
        ResetShuttle();
        gameObject.SetActive(false);
    }
}
