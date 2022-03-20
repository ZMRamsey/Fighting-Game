using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkersFighter : FighterController
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _shootLifetime;
    GameObject _bulletObject;
    float _lifeTime;

    public override void FighterAwake() {
        base.FighterAwake();
        _bulletObject = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        _bulletObject.gameObject.SetActive(false);
    }

    public override void OnFighterUpdate() {
        base.OnFighterUpdate();

        if(_lifeTime > 0) {
            _lifeTime -= Time.deltaTime;
            if(_lifeTime <= 0) {
                _bulletObject.GetComponent<TrailRenderer>().emitting = false;
                _bulletObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    public override void UpdateMove() {
        base.UpdateMove();
        _bulletObject.SetActive(true);
        _bulletObject.transform.position = transform.position;

        var move = _currentMove.GetHitDirection();
        if(_filter == FighterFilter.one) {
            move.x *= -1;
        }

        move.Normalize();

        _bulletObject.GetComponent<TrailRenderer>().Clear();
        _bulletObject.GetComponent<TrailRenderer>().emitting = true;

        var proj = _bulletObject.GetComponent<Rigidbody>();
        proj.velocity = Vector3.zero;
        proj.AddForce(move * _shootSpeed, ForceMode.Impulse);

        _lifeTime = _shootLifetime;

        var dir = _currentMove.GetHitDirection();

        if (_filter == FighterFilter.one) {
            dir.x *= -1;
        }

        HitMessage message = new HitMessage(dir, null, false, GetFilter(), _currentMove.GetType());
        proj.GetComponent<BonkerBullet>().ResetBullet(message);
    }
}
