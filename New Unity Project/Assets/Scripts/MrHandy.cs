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
    [SerializeField] ParticleSystem _death;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] AudioClip[] _spawnVoiceSounds;
    [SerializeField] AudioClip[] _deathVoiceSounds;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] SpriteRenderer _rgbRenderer;
    bool _frozen;
    float _buldtime;
    int hits;
    // Start is called before the first frame update

    public void SetPallete(Material pallete) {
        if(pallete == null) {
            _rgbRenderer.enabled = false;
        }
        else {
            _rgbRenderer.enabled = true;
            _rgbRenderer.material = pallete;
        }
    }

    private void Start() {
        _death.transform.SetParent(null);
    }

    public void ResetHandy() {
        _rb = GetComponent<Rigidbody>();
        hits = 0;
        _buldtime = 0;
        _scaler.transform.localScale = Vector3.one * 0.2f;

        float x = 1;
        if (_filter == FighterFilter.one) {
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

    public void OnSpawn(FighterFilter filter) {
        _filter = filter;

        if (filter == FighterFilter.one) {
            _renderer.material = GameManager.Get().GetRedOutline();
        }

        if (filter == FighterFilter.two) {
            _renderer.material = GameManager.Get().GetBlueOutline();
        }
    }

    public void OnDeath(bool instant) {
        if (!instant) {
            _death.transform.position = transform.position;
            _death.Play();
            _death.GetComponent<AudioSource>().PlayOneShot(_deathSound);
            _death.GetComponent<AudioSource>().PlayOneShot(_deathVoiceSounds[UnityEngine.Random.Range(0, _deathVoiceSounds.Length)], 4);
        }
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        GetComponent<AudioSource>().PlayOneShot(_spawnSound);
        GetComponent<AudioSource>().PlayOneShot(_spawnVoiceSounds[UnityEngine.Random.Range(0, _spawnVoiceSounds.Length)], 4);
    }

    // Update is called once per frame
    void FixedUpdate() {
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

            _rb.angularVelocity = Vector3.ClampMagnitude(_rb.angularVelocity, _speedCap);
        }
        else {
            _buldtime += Time.deltaTime;
        }

        _scaler.transform.localScale = Vector3.MoveTowards(_scaler.transform.localScale, Vector3.one, Time.deltaTime * 2f);
    }

    public void ForceFreeze() {
        _rb.isKinematic = true;
        _animator.speed = 0;
    }

    public void ForceUnfreeze() {
        _rb.isKinematic = false;
        _animator.speed = 1;
    }

    public FighterFilter GetFilter() {
        return _filter;
    }
}
