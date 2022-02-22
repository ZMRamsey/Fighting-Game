using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator : MonoBehaviour
{
    FighterController _controller;
    [SerializeField] ParticleSystem _KOEffects;
    // Start is called before the first frame update
    void Start()
    {
        _controller = transform.root.GetComponent<FighterController>();
    }

    public void ResetAttack() {
        _controller.ResetHitbox();
        _controller.ResetAttack();
    }

    public void PlayLeftFoot() {
        _controller.PlayLeftFoot();
    }

    public void PlayRightFoot() {
        _controller.PlayRightFoot();
    }

    public void PlaySound(AudioClip clip) {
        _controller.PlaySound(clip);
    }

    public void KOEffects() {
        _KOEffects.Play();
    }

}
