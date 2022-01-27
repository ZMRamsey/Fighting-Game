using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotaterCircle : MonoBehaviour
{
    public float turningRate = 0.75f;

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    public void rotateToPlay()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    public void rotateToOptions()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 30.0f);
    }
    public void rotateToQuit()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
    }

    public void rotateToPlay2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
    }
    public void rotateToOptions2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 120.0f);
    }
    public void rotateToQuit2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 150.0f);
    }

    public void rotateToPlay3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
    }
    public void rotateToOptions3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 210.0f);
    }
    public void rotateToQuit3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 240.0f);
    }

    public void rotateToPlay4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
    }
    public void rotateToOptions4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 300.0f);
    }
    public void rotateToQuit4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 330.0f);
    }



    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.5f);
    }
}
