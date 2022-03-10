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
        var hurt = collision.gameObject.GetComponent<ShuttleCock>();
        if (hurt != null && hurt.GetOwner() != null && (hurt.GetFilter() == FighterFilter.both || hurt.GetOwner() != transform.root.GetComponent<FighterController>()))
        {
            float axis = -3;
            if (hurt.GetOwner() && hurt.GetOwner().GetFilter() == FighterFilter.one)
            {
                axis = 3;
            }

            if (hurt.CanKill() && GameManager.Get().KOCoroutine == null && GameManager.Get().EndGameCoroutine == null)
            {
                ProcessHurt(collision, hurt, axis);
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
