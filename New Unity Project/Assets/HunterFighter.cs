using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunterFighter : FighterController
{
    [SerializeField] LineRenderer _rope;
    [SerializeField] GameObject _lion;
    [SerializeField] AnimationClip _lionClip;
    [SerializeField] LayerMask _netDetection;
    Transform _tetheredShuttle;
    bool _pullBack;
    float _tetheredTimer;
    float _canTether;
    float _range = 7;
    bool _superProcess;
    float _superLenghth;

    [Header("UI")]
    [SerializeField] Transform _UIHolder;
    [SerializeField] Animator _iconAnimator;
    [SerializeField] GameObject _loadBarHolder;
    [SerializeField] Image _loadBar;
    [SerializeField] CanvasGroup _canvas;

    public override void InitializeFighter() {
        base.InitializeFighter();

        if (GetFilter() == FighterFilter.two) {
            _lion.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            _UIHolder.transform.localScale = new Vector3(-1, 1, 1);
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
        _canvas.alpha = CanTether() ? 1f : 0.2f;
        _loadBarHolder.SetActive(!CanTether());
        _loadBar.fillAmount =  1 - (_canTether / 5);

        if (_superProcess)
        {
            var shuttle = GameManager.Get().GetShuttle().transform;
            var rb = shuttle.GetComponent<Rigidbody>();

            _rope.SetPosition(0, transform.position);
            _rope.SetPosition(1, shuttle.transform.position);

            rb.AddForce(100f * (transform.position - shuttle.position));
            rb.velocity *= 0.9f;

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.position - shuttle.position, out hit, Mathf.Infinity, _netDetection)) {
            //    Debug.DrawRay(transform.position, transform.position - shuttle.position * hit.distance, Color.yellow);
            //    Debug.Log("Did Hit");
            //}

            if (Vector3.Distance(shuttle.position, transform.position) < 1)
            {
                GameManager.Get().GetShuttle().BoundToPlayer(this);
                OnSuccessfulHit(shuttle.position, Vector3.zero, false, ShotType.chip, true, GameManager.Get().GetShuttle());
                _tetheredTimer = 4.0f;
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
            if(_canTether <= 0) {
                _iconAnimator.SetTrigger("Flash");
            }
        }

        _rope.enabled = _tetheredShuttle != null || _superProcess;

    }

    public void UnTether() {
        _tetheredTimer = 0;
        _tetheredShuttle = null;
    }

    public override void OnSuccessfulHit(Vector3 point, Vector3 dir, bool big, ShotType shot, bool isGrab, ShuttleCock shuttle) {
        base.OnSuccessfulHit(point, dir, big, shot, isGrab, shuttle);

        if (shuttle != null && isGrab && CanTether()) {
            _tetheredShuttle = shuttle.transform;
            _tetheredTimer = 2.0f;
            _canTether = 5.0f;
        }

        _pullBack = false;

    }

    public bool CanTether() {
        return _canTether <= 0;
    }
}
