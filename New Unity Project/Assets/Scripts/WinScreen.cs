using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum player { dan, hunter, raket, esme, ganz, ray, bonkers, sword};
public class WinScreen : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _winner;
    [SerializeField] GameObject _winnerNameO;
    [SerializeField] TextMeshProUGUI _winnerName;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _winnerNumber;
    [SerializeField] GameObject _quote;

    [SerializeField] FighterProfile[] _profiles;

    string text;

    player winner;
    player loser;



    //public Sprite Danny;
    //public Sprite Hunter;
    //public Sprite Esme;
    //public Sprite Raket;
    //public Sprite Ganz;

    //public Image imageHolder;

    //[Header("Animations")]
    //public GameObject DannyA;
    //public GameObject HunterA;
    //public GameObject EsmeA;
    //public GameObject RaketA;
    //public GameObject GanzA;
    //public GameObject RayA;

    //public GameObject winnerAnimation;

    public void Awake()
    {
        //winnerAnimation = RaketA;
        //int[,] newScores = { { _p1r1, _p2r1 }, { _p1r2, _p2r2 }, { _p1r3, _p2r3 } };
        //ScoreManager.Get().SetScores(newScores);
        int[,] scores = ScoreManager.Get().GetScores();
        //for (int i = 0; i < _roundIndex + 1; i++)
        //{
        //    _score.text += "Round " + (i+1) + ": " + scores[i, 0] + " - " + scores[i, 1] + "\n";
        //}
        _score.text = "" + scores[0, 0] + " - " + scores[0, 1];

        winner = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideThreeRoundWinner())._enumTag;
        loser = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().GetLoser(ScoreManager.Get().DecideThreeRoundWinner()))._enumTag;

        //Debug.Log("Winner is " + winner + " and loser is " + loser);
        GetMatchData(ScoreManager.Get().gameOver);
    }

    public void GetMatchData(FighterFilter winnerFilter)
    {
        _winnerName.text = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideThreeRoundWinner()).GetName();

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

    bool hasContinued;
    public void Update()
    {
        if ((GlobalInputManager.Get().GetSubmitInput() && !hasContinued && !ArcadeContinue()) ||
            GlobalInputManager.Get().GetBackInput() && !hasContinued && ArcadeContinue())
        {
            hasContinued = true;
            GameLogic.Get().ResetArcade();
            GameLogic.Get().LoadScene("Menu", "WinScreen", false);
        }
        else if (GlobalInputManager.Get().GetSubmitInput() && !hasContinued && ArcadeContinue())
        {
            GameManager.Get().KillSwitch();
            hasContinued = true;
            ArcadeAdvance();
            GameLogic.Get().LoadScene("Base", "Menu", GameLogic.Get()._type != GameType.training);
        }
        //winnerAnimation.SetActive(true);
        //imageHolder.sprite = winnerAnimation.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetQuoteText()
    {
        switch (winner)
        {
            case player.dan:
                //winnerAnimation = DannyA;
                switch (loser)
                {
                    case player.hunter:
                        text = "Don?t worry, I?ll be the best badminton champion the world has ever seen!";
                        break;

                    case player.raket:
                        text = "You don?t need these tricks to win, you just need to believe in the badminton spirit!";
                        break;

                    case player.esme:
                        text = "Those sure are some neat moves, but nothing beats the feeling of real badminton!";
                        break;

                    case player.ganz:
                        text = "Woah! You were a goose this whole time? What a plot twist!";
                        break;

                    case player.ray:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "I?ve practiced in front of a mirror, but this is on a whole other level!";
                        break;
                }
                break;

            case player.hunter:
                //winnerAnimation = HunterA;
                switch (loser)
                {
                    case player.dan:
                        text = "You?re right to be jealous of me. I?m just naturally better than you, lil man.";
                        break;

                    case player.raket:
                        text = "No damn explosives near the face, I?ve got a photo shoot tomorrow and I?m not risking the money-maker.";
                        break;

                    case player.esme:
                        text = "Are you a parking ticket? Because you got FINE written all over you. Wait, come back I have better ones.";
                        break;

                    case player.ganz:
                        text = "I?ve got to get a new agent, I?m playing against weirdos now. What are you, a chicken or something?";
                        break;

                    case player.ray:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "I?ve got a copyright on all my moves, you?re gonna hear from my lawyers real soon punk.";
                        break;
                }
                break;

            case player.raket:
                //winnerAnimation = RaketA;
                switch (loser)
                {
                    case player.dan:
                        text = "BORING! Mix it up a bit, it?s like playing against my grandad.";
                        break;

                    case player.hunter:
                        text = "Nice job boomer, shouldn?t you have retired already?";
                        break;

                    case player.esme:
                        text = "Are you doing that with holograms? Drones? Strings? Whatever, my racquet is still better.";
                        break;

                    case player.ganz:
                        text = "Sweet mech, mind if I do just a few upgrades? You might have a shot against me with a few more booster rockets.";
                        break;

                    case player.ray:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "Wait, I didn?t make a cloning machine. I call dibs on being the original.";
                        break;
                }
                break;

            case player.esme:
                //winnerAnimation = EsmeA;
                switch (loser)
                {
                    case player.dan:
                        text = "You might be good at the sport, but you?re out of your depth with us.";
                        break;

                    case player.hunter:
                        text = "Do I have your attention now Hunter? Are you scared? You should be terrified.";
                        break;

                    case player.raket:
                        text = "I?m sorry Racket, but I need you out of my way for good this time.";
                        break;

                    case player.ganz:
                        text = "Your emotions, I can feel them so strong. What happened to you?";
                        break;

                    case player.ray:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "I?m not afraid of you, I know you?re just in my head.";
                        break;
                }
                break;

            case player.ganz:
                //winnerAnimation = GanzA;
                switch (loser)
                {
                    case player.dan:
                        text = "HONK (Your fighting spirit can only take you so far, true badminton prowess comes from intellect and invention)";
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

                    case player.ray:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "HONK (I assume you have arrived from a parallel timeline? You may return there now and report your failures at the wings of the one true Ganz)!";
                        break;
                }
                break;

            case player.ray:
                //winnerAnimation = RayA;
                switch (loser)
                {
                    case player.dan:
                        text = "Placeholder.txt";
                        break;

                    case player.hunter:
                        text = "Placeholder.txt";
                        break;

                    case player.raket:
                        text = "Placeholder.txt";
                        break;

                    case player.esme:
                        text = "Placeholder.txt";
                        break;

                    case player.ganz:
                        text = "Placeholder.txt";
                        break;

                    default:
                        text = "Placeholder.txt";
                        break;
                }
                break;

            default:
                text = "oops";
                break;
        }
        _quote.GetComponent<Scroller>().text = text;
        //winnerAnimation.GetComponent<SpriteRenderer>().material = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideGameWinner()).GetPallete(GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideGameWinner()).GetPalleteIndex());
    }

    IEnumerator DisplayPause()
    {
        yield return new WaitForSeconds(0.5f);

        showWinner();
        showWinnerName();
        showQuote();
        //showCharacter();
    }

    //IEnumerator StartGame()
    //{
    //    yield return new WaitForSeconds(1);
    //    _fadeWhite.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    GameLogic.Get().LoadScene("Base", "Menu", GameLogic.Get()._type != GameType.training);
    //}

    //public void RandomCharacter()
    //{
    //    int randomSelection =  Random.Range(0, _profiles.Length-1);
    //    GameLogic.Get().GetSettings().SetFighterTwoProfile(_profiles[randomSelection]);

    //    if (_profiles[randomSelection] == GameLogic.Get().GetSettings().GetFighterOneProfile() 
    //        && GameLogic.Get().GetSettings().GetFighterOneProfile().GetPalleteIndex() == 0)
    //    {
    //        GameLogic.Get().GetSettings().GetFighterTwoProfile().;
    //    }

    //}

    public bool ArcadeContinue()
    {
        return GameLogic.Get()._type == GameType.arcade && GameLogic.Get().GetArcadePoint() != GameLogic.Get().GetSettings().GetFighterOneProfile().GetArcadeLength() - 1 && ScoreManager.Get().DecideThreeRoundWinner() == FighterFilter.one;
    }

    public void ArcadeAdvance()
    {
        GameLogic.Get().IncreaseArcadeCount();
        FighterProfile upNext = GameLogic.Get().GetSettings().GetFighterOneProfile().GetNextArcadeFight(GameLogic.Get().GetArcadePoint());
        GameLogic.Get().GetSettings().SetFighterTwoProfile(upNext);
        if (upNext.GetName() == GameLogic.Get().GetSettings().GetFighterOneProfile().GetName() && GameLogic.Get().GetSettings().GetSkinOneID() == 0)
        {
            Debug.Log(upNext.GetName() + " " + GameLogic.Get().GetSettings().GetFighterOneProfile().GetName());
            GameLogic.Get().GetSettings().SetSkinTwoID(1);
        }
    }
}
