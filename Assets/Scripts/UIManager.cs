using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static int AppleCount = 0;
    public static int HighScore = 0;
    public static int MaxStage = 0;
    public static string LastStage = "Stage 1";
    public static UIManager S;

    public StageData[] stageData;
    public Text appleCountText;

    [Header("GamePanel")]
    public Transform gamePanel;
    public Text scoreCountText;
    public Text stageName;
    public GameObject[] knivesUI;

    [Header("MenuPanel")]
    public Transform menuPanel;
    public Text highScoreText;
    public Text maxStage;

    [Header("GameOverPanel")]
    public Transform gameOverPanel;

    [HideInInspector]
    public int nextStage = 0;
    public Text stageNameGO;
    private int knivesCount;
    private int score;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Start()
    {      
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            scoreCountText.text = "0";
            CreateStageUI(stageData[0]);
            GameManager.OnHitted += IsHitted;
            GameManager.GameWin += CreateNextStage;
            GameManager.GameLost += DelayToGameOver;
            GameManager.OnTouched += RemoveKnife;
            GameManager.GameWin += CheckHighscore;
            GameManager.GameLost += CheckHighscore;
            GameManager.GameWin += CheckMaxStage;
        }

        if (PlayerPrefs.HasKey("ApplesCount"))
        {
            AppleCount = PlayerPrefs.GetInt("ApplesCount");
        }
        PlayerPrefs.SetInt("ApplesCount", AppleCount);
        appleCountText.text = AppleCount.ToString();
        

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SetHighScoreText();
            SetMaxStageText();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2) SetLastStageText();


    }

    //=================================================BUTTONS================================================================
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("exitGame");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    //========================================================================================================================



    public void IsHitted(Collider2D col)
    {
        var go = col.gameObject;
        string tag = col.gameObject.tag;
        score = int.Parse(scoreCountText.text);

        if (tag == "Target")
        {
            score++;
            scoreCountText.text = score.ToString();
        }
        else if (tag == "Apple")
        {
            AppleCount = int.Parse(appleCountText.text);
            AppleCount++;
            appleCountText.text = AppleCount.ToString();
            PlayerPrefs.SetInt("ApplesCount", AppleCount);
        }
        else if(tag == "Knife")
        {
            //CheckHighscore(int.Parse(scoreCountText.text));
            //CheckMaxStage(nextStage - 1);
            PlayerPrefs.SetString("LastStage", stageName.text);
        }
    }

    public void DelayToGameOver()
    {
        StartCoroutine(DelayGameOver());
        //SceneManager.LoadScene(2);
    }

    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    private void CreateStageUI(StageData stage)
    {
        ShowKnivesUI(stage.freeKnivesCount );
        knivesCount = stage.freeKnivesCount;
        for (int i = 0; i < stage.freeKnivesCount; i++)
        {
            knivesUI[i].GetComponent<Image>().color = Color.white;
        }
        //scoreCountText.text = "0";
        stageName.text = stage.stageName;
        nextStage++;
    }

    public void ShowKnivesUI(int count)
    {
        for (int i = 0; i < count; i++)
        {
            knivesUI[i].SetActive(true);
        }
    }

    public void RemoveKnife()
    {
        knivesUI[knivesCount - 1].GetComponent<Image>().color = Color.black;
        knivesCount--;
    }

    private void CreateNextStage()
    {
        CreateStageUI(stageData[nextStage]);
    }

    private void CheckHighscore()
    {
        if (score > HighScore)
        {
            HighScore = score;
            PlayerPrefs.SetInt("Highscore", HighScore);
        }        
    }

    private void CheckMaxStage()
    {
        int value = nextStage - 1;
        if (value > MaxStage)
        {
            MaxStage = value;
            PlayerPrefs.SetInt("MaxStage", MaxStage);
        }
    }

    private void SetHighScoreText()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            HighScore = PlayerPrefs.GetInt("Highscore");
        }
        PlayerPrefs.SetInt("Highscore", HighScore);
        highScoreText.text = HighScore.ToString();
    }

    private void SetMaxStageText()
    {
        if (PlayerPrefs.HasKey("MaxStage"))
        {
            MaxStage = PlayerPrefs.GetInt("MaxStage");
        }
        PlayerPrefs.SetInt("MaxStage", MaxStage);
        maxStage.text = MaxStage.ToString();
    }

    private void SetLastStageText()
    {
        if (PlayerPrefs.HasKey("LastStage"))
        {
            LastStage = PlayerPrefs.GetString("LastStage");
        }
        PlayerPrefs.SetString("LastStage", LastStage);
        stageNameGO.text = LastStage.ToString();
    }

    private void OnDisable()
    {
        GameManager.OnHitted -= IsHitted;
        GameManager.GameWin -= CreateNextStage;
        GameManager.GameLost -= DelayToGameOver;
        GameManager.OnTouched -= RemoveKnife;
        GameManager.GameWin -= CheckHighscore;
        GameManager.GameLost -= CheckHighscore;
        GameManager.GameWin -= CheckMaxStage;
    }
}
