using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider collision) {
        var shuttleCock = collision.gameObject.GetComponent<ShuttleCock>();
        var self = transform.root.GetComponent<FighterController>();
        if (shuttleCock != null && (shuttleCock.GetFilter() == FighterFilter.both || shuttleCock.GetFilter() != self.GetFilter()))
        {
            float axis = -3;
            if (shuttleCock.GetOwner() && shuttleCock.GetOwner().GetFilter() == FighterFilter.one)
            {
                axis = 3;
            }

            if (shuttleCock.CanKill() && GameManager.Get().KOCoroutine == null && GameManager.Get().EndGameCoroutine == null)
            {
                ProcessHurt(collision, shuttleCock, axis);
            }
        }
    }

    public virtual void ProcessHurt(Collider collision, ShuttleCock hurt, float axis)
    {
        Vector3 prevVelocity = hurt.GetComponent<Rigidbody>().velocity;
        GameManager.Get().GetCameraShaker().SetShake(0.1f, 5.0f, true);
        hurt.Bounce(axis);
        transform.root.GetComponent<FighterController>().KO(prevVelocity);
        ScoreManager.Get().UpdateScore(transform.root.GetComponent<FighterController>().GetFilter().ToString(), "KO");
        GameManager.Get().KOEvent();
    }
}
