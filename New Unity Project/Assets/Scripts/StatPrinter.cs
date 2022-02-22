using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatPrinter : MonoBehaviour
{
    public ChampStats Danny = new ChampStats(1, "Danny");
    public ChampStats Hunter = new ChampStats(2, "Hunter");
    public ChampStats Esme = new ChampStats(3, "Esme");
    public ChampStats Racket = new ChampStats(4, "Racket");
    public ChampStats Ganz = new ChampStats(5, "Ganz");

    ChampStats[] champArray = new ChampStats[5];

    //GAME
    float gameLength;
    int score1;
    int score2;

    //SHUTTLE
    float averageSpeed;
    float percentKOSpeed;
    float percentLeftSide;
    float percentLeftPossession;

    //PLAYER 1
    float p1meterUsed;
    float p1percentFullMeter;
    float p1percentInAir;
    int p1smashes;
    int p1drives;
    int p1chips;
    int p1drops;
    float p1percentOfShots;
    float p1accuracy;
    float p1timeOnEnemySide;
    float p1percentKOReturned;
    int p1knockOuts;
    int p1groundOuts;

    //PLAYER 2
    float p2meterUsed;
    float p2percentFullMeter;
    float p2percentInAir;
    int p2smashes;
    int p2drives;
    int p2chips;
    int p2drops;
    float p2percentOfShots;
    float p2accuracy;
    float p2timeOnEnemySide;
    float p2percentKOReturned;
    int p2knockOuts;
    int p2groundOuts;

    // Start is called before the first frame update
    void Start()
    {
        champArray = new ChampStats[5] { Danny, Hunter, Esme, Racket, Ganz };
    }

    public void RecordGame(float length, int[,] scores, int roundsp1, int roundsp2, string p1, string p2)
    {
        gameLength = length;
        int p1victory;

        if (roundsp1 > roundsp2) { p1victory = 1; }
        else { p1victory = 0; }

        for (int i = 0; i < 5; i++)
        {
            if (p1 == champArray[i].champName)
            {
                champArray[i].SetPickRate(true);
                champArray[i].UpdateData(p1victory, p2);
            }
            else
            {
                champArray[i].SetPickRate(false);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (p2 == champArray[i].champName)
            {
                champArray[i].SetPickRate(true);
                champArray[i].UpdateData(p1victory, p1);
            }
            else
            {
                champArray[i].SetPickRate(true);
            }
        }


        PrintToCSV();
    }

    void PrintToCSV()
    {
        string gameStat = "Assets/Balancing/GameStats.csv";
        string champStat = "Assets/Balancing/CharacterStats.csv";
        string[] champStatHolder = File.ReadAllLines(champStat);

        using (StreamWriter gWriter = new StreamWriter(gameStat))
        {
            gWriter.WriteLine(GetPrintout());

            gWriter.Close();
            gWriter.Dispose();
        }

        using (StreamWriter cWriter = new StreamWriter(champStat))
        {
            cWriter.WriteLine(champStatHolder[0]);
            cWriter.WriteLine(Danny.GetPrintout());
            cWriter.WriteLine(Hunter.GetPrintout());
            cWriter.WriteLine(Esme.GetPrintout());
            cWriter.WriteLine(Racket.GetPrintout());
            cWriter.WriteLine(Ganz.GetPrintout());
        }
    }



    public string GetPrintout()
    {
        return "bleh";
    }
}

public class ChampStats : MonoBehaviour
{
    public string champName;
    int champID;
    float winRate;
    float winRateDanny;
    float winRateHunter;
    float winRateEsme;
    float winRateRacket;
    float winRateGanz;
    float pickRate;
    float gimmickUsage;
    float specialEffectiveness;
    float highSpeedPoints;
    float overallSmash;
    float overallDrive;
    float overallChip;
    float overallDrop;

    public ChampStats(int champNo, string name)
    {
        champID = champNo;
        champName = name;
        GetStatsFromCSV();
    }

    float RateCalc(float currentRate, int result)
    {
        return (currentRate + result) / 2;
    }

    public void UpdateData(int victory, string opponent)
    {
        SetWinRate(victory, opponent);
    }

    public void SetWinRate(int result, string champ)
    {
        winRate = RateCalc(winRate, result);

        switch (champ)
        {
            case "":
                //No champ
                break;

            case "Danny": //Danny
                winRateDanny = RateCalc(winRateDanny, result);
                break;

            case "Hunter": //Hunter
                winRateHunter = RateCalc(winRateHunter, result);
                break;

            case "Esme": //Esme
                winRateEsme = RateCalc(winRateEsme, result);
                break;

            case "Racket": //Racket
                winRateRacket = RateCalc(winRateRacket, result);
                break;

            case "Ganz": //Ganz
                winRateGanz = RateCalc(winRateGanz, result);
                break;
        }
    }

    public void SetPickRate(bool chosen)
    {
        if (chosen) { RateCalc(pickRate, 1); }
        else { RateCalc(pickRate, 0); }
    }

    public void SetShotRatio(int smash, int drive, int chip, int drop)
    {
        overallSmash = smash;
        overallDrive = drive / smash;
        overallChip = chip / smash;
        overallDrop = drop / smash;
    }


    public void GetStatsFromCSV()
    {
        string[] champStatHolder = File.ReadAllLines("Assets/Balancing/CharacterStats.csv");
        string[] statArray = champStatHolder[champID].Split(',');

        winRate = float.Parse(statArray[1]);
        winRateDanny = float.Parse(statArray[2]);
        winRateHunter = float.Parse(statArray[3]);
        winRateEsme = float.Parse(statArray[4]);
        winRateRacket = float.Parse(statArray[5]);
        winRateGanz = float.Parse(statArray[6]);
        pickRate = float.Parse(statArray[7]);
        //Gimmick
        //Special
        highSpeedPoints = float.Parse(statArray[10]);
        overallSmash = float.Parse(statArray[11]);
        overallDrive = float.Parse(statArray[12]);
        overallChip = float.Parse(statArray[13]);
        overallDrop = float.Parse(statArray[14]);
    }

    public string GetPrintout()
    {
        string printout = champName + "," + winRate + "," + winRateDanny + "," + winRateHunter + "," + winRateEsme + "," + winRateRacket + "," + winRateGanz + "," + pickRate + "0,0,"
            + highSpeedPoints + "," + overallSmash + "," + overallDrive + "," + overallChip + "," + overallDrop;

        return printout;
    }
}

