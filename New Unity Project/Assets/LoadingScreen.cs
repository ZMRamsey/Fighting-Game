using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _loadingText;
    [SerializeField] Animator _anim;
    public virtual void OnLoaded() {
        _loadingText.text = "Continue";
    }

    public void EndAnimation() {
        if (_anim == null) {
            return;
        }
        _anim.SetTrigger("End");
    }

    public void Rebind() {
        if(_anim == null) {
            return;
        }
        _anim.Rebind();
    }
}
