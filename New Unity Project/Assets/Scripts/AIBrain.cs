using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    InputHandler _handler;
    ShuttleCock _shuttle;
    FighterController _controller;
    float tick;
    Vector3 targetPosition;

    EsmeFighter _esme;
    RacketFighter _raket;

    void Start() {
        _shuttle = GameManager.Get().GetShuttle();
        _handler = GetComponent<InputHandler>();
        _controller = GetComponent<FighterController>();
        _esme = GetComponent<EsmeFighter>();
        _raket = GetComponent<RacketFighter>();
    }

    void Update() {
        tick += Time.deltaTime;

       targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;

        if (tick > 0.05f) {
            _handler._inputX = 0;
            bool moveAwayOverride = false;
            bool inRangeOfSubWoofer = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider hit in colliders) {
                SubWoofer woofer = hit.GetComponent<SubWoofer>();

                if(woofer != null) {
                    if(woofer.GetTimer() < 3 && IsOnMySide(woofer.transform) && !IsOnMySide()) {
                        if (woofer.transform.position.x < transform.position.x) {
                            _handler._inputX = -1;
                        }

                        if (woofer.transform.position.x > transform.position.x) {
                            _handler._inputX = 1;
                        }

                        inRangeOfSubWoofer = Vector3.Distance(transform.position, woofer.transform.position) < 1.5f;
                    }
                    else {
                        if (woofer.transform.position.x < transform.position.x) {
                            _handler._inputX = 1;
                        }

                        if (woofer.transform.position.x > transform.position.x) {
                            _handler._inputX = -1;
                        }
                    }

                    moveAwayOverride = true;
                }
            }

            if (!moveAwayOverride) {
                if (IsBallOnRight() && IsOnMySide()) {
                    _handler._inputX = 1;
                }

                if (IsBallOnLeft() && IsOnMySide()) {
                    _handler._inputX = -1;
                }
            }

            _handler._jumpHeld = IsOnMySide() && transform.position.y < _shuttle.transform.position.y;

            _handler._jumpInput = IsOnMySide() && IsBallAbovePlayer();

            _handler._chargeInput = true;

            if (_esme && _esme.CanGhostShot() && _controller.GetGrounded()) {
                _handler._crouchInput = true;

                if (Vector3.Distance(transform.position, targetPosition) > 4f) {
                    processHit();
                }
            }
            else {
                _handler._crouchInput = false;
            }

            if(_raket && _raket.GetMeter() > 0.5f) {
                int rndGimic = Random.Range(0, 250);
                if(rndGimic == 5) {
                    _handler._crouchInput = true;
                    _handler._chipInput = true;
                }
            }

            if (inRangeOfSubWoofer) {
                var rndDec = Random.Range(0, 3);
                if (rndDec == 0) {
                    _handler._smashInput = true;
                }
                else {
                    _handler._dropInput = true;
                }
            }


            if (Vector3.Distance(transform.position, targetPosition) <  1f + (_shuttle.GetSpeedPercent() * _shuttle.GetVelocity().magnitude)) {
                processHit();
            }
            tick = 0.0f;
        }
    }

    void processHit() {
        int rand = Random.Range(0, 3);
        int rndDec = 0;

        if (rand == 2 && _shuttle.transform.position.y > 1.2f && _shuttle.GetSpeedPercent() > 0.1f) {
            _handler._chipInput = true;
        }
        else {

            if (transform.position.y > 1.2f) {
                rndDec = Random.Range(0, 3);
                if (rndDec == 0) {
                    _handler._smashInput = true;
                }
                else if (rndDec == 1 && _controller.GetMeter() == 1) {
                    _handler._specialInput = true;
                }
                else {
                    _handler._dropInput = true;
                }

            }
            else {
                rndDec = Random.Range(0, 2);
                if (rndDec == 1) {
                    _handler._driveInput = true;
                }
                else {
                    _handler._dropInput = true;
                }
            }
        }
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
