using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWoofer : ShuttleCock
{
    [Header("Subwoofer Settings")]
    [SerializeField] float _subWooferThreshold;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _explosionSND;
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

    public override void Shoot(Vector3 distance, bool player, bool slowDown, FighterFilter filter) {
        base.Shoot(distance, player, slowDown, filter);
        _rb.AddTorque(distance, ForceMode.Impulse);
    }

    public override void ShuttleUpdate() {
        subWooferTimer += Time.deltaTime;
        if(subWooferTimer > _subWooferThreshold) {
            Explode();
        }
    }

    void Explode() {
        _isActive = false;
        _explosion.Play();
        _explosion.transform.SetParent(null);
        _explosion.transform.position = transform.position;
        _source.PlayOneShot(_explosionSND);
        GameManager.Get().GetCameraShaker().SetShake(1.1f, 4, false);
        ResetShuttle();
        gameObject.SetActive(false);
    }
}
