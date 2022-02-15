using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] string _stageName;
    [SerializeField] Sprite _stageIcon;
    [SerializeField] float _rightBound;
    [SerializeField] float _leftBound;

    private void Awake()
    {
        
    }
}
