using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] Transform _fighterOne, _fighterTwo;
    [SerializeField] Vector3 _cameraPositionOffset;
    [SerializeField] float _yOffset;
    [SerializeField] float _speed;
    [SerializeField] Transform _camera;
    Vector3 _cameraTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cameraTarget = (_fighterOne.position + _fighterTwo.position) / 2;

        _cameraTarget.y += _yOffset;

    }

    private void LateUpdate()
    {

        transform.position = Vector3.MoveTowards(transform.position, _cameraTarget + _cameraPositionOffset, Time.deltaTime * _speed);
    }
}
