using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private int score;

    const string FILE_DIR = "/Data/";

    const string DATA_FILE = "highScores.txt";

    string FILE_FULL_PATH; 

    public int highScoreSlot;

    public List<int> highScores;

    public int Score
    {

        get
        {

            return score;

        }

        set
        {

            score = value;

            if (isHighScore(score))
            {

                int highScoreSlot = -1;

                for (int i = 0; i < HighScores.Count; i++)
                {

                    highScoreSlot = i;

                    break;

                }
                
            }
            
            highScores.Insert(highScoreSlot, score);

            highScores = highScores.GetRange(0, 5); //show top 5 scores

            string scoreBoardText = "";

            foreach (var highScore in highScores)
            {

                scoreBoardText += highScore + "\n"; //each score ends up on its own line

            }

            highScoresString = scoreBoardText;
            
            File.WriteAllText(FILE_FULL_PATH,highScoresString); //store high scores

        }
        
    }

    string highScoresString = "";

    public List<int> HighScores
    {

        get
        {

            if (highScores == null)
            {

                highScores = new List<int>(); //creates list if one does not already exist

                highScoresString = File.ReadAllText(FILE_FULL_PATH); //pulls all data in this file

                highScoresString = highScoresString.Trim(); //cuts off extra white space

                string[] highScoreArray = highScoresString.Split("\n"); 
                //whenever it sees a line break, it'll split the string there

                for (int i = 0; i < highScoreArray.Length; i++)
                {

                    int currentScore = Int32.Parse(highScoreArray[i]); //converts string into int
                    
                    highScores.Add(currentScore); //add current score to high scores list

                }

            }

            return highScores;

        }
        
    }
    
    float timer = 0;

    float maxTime = 10;

    bool isInGame = true;

    public TextMeshProUGUI display;

    //called as soon as object created
    void Awake()
    {

        if (instance == null) //singleton
        {

            instance = this;
            
            DontDestroyOnLoad(gameObject); //lowercase = single instance, uppercase = class for game objects

        }
        else
        {
            
            Destroy(gameObject);
            
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE; //defines full path depending on operating system

    }

    // Update is called once per frame
    void Update()
    {

        if (isInGame)
        {
            
            display.text = "score: " + score + "\ntime: " + (maxTime - (int)timer); //this counts down + makes timer a whole number
            
        }
        else
        {

            display.text = "game over!" + "\nfinal score: " + score + 
                           "\nhigh scores:\n" + highScoresString;

        }

        timer += Time.deltaTime; //add fraction of a second between frames to time

        if (timer >= maxTime && isInGame) //if timer hits max
        {

            isInGame = false;

            SceneManager.LoadScene("EndScene");
            

        }

    }

    bool isHighScore(int score) //pass score into boolean
    {

        bool result = false; //

        for (int i = 0; i < HighScores.Count; i++) //simple for loop autofilled by rider
        {

            if (highScores[i] < score) //if high score in i place is less than score
            {

                return true; //this is a high score

            }
            
        }

        return false;

    }
}
