using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerNameO;
    [SerializeField] TextMeshProUGUI _winnerName;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _quote;
    //public string winnerName;
    string text;

    public void Awake()
    {
        //_winnerName.text = GameManager.Get().GetComponent<GameSettings>().GetFighterOneProfile().GetName().ToUpper();
        _winnerName.text = "Danny";
        int[,] scores = ScoreManager.Get().GetScores();
        int index = ScoreManager.Get().GetCurrentRound() - 1;
        for (int i = 0; i < ScoreManager.Get().GetCurrentRound(); i++)
        {
            _score.text += "Round " + (i+1) + ": " + scores[i, 0] + " - " + scores[i, 1] + "\n";
        }
    }

    public void showWinScreen(string winfighter, string losefighter)
    {
        showWinner();
        showWinnerName();
        showCharacter();
        showQuote(winfighter, losefighter);
    }

    public void showWinner()
    {
        _winner.SetActive(true);
    }

    public void showWinnerName()
    {
        _winnerNameO.SetActive(true);
    }

    public void showCharacter()
    {
        _character.SetActive(true);
    }

    public void showQuote(string winner, string loser)
    {
        SetQuoteText(winner, loser);
        _quote.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.IsPressed())
        {
            showWinScreen("Danny", "Hunter Blaze");
        }
    }

    public void SetQuoteText(string winner, string loser)
    {
        switch (winner)
        {
            case "Danny":
                switch (loser)
                {
                    case "Hunter Blaze":
                        text = "haha rekt skid";
                        break;

                    default:
                        text = "musta been a mirror match";
                        break;
                }
                break;

            case "Hunter Blaze":
                switch (loser)
                {
                    case "Danny":
                    text = "imagine not being called Hunter. loser";
                    break;

                    default:
                        text = "musta been a mirror match";
                        break;
                }
                break;

            default:
                text = "oops";
                break;
        }
        _quote.GetComponent<Scroller>().text = text;
    }
}
