using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Vector3 _ballVelocity;
    [SerializeField] Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        spawn = transform.position;
    }


    Vector3 spawn;
    void Update()
    {
        if (Vector3.Distance(spawn, transform.position) > 10)
        {
            transform.position = spawn;
        }

        transform.right = _ballVelocity;
        _rb.velocity = _ballVelocity;
    }
}
