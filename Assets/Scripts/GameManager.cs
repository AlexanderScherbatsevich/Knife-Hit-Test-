using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    //public static bool isKnifeInTarget = false;
    public static bool isGameOver = false;
    public static GameManager S;
    public GameObject prefabKnife;
    public GameObject prefabDestroyedApple;
    public GameObject prefabDestroyedTarget;
    public GameObject prefabSparks;
    public StageData[] stageData;
    public float delayBetweenThrows;
    //public delegate void OnTouch();
    public static event Action OnTouched;
    public static event Action GameWin;
    public static Action GameLost;
    public static Action <Collider2D> OnHitted;
    [HideInInspector]
    public int knivesCount;
    [HideInInspector]
    public int nextStage = 0;
    [HideInInspector]
    public GameObject target;
    private float lastThrowTime;
    private GameObject knife;
    void Start()
    {
        OnHitted += CheckCollider;
        GameLost += DestroyKnife;
        CreateStage(stageData[0]);
        lastThrowTime = Time.time;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastThrowTime < delayBetweenThrows) return;
            else 
            {
                OnTouched?.Invoke();
                lastThrowTime = Time.time;
            }
        }
    }

    public void CreateKnife()
    {
        if (knivesCount > 0)
        {           
             knife = Instantiate(prefabKnife, 
                new Vector2(0, -6f), Quaternion.identity);
            LeanTween.moveLocal(knife, new Vector2(0, -3f),
                0.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutExpo);
            knivesCount--;
        }
        else if (knivesCount <= 0)
        {
            StartToDestroyTarget();
            //if (nextStage < stageData.Length)
            //    CreateStage(stageData[nextStage]);
            //else return;

            //GameWin?.Invoke();
        }
    }

    public void DestroyKnife()
    {
        Destroy(knife, 0.5f);
    }
    public void CheckCollider(Collider2D collision)
    {
        var go = collision.gameObject;
        string tag = collision.gameObject.tag;
        switch (tag)
        {
            case "Target":
                ShakeAndFlash();
                target.GetComponent<Target>().isShake = true;
                CreateKnife();                
                var sGO = Instantiate(prefabSparks);
                sGO.GetComponent<ParticleSystem>().Play();
                Destroy(sGO, 1f);                
                break;
            case "Apple":              
                Instantiate(prefabDestroyedApple,
                    go.transform.position,go.transform.rotation);
                Destroy(go);              
                break;
            case "Knife":
                
                break;
            default:
                break;
        }
    }

    public void ShakeAndFlash()
    {        
        LeanTween.moveLocal(target, new Vector3(0, 1.6f, 0f),
                    0.05f).setEase(LeanTweenType.easeOutQuint); 
        
        LeanTween.moveLocal(target, new Vector3(0, 1.5f, 0f),
                    0.05f).setDelay(0.1f).setEase(LeanTweenType.easeOutQuint);  

        Color col = target.GetComponent<SpriteRenderer>().color;
        LeanTween.color(target, Color.white, 0.1f);
        LeanTween.color(target, col, 0.1f);        
    }

    private void CreateStage(StageData stage)
    {
        target = Instantiate(stage.Target);
        LeanTween.scale(target, new Vector3(1.5f, 1.5f, 1f),
                0.3f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        knivesCount = stage.freeKnivesCount;
        CreateKnife();
        nextStage++;
    }


    private void OnDisable()
    {
        OnHitted -= CheckCollider;
        GameLost -= DestroyKnife;

    }

    public void StartToDestroyTarget()
    {
        StartCoroutine(DestroyTarget());
    }

    private IEnumerator DestroyTarget()
    {
        Destroy(target);
        yield return null; 
        var go = Instantiate(stageData[nextStage -1].prefabDestroyedTarget);
        yield return null;
        CameraShaker.Instance.ShakeOnce(3f, 3f, 0.1f, 0.3f);
        yield return new WaitForSeconds(1.5f);
        Destroy(go);
        yield return null;
        if (nextStage < stageData.Length)
        {
            CreateStage(stageData[nextStage]);
        }
        else
        {
            GameWin?.Invoke();
            //yield return null;
            SceneManager.LoadScene(0);
        }          
        yield return null;
        GameWin?.Invoke();
    }
}




