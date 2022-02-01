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
        if (collision.gameObject.GetComponent<ShuttleCock>())
        {
            if (collision.gameObject.GetComponent<ShuttleCock>().GetSpeedPercent() > .7f)
            {
                Debug.Log("ouchie");
                //GameManager.Get().GetCameraShaker().SetShake(0.1f, 5.0f, true);
            }
            else
            {

            }
        }
    }
}
