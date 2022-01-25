using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterFilter { one, two, both};
public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] Shaker _cameraShaker;
    [SerializeField] FighterController _fighterOne;
    [SerializeField] FighterController _fighterTwo;
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Transform _speedRotator;
    float _rotateTarget;

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
        _rotateTarget = Mathf.Lerp(_rotateTarget, _shuttle.GetSpeedPercent(), Time.deltaTime * 12);
        _speedRotator.eulerAngles = -Vector3.Lerp(new Vector3(0,0, -75f), new Vector3(0, 0, 75f), _rotateTarget);
    }

    public Shaker GetCameraShaker() {
        return _cameraShaker;
    }

    public void StunFrames(float timer, FighterFilter filter) {
        if (filter == FighterFilter.one || filter == FighterFilter.both) {
            _fighterOne.StunController(timer);
        }
        if (filter == FighterFilter.two || filter == FighterFilter.both) {
            _fighterTwo.StunController(timer);
        }
    }
}
