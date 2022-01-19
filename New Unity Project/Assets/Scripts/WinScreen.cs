using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerName;
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _quote;

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
        _quote.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.IsPressed())
        {
            showWinScreen();
        }
    }
}
