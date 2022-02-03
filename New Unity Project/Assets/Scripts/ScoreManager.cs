using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager _instance;
    protected int[,] _scores = {{0,0},{0,0},{0,0}};
    protected int _roundIndex = 0;

    void Awake()
    {
        _instance = this;
    }

    public static ScoreManager Get()
    {
        return _instance;
    }

    public void UpdateScore(string scorer)
    {
        int scorerN = 0;
        if (scorer == "two")
        {
            scorerN = 1;
        }
        _scores[_roundIndex, scorerN]++;
        Debug.Log("Round " + (_roundIndex+1) + " Score: " + _scores[_roundIndex, 0] + " - " + _scores[_roundIndex, 1]);
        if (_scores[_roundIndex, scorerN] == 11)
        {
            NextRound();
        }
    }

    void NextRound()
    {
        if (_roundIndex != 2)
        {
            _roundIndex++;
        }
    }

    public int[,] GetScores()
    {
        return _scores;
    }

    public int GetCurrentRound()
    {
        return _roundIndex+1;
    }
}
