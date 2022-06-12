using UnityEngine;
using UnityEngine.SceneManagement;

public class UITween : MonoBehaviour
{
    public GameObject panel, startButton, quitButtom, gameName, testText;

    public GameObject stageNameGO, restartButton, goToMenuButtom;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LeanTween.scale(startButton, new Vector3(1f, 1f, 1f),
                1.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

            LeanTween.scale(quitButtom, new Vector3(1f, 1f, 1f), 
                1.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

            LeanTween.moveLocal(gameName, new Vector3(0, 625.2322f, 0f),
                0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);

            LeanTween.moveLocal(testText, new Vector3(0, 380.5043f, 0f),
                0.5f).setDelay(0.7f).setEase(LeanTweenType.easeOutCubic);

        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            LeanTween.scale(restartButton, new Vector3(1f, 1f, 1f),
                1.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

            LeanTween.scale(goToMenuButtom, new Vector3(1f, 1f, 1f),
                1.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

            LeanTween.scale(stageNameGO, new Vector3(1f, 1f, 1f),
                1f).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);
        }           
    }
}
