using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool _useColourTint, _useSize, _useSpriteSwap, _useInteractable;
    [Header("Colour Settings")]
    [SerializeField] Color _defaultColor = Color.white;
    [SerializeField] Color _highlightedColor = Color.white;
    [SerializeField] Color _disabledColor = Color.white;
    Color _currentColorTarget;
    [Header("Sprite Settings")]
    [SerializeField] Sprite _defaultSprite;
    [SerializeField] Sprite _highlightedSprite;
    [SerializeField] Sprite _currentSprite;
    [Header("Size Settings")]
    [SerializeField] Vector3 _defaultSize = Vector3.one;
    [SerializeField] Vector3 _highlightedSize = Vector3.one;
    Vector3 _currentSize;
    [Header("Global Settings")]
    [SerializeField] float _crossfadeSpeed = 10;
    [SerializeField] Transform _effectorObject;
    [SerializeField] Image _extraHighlight;
    bool _isFocused = false;
    bool _onClick = false;
    public bool interactable = true;

    void Update() {
        if (_useColourTint) {
            _currentColorTarget = _isFocused ? _highlightedColor : _defaultColor;
            _effectorObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(_effectorObject.GetComponent<TextMeshProUGUI>().color, _currentColorTarget, Time.deltaTime * _crossfadeSpeed);
            if (_extraHighlight) {
                _extraHighlight.color = Color.Lerp(_effectorObject.GetComponent<TextMeshProUGUI>().color, _currentColorTarget, Time.deltaTime * _crossfadeSpeed);
            }
        }

        if (_useInteractable)
        {
            _currentColorTarget = interactable ? _defaultColor : _disabledColor;
            _effectorObject.GetComponent<Image>().color = Color.Lerp(_effectorObject.GetComponent<Image>().color, _currentColorTarget, Time.deltaTime * _crossfadeSpeed); ;
        }

        if (_useSize) {
            _currentSize = _isFocused ? _highlightedSize : _defaultSize;
            _effectorObject.transform.localScale = Vector3.Lerp(_effectorObject.transform.localScale, _currentSize, Time.deltaTime * _crossfadeSpeed);
        }

        if (_useSpriteSwap) {
            _currentSprite = _isFocused ? _highlightedSprite : _defaultSprite;
            _effectorObject.GetComponent<Image>().sprite = _currentSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (interactable)
        {
            OnFocus();
        }
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
        if (interactable)
        {
            _onClick = true;
        }
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

        if (_useSpriteSwap) {
            transform.GetComponent<Image>().sprite = _defaultSprite;
        }

        if (_useSize) {
            _effectorObject.transform.localScale = _defaultSize;
        }
    }

    public void SetSprites(Sprite sprite)
    {
        _defaultSprite = sprite;
        _highlightedSprite = sprite;
    }
}
