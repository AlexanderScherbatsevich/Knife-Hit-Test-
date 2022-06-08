using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [HideInInspector] public TargetData targetData;
    [SerializeField] private List<Transform> spotTrans;
    [HideInInspector] public List<GameObject> ItemsInTarget;
    private float minRotSpeed;
    private float maxRotSpeed; 
    private bool isRandomSpeed;
    private SpriteRenderer sRend;
    private float timeForAccelerate;
    private float timeForAccelerateStart;
    [SerializeField] private float timeForChangeSpeed = 3f;
    private float timeForChangeSpeedStart;
    private float angle = 1000;
    private float angleRot;
    private Quaternion q;
    
    private float minSpeedT;
    private float maxSpeedT;

    private void Start()
    {
        ItemsInTarget = new List<GameObject>();
        sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = targetData.sprite;       
        minRotSpeed = targetData.minSpeedRot;
        maxRotSpeed = targetData.maxSpeedRot;
        minSpeedT = minRotSpeed;
        maxSpeedT = maxRotSpeed;

        timeForAccelerate = targetData.timeForAccelerate;
        isRandomSpeed = targetData.isRandomSpeed;

        timeForAccelerateStart = Time.time;
        ShuffleSpot(ref spotTrans);
        //SetSpots();
        SetItem(targetData.chanceForKnife, targetData.knifePrefab);
        SetItem(targetData.chanceForApple, targetData.applePrefab);

        LeanTween.scale(this.gameObject, new Vector3(1.5f, 1.5f, 1f),
        0.3f).setEase(LeanTweenType.easeOutCubic);

        StartCoroutine(VariableRotate());
    }

    private IEnumerator VariableRotate()
    {
        while (true)
        {
            float u = (Time.time - timeForAccelerateStart) / timeForAccelerate;
            float u1 = (Time.time - timeForChangeSpeedStart) / timeForChangeSpeed;

            if (u >= 1)
            {
                if (isRandomSpeed && u1 >= 1)
                    ChangeSpeed();

                float tSpeed = minRotSpeed;
                minRotSpeed = maxRotSpeed;
                maxRotSpeed = tSpeed;
                timeForAccelerateStart = Time.time;
            }


            float tSpeedRot = (1 - u) * minRotSpeed + u * maxRotSpeed;
            angleRot += tSpeedRot * angle * Time.deltaTime;
            q = Quaternion.AngleAxis(angleRot, Vector3.back);
            transform.rotation = q;
            yield return null;
        }
    }

    private void ChangeSpeed()
    {
        timeForChangeSpeed = Random.Range(2f, 5f);
        float minSpeed = Random.Range(minSpeedT/2, minSpeedT*1.2f);

        //float speed = Random.Range(minSpeedT, maxSpeedT);
        minRotSpeed = minSpeed;
        //maxRotSpeed = Random.Range(maxRotSpeed, maxRotSpeed / 2);
        timeForAccelerate = Random.Range(0.1f, timeForAccelerate);
        timeForChangeSpeedStart = Time.time;       
    }

    private void SetItem(int chance, GameObject prefab)
    {
        int randomValue = Random.Range(1, 100);
        if (randomValue <= chance)
        {
            //int countItems = Mathf.FloorToInt(chance / randomValue);    
            int countItems = Random.Range(1, 4);
            for (int i = 0; i < countItems; i++)
            {
                Instantiate(prefab, spotTrans[i]);
                spotTrans.Remove(spotTrans[i]);
                ItemsInTarget.Add(prefab);
            }
        }
        else return;
    }

    private void ShuffleSpot(ref List<Transform> Spots)
    {
        List<Transform> tSpots = new List<Transform>();
        int ndx;
        while (Spots.Count > 0)
        {
            ndx = Random.Range(0, Spots.Count);
            tSpots.Add(Spots[ndx]);
            ItemsInTarget.Add(Spots[ndx].gameObject);
            Spots.RemoveAt(ndx);
        }
        Spots = tSpots;
    }

    //private void OnDestroy()
    //{
    //    this.gameObject.transform.DetachChildren();
    //    for (int i = 0; i < ItemsInTarget.Count; i++)
    //    {            
    //        Rigidbody2D rb = ItemsInTarget[i].GetComponent<Rigidbody2D>();
    //        rb.bodyType = RigidbodyType2D.Dynamic;
    //        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
    //        rb.AddForce(new Vector2(Random.Range(-1000f, 1000f),
    //            Random.Range(-1000f, 1000f)));
    //        Destroy(ItemsInTarget[i], 3f);
    //    }
    //}
}




