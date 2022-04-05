using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterFighter : FighterController
{
	[SerializeField] LineRenderer _rope;
    [SerializeField] GameObject _lion;
    [SerializeField] AnimationClip _lionClip;
    bool _pullBack;
    bool _tethered;
    float _tetheredTimer;
    float _canTether;
    float _range = 7;
    bool _superProcess;
    float _superLenghth;

    public override void InitializeFighter() {
        base.InitializeFighter();

        if(GetFilter() == FighterFilter.two) {
            _lion.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
    }

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _superProcess = true;
        _superLenghth = _lionClip.length;
        _lion.SetActive(true);
    }

    public override void OnSuperEnd(bool instant) {
        base.OnSuperEnd(instant);
        _superProcess = false;
        _lion.SetActive(false);
    }

    public override void OnFighterUpdate() {
        var shuttle = GameManager.Get().GetShuttle();
        var rb = shuttle.GetComponent<Rigidbody>();

		_rope.SetPosition(0, transform.position);
		_rope.SetPosition(1, shuttle.transform.position);

        if (_tethered) {
            if (shuttle.IsBeingHeld() && !shuttle.IsGrabbed(GetComponent<FighterController>())) {
                UnTether();
            }

            if (Vector3.Distance(transform.position, rb.transform.position) > _range) {
                _pullBack = true;
            }

            if (_pullBack) {
                rb.AddForce(150f * (transform.position - rb.transform.position));
                rb.velocity *= 0.9f;

                if (rb.velocity.magnitude < 1f) {
                    UnTether();
                }
            }
        }

        if(_tetheredTimer > 0) {
            _tetheredTimer -= Time.deltaTime;
            if(_tetheredTimer <= 0) {
                _tethered = false;
            }
        }

        if(_canTether > 0) {
            _canTether -= Time.deltaTime;
        }

        if(_superLenghth > 0 && _superProcess) {
            _superLenghth -= Time.deltaTime;
            if(_superLenghth <= 0) {
                OnSuperEnd(false);
                _superProcess = false;
            }
        }

        _rope.enabled = _tethered;

    }

    public void UnTether() {
        _tetheredTimer = 0;
        _tethered = false;
    }

    public override void OnSuccessfulHit(Vector3 point, Vector3 dir, bool big, ShotType shot, bool isGrab) {
        base.OnSuccessfulHit(point, dir, big, shot, isGrab);

        if (isGrab && _canTether <=0) {
            _tethered = true;
            _tetheredTimer = 2.0f;
            _canTether = 5.0f;
        }

        _pullBack = false;

    }
}
