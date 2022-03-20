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
    bool grabbing;
    //bool hasBall;
    //ShuttleCock heldBall;

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
                currentBall = ball;

                float facing = 1;
                if (_character.GetComponent<SpriteRenderer>().flipX)
                {
                    facing = -1;
                }

                if (_self != ball.GetOwner())
                {
                    GameManager.Get().IncreaseRally();
                }
                else
                {
                    GameManager.Get().SuccessiveHit();
                }

                InputHandler handler = _self.GetComponent<InputHandler>();


                float xFace = _currentMove.GetHitDirection().x * facing;
                Vector3 dir = _currentMove.GetHitDirection();
                dir.x = xFace;


                ball.SetBounciness(1);
                if (_currentMove.GetType() == ShotType.chip)
                {
                    ball.SetBounciness(0.2f);
                }

                //ball.BoundToPlayer(_character);

                var velInf = new VelocityInfluence(handler.GetInput(), _self.GetHitType());
                var hiMes = new HitMessage(dir, velInf, _currentMove.GetType() == ShotType.chip, _self.GetFilter(), _currentMove.GetType());

                bool isGrab = false;

                if (_currentMove.GetType() == ShotType.chip && _self.GetComponent<InputHandler>().GetGrab() && _self.CanGrab() && !ball.IsGrabbed(_self))
                {
                    isGrab = true;
                    ball.BoundToPlayer(_self);
                    ball.SetOwner(_self.GetFilter());
                }
                else
                {
                    ball.ReleaseFromPlayer(false);
                    ball.Shoot(hiMes, _self);
                }

                _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position), dir, ball.CanKill(), _currentMove.GetType(), isGrab);

                UpdateDebug(collision);
            }

            cooldowns.Add(collision.gameObject);
        }
    }

    public void UpdateDebug(Collider collision)
    {
        if (_debugRenderer)
        {
            _debugRenderer.enabled = true;

            if (_currentMove.GetType() == ShotType.chip)
            {
                _debugRenderer.startColor = Color.blue;
                _debugRenderer.endColor = Color.blue;
            }

            if (_currentMove.GetType() == ShotType.drive)
            {
                _debugRenderer.startColor = Color.red;
                _debugRenderer.endColor = Color.red;
            }

            if (_currentMove.GetType() == ShotType.smash)
            {
                _debugRenderer.startColor = Color.red + Color.white;
                _debugRenderer.endColor = Color.red + Color.white;
            }

            if (_currentMove.GetType() == ShotType.lift)
            {
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

    Vector3 lastVel;
    void Update()
    {
        if (_debugRenderer && _debugRenderer.enabled && currentBall != null)
        {
            InputHandler handler = _self.GetComponent<InputHandler>();
            var speed = currentBall.GetSpeed();

            //if(currentBall.GetChargeTime() <= 0.1) {
            //    speed = 1;
            //}

            lastVel = _debugRenderer.GetPosition(_debugRenderer.positionCount - 1);

            _debugRenderer.SetPosition(_debugRenderer.positionCount - 1, (lastVel + currentBall.GetComponent<Rigidbody>().velocity));
        }
    }

    void OnEnable()
    {
        _debugRenderer.enabled = false;
    }

    public void SetMove(FighterMove move)
    {
        _currentMove = move;
    }

    //public void SetGrab(bool grab)
    //{
    //    grabbing = grab;

    //    if ((!grabbing) && (heldBall != null))
    //    {
    //        hasBall = false;

    //        InputHandler handler = _self.GetComponent<InputHandler>();

    //        float facing = 1;
    //        if (_character.GetComponent<SpriteRenderer>().flipX)
    //        {
    //            facing = -1;
    //        }

    //        float xFace = _currentMove.GetHitDirection().x * facing;
    //        Vector3 dir = _currentMove.GetHitDirection();
    //        dir.x = xFace;
    //        heldBall.SetBounciness(0.2f);
    //        heldBall.BoundToPlayer(_character, false);

    //        var velInf = new VelocityInfluence(handler.GetInput(), _self.GetHitType());
    //        var hiMes = new HitMessage(dir, velInf, _currentMove.GetType() == ShotType.chip, _self.GetFilter());
    //        heldBall.Shoot(hiMes, _self);

    //        Debug.Log("Ball launched");
    //    }
    //    else
    //    {
    //        heldBall = null;
    //    }
    //}

    //public bool HasShuttle()
    //{
    //    return hasBall;
    //}

    public void ResetCD()
    {
        cooldowns.Clear();
    }
}
