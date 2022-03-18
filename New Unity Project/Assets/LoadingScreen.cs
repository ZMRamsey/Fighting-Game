using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _loadingText;
    [SerializeField] Animator _anim;
    [SerializeField] bool _versusScreen;
    [SerializeField] Image _fighterOne;
    [SerializeField] Image _fighterTwo;
    [SerializeField] Image _fighterOneName;
    [SerializeField] Image _fighterTwoName;

    private void OnEnable() {
        if (!_versusScreen) {
            return;
        }
        _loadingText.text = "Prepare For The Upcoming Battle!";
        _fighterOne.sprite = GameLogic.Get().GetSettings().GetFighterOneProfile().GetIconSprite();
        _fighterTwo.sprite = GameLogic.Get().GetSettings().GetFighterTwoProfile().GetIconSprite();
        _fighterOneName.sprite = GameLogic.Get().GetSettings().GetFighterOneProfile().GetNameSprite(FighterFilter.one);
        _fighterTwoName.sprite = GameLogic.Get().GetSettings().GetFighterTwoProfile().GetNameSprite(FighterFilter.two);
    }

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
