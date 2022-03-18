using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimator : MonoBehaviour
{
    public void OnClipPlay(AudioClip clip) {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    public void EndTransition() {
        GameLogic.Get().FinishLoad();
    }
}
