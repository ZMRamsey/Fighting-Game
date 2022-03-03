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

    void Start() {
        _shuttle = GameManager.Get().GetShuttle();
        _handler = GetComponent<InputHandler>();
        _controller = GetComponent<FighterController>();
    }

    void Update() {
        tick += Time.deltaTime;

       targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;

        if (tick > 0.05f) {
            _handler._inputX = 0;

            if (IsBallOnRight() && IsOnMySide()) {
                _handler._inputX = 1;
            }

            if (IsBallOnLeft() && IsOnMySide()) {
                _handler._inputX = -1;
            }


            _handler._jumpHeld = IsOnMySide() && transform.position.y < _shuttle.transform.position.y;

            _handler._jumpInput = IsOnMySide() && IsBallAbovePlayer();

            _handler._chargeInput = true;


            if (Vector3.Distance(transform.position, targetPosition) <  1f + (_shuttle.GetSpeedPercent() * _shuttle.GetVelocity().magnitude)) {
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
            tick = 0.0f;
        }
    }

    bool IsOnMySide() {
        if(_controller.GetFilter() == FighterFilter.one) {
            return targetPosition.x > 1;
        }
        else {
            return targetPosition.x < -1;
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
