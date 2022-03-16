using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsmeFighter : FighterController
{
    [SerializeField] GameObject _netPrefab;
    [SerializeField] GameObject _magicRaketPrefab;
    GameObject _netObject;
    GameObject _magicRaketObject;
    bool _ghostHitUsed;
    float _shieldTimer;
    float _coolDown;

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _shieldTimer = 0.0f;

        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetEsmetSuper(), _filter, this);

        _netObject.SetActive(true);
        //_timeStop = true;
        //_timeStopTimer = 0.0f;
        //FighterFilter filter = FighterFilter.one;
        //if(GetFilter() == filter) {
        //    filter = FighterFilter.two;
        //}

        //GameManager.Get().StunFrames(6.0f, filter);
    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _netObject = Instantiate(_netPrefab, new Vector3(0, 6.39f, -0.1f), Quaternion.identity);
        _magicRaketObject = Instantiate(_magicRaketPrefab, Vector3.zero, Quaternion.identity);
        _netObject.SetActive(false);
        _magicRaketObject.SetActive(false);
    }

    public override void OnSuccessfulHit(Vector3 point, bool big) {
        base.OnSuccessfulHit(point, big);
        _ghostHitUsed = false;
    }

    public override void OnSuperEnd(bool instant) {
        if (instant) {

        }

        if (_netObject) {
            _netObject.SetActive(false);
        }
        _shieldTimer = 0.0f;
    }


    public override void OnFighterUpdate() {
        base.OnFighterUpdate();

        var powered = !_ghostHitUsed && GameManager.Get().GetShuttle().GetFilter() == GetFilter() && _myState == FighterState.inControl && _coolDown > 0.5f;

        _animator.SetBool("charge", _inputHandler.GetCrouch());
        _animator.SetBool("power", powered);

        _magicRaketObject.SetActive(powered);
        _magicRaketObject.transform.position = GameManager.Get().GetShuttle().transform.position;

        _coolDown += Time.deltaTime;

        if (_netObject) {
            _shieldTimer += Time.deltaTime;
            if (_shieldTimer > 8.0f) {
                OnSuperEnd(false);
            }
        }

        //if (_timeStop) {
        //    _timeStopTimer += Time.deltaTime;
        //    GameManager.Get().GetShuttle().ForceFreeze();
        //    if (_timeStopTimer > 6.0f) {
        //        GameManager.Get().GetShuttle().ForceUnfreeze();
        //        _netObject.SetActive(false);
        //        _timeStop = false;
        //        _timeStopTimer = 0.0f;
        //    }
        //}
    }

    public override void UpdateMove() {
        base.UpdateMove();
         
        if (_inputHandler.GetCrouch() && !_ghostHitUsed && GameManager.Get().GetShuttle().GetFilter() == GetFilter() && _myState == FighterState.inControl && _coolDown > 0.5f) {
            var ball = GameManager.Get().GetShuttle();

            float facing = 1;
            if (_renderer.flipX) {
                facing = -1;
            }

            float xFace = _currentMove.GetHitDirection().x * facing;
            Vector3 dir = _currentMove.GetHitDirection();
            dir.x = xFace;

            ball.SetBounciness(1);
            if (_currentMove.GetType() == ShotType.chip) {
                ball.SetBounciness(0.2f);
            }

            var hitMes = new HitMessage(dir, new VelocityInfluence(), _currentMove.GetType() == ShotType.chip, GetFilter());
            ball.Shoot(hitMes, this);

            OnSuccessfulHit(ball.transform.position, false);
            _ghostHitUsed = true;
            _animator.SetTrigger("gimicHit");
        }
        _coolDown = 0;
    }

    public bool CanGhostShot() {
        return !_ghostHitUsed;
    }
}
