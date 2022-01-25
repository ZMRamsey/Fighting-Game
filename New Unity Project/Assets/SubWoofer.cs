using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWoofer : ShuttleCock
{
    [Header("Subwoofer Settings")]
    [SerializeField] float _subWooferThreshold;
    [SerializeField] ParticleSystem _explosion;
    float subWooferTimer;
    public override void UpdateShuttleApperance(Vector3 vel) {
        //transform.right = Vector3.Lerp(transform.right, vel, Time.deltaTime * _smoothing);

        if (_squishTimer <= 0) {
            //_ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
        }
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
        _explosion.Play();
        _explosion.transform.SetParent(null);
        Destroy(gameObject);
    }
}
