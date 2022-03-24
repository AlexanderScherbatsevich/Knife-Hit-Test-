using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static bool MenuIsOpened = false;
    public static UIManager S;

    public Text appleCountText;

    [Header("GamePanel")]
    public Transform gamePanel;
    public Text scoreCountText;
    public Text stageGP;
    public GameObject[] knivesCount;

    [Header("MenuPanel")]
    public Transform menuPanel;
    public Text maxScoreText;
    public Text maxStage;

    [Header("GameOverPanel")]
    public Transform gameOverPanel;
    public Text lastStage;

    void Start()
    {
        //menuPanel.SetAsLastSibling();
        MenuIsOpened = true;
        Time.timeScale = 0f;
    }


    public void NewGame()
    {
        //SceneManager.LoadScene(0);
        menuPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        MenuIsOpened = false;
    }

    public void ExitGame()
    {
        Debug.Log("exitGame");
        Application.Quit();
    }

    public void OpenMenu()   //переделать в рестарт
    {
        menuPanel.gameObject.SetActive(true);
        menuPanel.SetAsLastSibling();
        Time.timeScale = 0f;
        MenuIsOpened = true;
    }

    public IEnumerator ShowKnivesCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
            knivesCount[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowKnivesUI(int count)
    {
        //StartCoroutine(ShowKnivesCount(count));
        for (int i = 0; i < count; i++)
        {
            knivesCount[i].SetActive(true);
          
        }
    }

    public void AddAppleCount()
    {
        int value = int.Parse(appleCountText.text);
        value++;
        appleCountText.text = value.ToString();
    }

    public void AddScoreCount()
    {
        int value = int.Parse(scoreCountText.text);
        value++;
        scoreCountText.text = value.ToString();
    }
}
