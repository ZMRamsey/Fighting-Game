using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterTab
{
    [SerializeField] FighterController _controller;
    [SerializeField] FighterUI _fighterUI;
    [SerializeField] Vector3 _spawn;

    public FighterUI GetUI() {
        return _fighterUI;
    }

    public FighterTab(FighterController controller) {
        _controller = controller;
    }

    public FighterController GetController() {
        return _controller;
    }

    public void SetControler(FighterController controller) {
        _controller = controller;
    }

    public Vector3 GetSpawn() {
        return _spawn;
    }
}
