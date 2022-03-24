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
    public StageDefinition[] StageDef;
    //public delegate void OnTouch();
    public static event Action OnTouched;
    public static Action <Collider2D> OnHitted;
    [HideInInspector]
    public int knivesCount;

    private void Awake()
    {
        
    }
    void Start()
    {
         
        OnHitted += CheckCollider;
        CreateStage(0);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTouched?.Invoke();       
    }

    public void CreateKnife()
    {      
        GameObject go = Instantiate(prefabKnife, new Vector2(0, -6.5f), Quaternion.identity);
        go.transform.position = Vector2.Lerp(transform.position, new Vector2(0, -4.5f), 0.9f);        
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
                //CreateKnife();
                //go.GetComponent<Apple>().Break();               
                var tgo = Instantiate(prefabAppleDestroyed,
                    go.transform.position,go.transform.rotation);
                Destroy(go);

                break;
            case "Knife":
                //CreateKnife();
                break;
            default:
                break;
        }
    }

    public void CreateStage(int stageIndex)
    {
        var target = Instantiate(StageDef[stageIndex].Target);
        CreateKnife();
        knivesCount = StageDef[stageIndex].freeKnivesCount;
        //UIManager.S.ShowKnivesUI(knivesCount);
        
    }

    private void OnDisable()
    {
        OnHitted -= CheckCollider;
    }
}

public enum StageType
{
    stage,
    boss,
}

[System.Serializable]
public class StageDefinition
{
    public StageType type = StageType.stage;
    public string stageName;                       
    public GameObject Target;
    public GameObject prefabTargetDestroyed;
    [Range(1,10)]
    public int freeKnivesCount = 0;
    public Sprite newKnifeSkin;
}