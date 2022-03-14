using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShotType { chip, drive, lift, smash, special };
public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] FighterMove _currentMove;
    [SerializeField] GameObject _character;
    [SerializeField] LineRenderer _debugRenderer;
    ShuttleCock currentBall;

    public List<GameObject> cooldowns = new List<GameObject>();

    private void Awake() {
        _self = transform.root.GetComponent<FighterController>();
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.transform == _self.transform) {
            return;
        }
        if (!cooldowns.Contains(collision.gameObject)) {
            var ball = collision.transform.GetComponent<ShuttleCock>();
            if (ball != null) {
                currentBall = ball;
                float facing = 1;
                if (_character.GetComponent<SpriteRenderer>().flipX) {
                    facing = -1;
                }

                if (_self != ball.GetOwner()) {
                    GameManager.Get().IncreaseRally();
                }
                else {
                    GameManager.Get().SuccessiveHit();
                }

                _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position));

                InputHandler handler = _self.GetComponent<InputHandler>();


                float xFace = _currentMove.GetHitDirection().x * facing;
                Vector3 dir = _currentMove.GetHitDirection();
                dir.x = xFace;

                ball.SetBounciness(1);
                if (_currentMove.GetType() == ShotType.chip) {
                    ball.SetBounciness(0.2f);
                }

                float velInfY = 0;
                if (handler.GetJumpHeld()) {
                    velInfY = 1;
                }

                if (handler.GetCrouch()) {
                    velInfY = -1;
                }

                var extraVel = handler.GetInput();

                if(_currentMove.GetType() == ShotType.drive) {
                    extraVel = new Vector3(handler.GetInput().x, velInfY * 6, 0);
                }

                if (_currentMove.GetType() == ShotType.smash) {
                    extraVel = new Vector3(handler.GetInput().x, -Mathf.Abs(velInfY * 3), 0);
                }

                if (_currentMove.GetType() == ShotType.lift) {
                    extraVel = new Vector3(handler.GetInput().x * 4, 0, 0);
                }


                if (_currentMove.GetType() == ShotType.lift) {
                    extraVel = new Vector3(handler.GetInput().x * 3, velInfY * 3, 0);
                }

                var velInf = new VelocityInfluence(new Vector3(handler.GetInput().x, velInfY * 6, 0), _self.GetHitType());
                var hiMes = new HitMessage(dir, velInf, _currentMove.GetType() == ShotType.chip, _self.GetFilter());
                ball.Shoot(hiMes, _self);

                if (_debugRenderer) {
                    _debugRenderer.enabled = true;

                    if (_currentMove.GetType() == ShotType.chip) {
                        _debugRenderer.startColor = Color.blue;
                        _debugRenderer.endColor = Color.blue;
                    }

                    if (_currentMove.GetType() == ShotType.drive) {
                        _debugRenderer.startColor = Color.red;
                        _debugRenderer.endColor = Color.red;
                    }

                    if (_currentMove.GetType() == ShotType.smash) {
                        _debugRenderer.startColor = Color.red + Color.white;
                        _debugRenderer.endColor = Color.red + Color.white;
                    }

                    if (_currentMove.GetType() == ShotType.lift) {
                        _debugRenderer.startColor = Color.green;
                        _debugRenderer.endColor = Color.green;
                    }

                    Vector3[] arc = new Vector3[]{
                    collision.transform.position,
                    (collision.transform.position)
                    };

                    _debugRenderer.SetPositions(arc);
                }
            }

            cooldowns.Add(collision.gameObject);
        }
    }

    Vector3 lastVel;
    void Update() {
        if (_debugRenderer && _debugRenderer.enabled && currentBall != null) {
            InputHandler handler = _self.GetComponent<InputHandler>();
            var speed = currentBall.GetSpeed();

            if(currentBall.GetForce() <= 0.4f) {
                speed = 1;
            }

            lastVel = _debugRenderer.GetPosition(_debugRenderer.positionCount - 1);

            _debugRenderer.SetPosition(_debugRenderer.positionCount - 1, (lastVel + currentBall.GetComponent<Rigidbody>().velocity));
        }
    }

    void OnEnable() {
        _debugRenderer.enabled = false;
    }

    public void SetMove(FighterMove move) {
        _currentMove = move;
    }

    public void ResetCD() {
        cooldowns.Clear();
    }
}
