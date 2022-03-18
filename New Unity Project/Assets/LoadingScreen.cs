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
        _anim.SetTrigger("End");
    }
}
