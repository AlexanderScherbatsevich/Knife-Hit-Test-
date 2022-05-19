using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetData targetData;
    private float minRotSpeed;
    private float maxRotSpeed;
    private float timeForAccelerate;
    private bool isRandomSpeed;
    private SpriteRenderer sRend;
    private float timeStart;
    private float angle = 1000;
    private float angleRot;
    private Quaternion q;
    [SerializeField]
    private List<Spot> Spots;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = targetData.sprite;       
        minRotSpeed = targetData.minSpeedRot;
        maxRotSpeed = targetData.maxSpeedRot;
        timeForAccelerate = targetData.timeForAccelerate;
        isRandomSpeed = targetData.isRandomSpeed;

        timeStart = Time.time;
        ShuffleSpot(ref Spots);
        Invoke("SetSpots", 0.3f);

        StartCoroutine(VariableRotate());
    }

    private IEnumerator VariableRotate()
    {
        while (true)
        {
            float u = (Time.time - timeStart) / timeForAccelerate;
            if (u >= 1)
            {
                if (isRandomSpeed)
                    ChangeSpeed();
                
                float tSpeed = minRotSpeed;
                minRotSpeed = maxRotSpeed;
                maxRotSpeed = tSpeed;
                timeStart = Time.time;
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
        minRotSpeed = Random.Range(minRotSpeed, maxRotSpeed);
        timeForAccelerate = Random.Range(0.01f, timeForAccelerate);
    }

    public void SetSpots()
    {
        int chanceApple = targetData.chanceForApple;
        int chanceKnife = targetData.chanceForKnife;
        int randomValue = Random.Range(1, 100);

        SetItem(Random.Range(30, 100),
            chanceKnife, SpotType.knife, targetData.knifePrefab);

        SetItem(randomValue, chanceApple, SpotType.apple, targetData.applePrefab);
    }

    public void SetItem(int randomValue, int chance, SpotType type, GameObject prefab)
    {
        Debug.Log($"randomValue = {randomValue}");
        if (randomValue <= chance)
        {          
            int countItems = Mathf.FloorToInt(chance / randomValue);
            Debug.Log( $"count = {countItems}");
            for (int i = 0; i < countItems; i++)
            {
                Spot s = GetEmptySpot();
                if (s != null)
                {
                    s.SetType(type, prefab);
                }
                else return;
            }
        }
        else return;
    }

    Spot GetEmptySpot()
    {
        for (int i = 0; i < Spots.Count; i++)
        {
            if (Spots[i].type == SpotType.none) return (Spots[i]);
        }
        return (null);
    }

    private void ShuffleSpot(ref List<Spot> Spots)
    {
        List<Spot> tSpots = new List<Spot>();
        int ndx;
        while (Spots.Count > 0)
        {
            ndx = Random.Range(0, Spots.Count);
            tSpots.Add(Spots[ndx]);   
            Spots.RemoveAt(ndx);
        }
        Spots = tSpots; 
    }
}




