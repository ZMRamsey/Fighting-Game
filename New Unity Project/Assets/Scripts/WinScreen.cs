using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerName;
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _quote;
    public enum players { Danny, RNB };
    [SerializeField] players winnerName;
    [SerializeField] players loserName;
    TextMeshProUGUI quoteText;
    string text;

    public void Start()
    {
        _winner.SetActive(false);
        _winnerName.SetActive(false);
        _character.SetActive(false);
        _quote.SetActive(false);
    }

    public void showWinScreen()
    {
        showWinner();
        showWinnerName();
        showCharacter();
        showQuote();
    }

    public void showWinner()
    {
        _winner.SetActive(true);
    }

    public void showWinnerName()
    {
        _winnerName.SetActive(true);
    }

    public void showCharacter()
    {
        _character.SetActive(true);
    }

    public void showQuote()
    {
        SetQuoteText(winnerName, loserName);
        _quote.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.IsPressed())
        {
            showWinScreen();
        }
    }

    public void SetQuoteText(players winner, players loser)
    {
        switch (winner)
        {
            case players.Danny:
                switch (loser)
                {
                    case players.RNB:
                        text = "haha rekt skid";
                        break;
                }
                break;
            default:
                text = "oops";
                break;
        }
        _quote.GetComponent<TextMeshProUGUI>().text = text;
    }
}
