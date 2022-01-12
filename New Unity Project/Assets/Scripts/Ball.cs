using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    //[SerializeField] Vector3 _ballVelocity;
    [SerializeField] Transform _ballHolder;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _smoothing;
    [SerializeField] float _squishThreshold = 0.1f;
    [SerializeField] float _airTime;
    float _squishTimer;

    void Awake() {
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
        _rb.velocity = Vector3.zero;
        Vector3 targetVelocity = new Vector3(20, 5, 0);
        _airTime = targetVelocity.magnitude;
        _rb.velocity = targetVelocity;
    }


    Vector3 spawn;
    void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            Shoot();
        }

        Vector3 velocity = _rb.velocity;

        Vector3 scale = Vector3.one;
        scale.x = Mathf.Clamp(velocity.magnitude * 0.15f, 1, 3);

        //Negative Check
        if (velocity.magnitude < 0) {
            scale.x = Mathf.Clamp(scale.x, -1, -10);
        }

        transform.right = Vector3.Lerp(transform.right, velocity, Time.deltaTime * _smoothing);

        if (_squishTimer <= 0) {
            _ballHolder.localScale = scale;
        }
        else {
            _squishTimer -= Time.deltaTime;
        }

        if (_airTime < 0) {
            velocity.x = velocity.x * 0.9f;
        }
        else {
            _airTime -= Time.deltaTime;
        }

        _rb.velocity = velocity;
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
