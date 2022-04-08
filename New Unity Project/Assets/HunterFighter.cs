using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterFighter : FighterController
{
    [SerializeField] LineRenderer _rope;
    [SerializeField] GameObject _lion;
    [SerializeField] AnimationClip _lionClip;
    Transform _tetheredShuttle;
    bool _pullBack;
    float _tetheredTimer;
    float _canTether;
    float _range = 7;
    bool _superProcess;
    float _superLenghth;

    public override void InitializeFighter() {
        base.InitializeFighter();

        if (GetFilter() == FighterFilter.two) {
            _lion.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
    }

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _superProcess = true;
        _superLenghth = _lionClip.length;
        //_lion.SetActive(true);
    }

    public override void OnSuperEnd(bool instant) {
        base.OnSuperEnd(instant);
        _superProcess = false;
        //_lion.SetActive(false);
    }

    public override void OnFighterUpdate() {
        if (_superProcess)
        {
            var shuttle = GameManager.Get().GetShuttle().transform;
            var rb = shuttle.GetComponent<Rigidbody>();

            _rope.SetPosition(0, transform.position);
            _rope.SetPosition(1, shuttle.transform.position);

            rb.AddForce(100f * (transform.position - shuttle.position));
            rb.velocity *= 0.9f;

            if (Vector3.Distance(shuttle.position, transform.position) < 1)
            {
                _canTether = 0;
                GameManager.Get().GetShuttle().BoundToPlayer(this);
                _superProcess = false;
            }
        }

        if (_tetheredShuttle != null) {
            _rope.SetPosition(0, transform.position);
            _rope.SetPosition(1, _tetheredShuttle.transform.position);

            var shuttle = _tetheredShuttle.GetComponent<ShuttleCock>();
            var rb = _tetheredShuttle.GetComponent<Rigidbody>();

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

            if (_tetheredTimer > 0) {
                _tetheredTimer -= Time.deltaTime;
                if (_tetheredTimer <= 0) {
                    _tetheredShuttle = null;
                }
            }
        }

        if (_canTether > 0) {
            _canTether -= Time.deltaTime;
        }

        //if (_superLenghth > 0 && _superProcess) {
        //    _superLenghth -= Time.deltaTime;
        //    if (_superLenghth <= 0) {
        //        OnSuperEnd(false);
        //        _superProcess = false;
        //    }
        //}

        _rope.enabled = _tetheredShuttle != null || _superProcess;

    }

    public void UnTether() {
        _tetheredTimer = 0;
        _tetheredShuttle = null;
    }

    public override void OnSuccessfulHit(Vector3 point, Vector3 dir, bool big, ShotType shot, bool isGrab, ShuttleCock shuttle) {
        base.OnSuccessfulHit(point, dir, big, shot, isGrab, shuttle);

        if (shuttle != null && isGrab && _canTether <= 0) {
            _tetheredShuttle = shuttle.transform;
            _tetheredTimer = 2.0f;
            _canTether = 5.0f;
        }

        _pullBack = false;

    }
}
