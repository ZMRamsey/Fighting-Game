using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    InputHandler _handler;
    [SerializeField] ShuttleCock _shuttle;
    float tick;

    void Start() {
        _handler = GetComponent<InputHandler>();
    }

    void Update() {
        //if(_handler.GetState() == InputState.ai && _shuttle.IsBallActive()) {
        //    print("Test");
        //    Vector3 targetPosition = _shuttle.transform.position + _shuttle.GetVelocity();
        //    _debug.position = targetPosition;
        //}

        tick += Time.deltaTime;

        Vector3 targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;

        if (tick > 0.05f) {
            _handler._inputX = 0;

            if (transform.position.x < targetPosition.x && targetPosition.x < -1) {
                _handler._inputX = 1;
            }

            if (transform.position.x > targetPosition.x || transform.position.x > -1) {
                _handler._inputX = -1;
            }


            _handler._jumpHeld = transform.position.y < _shuttle.transform.position.y;

            if (_shuttle.GetSpeedPercent() != 0) {
                if ((_shuttle.transform.position.y < targetPosition.y && _shuttle.transform.position.y > 1.2f) || _shuttle.transform.position.y > 6.8f) {
                    _handler._jumpInput = true;
                }
                else {
                    _handler._jumpInput = false;
                }
            }


            if (Vector3.Distance(transform.position, targetPosition) <  5 + (_shuttle.GetVelocity().magnitude / 4)) {
                int rand = Random.Range(0, 3);
                int rndDec = 0;

                if (rand == 2 && _shuttle.transform.position.y > 1.2f && _shuttle.GetSpeedPercent() > 0.1f) {
                    _handler._chipInput = true;
                }
                else {
                    if (transform.position.y > 1.2f) {
                        rndDec = Random.Range(0, 2);
                        if (rndDec == 1) {
                            _handler._smashInput = true;
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
}
