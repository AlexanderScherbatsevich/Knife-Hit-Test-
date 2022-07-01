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
    public static UIManager Instance;

    public GameObject newKnife;
    public Text appleCountText;

    [Header("GamePanel")]
    public Text scoreCountText;
    public Text stageName;
    public Text youWinText;
    public GameObject[] knivesUI;

    [Header("MenuPanel")]
    public Toggle sound;
    public Toggle vibration;
    public Transform shopPanel;
    public Text highScoreText;
    public Text maxStageText;

    [Header("GameOverPanel")]  
    public Text stageNameGO;
    [HideInInspector] public int nextStage = 0;

    private int knivesCount;
    private int score = 0 ;
    private bool isShopOpen = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        AppleCount = Save.Instance.appleCount;
        appleCountText.text = AppleCount.ToString();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            highScoreText.text = Save.Instance.highScore.ToString();
            maxStageText.text = Save.Instance.maxStage.ToString();

            Instance.sound.isOn = Save.Instance.isSoundOff;
            Instance.vibration.isOn = Save.Instance.isVibrationOff;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            scoreCountText.text = "0";

            GameManager.OnCollision += OnHit;
            GameManager.GameWin += CheckHighscore;
            GameManager.GameWin += CheckMaxStage;
            GameManager.GameLost += CheckHighscore;
            GameManager.GameLost += DelayToGameOver;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
            stageNameGO.text = Save.Instance.lastStage;
    }

    #region Buttons and toggles

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

    public void OpenShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.gameObject.SetActive(isShopOpen);
    }

    public void ToggleVibration(bool isTurnOn)
    {
        Save.Instance.isVibrationOff = isTurnOn;
    }

    #endregion

    public void OnHit(Collider2D col)
    {
        if (col.CompareTag("Target"))
        {
            score++;
            scoreCountText.text = score.ToString();
        }
        else if (col.CompareTag("Apple"))
        {
            AppleCount++;
            appleCountText.text = AppleCount.ToString();
            Save.Instance.appleCount = AppleCount;
        }
        else if(col.CompareTag("Knife"))
        {           
            CheckMaxStage();
            Save.Instance.lastStage = stageName.text;
        }
    }

    public void DelayToGameOver()
    {
        StartCoroutine(LoadScene(2,1f));
    }

    private IEnumerator LoadScene(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    public void CreateStageUI(StageData stage)
    {       
        ShowKnivesUI(stage.freeKnivesCount );
        knivesCount = stage.freeKnivesCount;
        ShowNameStage(stage);
        nextStage++;
    }

    public void ShowNameStage(StageData stage)
    {
        stageName.text = stage.stageName;
        LeanTween.scale(stageName.gameObject, new Vector3(1f, 1f, 1f),
                0.3f).setEase(LeanTweenType.easeOutBack);
    }

    public void HideNameStage()
    {
        LeanTween.scale(stageName.gameObject, new Vector3(0f, 0f, 0f),
        0.3f);
    }

    public void ShowKnivesUI(int count)
    {
        foreach (var knifeUI in knivesUI)
        {
            knifeUI.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            knivesUI[i].SetActive(true);
            knivesUI[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void RemoveKnife()
    {
        knivesUI[knivesCount - 1].GetComponent<Image>().color = Color.black;
        knivesCount--;
    }

    private void CheckHighscore()
    {
        if (score > HighScore)
        {
            HighScore = score;
            Save.Instance.highScore = HighScore;
        }        
    }

    public void CheckMaxStage()
    {
        int value = nextStage ;
        MaxStage = Save.Instance.maxStage;
        if (value >= MaxStage)
        {
            MaxStage = value;
            Save.Instance.maxStage = MaxStage;
        }
    }

    public void SellKnife(int cost)
    {
        AppleCount -= cost;
        appleCountText.text = AppleCount.ToString();
        Save.Instance.appleCount = AppleCount;
    }

    public void ResetShopState()
    {
        KnifeKeeper.Instance.openedKnivesID.Clear();
        KnifeKeeper.Instance.SelectKnifeData(0);
        Save.Instance.selectedKnifeID = 0;
        Save.Instance.openedKnivesID.Clear();
        KnifeKeeper.Instance.AddOpenedKnife(0);
    }

    public void OpenNewKnife(KnifeData knife)
    {
        KnifeKeeper.Instance.AddOpenedKnife(knife.ID);

        GameObject openedKnife = Instantiate(newKnife);
        openedKnife.GetComponentInChildren<Image>().sprite = knife.knifeSkin;
        GameObject knifeSkin = openedKnife.GetComponentInChildren<Image>().gameObject;
        GameObject text = openedKnife.GetComponentInChildren<Text>().gameObject;

        LeanTween.scale(knifeSkin, new Vector3(1.2f, 1.2f, 1.2f),
                2f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(text, new Vector3(2f, 2f, 1.5f), 
            2f).setEase(LeanTweenType.easeOutExpo);


        LeanTween.scale(knifeSkin, new Vector3(0.01f, 0.01f, 1f),
                0.5f).setDelay(2f).setEase(LeanTweenType.easeOutExpo);

        LeanTween.scale(text, new Vector3(0.01f, 0.01f, 1f),
                0.5f).setDelay(2f).setEase(LeanTweenType.easeOutExpo);

        Destroy(openedKnife, 2.5f);
    }

    public void EndGame()
    {
        youWinText.gameObject.SetActive(true);
        LeanTween.scale(youWinText.gameObject, new Vector3(1f, 1f, 1f),
            1f).setEase(LeanTweenType.easeOutBack);
        StartCoroutine(LoadScene(0, 2.5f));
    }

    private void OnDisable()
    {
        GameManager.OnCollision -= OnHit;
        GameManager.GameLost -= DelayToGameOver;
        GameManager.GameWin -= CheckHighscore;
        GameManager.GameLost -= CheckHighscore;
        GameManager.GameWin -= CheckMaxStage;
    }
}
