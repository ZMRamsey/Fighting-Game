using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager _instance;
    protected int[,] _scores = {{0,0},{0,0},{0,0}};
    protected int _roundIndex = 0;

    //public int _roundIndex;
    //public int _p1r1s;
    //public int _p2r1s;
    //public int _p1r2s;
    //public int _p2r2s;
    //public int _p1r3s;
    //public int _p2r3s;

    void Awake()
    {
        _instance = this;
        //_scores = new int[,] {{_p1r1s,_p2r1s},{ _p1r2s,_p2r2s},{ _p1r3s,_p2r3s}};
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
    }

    void NextRound()
    {
        _roundIndex++;
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
