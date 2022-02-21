using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShotType { chip, drive, drop, smash, special};
public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] FighterMove _currentMove;
    [SerializeField] GameObject _character;

    public List<GameObject> cooldowns = new List<GameObject>();

    private void Awake()
    {
        _self = transform.root.GetComponent<FighterController>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform == _self.transform)
        {
            return;
        }
        if (!cooldowns.Contains(collision.gameObject))
        {
            var ball = collision.transform.GetComponent<ShuttleCock>();
            if (ball != null)
            {
                float facing = 1;
                if (_character.GetComponent<SpriteRenderer>().flipX)
                {
                    facing = -1;
                }

                _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position));

                InputHandler handler = _self.GetComponent<InputHandler>();

                float xFace = _currentMove.GetHitDirection().x * facing;
                Vector3 dir = _currentMove.GetHitDirection();
                dir.x = xFace;

                if(_currentMove.GetType() == ShotType.smash && _self.IsGrounded())
                {
                    dir.y = 0.5f;
                }

                ball.Shoot(dir, handler.GetInput(), true, _currentMove.GetType() == ShotType.chip, _self.GetFilter(), _self);

                //Play shot type depending on button pressed
                //switch (_shotType)
                //{
                //    case ShotType.chip:
                //        ball.JailSpeed();
                //        ball.Shoot(new Vector3(1f * facing, 2f), handler.GetInput(), true, true, _self.GetFilter(), _self);
                //        break;

                //    case ShotType.drive:
                //        ball.Shoot(new Vector3(12f * facing, 3f), handler.GetInput(), true, false, _self.GetFilter(), _self);
                //        ball.UnJailSpeed();
                //        break;

                //    case ShotType.drop:
                //        ball.Shoot(new Vector3(6f * facing, 6f), handler.GetInput(), true, false, _self.GetFilter(), _self);
                //        ball.UnJailSpeed();
                //        break;

                //    case ShotType.smash:
                //        if (!_self.IsGrounded()) {
                //            y = -2f;
                //        }
                //        ball.Shoot(new Vector3(16f * facing, y), handler.GetInput(), true, false, _self.GetFilter(), _self);
                //        ball.UnJailSpeed();
                //        break;

                //    default:
                //        Debug.Log("Fuccy Wuccy Has Occurred");
                //        break;
                //}
            }
            if (_self.GetFilter() != GameManager.Get().GetLastHit())
            {
                GameManager.Get().IncreaseRally();
                GameManager.Get().SetLastHitter(_self.GetFilter());
                //Debug.Log("Rally:" + GameManager.Get()._rally + " - Last Hit By:" + GameManager.Get().GetLastHit().ToString());
            }
            else
            {
                //GameManager.Get().SuccessiveHit();
            }
            cooldowns.Add(collision.gameObject);
        }
    }

    public void SetMove(FighterMove move) {
        _currentMove = move;
    }

    public void ResetCD()
    {
        cooldowns.Clear();
    }
}
