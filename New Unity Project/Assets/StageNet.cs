using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNet : MonoBehaviour
{
    [SerializeField] Mesh[] _netVariations; //0 norm, 1 left, 2 right
    [SerializeField] MeshFilter _renderer;
    [SerializeField] Shaker _shaker;

    public Shaker GetShaker() {
        return _shaker;
    }

    Coroutine netCoroutine;
    public void NetHit(Vector3 contactForce) {
        if (contactForce.magnitude < 20) {
            return;
        }

        if (netCoroutine != null) {
            StopCoroutine(netCoroutine);
        }

        int starting = 1;

        if(contactForce.x > 0) {
            _renderer.mesh = _netVariations[1];
        }
        else {
            _renderer.mesh = _netVariations[2];
            starting = 2;
        }

        netCoroutine = StartCoroutine(NetReset(starting, contactForce.magnitude > 40));
    }

    IEnumerator NetReset(int starting, bool rebound) {
        GetShaker().SetShake(0.1f, 4f, true);
        if (rebound) {
            yield return new WaitForSeconds(0.05f);
            _renderer.mesh = _netVariations[0];
            yield return new WaitForSeconds(0.05f);
            if (starting == 2) {
                _renderer.mesh = _netVariations[1];
            }

            if (starting == 1) {
                _renderer.mesh = _netVariations[2];
            }
        }
        yield return new WaitForSeconds(0.1f);
        _renderer.mesh = _netVariations[0];
    }
}
