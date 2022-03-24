using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool MenuIsOpened = false;

    public Transform menuPanel;

    private void Start()
    {

        //if (PlayerPrefs.HasKey("MusicState"))
        //{
        //    _musicState = (PlayerPrefs.GetInt("MusicState") != 0);
        //}
        //PlayerPrefs.SetFloat("MusicState", (_musicState ? 1 : 0));

        //if (PlayerPrefs.HasKey("SoundState"))
        //{
        //    _soundState = (PlayerPrefs.GetInt("SoundState") != 0);
        //}
        //PlayerPrefs.SetFloat("SoundState", (_soundState ? 1 : 0));

        //musicToggle.isOn = true;
        //soundToggle.isOn = true;


        //musicTurn();
        //soundTurn();
        menuPanel.SetAsLastSibling();
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

}
