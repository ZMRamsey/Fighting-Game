using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotaterCircle : MonoBehaviour
{
    public float turningRate = 0.75f;
    int sceneIndex = 0;

    public GameObject playMenu;
    public GameObject optionsMenu;

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    public void rotateToPlay()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        sceneIndex = 0;
    }
    public void rotateToOptions()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 30.0f);
        sceneIndex = 1;
    }
    public void rotateToQuit()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
        sceneIndex = 2;
    }

    public void rotateToPlay2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        sceneIndex = 0;
    }
    public void rotateToOptions2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 120.0f);
        sceneIndex = 1;
    }
    public void rotateToQuit2()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 150.0f);
        sceneIndex = 2;
    }

    public void rotateToPlay3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        sceneIndex = 0;
    }
    public void rotateToOptions3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 210.0f);
        sceneIndex = 1;
    }
    public void rotateToQuit3()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 240.0f);
        sceneIndex = 2;
    }

    public void rotateToPlay4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
        sceneIndex = 0;
    }
    public void rotateToOptions4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 300.0f);
        sceneIndex = 1;
    }
    public void rotateToQuit4()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, 330.0f);
        sceneIndex = 2;
    }



    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.5f);
    }
}
