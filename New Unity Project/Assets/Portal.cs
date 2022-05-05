using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Vector3 _teleportPosition;

    void OnTriggerEnter(Collider other) {
        var shuttle = other.GetComponent<ShuttleCock>();
        if(shuttle != null && ((_teleportPosition.x > 0 && shuttle.GetVelocity().x < 0) || (_teleportPosition.x < 0 && shuttle.GetVelocity().x > 0))) {
            shuttle.transform.position = new Vector3(_teleportPosition.x, shuttle.transform.position.y, 0);
        }
    }
}
