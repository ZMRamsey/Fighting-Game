using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] Shaker _cameraShaker;

    public static GameManager Get() {
        return _instance;
    }

    void Awake()
    {
        _instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Shaker GetCameraShaker() {
        return _cameraShaker;
    }
}
