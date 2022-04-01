using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool _useColourTint, _useSize;
    [Header("Colour Settings")]
    [SerializeField] Color _defaultColor = Color.white;
    [SerializeField] Color _highlightedColor = Color.white;
    Color _currentColorTarget;
    [Header("Size Settings")]
    [SerializeField] Vector3 _defaultSize = Vector3.one;
    [SerializeField] Vector3 _highlightedSize = Vector3.one;
    Vector3 _currentSize;
    [Header("Global Settings")]
    [SerializeField] float _crossfadeSpeed = 10;
    [SerializeField] Transform _effectorObject;
    bool _isFocused = false;
    bool _onClick = false;

    void Update() {
        if (_useColourTint) {
            _currentColorTarget = _isFocused ? _highlightedColor : _defaultColor;
            _effectorObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(_effectorObject.GetComponent<TextMeshProUGUI>().color, _currentColorTarget, Time.deltaTime * _crossfadeSpeed);
        }

        if (_useSize) {
            _currentSize = _isFocused ? _highlightedSize : _defaultSize;
            _effectorObject.transform.localScale = Vector3.Lerp(_effectorObject.transform.localScale, _currentSize, Time.deltaTime * _crossfadeSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        OnFocus();
    }

    public void OnPointerExit(PointerEventData eventData) {
        OnUnfocus();
    }

    public void OnFocus() {
        _isFocused = true;
    }
    
    public void OnUnfocus() {
        _isFocused = false;
    }

    public bool IsFocused() {
        return _isFocused;
    }

    public virtual void OnSubmit() {
        _onClick = true;
    }

    public bool OnClick() {
        var temp = _onClick;
        _onClick = false;
        return temp;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnSubmit();
        }
    }

    void OnEnable() {
        OnUnfocus();
        if (_useColourTint) {
            _effectorObject.GetComponent<TextMeshProUGUI>().color = _defaultColor;
        }
        if (_useSize) {
            _effectorObject.transform.localScale = _defaultSize;
        }
    }
}
