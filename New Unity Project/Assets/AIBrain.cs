using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    InputHandler _handler;
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Transform _debug;
    float tick;

    void Start() {
        _handler = GetComponent<InputHandler>();    
    }

    void Update()
    {
        //if(_handler.GetState() == InputState.ai && _shuttle.IsBallActive()) {
        //    print("Test");
        //    Vector3 targetPosition = _shuttle.transform.position + _shuttle.GetVelocity();
        //    _debug.position = targetPosition;
        //}

        tick += Time.deltaTime;

        Vector3 targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;

        if (tick > 0.05f) {

            if (transform.position.x < targetPosition.x && transform.position.x < -1) {
                _handler._inputX = 1;
            }

            if (transform.position.x > targetPosition.x || transform.position.x > -1) {
                _handler._inputX = -1;
            }

            if ((_shuttle.GetVelocity().y > 0 && _shuttle.transform.position.y > transform.position.y) || _shuttle.transform.position.y > 6.8f) {
                _handler._jumpHeld = transform.position.y < _shuttle.transform.position.y;
                _handler._jumpInput = true;
            }
            else {
                _handler._jumpInput = false;
            }

            if (Vector3.Distance(transform.position, _shuttle.transform.position) < _shuttle.GetVelocity().magnitude / 4) {
                int rand = Random.Range(0, 3);

                if (rand == 2) {
                    _handler._chipInput = true;
                }
                else {
                    if (transform.position.y < 1.2f) {
                        _handler._driveInput = true;
                    }
                    else {
                        _handler._dropInput = true;
                    }
                }
            }
            tick = 0.0f;
        }
        _debug.position = targetPosition;
    }
}
