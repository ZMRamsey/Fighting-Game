using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TekaFighter : FighterController
{
    [SerializeField] GameObject _rayPrefab;
    [SerializeField] RectTransform _timeStopCanvas;
    [SerializeField] RectTransform _timeCenter;
    [SerializeField] AudioClip _startTimeSFX, _stopTimeSFX;
    [SerializeField] Image _tangibleBar;
    SpriteRenderer _rayRenderer;
    GameObject _rayObject;
    InputHandler _handler;
    float _verticalProcess = 0;
    bool _timeStop;
    float _timeStopTimer;
    float _tangibleLevel;
    bool _depleted;
    public override void InitializeFighter() {
        base.InitializeFighter();
        _rigidbody.useGravity = false;
        _inputHandler.SetInputState(InputState.none);
        _handler = _rayObject.GetComponent<InputHandler>();
        _rayRenderer = _rayObject.GetComponent<RayAI>().GetSpriteRenderer();
        _timeStopCanvas.gameObject.SetActive(false);
        _tangibleLevel = 1;
    }

    public override void FighterAwake() {
        _rayObject = Instantiate(_rayPrefab, transform.position, Quaternion.identity);
    }

    public override void SetFilter(FighterFilter filter) {
        base.SetFilter(filter);
        _rayObject.GetComponent<FighterController>().SetFilter(_filter);
    }

    public override void OnSuperMechanic() {
        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter, this);
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
        TimeStop();
        GameManager.Get().StunFrames(3.0f, filter);
        GameManager.Get().GetCameraShaker().SetShake(0.5f, 3f, true);
        CameraContoller.Get().SetZoomTimer(0.3f);
        GameManager.Get().SlowDownMusic();
        _source.PlayOneShot(_stopTimeSFX);
    }

    public override void OnSuperEnd(bool instant) {
        base.OnSuperEnd(instant);
        if (instant) {
            _timeStopCanvas.gameObject.SetActive(false);
        }
        else {
            TimeStart();
            _source.PlayOneShot(_startTimeSFX);
            _timeStopCanvas.GetComponent<Animator>().SetTrigger("stop");
        }

        GameManager.Get().SpeedUpMuisc();
        _timeStop = false;
        GameManager.Get().GetCameraShaker().SetShake(0.5f, 3f, true);
        CameraContoller.Get().SetZoomOutTimer(0.3f);
        _timeStopTimer = 0.0f;
    }

    public void TimeStop() {
        Collider[] collidersPlayers = Physics.OverlapSphere(transform.position, 100f);
        foreach (Collider hit in collidersPlayers) {
            MrHandy handy = hit.GetComponent<MrHandy>();
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();

            if (handy != null) {
                handy.ForceFreeze();
            }

            if (shuttle != null) {
                shuttle.ForceFreeze();
            }
        }
    }

    public void TimeStart() {
        Collider[] collidersPlayers = Physics.OverlapSphere(transform.position, 100f);
        foreach (Collider hit in collidersPlayers) {
            MrHandy handy = hit.GetComponent<MrHandy>();
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();

            if (handy != null) {
                handy.ForceUnfreeze();
            }

            if (shuttle != null) {
                shuttle.ForceUnfreeze();
            }
        }
    }

    public override void ResetFighter() {
        base.ResetFighter();
        if (_rayObject != null) {
            _rayObject.GetComponent<FighterController>().ResetFighter();
            _rayObject.transform.position = transform.position;
        }
    }

    public override void OnAirMovement() {

    }

    public override void OnGroundMovement() {

    }

    public override bool IsGrounded() {
        return false;
    }

    float _rayUpdateTimer;
    public override void OnFighterUpdate() {
        _renderer.enabled = IsGrabbing == null;

        if (IsGrabbing != null) {
            IsGrabbing.transform.right = Vector3.Lerp(IsGrabbing.transform.right, _rigidbody.velocity, Time.deltaTime * 2);
        }


        if (_depleted && _tangibleLevel >= 1) {
            print("RESTOCK");
            _depleted = false;
            _canAttack = true;
        }

        if (!_depleted) {
            Color color = _renderer.color;
            color.a = 0.5f + (_tangibleLevel / 2);
            _renderer.color = Color.Lerp(_renderer.color, color, Time.deltaTime);

            if (_canAttack && _tangibleLevel < 1) {
                _tangibleLevel += Time.deltaTime * 0.5f;
            }
            _tangibleBar.color = Color.white;
        }
        else {
            _tangibleLevel += Time.deltaTime * 0.5f;
            _tangibleBar.color = Color.red;
        }

        _tangibleBar.fillAmount = _tangibleLevel;

        _verticalProcess = 0;
        if (_inputHandler.GetJumpHeld()) {
            _verticalProcess = 1;
        }

        if (_inputHandler.GetCrouch()) {
            _verticalProcess = -1;
        }

        _rayUpdateTimer += Time.deltaTime;

        float _rayXTarget = 0;
        Vector3 distance = transform.position;
        distance.x = _rayObject.transform.position.x;

        if (_rayObject.transform.position.x > transform.position.x) {
            _rayRenderer.flipX = true;
        }

        if (_rayObject.transform.position.x < transform.position.x) {
            _rayRenderer.flipX = false;
        }

        if (_rayUpdateTimer > 0.1f) {
            if (Vector3.Distance(transform.position, distance) > 2) {
                if (_rayObject.transform.position.x > transform.position.x) {
                    _rayXTarget = -1;
                }

                if (_rayObject.transform.position.x < transform.position.x) {
                    _rayXTarget = 1;
                }
            }

            _handler._inputX = _rayXTarget;
            _rayUpdateTimer = 0;
        }

        if (_timeStop) {
            _timeStopTimer += Time.deltaTime;
            TimeStop();
            if (_timeStopTimer > 3.0f) {
                OnSuperEnd(false);
            }
        }

        //Vector3 pos = transform.position;
        //pos.y = Mathf.Clamp(pos.y, 2, 11);

        //_rigidbody.position = pos;
    }

    Vector3 _targetMovement;

    public override void UpdateMove() {
        if (_depleted) {
            _canAttack = true;
            return;
        }

        base.UpdateMove();

        if (!_timeStop) {
            _tangibleLevel -= 0.35f;
        }

        if (_tangibleLevel <= 0) {
            _depleted = true;
        }
    }

    public override void OnFixedFighterUpdate() {
        base.OnFixedFighterUpdate();

        Vector3 movementProcessing = new Vector3(_inputHandler._inputX, _verticalProcess, 0);
        movementProcessing.Normalize();
        movementProcessing.y *= 2;
        movementProcessing *= _settings.GetSpeed();

        if (movementProcessing != Vector3.zero || _verticalProcess != 0) {
            _targetMovement = Vector3.Lerp(_targetMovement, movementProcessing, Time.deltaTime * 12);
        }

        _targetMovement = Vector3.Lerp(_targetMovement, movementProcessing, Time.deltaTime * 6);
        _controllerVelocity = _targetMovement;
    }

    public override Transform GetFocusTransform() {
        return _rayObject.transform;
    }
}
