using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager _instance;
    protected int[,] _scores = {{0,0},{0,0},{0,0}};
    protected int _roundIndex = 0;
    protected int _lastScorer = 0;
    protected int _p1Wins = 0;
    protected int _p2Wins = 0;
    public FighterFilter gameOver = FighterFilter.both;

    void Awake()
    {
        _instance = this;
    }

    public static ScoreManager Get()
    {
        return _instance;
    }

    public void UpdateScore(string scorer, string method)
    {
        int scorerN = 0;
        if (scorer == "one")
        {
            scorerN = 1;
        }
        _scores[_roundIndex, scorerN]++;
        Debug.Log("Round " + (_roundIndex+1) + " Score: " + _scores[_roundIndex, 0] + " - " + _scores[_roundIndex, 1] + " Point Scored Via: " + method);
        if (_scores[_roundIndex, scorerN] == 11)
        {
            NextRound();
        }
        SetLastScorer(scorerN);
        GameManager.Get().ResetSuccessive();
    }

    void NextRound()
    {
        FighterFilter roundWinner = DecideRoundWinner();
        Debug.Log("Round: " + GetCurrentRound() + " Was Won By Player " + roundWinner.ToString() + " " +_scores[_roundIndex, 0] + " - " + _scores[_roundIndex, 1]);

        if (roundWinner == FighterFilter.one)
        {
            _p1Wins++;
        }
        else
        {
            _p2Wins++;
        }

        if ((_p1Wins < 2) && (_p2Wins < 2))
        {
            _roundIndex++;
        }
        else
        {
            //End Game
            gameOver = DecideThreeRoundWinner();
            Debug.Log("The winner is " + gameOver.ToString());
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

    public FighterFilter DecideThreeRoundWinner()
    {
        FighterFilter winner;

        if (_p1Wins > _p2Wins)
        {
            winner = FighterFilter.one;
        }
        else
        {
            winner = FighterFilter.two;
        }

        return winner;
    }

    //One round system
    public FighterFilter DecideGameWinner()
    {
        FighterFilter winner = FighterFilter.one;
        if (GetScores()[GetCurrentRound() - 1, 1] > GetScores()[GetCurrentRound() - 1, 0])
        {
            winner = FighterFilter.two;
        }
        return winner;
    }
}
