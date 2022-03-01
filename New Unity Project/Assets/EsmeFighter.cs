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

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _shieldTimer = 0.0f;
        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetEsmetSuper(), _filter);

        _netObject.SetActive(true);

    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _netObject = Instantiate(_netPrefab, new Vector3(0, 6.39f, -0.1f), Quaternion.identity);
        _magicRaketObject = Instantiate(_magicRaketPrefab, Vector3.zero, Quaternion.identity);
        _netObject.SetActive(false);
        _magicRaketObject.SetActive(false);
    }

    public override void OnSuccessfulHit(Vector3 point) {
        base.OnSuccessfulHit(point);
        _ghostHitUsed = false;
    }

    public override void OnFighterUpdate() {
        base.OnFighterUpdate();

        _magicRaketObject.SetActive(!_ghostHitUsed && GameManager.Get().GetShuttle().GetFilter() == GetFilter());
        _magicRaketObject.transform.position = GameManager.Get().GetShuttle().transform.position;

        if (_netObject) {
            _shieldTimer += Time.deltaTime;
            if (_shieldTimer > 5.0f) {
                _netObject.SetActive(false);
                _shieldTimer = 0.0f;
            }
        }
    }

    public override void UpdateMove() {
        base.UpdateMove();

        if (_inputHandler.GetCrouch() && !_ghostHitUsed && GameManager.Get().GetShuttle().GetFilter() == GetFilter()) {
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

            ball.Shoot(dir, Vector3.zero, true, _currentMove.GetType() == ShotType.chip, GetFilter(), this);

            OnSuccessfulHit(ball.transform.position);
            _ghostHitUsed = true;
        }
    }
}
