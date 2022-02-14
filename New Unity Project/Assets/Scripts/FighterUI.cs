using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    [SerializeField] Image _mainBar;
    [SerializeField] Image _easeBar;

    void Start()
    {
        SetBarValue(0);
    }

    void Update()
    {
        if (_mainBar.fillAmount > _easeBar.fillAmount) {
            _easeBar.fillAmount = Mathf.Lerp(_easeBar.fillAmount, _mainBar.fillAmount, Time.deltaTime * 2);
        }
        else {
            if (_easeBar.fillAmount != _mainBar.fillAmount) {
                _easeBar.fillAmount = _mainBar.fillAmount;
            }
        }
    }

    public void SetBarValue(float value) {
        _mainBar.fillAmount = value;
    }
}
