using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public enum player { danny, hunter, raket, esme, ganz};
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
                        text = "Don’t worry, I’ll be the best badminton champion the world has ever seen!";
                        break;

                    case player.raket:
                        text = "You don’t need these tricks to win, you just need to believe in the badminton spirit!";
                        break;

                    case player.esme:
                        text = "Those sure are some neat moves, but nothing beats the feeling of real badminton!";
                        break;

                    case player.ganz:
                        text = "Woah! You were a goose this whole time? What a plot twist!";
                        break;

                    default:
                        text = "I’ve practiced in front of a mirror, but this is on a whole other level!";
                        break;
                }
                break;

            case player.hunter:
                switch (loser)
                {
                    case player.danny:
                        text = "You’re right to be jealous of me. I’m just naturally better than you, lil man.";
                        break;

                    case player.raket:
                        text = "placeholder.txt";
                        break;

                    case player.esme:
                        text = "Are you a parking ticket? Because you got FINE written all over you. Wait, come back I have better ones.";
                        break;

                    case player.ganz:
                        text = "placeholder.txt";
                        break;

                    default:
                        text = "I’ve got a copyright on all my moves, you’re gonna hear from my lawyers real soon punk.";
                        break;
                }
                break;

            case player.raket:
                switch (loser)
                {
                    case player.danny:
                        text = "placeholder.txt";
                        break;

                    case player.hunter:
                        text = "placeholder.txt";
                        break;

                    case player.esme:
                        text = "placeholder.txt";
                        break;

                    case player.ganz:
                        text = "placeholder.txt";
                        break;

                    default:
                        text = "placeholder.txt";
                        break;
                }
                break;

            case player.esme:
                switch (loser)
                {
                    case player.danny:
                        text = "placeholder.txt";
                        break;

                    case player.hunter:
                        text = "Do I have your attention now Hunter? Are you scared? You should be terrified.";
                        break;

                    case player.raket:
                        text = "tennis is garbage kid";
                        break;

                    case player.ganz:
                        text = "tennis is garbage kid";
                        break;

                    default:
                        text = "I’m not afraid of you, I know you’re just in my head.";
                        break;
                }
                break;

            case player.ganz:
                switch (loser)
                {
                    case player.danny:
                        text = "placeholder.txt";
                        break;

                    case player.hunter:
                        text = "HONK (You humans could never hope to stay the dominant species forever if this is the best you have to offer).";
                        break;

                    case player.raket:
                        text = "HONK (Foolish child, you play with simple toys while I reshape our very world).";
                        break;

                    case player.esme:
                        text = "HONK (Your powers intrigue me young witch, what a goose you could have been. Alas, your failure only proves that you are the inferior species).";
                        break;

                    default:
                        text = "HONK (I assume you have arrived from a parallel timeline? You may return there now and report your failures at the wings of the one true Ganz)!";
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
