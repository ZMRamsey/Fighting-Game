using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrHandyHitBox : MonoBehaviour
{
    MrHandy _handy;
    [SerializeField] Animator _anim;
    [SerializeField] AudioClip[] _smacks;

    private void Start() {
        _handy = transform.root.GetComponent<MrHandy>();
    }

    public void OnTriggerEnter(Collider other) {
        var shuttle = other.GetComponent<ShuttleCock>();
        var rb = other.GetComponent<Rigidbody>();

        float rot = 0;
        if (rb != null) {
            if (rb.velocity.magnitude < 0) {
                rot = 20;

            }
            else if (rb.velocity.magnitude > 0) {
                rot = 340;
            }
        }

        var rota = new Vector3(0, 0, rot);
        _handy.transform.eulerAngles = rota;

        if(shuttle != null) {
            _anim.SetTrigger("stun");

            Vector3 calcVel = new Vector3(-1f, 0f, 0f);

            if (_handy.GetFilter() == shuttle.GetFilter()) {
                calcVel = shuttle.GetVelocity().normalized;
                calcVel *= shuttle.GetSpeed() + 2;
            }
            else {
                if (_handy.GetFilter() == FighterFilter.two) {
                    calcVel = new Vector3(1f, 0f, 0f);
                }
            }

            shuttle.SetVelocity(calcVel);
            _handy.AddHit();
            transform.root.GetComponent<AudioSource>().PlayOneShot(_smacks[Random.Range(0, _smacks.Length)], 1.5f);
        }

        if (_handy.MaxHits()) {
            _handy.OnDeath(false);
        }
    }
}
