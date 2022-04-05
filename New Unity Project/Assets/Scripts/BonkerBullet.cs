using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkerBullet : MonoBehaviour
{
    Vector3 _lastPosition;
    HitMessage _message;
    [SerializeField] LayerMask _layers;

    void Start() {
        ResetBullet(null);
    }

    public void ResetBullet(HitMessage message) {
        _lastPosition = transform.position;
        _message = message;
    }

    void Update() {
        RaycastHit hit;
        Vector3 direction = _lastPosition - transform.position;


        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(_lastPosition, transform.position), _layers)) {
            var shuttle = hit.transform.root.GetComponent<ShuttleCock>();

            if (shuttle != null && _message != null) {
                transform.position = shuttle.transform.position;
                SuccessfulHit();
                GameManager.Get().GetFighter(_message.sender).OnSuccessfulHit(hit.point, _message.direction, false, _message.shot, false, null);
                shuttle.Shoot(_message);
            }
        }

        _lastPosition = transform.position;
    }

    void SuccessfulHit() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
