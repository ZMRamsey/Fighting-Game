using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShadow : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Vector3 _position = new Vector3(0,0.84f,0);
    [SerializeField] float _shadowShrinkFactor;
    Vector3 maxSize;
    Vector3 minSize;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = transform.GetComponent<SpriteRenderer>();
        //_parent = transform.GetComponentInChildren<FighterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _position.x = _parent.transform.root.position.x;
        transform.position = _position;

        float heightCoeff = Mathf.Clamp(Mathf.Abs(((_parent.position.y - 2) / 10) - 1), 0,1);

        float newAlpha = Mathf.Clamp((255 * heightCoeff), 0, 255);
        _renderer.color = Color.black * heightCoeff;

        Vector3 newScale = minSize + ((maxSize - minSize) * heightCoeff);
        transform.localScale = newScale;
    }

    public void SetParent(Transform transform)
    {
        _parent = transform;
    }

    public void SetSize(Vector3 size)
    {
        transform.localScale = size;
        maxSize = size;
        minSize = size / _shadowShrinkFactor;
    }
}
