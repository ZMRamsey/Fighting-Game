using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    public Sprite Danny;
    public Sprite Hunter;
    public Sprite Esme;
    public Sprite Racket;
    public Sprite Ganz;
    public Image imageHolder;

    public void Awake()
    {
        int[,] newScores = { { _p1r1, _p2r1 }, { _p1r2, _p2r2 }, { _p1r3, _p2r3 } };
        ScoreManager.Get().SetScores(newScores);

        int[,] scores = ScoreManager.Get().GetScores();
        for (int i = 0; i < _roundIndex + 1; i++)
            {
            _score.text += "Round " + (i+1) + ": " + scores[i, 0] + " - " + scores[i, 1] + "\n";
        }

        string win = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().DecideThreeRoundWinner()).GetName();
        string lose = GameManager.Get().GetGameSettings().GetFighterProfile(ScoreManager.Get().GetLoser(ScoreManager.Get().DecideThreeRoundWinner())).GetName();
        player winnerPlayer = player.danny;
        player loserPlayer = player.danny;

        switch (win)
        {
            case "":
                //No champ
                break;

            case "Danny": //Danny
                winnerPlayer = player.danny;
                break;

            case "Hunter": //Hunter
                winnerPlayer = player.hunter;
                break;

            case "Esme": //Esme
                winnerPlayer = player.esme;
                break;

            case "Raket": //Raket
                winnerPlayer = player.raket;
                break;

            case "Ganz": //Ganz
                winnerPlayer = player.ganz;
                break;
        }

        switch (lose)
        {
            case "":
                //No champ
                break;

            case "Danny": //Danny
                loserPlayer = player.danny;
                break;

            case "Hunter": //Hunter
                loserPlayer = player.hunter;
                break;

            case "Esme": //Esme
                loserPlayer = player.esme;
                break;

            case "Raket": //Raket
                loserPlayer = player.raket;
                break;

            case "Ganz": //Ganz
                loserPlayer = player.ganz;
                break;
        }

        Debug.Log("Winner is " + winnerPlayer.ToString() + " and loser is " + loserPlayer.ToString());
        GetMatchData(winnerPlayer, loserPlayer, ScoreManager.Get().gameOver);
    }

    public void GetMatchData(player winfighter, player losefighter, FighterFilter winner)
    {
        _winnerName.text = winfighter.ToString().ToUpper();
        string playerNum = "P2";

        if (winner == FighterFilter.one) { playerNum = "P1"; }

        _winnerNumber.text = playerNum;

        switch (winfighter)
        {
            case player.danny:
                imageHolder.sprite = Danny;
                break;

            case player.hunter:
                imageHolder.sprite = Hunter;
                break;

            case player.esme:
                imageHolder.sprite = Esme;
                break;

            case player.raket:
                imageHolder.sprite = Racket;
                break;

            case player.ganz:
                imageHolder.sprite = Ganz;
                break;
        }

        showWinner();
        showWinnerName();
        showQuote(winfighter, losefighter);
        showCharacter();
    }

    public void showWinScreen(player winfighter, player losefighter)
    {
        epicGamerVictoryRoyal.Play();
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
            //showWinScreen(winner, loser);
            GetMatchData(winner, loser, FighterFilter.one);
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
                        text = "No damn explosives near the face, I’ve got a photo shoot tomorrow and I’m not risking the money-maker";
                        break;

                    case player.esme:
                        text = "Are you a parking ticket? Because you got FINE written all over you. Wait, come back I have better ones.";
                        break;

                    case player.ganz:
                        text = "I’ve got to get a new agent, I’m playing against weirdos now. What are you, a chicken or something?";
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
                        text = "BORING! Mix it up a bit, it’s like playing against my grandad";
                        break;

                    case player.hunter:
                        text = "Nice job boomer, shouldn’t you have retired already?";
                        break;

                    case player.esme:
                        text = "Are you doing that with holograms? Drones? Strings? Whatever, my racquet is still better";
                        break;

                    case player.ganz:
                        text = "Sweet mech, mind if I do just a few upgrades? You might have a shot against me with a few more booster rockets";
                        break;

                    default:
                        text = "Wait, I didn’t make a cloning machine. I call dibs on being the original";
                        break;
                }
                break;

            case player.esme:
                switch (loser)
                {
                    case player.danny:
                        text = "You might be good at the sport, but you’re out of your depth with us";
                        break;

                    case player.hunter:
                        text = "Do I have your attention now Hunter? Are you scared? You should be terrified.";
                        break;

                    case player.raket:
                        text = "I’m sorry Racket, but I need you out of my way for good this time.";
                        break;

                    case player.ganz:
                        text = "Your emotions, I can feel them so strong. What happened to you?";
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
