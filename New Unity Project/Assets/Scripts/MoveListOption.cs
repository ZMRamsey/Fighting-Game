using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovelistOption", menuName = "Badminton/MoveListOption", order = 1)]
[System.Serializable]
public class MoveListOption : ScriptableObject
{
    [Header("Movelist Data")]
    [SerializeField] string _optionName;
    [SerializeField] GameObject _optionVideo;
    [SerializeField] string _optionDescription;

    public string GetOptionName()
    {
        return _optionName;
    }

    public string GetOptionDesc()
    {
        return _optionDescription;
    }
}
