using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter", menuName = "Badminton/FighterProfile", order = 1)]
public class FighterProfile : ScriptableObject
{
    [SerializeField] string _fighterName;
    [SerializeField] GameObject _fighterPrefab;

    public string GetName() {
        return _fighterName;
    }

    public GameObject GetPrefab() {
        return _fighterPrefab;
    }
}
