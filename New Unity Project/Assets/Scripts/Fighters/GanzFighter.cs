using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GanzFighter : FighterController
{
    [SerializeField] GameObject _goosePrefab;
    [SerializeField] GameObject _gooseSuit;
    [SerializeField] GameObject _parachute;
    [SerializeField] ParticleSystem _explode;
    [SerializeField] ParticleSystem _drivingLeft;
    [SerializeField] ParticleSystem _drivingRight;
    [SerializeField] ParticleSystem _helmet;
    [SerializeField] AudioClip _damage;
    [SerializeField] AudioClip _explodeSFX;
    GanzHurtbox hurtbox;
    GameObject _parachuteGans;

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
    }

    public override void OnSuperEnd(bool instant) {
    }

    public override void OnSuperEvent() {
    }

    public override void OnFighterUpdate() {
        //var drivingLeft = GetGrounded() && _rigidbody.velocity.x > 0;
        //var drivingRight = GetGrounded() && _rigidbody.velocity.x < 0;

        //if (drivingLeft && !_drivingLeft.isPlaying) {
        //    _drivingLeft.Play();
        //}

        //if (drivingRight && !_drivingRight.isPlaying) {
        //    _drivingRight.Play();
        //}

        //if (!drivingRight) {
        //    _drivingRight.Stop();
        //}

        //if (!drivingLeft) {
        //    _drivingLeft.Stop();
        //}
    }

    public override void KO(Vector3 velocity) {
        base.KO(velocity);

        _renderer.enabled = false;
        _gooseSuit.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(_explodeSFX);

        _parachuteGans.gameObject.SetActive(true);
        _parachuteGans.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);

        _explode.Play();

    }

    public override void InitializeFighter() {
        base.InitializeFighter();
        hurtbox = transform.root.GetComponentInChildren<GanzHurtbox>();

        _parachuteGans = Instantiate(_parachute, transform.position, transform.rotation);
        _parachuteGans.transform.localScale = new Vector3(0.5f,0.5f, 0.5f);
        _parachuteGans.SetActive(false);


        if (GetFilter() == FighterFilter.two) {
            _gooseSuit.transform.localScale = new Vector3(-1, 1, 1);
            _parachuteGans.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

    }

    public override void ResetFighter() {
        base.ResetFighter();
        if (hurtbox) {
            hurtbox.Repair();
        }
        SetInSuit();
        if (_parachuteGans) {
            _parachuteGans.SetActive(false);
        }
    }

    void SetInSuit() {
        _gooseSuit.SetActive(false);
        _renderer.enabled = true;
    }

    public void SetOutSuit() {
        _gooseSuit.SetActive(true);
        _helmet.Play();
        GetComponent<AudioSource>().PlayOneShot(_damage);
        _renderer.enabled = false;
    }

}
