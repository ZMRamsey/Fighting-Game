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
    bool grabbing;
    bool hasBall;
    ShuttleCock heldBall;

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

                //if (_self.GetFilter() != GameManager.Get().GetLastHit())
                //Debug.Log("Current Owner: " + ball.GetOwner());
                if (_self != ball.GetOwner())
                {
                    GameManager.Get().IncreaseRally();
                    //GameManager.Get().SetLastHitter(_self.GetFilter());
                }
                else
                {
                    GameManager.Get().SuccessiveHit();
                }

                _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position));

                InputHandler handler = _self.GetComponent<InputHandler>();
                

                float xFace = _currentMove.GetHitDirection().x * facing;
                Vector3 dir = _currentMove.GetHitDirection();
                dir.x = xFace;

                if (!grabbing)
                {

                    ball.SetBounciness(1);
                    if (_currentMove.GetType() == ShotType.chip)
                    {
                        ball.SetBounciness(0.2f);
                    }

                    ball.followPlayer(_character, false);

                    ball.Shoot(dir, handler.GetInput(), true, _currentMove.GetType() == ShotType.chip, _self.GetFilter(), _self);
                }
                else
                {
                    ball.followPlayer(_character, true);
                    hasBall = true;
                    heldBall = ball;
                    //fuccy wuccy
                }
                
            }
            cooldowns.Add(collision.gameObject);
        }
    }

    public void SetMove(FighterMove move) {
        _currentMove = move;
    }

    public void SetGrab(bool grab)
    {
        grabbing = grab;

        if ((!grabbing) && (heldBall != null))
        {
            hasBall = false;

            InputHandler handler = _self.GetComponent<InputHandler>();

            float facing = 1;
            if (_character.GetComponent<SpriteRenderer>().flipX)
            {
                facing = -1;
            }

            float xFace = _currentMove.GetHitDirection().x * facing;
            Vector3 dir = _currentMove.GetHitDirection();
            dir.x = xFace;
            heldBall.SetBounciness(0.2f);
            heldBall.followPlayer(_character, false);

            heldBall.Shoot(dir, handler.GetInput(), true, _currentMove.GetType() == ShotType.chip, _self.GetFilter(), _self);

            Debug.Log("Ball launched");
        }
        else
        {
            heldBall = null;
        }
    }

    public bool HasShuttle()
    {
        return hasBall;
    }

    public void ResetCD()
    {
        cooldowns.Clear();
    }
}
