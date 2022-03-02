using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNet : MonoBehaviour
{
    public void OnHit() {
        GetComponent<Animator>().SetTrigger("Impact");
        GameManager.Get().GetShuttle().SetOwner(FighterFilter.both);
    }
}
