using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        var hurt = collision.gameObject.GetComponent<ShuttleCock>();
        if (hurt != null && (hurt.GetOwner() != transform.root.GetComponent<FighterController>()))
        {
            if (hurt.GetSpeedPercent() >= .7f)
            {
                GameManager.Get().GetCameraShaker().SetShake(0.1f, 5.0f, true);
                ScoreManager.Get().UpdateScore(transform.root.GetComponent<FighterController>().GetFilter().ToString());
                GameManager.Get().KOEvent();
            }
            else if (hurt.GetSpeedPercent() >= .3f)
            {
                Debug.Log("Sonic_Spring_Noise.MP3");
                transform.root.GetComponent<FighterController>().SetFighterStance(FighterStance.blow);
                //hurt.Shoot(hurt.GetVelocity() / -10, new Vector3(), false, false, FighterFilter.both);
                hurt.Bounce(collision.transform.position);
                transform.root.GetComponent<FighterController>().ReduceMeter(5f);
                hurt.SetOwner(transform.root.GetComponent<FighterController>());
            }
            else
            {
                Debug.Log("We Tech Those");
            }
        }
    }
}
