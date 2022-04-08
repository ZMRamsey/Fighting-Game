using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float _height;
    [SerializeField] float _speed;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddForce(100 * (new Vector3(0, 8, 0) - _rb.transform.position));
        _rb.velocity *= 0.9f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in colliders) {
            FighterController controller = hit.GetComponent<FighterController>();
            ShuttleCock shuttle = hit.GetComponent<ShuttleCock>();

            if (controller != null && GameManager.Get().KOCoroutine == null && GameManager.Get().EndGameCoroutine == null && !controller.GetComponent<TekaFighter>()) {
                var dir = transform.position - controller.transform.position;
                dir.y = 0;
                dir.Normalize();
                controller.SetExternalForce(dir * 0.5f);
            }

            if(shuttle != null) {
                var dir = transform.position - shuttle.transform.position;
                shuttle.GetComponent<Rigidbody>().AddForce(dir * 8);
                shuttle.IncreaseOverSpeed();
            }
        }
        GameManager.Get().GetCameraShaker().SetShake(0.1f, 0.2f, true);
    }
}
