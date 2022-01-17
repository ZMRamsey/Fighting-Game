using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] Transform _fighterOne, _fighterTwo, _ball;
    [SerializeField] Vector3 _cameraPositionOffset;
    [SerializeField] float _speed;
    [SerializeField] Transform _camera;
    [SerializeField] Vector2 _limitX;
    [SerializeField] Vector2 _limitY;
    Vector3 _cameraTarget;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //_cameraTarget = (_focusOne.position + _focusTwo.position) / 2;
        _cameraTarget = _ball.position;

        _cameraTarget.y += _cameraPositionOffset.y;

        _cameraTarget.x = Mathf.Clamp(_cameraTarget.x, _limitX.x, _limitX.y);
        _cameraTarget.y = Mathf.Clamp(_cameraTarget.y, _limitY.x, _limitY.y);

    }

    private void LateUpdate()
    {
        Vector3 pos = Vector3.Lerp(transform.position, _cameraTarget + _cameraPositionOffset, Time.deltaTime * _speed);
        pos.x = Mathf.Clamp(pos.x, _limitX.x, _limitX.y);
        transform.position = pos;
    }
}
