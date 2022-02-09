using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public enum player { danny, hunter, raket};
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

    public player winner;
    public player loser;

    public int _roundIndex;
    public int _p1r1;
    public int _p2r1;
    public int _p1r2;
    public int _p2r2;
    public int _p1r3;
    public int _p2r3;


    public void Awake()
    {
        int[,] newScores = { { _p1r1, _p2r1 }, { _p1r2, _p2r2 }, { _p1r3, _p2r3 } };
        ScoreManager.Get().SetScores(newScores);

        //_winnerName.text = GameManager.Get().GetComponent<GameSettings>().GetFighterOneProfile().GetName().ToUpper();
        _winnerName.text = winner.ToString().ToUpper();
        int[,] scores = ScoreManager.Get().GetScores();
        //for (int i = 0; i < ScoreManager.Get().GetCurrentRound(); i++)
        for (int i = 0; i < _roundIndex + 1; i++)
            {
            _score.text += "Round " + (i+1) + ": " + scores[i, 0] + " - " + scores[i, 1] + "\n";
        }
    }

    public void showWinScreen(player winfighter, player losefighter)
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

    public void showQuote(player winner, player loser)
    {
        SetQuoteText(winner, loser);
        _quote.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.IsPressed())
        {
            showWinScreen(winner, loser);
        }
    }

    public void SetQuoteText(player winner, player loser)
    {
        switch (winner)
        {
            case player.danny:
                switch (loser)
                {
                    case player.hunter:
                        text = "friendship or something, idk";
                        break;

                    case player.raket:
                        text = "you were fun to play, let's do this again";
                        break;

                    default:
                        text = "musta been a mirror match";
                        break;
                }
                break;

            case player.hunter:
                switch (loser)
                {
                    case player.danny:
                        text = "something about tennis or being a dick, idk";
                        break;

                    case player.raket:
                        text = "im just a bad person";
                        break;

                    default:
                        text = "musta been a mirror match";
                        break;
                }
                break;

            case player.raket:
                switch (loser)
                {
                    case player.danny:
                        text = "GGWP";
                        break;

                    case player.hunter:
                        text = "tennis is garbage kid";
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
