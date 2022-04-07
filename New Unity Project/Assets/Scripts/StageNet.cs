using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNet : MonoBehaviour
{
    [SerializeField] Mesh[] _netVariations; //0 norm, 1 left, 2 right
    [SerializeField] MeshFilter _renderer;
    [SerializeField] Shaker _shaker;
    [SerializeField] LineRenderer _net;
    [SerializeField] Transform[] _netPoints;
    [SerializeField] Rigidbody _netMP;
    [SerializeField] Animator _netAnim;
    [SerializeField] Animator _netScreen;
    [SerializeField] Image[] _ballSpeed;
    [SerializeField] Image _icon;
    [SerializeField] Sprite[] _adverts;
    [SerializeField] Image _advert;
    bool _canRetract;
    [SerializeField] Vector3 _netDefault;

    bool _hasWarning;
    float _coolDown;

    public Shaker GetShaker()
    {
        return _shaker;
    }

    public void OnKO(Sprite spr) {
        _netScreen.SetTrigger("KO");
        _icon.sprite = spr;
    }

    public void OnIntro() {
        _advert.sprite = _adverts[Random.Range(0, _adverts.Length)];
        _netScreen.SetTrigger("ADVERT");
    }

    public void OnIntroEnd() {
        _netScreen.SetTrigger("ADVERTEND");
    }


    private void Awake()
    {
        _netDefault = _netMP.transform.position;
    }

    void Update()
    {
        _netMP.AddForce(60 * (_netDefault - _netMP.transform.position));
        _netMP.velocity *= 0.9f;

        _net.SetPosition(0, _netPoints[0].position);
        _net.SetPosition(1, _netPoints[1].position);
        _net.SetPosition(2, _netPoints[2].position);
        _net.SetPosition(3, _netPoints[3].position);

        var speed = GameManager.Get().GetShuttle().GetSpeedPercent();

        if(speed > 0.5f && !_hasWarning && _coolDown <= 0) {
            _netScreen.SetTrigger("Flash");
            _hasWarning = true;
            _coolDown = 10f;
        }
        
        if(speed < 0.5f) {
            _hasWarning = false;
        }

        if(_coolDown > 0) {
            _coolDown -= Time.deltaTime;
        }

        if(speed > 0) {
            _ballSpeed[0].color = Color.red;
        }
        else {
            _ballSpeed[0].color = Color.clear;
        }

        if (speed > 0.16f) {
            _ballSpeed[1].color = Color.red;
        }
        else {
            _ballSpeed[1].color = Color.clear;
        }

        if(speed > 0.32f) {
            _ballSpeed[2].color = Color.red;
        }
        else {
            _ballSpeed[2].color = Color.clear;
        }

        if(speed > 0.48f) {
            _ballSpeed[3].color = Color.red;
        }
        else {
            _ballSpeed[3].color = Color.clear;
        }

        if(speed > 0.64f) {
            _ballSpeed[4].color = Color.red;
        }
        else {
            _ballSpeed[4].color = Color.clear;
        }

        if (speed > 0.8f) {
            _ballSpeed[5].color = Color.red;
        }
        else {
            _ballSpeed[5].color = Color.clear;
        }

    }

    public void NetHit(Vector3 contactForce)
    {
        if (contactForce.magnitude < 1)
        {
            return;
        }

        _netAnim.SetTrigger("Hit");
        _netMP.transform.position = new Vector3(_netDefault.x + Mathf.Clamp((contactForce.x * -1) / 2, -5, 5), _netDefault.y, _netDefault.z);
    }
}
