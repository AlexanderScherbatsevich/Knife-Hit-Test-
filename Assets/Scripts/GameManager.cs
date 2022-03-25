using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    //public static bool isKnifeInTarget = false;
    public static bool isGameOver = false;
    public static GameManager S;
    public GameObject prefabKnife;
    public GameObject prefabAppleDestroyed;
    public GameObject prefabSparks;
    public StageData[] stageData;
    //public delegate void OnTouch();
    public static event Action OnTouched;
    public static event Action GameWin;
    public static Action <Collider2D> OnHitted;
    [HideInInspector]
    public int knivesCount;
    [HideInInspector]
    public int nextStage = 0;
    [HideInInspector]
    public GameObject target;
    public float delayToTouch = 0.2f;
    void Start()
    {
        OnHitted += CheckCollider;
        CreateStage(stageData[0]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTouched?.Invoke();       
    }

    public void CreateKnife()
    {
        if (knivesCount > 0)
        {
            knivesCount--;
            var go = (GameObject)Instantiate(prefabKnife, 
                new Vector2(0, -6f), Quaternion.identity);
            LeanTween.moveLocal(go, new Vector2(0, -3f),
                0.2f).setDelay(delayToTouch).setEase(LeanTweenType.easeOutCubic);

        }
        else if (knivesCount <= 0)
        {
            DestroyTarget();
            CreateStage(stageData[nextStage]);
            GameWin?.Invoke();
        }
 
    }

    public void CheckCollider(Collider2D collision)
    {
        var go = collision.gameObject;
        string tag = collision.gameObject.tag;
        switch (tag)
        {
            case "Target":
                CreateKnife();
                var sGO = Instantiate(prefabSparks);
                sGO.GetComponent<ParticleSystem>().Play();
                Destroy(sGO, 1f);                
                break;
            case "Apple":              
                Instantiate(prefabAppleDestroyed,
                    go.transform.position,go.transform.rotation);
                Destroy(go);
               
                break;
            case "Knife": 
                
                break;
            default:
                break;
        }
    }

    private void CreateStage(StageData stage)
    {
        target = Instantiate(stage.Target);
        //target.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(target, new Vector3(2f, 2f, 1f),
                0.3f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        knivesCount = stage.freeKnivesCount;
        CreateKnife();
        //stageName.text = stage.stageName;
        //ShowKnivesUI(knivesCount);
        nextStage++;
    }


    private void OnDisable()
    {
        OnHitted -= CheckCollider;
    }

    public void DestroyTarget()
    {
        //Instantiate(prefabAppleDestroyed,go.transform.position, go.transform.rotation);
        Destroy(target);
    }
}

//public enum StageType
//{
//    stage,
//    boss,
//}

//[System.Serializable]
//public class StageDefinition
//{
//    public StageType type = StageType.stage;
//    public string stageName;
//    public GameObject Target;
//    public GameObject prefabTargetDestroyed;
//    [Range(1, 10)]
//    public int freeKnivesCount = 0;
//    public Sprite newKnifeSkin;
//}



