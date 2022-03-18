using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter", menuName = "Badminton/FighterProfile", order = 1)]
[System.Serializable]
public class FighterProfile : ScriptableObject
{
    [SerializeField] string _fighterName;
    [SerializeField] GameObject _fighterPrefab;
    [SerializeField] Material[] _fighterPalettes;

    [Header("VS Screen")]
    [SerializeField] Sprite _fighterIconSprite;
    [SerializeField] Sprite _fighterNameLSprite;
    [SerializeField] Sprite _fighterNameRSprite;

    public string GetName() {
        return _fighterName;
    }

    public GameObject GetPrefab() {
        return _fighterPrefab;
    }
    
    public Material GetPallete(int ID) {
        return _fighterPalettes[ID];
    }

    public Material GetRandomPallete() {
        return _fighterPalettes[Random.Range(1, _fighterPalettes.Length)];
    }

    public Sprite GetIconSprite() {
        return _fighterIconSprite;
    }

    public Sprite GetNameSprite(FighterFilter filter) {
        if(filter == FighterFilter.one) {
            return _fighterNameLSprite;
        }
        return _fighterNameRSprite;
    }
}
