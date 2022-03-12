using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAndTekaFighter : FighterController
{
    [SerializeField] GameObject _rayPrefab;
    [SerializeField] RectTransform _timeStopCanvas;
    [SerializeField] RectTransform _timeCenter;
    [SerializeField] AudioClip _startTimeSFX, _stopTimeSFX;
    SpriteRenderer _rayRenderer;
    GameObject _rayObject;
    InputHandler _handler;
    float _verticalProcess = 0;
    bool _timeStop;
    float _timeStopTimer;
    public override void InitializeFighter()
    {
        base.InitializeFighter();
        _rigidbody.useGravity = false;
        _inputHandler.SetInputState(InputState.none);
        _handler = _rayObject.GetComponent<InputHandler>();
        _rayRenderer = _rayObject.GetComponent<RayAI>().GetSpriteRenderer();
        _timeStopCanvas.gameObject.SetActive(false);
    }

    public override void FighterAwake()
    {
        _rayObject = Instantiate(_rayPrefab, transform.position, Quaternion.identity);
    }

    public override void SetFilter(FighterFilter filter)
    {
        base.SetFilter(filter);
        _rayObject.GetComponent<FighterController>().SetFilter(_filter);
    }

    public override void OnSuperMechanic()
    {
        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter, this);
        //_timeStopCanvas.anchoredPosition = viewportPoint;
        //_slowObject.transform.position = new Vector3(transform.position.x, transform.position.y, 12);
        //_slowObject.SetActive(true);
        //GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter);
    }

    public Vector3 GetRayPos() {
        return _rayObject.transform.position;
    }

    public override void OnAfterSuperScreen() {
        _timeStopCanvas.gameObject.SetActive(true);
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        _timeCenter.anchoredPosition = viewportPoint;

        _timeStop = true;
        _timeStopTimer = 0.0f;
        FighterFilter filter = FighterFilter.one;
        if (GetFilter() == filter) {
            filter = FighterFilter.two;
        }

        _timeStopCanvas.GetComponent<Animator>().SetTrigger("start");
        GameManager.Get().StunFrames(3.0f, filter);
        GameManager.Get().SlowDownMusic();
        _source.PlayOneShot(_stopTimeSFX);
    }

    public override void OnSuperEnd(bool instant) {
        base.OnSuperEnd(instant);
        if (instant) {
            _timeStopCanvas.gameObject.SetActive(false);
        }
        else {
            _source.PlayOneShot(_startTimeSFX);
            _timeStopCanvas.GetComponent<Animator>().SetTrigger("stop");
        }
        GameManager.Get().GetShuttle().ForceUnfreeze();
        GameManager.Get().SpeedUpMuisc();
        _timeStop = false;
        _timeStopTimer = 0.0f;
    }

    public override void ResetFighter()
    {
        base.ResetFighter();
        if (_rayObject != null)
        {
            _rayObject.GetComponent<FighterController>().ResetFighter();
            _rayObject.transform.position = transform.position;
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

        if (_timeStop) {
            _timeStopTimer += Time.deltaTime;
            GameManager.Get().GetShuttle().ForceFreeze();
            if (_timeStopTimer > 3.0f) {
                OnSuperEnd(false);
            }
        }
    }

    Vector3 _targetMovement;

    public override void OnFixedFighterUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, 2, 11);

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

        _rigidbody.position = pos;
    }

    public override Transform GetFocusTransform() {
        return _rayObject.transform;
    }
}
