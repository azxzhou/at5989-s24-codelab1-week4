using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //public means it can be seen everywhere
    
    private int score = 0; //private means that it can only be seen in this script

    private const string FILE_DIR = "/Logs"; //separate string variable for logs folder
    
    private const string DATA_FILE = FILE_DIR + "/highScores.txt"; //add slash so it doesnt break the file path, direct to logs folder

    string FILE_FULL_PATH;

    public int highScoreSlot;

    public List<int> highScores;

    public int Score //property is way of wrapping variable up - a stand in, can transfer priv variable to public
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

                    for (int i = 0; i < highScores.Count; i++)
                    {

                        highScoreSlot = i;

                        break;

                    }

                }

                highScores.Insert(highScoreSlot, score);

                highScores = highScores.GetRange(0, 5); //show top 5 scores

                string scoreBoardText = "";

                foreach (var highScore in highScores){}

                {

                    scoreBoardText += highScores + "\n"; //each score has its own line

                }

                highScoresString = scoreBoardText;

                File.WriteAllText(FILE_FULL_PATH, highScoresString);
                
            }

        }

    string highScoresString = "";

    public List<int> HighScores
    {

        get
        {

            if (highScores == null)
            {

                highScores = new List<int>(); //creates list if one doesnt already exist

                highScoresString = File.ReadAllText(FILE_FULL_PATH); //pulls all file data

                highScoresString = highScoresString.Trim(); //removes extra white space

                string[] highScoreArray = highScoresString.Split("\n"); //split when u see this

                for (int i = 0; i < highScoreArray.Length; i++)
                {

                    int currentScore = Int32.Parse(highScoreArray[i]); //turns string into int
                    
                    highScores.Add(currentScore); //add score to hs list

                }

            }

            return highScores;

        }
        
    }

    float timer = 0;

    float maxTime = 10;

    bool isInGame = true;

    public TextMeshProUGUI scoreText;

    public int targetScore = 5;

    public int levelNumber = 1;

    private const string PREF_KEY_HIGH_SCORE = "highScoreKey"; 
    //makes the key a variable that youre unlikely to get wrong
    private void Awake() //happens before first frame, before start - put singleton stuff here
    {

        if (instance == null) //if instance var has nothing aka no singleton
        {

            instance = this; //change to current instance
            
            DontDestroyOnLoad(gameObject);

        }
        else //if theres already a singleton of this type
        {
            
            Destroy(gameObject);
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerPrefs.SetInt(PREF_KEY_HIGH_SCORE, 3); //resets high score at beginning

        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE; //os dependent path

    }

    // Update is called once per frame
    void Update()
    {

        if (isInGame)
        {
            
            scoreText.text = "level: " + levelNumber +
                             "\nscore: " + Score +
                             "\ntarget: " + targetScore + 
                             "\ntime: " + (maxTime - (int)timer);

            if (score == targetScore)
            {

                levelNumber++;

                SceneManager.LoadScene("Level" + levelNumber);

                targetScore = Mathf.RoundToInt(targetScore + targetScore * 1.5f);
                //mult. original target score by 1.5f (add f for float)

            }

        }
        else
        {

            scoreText.text = "game over!" + 
                             "\nfinal score: " + 
                             score + 
                             "\nhigh scores: \n" + 
                             highScoresString;

        }

        timer += Time.deltaTime;

        if (timer >= maxTime && isInGame)
        {

            isInGame = false;

            SceneManager.LoadScene("EndScene");

        }

        /*if (score >= 0)
        {

            loseText.text = " ";

        }

        if (score < 0)
        {

            loseText.text = "you lose. f";

            File.WriteAllText(FILE_PATH_FAILURE, "L + ratio. hi matt");

        }*/

    }

    bool isHighScore(int score)
    {

        bool result = false;

        for (int i = 0; i < HighScores.Count; i++)
        {

            if (highScores[i] < score)
            {

                return true;

            }
            
        }

        return false;

    }
}
