using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator : MonoBehaviour
{
    FighterController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = transform.root.GetComponent<FighterController>();
    }

    public void ResetAttack() {
        _controller.ResetHitbox();
        _controller.ResetAttack();
    }
}
