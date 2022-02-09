using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageHandler : MonoBehaviour
{
    SpriteRenderer _renderer;
    [SerializeField] float _fadeSpeed = 3;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();   
    }

    void Update()
    {
        Color color = _renderer.color;
        color.a -= Time.deltaTime * _fadeSpeed;

        _renderer.color = color;

        if(color.a <= 0) {
            Destroy(gameObject);
        }
    }
}
