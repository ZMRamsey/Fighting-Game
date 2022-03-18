using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    static CameraContoller _instance;

    public static CameraContoller Get() {
        return _instance;
    }

    [SerializeField] Transform _fighterOne, _fighterTwo, _ball;
    [SerializeField] Vector3 _cameraPositionOffset;
    [SerializeField] float _speed;
    [SerializeField] Transform _camera;
    [SerializeField] Vector2 _limitX;
    [SerializeField] Vector2 _limitY;
    [SerializeField] Camera[] _main;
    Vector3 _cameraTarget;

    Transform _focus;

    // Start is called before the first frame update
    void Awake() {
        _instance = this;
    }

    float zoom;
    // Update is called once per frame
    void Update()
    {
        //_cameraTarget = (_focusOne.position + _focusTwo.position) / 2;
        _cameraTarget = _ball.position;

        _cameraTarget.y += _cameraPositionOffset.y;

        _cameraTarget.x = Mathf.Clamp(_cameraTarget.x, _limitX.x, _limitX.y);
        _cameraTarget.y = Mathf.Clamp(_cameraTarget.y, _limitY.x, _limitY.y);

        //if (GameManager.Get().GetShuttle()) {
        //    zoom = Mathf.Lerp(zoom, 10f * GameManager.Get().GetShuttle().GetSpeedPercent(), Time.fixedDeltaTime * 0.2f);
           
        //    var zoomVal = 60f + (zoom);

        //    foreach(Camera cam in _main) {
        //        cam.fieldOfView = zoomVal; 
        //    }
        //}

        if (GameManager.Get().GetShuttle().CanKill() || zoomOutTimer > 0) {
            zoomOutTimer -= Time.fixedDeltaTime;
            foreach (Camera cam in _main) {
                var zoomVal = Mathf.Lerp(cam.fieldOfView, 70f, Time.fixedDeltaTime * 10);
                cam.fieldOfView = zoomVal;
            }
        }
        else if (zoomTimer > 0) {
            zoomTimer -= Time.fixedDeltaTime;

            foreach (Camera cam in _main) {
                var zoomVal = Mathf.Lerp(cam.fieldOfView, 50, Time.fixedDeltaTime * 10);
                cam.fieldOfView = zoomVal;
            }
        }
        else {
            foreach (Camera cam in _main) {
                var zoomVal = Mathf.Lerp(cam.fieldOfView, 60, Time.fixedDeltaTime * 3);
                cam.fieldOfView = zoomVal;
            }
        }
    }

    float zoomTimer;
    public void SetZoomTimer(float time) {
        zoomTimer = time;
    }

    float zoomOutTimer;
    public void SetZoomOutTimer(float time) {
        zoomOutTimer = time;
    }

    private void LateUpdate()
    {
        Vector3 pos = Vector3.zero;

        if (_focus == null) {
            pos = Vector3.Lerp(transform.position, _cameraTarget + _cameraPositionOffset, Time.fixedDeltaTime * _speed);
            pos.x = Mathf.Clamp(pos.x, _limitX.x, _limitX.y);
        }
        else {
            pos = Vector3.Lerp(transform.position, _focus.transform.position + transform.forward * -4f, Time.fixedDeltaTime * 6);
        }

        transform.position = pos;
    }

    public void SetFocus(Transform focus) {
        _focus = focus;
    }

    public void  TeleportFocus() {
        transform.position = _focus.transform.position + transform.forward * -4f;
    }
}
