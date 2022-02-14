using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffects : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _afterImageDelay = 0.1f;
    [SerializeField] bool _afterImageActive;
    float _afterImageTimer;

    void Update()
    {
        if (_afterImageActive) {
            _afterImageTimer += Time.deltaTime;
            if (_afterImageTimer > _afterImageDelay && GetComponent<Rigidbody>().velocity.magnitude > 1) {
                GameObject AI = new GameObject();
                AI.transform.position = _renderer.transform.position;
                AI.transform.localScale = _renderer.transform.localScale;

                AI.AddComponent<SpriteRenderer>();
                SpriteRenderer renderer = AI.GetComponent<SpriteRenderer>();

                AI.AddComponent<AfterImageHandler>();
                renderer.sprite = _renderer.sprite;
                renderer.flipX = _renderer.flipX;
                AI.layer = gameObject.layer;

                _afterImageTimer = 0.0f;
            }
        }
    }

    public void SetAfterImage() {
        _afterImageActive = true;
    }

    public void DisableAfterImage() {
        _afterImageActive = false;
    }
}
