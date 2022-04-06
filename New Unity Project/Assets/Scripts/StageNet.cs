using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNet : MonoBehaviour
{
    [SerializeField] Mesh[] _netVariations; //0 norm, 1 left, 2 right
    [SerializeField] MeshFilter _renderer;
    [SerializeField] Shaker _shaker;
    [SerializeField] LineRenderer _net;
    [SerializeField] Transform[] _netPoints;
    [SerializeField] Rigidbody _netMP;
    [SerializeField] Animator _netAnim;
    bool _canRetract;
    [SerializeField] Vector3 _netDefault;

    public Shaker GetShaker()
    {
        return _shaker;
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
