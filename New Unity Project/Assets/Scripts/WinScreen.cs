using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum player { danny, hunter, raket, esme, ganz};
public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerNameO;
    [SerializeField] TextMeshProUGUI _winnerName;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _winnerNumber;
    [SerializeField] GameObject _quote;
    [SerializeField] AudioSource epicGamerVictoryRoyal;
    string text;

    string winner;
    string loser;

    //public int _roundIndex;
    //public int _p1r1;
    //public int _p2r1;
    //public int _p1r2;
    //public int _p2r2;
    //public int _p1r3;
    //public int _p2r3;


    public Sprite Danny;
    public Sprite Hunter;
    public Sprite Esme;
    public Sprite Raket;
    public Sprite Ganz;
    public Image imageHolder;

    public GameObject winnerAnimation;

    public GameObject DannyA;
    public GameObject HunterA;
    public GameObject EsmeA;
    public GameObject RaketA;
    public GameObject GanzA;

    public void Awake()
    {
        winnerAnimation = RaketA;
        //int[,] newScores = { { _p1r1, _p2r1 }, { _p1r2, _p2r2 }, { _p1r3, _p2r3 } };
        //ScoreManager.Get().SetScores(newScores);
        int[,] scores = ScoreManager.Get().GetScores();
        //for (int i = 0; i < _roundIndex + 1; i++)
        //{
        //    _score.text += "Round " + (i+1) + ": " + scores[i, 0] + " - " + scores[i, 1] + "\n";
        //}
        _score.text = "" + scores[0, 0] + " - " + scores[0, 1];

        winner = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideThreeRoundWinner()).GetName();
        loser = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().GetLoser(ScoreManager.Get().DecideThreeRoundWinner())).GetName();

        Debug.Log("Winner is " + winner + " and loser is " + loser);
        GetMatchData(ScoreManager.Get().gameOver);
    }

    public void GetMatchData(FighterFilter winnerFilter)
    {
        _winnerName.text = winner;

        string playerNum = "P2";
        if (winnerFilter == FighterFilter.one) 
        { 
            playerNum = "P1"; 
        }
        _winnerNumber.text = playerNum;

        StartCoroutine("DisplayPause");
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

    public void showQuote()
    {
        SetQuoteText();
        _quote.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("InputTest");
        }
        winnerAnimation.SetActive(true);
        imageHolder.sprite = winnerAnimation.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetQuoteText()
    {
        switch (winner)
        {
            case "Danny":
                winnerAnimation = DannyA;
                switch (loser)
                {
                    case "Hunter":
                        text = "Don�t worry, I�ll be the best badminton champion the world has ever seen!";
                        break;

                    case "Raket":
                        text = "You don�t need these tricks to win, you just need to believe in the badminton spirit!";
                        break;

                    case "Esme":
                        text = "Those sure are some neat moves, but nothing beats the feeling of real badminton!";
                        break;

                    case "Ganz":
                        text = "Woah! You were a goose this whole time? What a plot twist!";
                        break;

                    default:
                        text = "I�ve practiced in front of a mirror, but this is on a whole other level!";
                        break;
                }
                break;

            case "Hunter":
                winnerAnimation = HunterA;
                switch (loser)
                {
                    case "Danny":
                        text = "You�re right to be jealous of me. I�m just naturally better than you, lil man.";
                        break;

                    case "Raket":
                        text = "No damn explosives near the face, I�ve got a photo shoot tomorrow and I�m not risking the money-maker.";
                        break;

                    case "Esme":
                        text = "Are you a parking ticket? Because you got FINE written all over you. Wait, come back I have better ones.";
                        break;

                    case "Ganz":
                        text = "I�ve got to get a new agent, I�m playing against weirdos now. What are you, a chicken or something?";
                        break;

                    default:
                        text = "I�ve got a copyright on all my moves, you�re gonna hear from my lawyers real soon punk.";
                        break;
                }
                break;

            case "Raket":
                winnerAnimation = RaketA;
                switch (loser)
                {
                    case "Danny":
                        text = "BORING! Mix it up a bit, it�s like playing against my grandad.";
                        break;

                    case "Hunter":
                        text = "Nice job boomer, shouldn�t you have retired already?";
                        break;

                    case "Esme":
                        text = "Are you doing that with holograms? Drones? Strings? Whatever, my racquet is still better.";
                        break;

                    case "Ganz":
                        text = "Sweet mech, mind if I do just a few upgrades? You might have a shot against me with a few more booster rockets.";
                        break;

                    default:
                        text = "Wait, I didn�t make a cloning machine. I call dibs on being the original.";
                        break;
                }
                break;

            case "Esme":
                winnerAnimation = EsmeA;
                switch (loser)
                {
                    case "Danny":
                        text = "You might be good at the sport, but you�re out of your depth with us.";
                        break;

                    case "Hunter":
                        text = "Do I have your attention now Hunter? Are you scared? You should be terrified.";
                        break;

                    case "Raket":
                        text = "I�m sorry Racket, but I need you out of my way for good this time.";
                        break;

                    case "Ganz":
                        text = "Your emotions, I can feel them so strong. What happened to you?";
                        break;

                    default:
                        text = "I�m not afraid of you, I know you�re just in my head.";
                        break;
                }
                break;

            case "Ganz":
                winnerAnimation = GanzA;
                switch (loser)
                {
                    case "Danny":
                        text = "HONK (Your fighting spirit can only take you so far, true badminton prowess comes from intellect and invention)";
                        break;

                    case "Hunter":
                        text = "HONK (You humans could never hope to stay the dominant species forever if this is the best you have to offer).";
                        break;

                    case "Raket":
                        text = "HONK (Foolish child, you play with simple toys while I reshape our very world).";
                        break;

                    case "Esme":
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

    IEnumerator DisplayPause()
    {
        yield return new WaitForSeconds(0.5f);

        showWinner();
        showWinnerName();
        showQuote();
        showCharacter();
    }
}
