using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] LayerMask _netDetection;
    InputHandler _handler;
    ShuttleCock _shuttle;
    FighterController _controller;
    float tick;
    Vector3 targetPosition;

    FighterController _enemy;

    EsmeFighter _esme;
    RacketFighter _raket;
    RayAndTekaFighter _ray;

    void Start() {
        _netDetection = LayerMask.GetMask("Net");
        _shuttle = GameManager.Get().GetShuttle();
        _handler = GetComponent<InputHandler>();
        _controller = GetComponent<FighterController>();
        _esme = GetComponent<EsmeFighter>();
        _raket = GetComponent<RacketFighter>();
        _ray = GetComponent<RayAndTekaFighter>();
        _enemy = GameManager.Get().GetFighterOne();
        if(_controller == _enemy) {
            _enemy = GameManager.Get().GetFighterTwo();
        }
    }

    void Update() {
        tick += Time.deltaTime;

       targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;
        targetPosition -= _shuttle.GetVelocity().normalized * 2;

        if (tick > 0.05f) {
            _handler._inputX = 0;
            bool moveAwayOverride = false;
            bool inRangeOfSubWoofer = false;
            bool inHittingRangeOfSubWoofer = false;
            bool isWooferOnMySide = false;
            bool isTimeToMove = false;
            RaycastHit netDetection;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 4);
            foreach (Collider hit in colliders) {
                SubWoofer woofer = hit.GetComponent<SubWoofer>();


                if(woofer != null) {
                    if (IsOnMySide(woofer.transform)) {
                        isWooferOnMySide = true;
                    }

                    isTimeToMove = woofer.GetTimer() > 3.5f;

                    inRangeOfSubWoofer = Vector3.Distance(transform.position, woofer.transform.position) < 4f;
                    inHittingRangeOfSubWoofer = Vector3.Distance(transform.position, woofer.transform.position) < 1f;
                    if (!isTimeToMove && IsOnMySide(woofer.transform) && !IsOnMySide()) {
                        if (woofer.transform.position.x < transform.position.x) {
                            _handler._inputX = -1;
                        }

                        if (woofer.transform.position.x > transform.position.x) {
                            _handler._inputX = 1;
                        }
                    }
                    else { 
                        if (inRangeOfSubWoofer || IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.one) {
                            _handler._inputX = -1;
                            if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 2, _netDetection))
                            {
                                _handler._jumpHeld = true;
                                _handler._jumpInput = true;
                            }
                        }

                        if (inRangeOfSubWoofer || !IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.one) {
                            _handler._inputX = 1;
                            if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out netDetection, 2, _netDetection)) {
                                _handler._jumpHeld = true;
                                _handler._jumpInput = true;
                            }
                        }

                        if (inRangeOfSubWoofer || IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.two) {
                            _handler._inputX = 1;
                            if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out netDetection, 2, _netDetection))
                            {
                                _handler._jumpHeld = true;
                                _handler._jumpInput = true;
                            }
                        }

                        if (inRangeOfSubWoofer || !IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.two) {
                            _handler._inputX = -1;
                            if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 2, _netDetection)) {
                                _handler._jumpHeld = true;
                                _handler._jumpInput = true;
                            }
                        }
                    }

                    moveAwayOverride = true;
                }
            }


            if (!moveAwayOverride) {
                _handler._jumpHeld = (IsOnMySide() || HeadingMyDirection()) && transform.position.y < _shuttle.transform.position.y;
                _handler._jumpInput = (IsOnMySide() || HeadingMyDirection()) && IsBallAbovePlayer();

                if (IsOnMySide() || HeadingMyDirection() || _controller.InSuper()) {
                    if (IsBallOnRight()) {
                        if (!Physics.Raycast(transform.position, new Vector3(1, 0, 0), out netDetection, 0.5f, _netDetection)) {
                            _handler._inputX = 1;
                        }
                    }

                    if (IsBallOnLeft()) {
                        if (!Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 0.5f, _netDetection)) {
                            _handler._inputX = -1;
                        }
                    }
                }

                if(!IsOnMySide() && !IsOnMySide(transform) && !isWooferOnMySide && Vector3.Distance(transform.position, _shuttle.transform.position) > 2)
                {

                    if (_controller.GetFilter() == FighterFilter.one)
                    {
                        _handler._inputX = 1;
                        if (Physics.Raycast(transform.position, new Vector3(1,0,0), out netDetection, 2, _netDetection))
                        {
                            _handler._jumpHeld = true;
                            _handler._jumpInput = true;
                        }
                    }
                    else
                    {
                        _handler._inputX = -1;
                        if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 2, _netDetection))
                        {
                            _handler._jumpHeld = true;
                            _handler._jumpInput = true;
                        }
                    }
                }
            }

            if (_esme != null) {
                _handler._crouchInput = false;
                if (_esme.GetComponent<Rigidbody>().velocity.y < 0 && IsBallAbovePlayer()) {
                    _handler._jumpExtraInput = true;
                }

                if (_esme.CanGhostShot() && _controller.GetGrounded() && Vector3.Distance(transform.position, targetPosition) > 4f) {
                    _handler._crouchInput = true;
                    ProcessHit();
                }

                if (!IsOnMySide() && _esme.GetMeter() >= 1) {
                    _handler._specialInput = true;
                }
            }

            if(_raket != null) {
                _handler._crouchInput = false;
                if (_raket.GetBuildMeter() < 1f && !IsOnMySide() && !HeadingMyDirection() && !isTimeToMove && !inRangeOfSubWoofer) {
                    _handler._crouchInput = true;
                }

                if (_raket.GetBuildMeter() >= 1) {
                    _handler._crouchInput = true;
                    _handler._chipInput = true;
                }

                if(!IsOnMySide() && _raket.GetMeter() >= 1) {
                    _handler._jumpHeld = true;
                    _handler._jumpInput = true;
                    if(transform.position.y > 6) {
                        _handler._specialInput = true;
                    }
                }
            }

            if (_ray != null)
            {
                _handler._specialInput = false;
                if (_shuttle.transform.position.y < transform.position.y)
                {
                    _handler._crouchInput = true;
                }
                else
                {
                    _handler._crouchInput = false;

                }

                if (IsOnMySide() && _ray.GetMeter() >= 1 && ((_shuttle.GetComponent<Rigidbody>().velocity.y > 6 && HeadingMyDirection()) || Vector3.Distance(_shuttle.transform.position, _ray.GetRayPos()) < 3)) {
                    _handler._specialInput = true;
                }
            }

            if ((!isTimeToMove && inHittingRangeOfSubWoofer) || Vector3.Distance(transform.position, targetPosition) <  1f || Vector3.Distance(transform.position, _shuttle.transform.position) < 1f) {
                ProcessHit();
            }

            tick = 0.0f;
        }
    }

    void ProcessHit() {
        int chipShot = Random.Range(0, 5);
        bool _heavy = true;

        if (chipShot == 2 && _shuttle.transform.position.y > 1.2f && _shuttle.GetSpeedPercent() > 0.1f) {
            _handler._chipInput = true;
        }
        else {
            var enemyAngle = (transform.position - _enemy.transform.position).normalized;

            if(_ray != null) {
                enemyAngle = (transform.position - _ray.GetRayPos()).normalized;
            }

            var shootDirection = _controller.GetLiftMove().GetHitDirection().normalized;

            if(_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            var testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            var currentAngle = testingAngle;
            var angle = ShotType.drop;

            shootDirection = _controller.GetSmashMove().GetHitDirection().normalized;

            if (_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            if (testingAngle > currentAngle) {
                currentAngle = testingAngle;
                angle = ShotType.smash;
            }

            shootDirection = _controller.GetDriveMove().GetHitDirection().normalized;

            if (_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            if (testingAngle > currentAngle) {
                currentAngle = testingAngle;
                angle = ShotType.drive;
            }

            if(angle == ShotType.drive) {
                if (Physics.Raycast(transform.position, shootDirection, _netDetection)) {
                    _heavy = false;
                    angle = ShotType.drop;
                }
            }

            _handler._chargeInput = _heavy;

            if(angle == ShotType.drop) {
                _handler._dropInput = true;
            }
            if (angle == ShotType.smash) {
                _handler._smashInput = true;
            }
            if (angle == ShotType.drive) {
                _handler._driveInput = true;
            }
            if (angle == ShotType.chip) {
                _handler._chipInput = true;
            }
        }
    }

    bool HeadingMyDirection() {
        if(_controller.GetFilter() == FighterFilter.one) {
            return _shuttle.GetComponent<Rigidbody>().velocity.x > 0;
        }
        return _shuttle.GetComponent<Rigidbody>().velocity.x < 0;
    }

    bool IsOnMySide() {
        if(_controller.GetFilter() == FighterFilter.one) {
            return targetPosition.x > 0.1f;
        }
        else {
            return targetPosition.x < -0.1f;
        }
    }

    bool IsOnMySide(Transform transform) {
        if (_controller.GetFilter() == FighterFilter.one) {
            return transform.position.x > 0.1f;
        }
        else {
            return transform.position.x < -0.1f;
        }
    }

    bool IsBallOnLeft() {
        return transform.position.x > targetPosition.x;
    }

    bool IsBallOnRight() {
        return transform.position.x < targetPosition.x;
    }

    bool IsBallAbovePlayer() {
        return _shuttle.transform.position.y > 3;
    }

    bool InHittingRange() {
        if(Vector3.Distance(transform.position, targetPosition) < 1f + (_shuttle.GetSpeedPercent() * _shuttle.GetVelocity().magnitude)) {
            return true;
        }

        return Vector3.Distance(transform.position, _shuttle.transform.position) < 2f;
    }
}
