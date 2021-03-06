using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    [SerializeField] Image _mainBar;
    //[SerializeField] Image _fill;
    //[SerializeField] Image _arrowPosBar;
    //[SerializeField] Image _arrowNegBar;
    //[SerializeField] Image _superEffectBar;
    //[SerializeField] Gradient _superPulse;
    [SerializeField] GameObject _flash;
    float _targetAmount;
    float _currentValue;

    float _positiveTimer;
    float _negativeTimer;
    float _superTimer;

    void Start() {
        SetBarValue(0);
        _flash.SetActive(false);
        //_arrowPosBar.material.SetFloat("_Opacity", 0);
        //_arrowNegBar.material.SetFloat("_Opacity", 0);
        //_superEffectBar.material.SetFloat("_Opacity", 0);
       // _fill.color = Color.white;
    }

    void Update() {
        _currentValue = Mathf.Lerp(_currentValue, _targetAmount, Time.deltaTime * 2);
        _mainBar.fillAmount = Mathf.Lerp(0.42f, 0.83f, _currentValue);
        //_arrowPosBar.fillAmount = _mainBar.fillAmount;
        //_arrowNegBar.fillAmount = _mainBar.fillAmount;
        //_superEffectBar.fillAmount = _mainBar.fillAmount;

        //if (_positiveTimer > 0.0f) {
        //    _positiveTimer -= Time.deltaTime;
        //}

        //float pos = _arrowPosBar.material.GetFloat("_Opacity");
        //float neg = _arrowNegBar.material.GetFloat("_Opacity");
        //float super = _superEffectBar.material.GetFloat("_Opacity");

        //if (_positiveTimer <= 0.0f && pos > 0) {
        //    pos -= Time.deltaTime;
        //    _arrowPosBar.material.SetFloat("_Opacity", pos);
        //}

        //if (_negativeTimer <= 0.0f && neg > 0) {
        //    neg -= Time.deltaTime;
        //    _arrowNegBar.material.SetFloat("_Opacity", neg);
        //}

        //if (_targetAmount == 1) {
        //    _superTimer += Time.deltaTime;
        //    if (_superTimer > 1) {
        //        _superTimer = 0.0f;
        //    }

        //    _fill.color = _superPulse.Evaluate(_superTimer);
        //}
    }

    public void SetBarValue(float value) {
        _targetAmount = value;

        if(value == 0)
        {
            _currentValue = 0;
        }

        if (_targetAmount >= 1)
        {
            _flash.SetActive(true);
            //_superEffectBar.material.SetFloat("_Opacity", 1);
            return;
        }
        else
        {
            _flash.SetActive(false);
            //_superEffectBar.material.SetFloat("_Opacity", 0);
        }

        //if (_fill.color != Color.white) {
        //    _fill.color = Color.white;
        //}

        //if (_targetAmount > oldValue) {
        //    _arrowPosBar.material.SetFloat("_Opacity", 1);
        //    _positiveTimer = 0.5f;
        //}
        //else {
        //    _arrowNegBar.material.SetFloat("_Opacity", 1);
        //    _positiveTimer = 0.5f;
        //}
    }
}
