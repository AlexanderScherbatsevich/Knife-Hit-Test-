using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static bool isGameOver = false;
    

    public static GameManager Instance;

    public GameObject prefabKnife;
    public GameObject prefabTarget;
    public GameObject prefabDestroyedApple;
    public GameObject prefabSparks;
    public GameObject prefabKnifeKeeper;

    public StageData[] stagesData;

    public float delayBetweenThrows;
    public static event Action OnTouched;
    public static event Action GameWin;
    public static Action GameLost;
    public static Action<Collider2D> OnCollision;

    [HideInInspector] public int knivesCount;
    [HideInInspector] public int nextStage = 0;
    [HideInInspector] public GameObject target;
    [HideInInspector] public StageData.StageType stageType;

    private float lastThrowTime;
    private KnifeData _knifeData;
    private KnifeData _newOpenedKnife;     
    private Queue<StageData> queueStages;
    private Queue<GameObject> queueKnives;
    private IEnumerator destoyTarget;
    private bool isVibrationOff;

    void Start()
    {

        _knifeData = KnifeKeeper.Instance.SelectKnifeData(Save.Instance.selectedKnifeID);

        isVibrationOff = Save.Instance.isVibrationOff;
        if(!isVibrationOff)
            Vibration.Init();

        OnCollision += CheckCollider;
        GameWin += StopToDestroyTarget;       

        queueStages = new Queue<StageData>(stagesData);
        CreateStage(queueStages.Dequeue());
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
        //if (Input.touchCount > 0)
        //{
        //    if (Time.time - lastThrowTime < delayBetweenThrows) return;
        //    else
        //    {
        //        OnTouched?.Invoke();
        //        lastThrowTime = Time.time;
        //    }
        //}
    }

    private Queue<GameObject> CreateKnives(int knivesCount)
    {
        for (int i = 0; i < knivesCount; i++)
        {
            var knife = Instantiate(prefabKnife, new Vector2(0, -6f), Quaternion.identity);
            knife.GetComponent<SpriteRenderer>().sprite = _knifeData.knifeSkin;
            knife.SetActive(false);
            queueKnives.Enqueue(knife);
        }
        return queueKnives;
    }

    private void GetKnife()
    {
        var knife = queueKnives.Dequeue();
        knife.SetActive(true);
        LeanTween.moveLocal(knife, new Vector2(0, -3f), 0.2f).setEase(LeanTweenType.easeOutExpo);
        knivesCount--;
    }

    public void CheckCollider(Collider2D collision)
    {
        var go = collision.gameObject;
        string tag = collision.gameObject.tag;
        switch (tag)
        {
            case "Target":               
                if (queueKnives.Count > 0) 
                    GetKnife();
                else
                {
                    if (destoyTarget != null)
                        StopCoroutine(destoyTarget);
                    destoyTarget = ChangeStage();
                    StartCoroutine(destoyTarget);
                }
                if(stageType != StageData.StageType.boss)
                {
                    var tGO = Instantiate(prefabSparks);
                    Destroy(tGO, 1f);
                }
                if (!isVibrationOff)
                    Vibration.VibratePop();
                break;
            case "Apple":
                var destroyedApple = Instantiate(prefabDestroyedApple,
                    go.transform.position, go.transform.rotation);
                Destroy(go);
                Destroy(destroyedApple, 3f);
                break;
            case "Knife":
                var sGO = Instantiate(prefabSparks);
                Destroy(sGO, 1f);
                if (!isVibrationOff)
                    Vibration.VibratePeek();
                break;
            default:
                break;
        }
    }

    private void CreateStage(StageData stage)
    {
        UIManager.Instance.CreateStageUI(stage);
        target = Instantiate(prefabTarget);
        target.GetComponent<Target>().targetData = stage.targetData;

        _newOpenedKnife = stage.openedKnife;
        knivesCount = stage.freeKnivesCount;
        stageType = stage.type;
        AudioManager.Instance.stageType = this.stageType;

        nextStage++;

        queueKnives = new Queue<GameObject>();
        queueKnives = CreateKnives(stage.freeKnivesCount);
        GetKnife();
    }

    private void OnDisable()
    {
        OnCollision -= CheckCollider;
        GameWin -= StopToDestroyTarget;
    }

    public void StopToDestroyTarget()
    {
        if (destoyTarget != null)
            StopCoroutine(destoyTarget);
    }

    //TO DO !!!!
    //private void DestroyTarget()
    //{
    //    Destroy(target);
    //    Vibration.Vibrate();
    //    CameraShaker.Instance.ShakeOnce(5f, 5f, 0.1f, 0.3f);

    //    target.transform.DetachChildren();
    //    Target targetTemp = target.GetComponent<Target>();
    //    for (int i = 0; i < targetTemp.ItemsInTarget.Count; i++)
    //    {
    //        Rigidbody2D rb = targetTemp.ItemsInTarget[i].GetComponent<Rigidbody2D>();
    //        rb.bodyType = RigidbodyType2D.Dynamic;
    //        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
    //        rb.AddForce(new Vector2(UnityEngine.Random.Range(-1000f,1000f),
    //            UnityEngine.Random.Range(-1000f, 1000f)));
    //        Destroy(targetTemp.ItemsInTarget[i], 3f);
    //    }
    //}

    private IEnumerator ChangeStage()
    {
        yield return new WaitForSeconds(0.03f);
        Destroy(target);

        if (stageType == StageData.StageType.boss)
            AudioManager.Instance.destroyFruit.Play();
        else
            AudioManager.Instance.destroyLog.Play();

        if (!isVibrationOff)
            Vibration.Vibrate();

        CameraShaker.Instance.ShakeOnce(5f, 7f, 0.3f, 0.1f);
        UIManager.Instance.HideNameStage();
        var go = Instantiate(stagesData[nextStage -1].prefabDestroyedTarget);
        yield return new WaitForSeconds(0.5f);

        if (stageType == StageData.StageType.boss && !KnifeKeeper.Instance.openedKnivesID.Contains(_newOpenedKnife.ID))
        {
            UIManager.Instance.OpenNewKnife(_newOpenedKnife);
            AudioManager.Instance.openNewKnife.Play();
            stageType = StageData.StageType.stage;
            yield return new WaitForSeconds(2.5f);
        }
        else
            yield return new WaitForSeconds(0.3f);
        Destroy(go);
        yield return new WaitForSeconds(0.1f);

        if (queueStages.Count != 0)
        {
            CreateStage(queueStages.Dequeue()); ;
        }
        else 
        {
            UIManager.Instance.EndGame();
            AudioManager.Instance.victory.Play();
            GameWin?.Invoke();
        }          
        GameWin?.Invoke();
    }
}




