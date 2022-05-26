using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShadow : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] SpriteRenderer _renderer;
     Vector3 _position = new Vector3(0,0.84f,0);
    [SerializeField] float _shadowShrinkFactor;
    Vector3 _baseShadowSize;
    [SerializeField] bool _canRotate;
    [Range(0.0f, 1.0f)] [SerializeField] float _alphaCap = 0.66f;
    Vector3 maxSize;
    Vector3 minSize;
    [SerializeField] Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = transform.GetComponent<SpriteRenderer>();
        _baseShadowSize = transform.localScale;
        SetSize(_baseShadowSize);
        //_offset = transform.position;
        transform.SetParent(null);
        //_parent = transform.GetComponentInChildren<FighterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _position = transform.position;
        _position.y = 0.84f;
        _position.x = _offset.x + _parent.transform.position.x;
        transform.position = _position;
        //transform.rotation = Quaternion.identity;
    }
    private void LateUpdate()
    {
        if (_canRotate)
        {
            transform.rotation = Quaternion.Euler(-90.0f, _parent.rotation.y * -1, _parent.rotation.z * -1);
        }
    }

    private void FixedUpdate()
    {
        float heightCoeff = Mathf.Clamp(Mathf.Abs(((_parent.position.y - 2) / 10) - 1), 0, 1);

        //float newAlpha = Mathf.Clamp(_alphaCap * heightCoeff, 0, _alphaCap);
        _renderer.color = Color.black * heightCoeff * _alphaCap;

        Vector3 newScale = minSize + ((maxSize - minSize) * heightCoeff);
        transform.localScale = newScale;
    }

    public void SetSize(Vector3 size)
    {
        transform.localScale = size;
        maxSize = size;
        minSize = size / _shadowShrinkFactor;

        _renderer.color = Color.black * _alphaCap;
    }
}
