using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerNameO;
    [SerializeField] TextMeshProUGUI _winnerNameT;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _quote;
    //public string winnerName;
    string text;


    public void showWinScreen(string winfighter, string losefighter)
    {
        showWinner();
        showWinnerName(winfighter);
        showCharacter();
        showQuote(winfighter, losefighter);
    }

    public void showWinner()
    {
        _winner.SetActive(true);
    }

    public void showWinnerName(string winnerName)
    {
        //winnerName = FindObjectOfType<GameManager>().GetComponent<GameSettings>().GetFighterOneProfile().GetName();
        _winnerNameT.text = winnerName.ToUpper();
        _score.text = "Round 1: 11 - 8\nRound 2: 7 - 11\nRound 3: 11 - 6"; //+ FindObjectOfType<GameManager>().GetComponent<GameSettings>().GetFighterOneProfile().GetR1Score()"";
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
