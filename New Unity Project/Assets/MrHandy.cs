using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrHandy : MonoBehaviour
{
    [SerializeField] float _floatHeight;
    [SerializeField] float _floatSpeed = 4;
    Rigidbody _rb;
    [SerializeField] FighterFilter _filter;
    [SerializeField] int _hitThreshold = 10;
    [SerializeField] Transform _scaler;
    [SerializeField] float _speedCap = 4;
    float _buldtime;
    int hits;
    // Start is called before the first frame update

    public void ResetHandy(FighterFilter filter) {
        _rb = GetComponent<Rigidbody>();
        hits = 0;
        _buldtime = 0;
        _filter = filter;
        _scaler.transform.localScale = Vector3.one * 0.2f;

        float x = 1;
        if(filter == FighterFilter.one) {
            x = -1;
        }
        _rb.velocity = new Vector3(x * 2, 7, 0);
    }

    public void AddHit() {
        hits++;
    }

    public bool MaxHits() {
        return hits >= _hitThreshold;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_buldtime > 1.0f) {
            var y = GameManager.Get().GetShuttle().transform.position.y;
            y = Mathf.Clamp(y, 4, 9f);
            _floatHeight = y;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, _floatHeight)) {
                _rb.AddForce(transform.up * _floatSpeed);
            }

            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _speedCap);

            var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
            _rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * _floatSpeed * 2);

            _rb.angularVelocity *= 0.98f;
        }
        else {
            _buldtime += Time.deltaTime;
        }

        _scaler.transform.localScale = Vector3.MoveTowards(_scaler.transform.localScale, Vector3.one, Time.deltaTime * 2f);
    }

    public FighterFilter GetFilter() {
        return _filter;
    }
}
