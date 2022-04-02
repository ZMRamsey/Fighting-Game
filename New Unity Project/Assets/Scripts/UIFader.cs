using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    [SerializeField] CanvasGroup _group;
    [SerializeField] float _speed = 1;
    
    void Update()
    {
        if(_group.alpha > 0) {
            _group.alpha -= Time.deltaTime * _speed;
        }
    }

    public void SetAlpha(float alpha) {
        _group.alpha = alpha;
    }
}
