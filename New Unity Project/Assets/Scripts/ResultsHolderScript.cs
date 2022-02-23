using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsHolderScript : MonoBehaviour
{
    string winner;
    player winnerPlayer;
    player loserPlayer;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetData(string win, string lose, string winnerName)
    {
        winner = winnerName;

        string testString = win;

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

            case "Racket": //Racket
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

            case "Racket": //Racket
                loserPlayer = player.raket;
                break;

            case "Ganz": //Ganz
                loserPlayer = player.ganz;
                break;
        }
    }

    public player GetPlayer(bool winner)
    {
        if (winner) { return winnerPlayer; }
        else { return loserPlayer; }
    }

    public string GetName()
    {
        return winner;
    }


}
