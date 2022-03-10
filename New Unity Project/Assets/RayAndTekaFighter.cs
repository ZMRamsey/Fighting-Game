using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAndTekaFighter : FighterController
{
    [SerializeField] GameObject _rayPrefab;
    [SerializeField] GameObject _slowPrefab;
    SpriteRenderer _rayRenderer;
    GameObject _rayObject;
    GameObject _slowObject;
    InputHandler _handler;
    float _verticalProcess = 0;
    public override void InitializeFighter()
    {
        base.InitializeFighter();
        _rigidbody.useGravity = false;
        _inputHandler.SetInputState(InputState.none);
        _rayObject = Instantiate(_rayPrefab, transform.position, Quaternion.identity);
        _handler = _rayObject.GetComponent<InputHandler>();
        _slowObject = Instantiate(_slowPrefab, transform.position, Quaternion.identity);
        _rayRenderer = _rayObject.GetComponent<RayAI>().GetSpriteRenderer();
        _slowObject.SetActive(false);
        _rayObject.GetComponent<FighterController>().SetFilter(_filter);
    }

    public override void OnSuperMechanic()
    {
        //_slowObject.transform.position = new Vector3(transform.position.x, transform.position.y, 12);
        //_slowObject.SetActive(true);
        //GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter);
    }

    public override void ResetFighter()
    {
        base.ResetFighter();
        if (_rayObject != null)
        {
            _rayObject.GetComponent<FighterController>().ResetFighter();
        }
    }

    public override void OnAirMovement()
    {

    }

    public override void OnGroundMovement()
    {

    }

    float _rayUpdateTimer;
    public override void OnFighterUpdate()
    {
        _verticalProcess = 0;
        if (_inputHandler.GetJumpHeld())
        {
            _verticalProcess = 1;
        }

        if (_inputHandler.GetCrouch())
        {
            _verticalProcess = -1;
        }

        _rayUpdateTimer += Time.deltaTime;

        float _rayXTarget = 0;
        Vector3 distance = transform.position;
        distance.x = _rayObject.transform.position.x;

        if (_rayObject.transform.position.x > transform.position.x)
        {
            _rayRenderer.flipX = true;
        }

        if (_rayObject.transform.position.x < transform.position.x)
        {
            _rayRenderer.flipX = false;
        }

        if (_rayUpdateTimer > 0.1f)
        {
            if (Vector3.Distance(transform.position, distance) > 2)
            {
                if (_rayObject.transform.position.x > transform.position.x)
                {
                    _rayXTarget = -1;
                }

                if (_rayObject.transform.position.x < transform.position.x)
                {
                    _rayXTarget = 1;
                }
            }

            _handler._inputX = _rayXTarget;
            _rayUpdateTimer = 0;
        }
    }

    Vector3 _targetMovement;

    public override void OnFixedFighterUpdate()
    {
        base.OnFixedFighterUpdate();

        Vector3 movementProcessing = new Vector3(_inputHandler._inputX, _verticalProcess, 0);
        movementProcessing.Normalize();
        movementProcessing.y *= 2;
        movementProcessing *= _speed;

        if (movementProcessing != Vector3.zero || _verticalProcess != 0)
        {
            _targetMovement = Vector3.Lerp(_targetMovement, movementProcessing, Time.deltaTime * 12);
        }

        _targetMovement = Vector3.Lerp(_targetMovement, movementProcessing, Time.deltaTime * 6);
        _controllerVelocity = _targetMovement;
    }
}
