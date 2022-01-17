using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] Shaker _cameraShaker;
    [SerializeField] FighterController _fighterOne;
    [SerializeField] FighterController _fighterTwo;
    [SerializeField] ShuttleCock _shuttle;

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

    public void StunFrames(float timer) {
        _fighterOne.StunController(timer);
        _fighterTwo.StunController(timer);
    }
}
