using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //[SerializeField] Vector3 _ballVelocity;
    [SerializeField] Transform _ballHolder;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;
    float _squishTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        spawn = transform.position;
    }

    [ContextMenu("Reset Ball")]
    public void ResetBall() {
        transform.position = spawn;
    }


    [ContextMenu("Shoot")]
    public void Shoot() {
        SquishBall();
        _rb.velocity = new Vector3(5, 5, 0);
    }


    Vector3 spawn;
    void Update()
    {
        Vector3 velocity = _rb.velocity;

        Vector3 scale = Vector3.one;
        scale.x = Mathf.Clamp(velocity.magnitude * 0.25f, 1, 10);

        //Negative Check
        if (velocity.magnitude < 0 ) {
            scale.x = Mathf.Clamp(scale.x, -1, -10);
        }

        transform.right = Vector3.Lerp(transform.right, velocity, Time.deltaTime * _smoothing);
        if (_squishTimer <= 0) {
            _ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
        }

        velocity.x = velocity.x * 0.9f;

        _rb.velocity = _rb.velocity;
    }

    void SquishBall() {
        _squishTimer = _squishThreshold;

        float x = 0.8f;
        if (_ballHolder.localScale.x < 0) {
            x = -0.8f;
        }
        _ballHolder.localScale = new Vector3(x, 1, 1);
    }

    private void OnCollisionEnter(Collision collision) {
        SquishBall();
    }
}
