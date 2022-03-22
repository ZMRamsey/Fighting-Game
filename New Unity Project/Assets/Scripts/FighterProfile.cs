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

    [Header("Movelist Data")]
    [SerializeField] string _fighterDescription;
    [SerializeField] string _gimmickName;
    [SerializeField] GameObject _gimmickVideo;
    [SerializeField] string _gimmickDescription;
    [SerializeField] string _superName;
    [SerializeField] GameObject _superVideo;
    [SerializeField] string _superDescription;

    [SerializeField] int _selectedIndex;

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
        _selectedIndex = Random.Range(1, _fighterPalettes.Length);
        return _fighterPalettes[_selectedIndex];
    }

    public int GetPalleteIndex()
    {
        return _selectedIndex;
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

    public string GetFighterDesc()
    {
        return _fighterDescription;
    }

    public string GetGimmickName()
    {
        return _gimmickName;
    }

    public string GetGimmickDesc()
    {
        return _gimmickDescription;
    }

    public string GetSuperName()
    {
        return _superName;
    }

    public string GetSuperDesc()
    {
        return _superDescription;
    }
}
