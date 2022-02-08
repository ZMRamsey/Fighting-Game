using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager _instance;
    protected int[,] _scores = {{0,0},{0,0},{0,0}};
    protected int _roundIndex = 0;
    protected int _lastScorer = 0;

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
        SetLastScorer(scorerN);
    }

    void NextRound()
    {
        if (_roundIndex != 2)
        {
            Debug.Log("Round: " + GetCurrentRound() + " Was Won By Player" + DecideRoundWinner().ToString() + " " +_scores[_roundIndex, 0] + " - " + _scores[_roundIndex, 1]);
            _roundIndex++;
        }
    }

    public int[,] GetScores()
    {
        return _scores;
    }

    public void SetScores(int[,] newScores)
    {
        _scores = newScores; 
    }

    public int GetCurrentRound()
    {
        return _roundIndex+1;
    }

    private void SetLastScorer(int scorer)
    {
        _lastScorer = scorer;
    }

    public int GetLastScorer()
    {
        return _lastScorer;
    }

    public FighterFilter DecideRoundWinner()
    {
        FighterFilter winner = FighterFilter.one;
        if (GetScores()[GetCurrentRound() - 1,1] > GetScores()[GetCurrentRound() - 1, 0])
        {
            winner = FighterFilter.two;
        }
        return winner;
    }
}
