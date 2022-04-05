using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsmeFighter : FighterController
{
    [SerializeField] GameObject _blachHolePrefab;
    [SerializeField] GameObject _magicRaketPrefab;
    GameObject _blackHoleObject;
    GameObject _magicRaketObject;
    [SerializeField] AnimationClip _clap;
    ShuttleCock _ghostHitUsed;
    float _shieldTimer;
    float _coolDown;

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _shieldTimer = 0.0f;

        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetEsmetSuper(), _filter, this);

        _blackHoleObject.transform.position = transform.position;
        _blackHoleObject.SetActive(true);
    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _blackHoleObject = Instantiate(_blachHolePrefab, new Vector3(0, 6.39f, -0.1f), Quaternion.identity);
        _magicRaketObject = Instantiate(_magicRaketPrefab, Vector3.zero, Quaternion.identity);
        _blackHoleObject.SetActive(false);
        _magicRaketObject.SetActive(false);
    }

    public override void OnSuccessfulHit(Vector3 point, Vector3 dir, bool big, ShotType shot, bool isGrab, ShuttleCock shuttle) {
        base.OnSuccessfulHit(point, dir, big, shot, isGrab, shuttle);
        _ghostHitUsed = shuttle;
    }

    public override void OnSuperEnd(bool instant) {
        if (instant) {

        }

        if (_blackHoleObject) {
            _blackHoleObject.SetActive(false);
        }
        _shieldTimer = 0.0f;
    }


    public override void OnFighterUpdate() {
        base.OnFighterUpdate();

        var powered = _ghostHitUsed != null && _ghostHitUsed.GetFilter() == GetFilter() && _myState == FighterState.inControl && _coolDown > 0.5f;

        _animator.SetBool("charge", _inputHandler.GetCrouch());
        _animator.SetBool("power", powered);

        _magicRaketObject.SetActive(powered);

        if (powered) {
            _magicRaketObject.transform.position = _ghostHitUsed.transform.position;
        }

        _coolDown += Time.deltaTime;

        if (_blackHoleObject) {
            _shieldTimer += Time.deltaTime;
            if (_shieldTimer > 10.0f) {
                OnSuperEnd(false);
            }
        }
    }

    public override void UpdateMove() {
        base.UpdateMove();
         
        if (_inputHandler.GetCrouch() && _ghostHitUsed != null && _ghostHitUsed.GetFilter() == GetFilter() && _myState == FighterState.inControl && _coolDown > 0.5f) {
            float facing = 1;
            if (_renderer.flipX) {
                facing = -1;
            }

            float xFace = _currentMove.GetHitDirection().x * facing;
            Vector3 dir = _currentMove.GetHitDirection();
            dir.x = xFace;

            _ghostHitUsed.SetBounciness(1);
            if (_currentMove.GetType() == ShotType.chip) {
                _ghostHitUsed.SetBounciness(0.2f);
            }

            var hitMes = new HitMessage(dir, new VelocityInfluence(), _currentMove.GetType() == ShotType.chip, GetFilter(), _currentMove.GetType());
            _ghostHitUsed.Shoot(hitMes, this);

            OnSuccessfulHit(_ghostHitUsed.transform.position, dir, false, _currentMove.GetType(), false, null);
            _ghostHitUsed = null;

            _canAttack = false;
            _animator.SetTrigger("gimicHit");
            _failSafeAttack = _clap.length;
        }
        _coolDown = 0;
    }

    public bool CanGhostShot() {
        return !_ghostHitUsed;
    }
}
