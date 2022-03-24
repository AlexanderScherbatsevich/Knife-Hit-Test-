using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //public Animation shakeAndFlash;
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

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = targetData.sprite;       
        minRotSpeed = targetData.minSpeedRot;
        maxRotSpeed = targetData.maxSpeedRot;
        timeForAccelerate = targetData.timeForAccelerate;
        isRandomSpeed = targetData.isRandomSpeed;

        timeStart = Time.time;
        ShuffleSpot(ref Spots);
        SetSpots();

        StartCoroutine(VariableRotate());

        //GameManager.OnHitted += ShakeAndFlash;
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
        minRotSpeed = Random.Range(-0.8f, 0.8f);
        timeForAccelerate = Random.Range(0.5f, 3f);
    }

    public void ShakeAndFlash()
    {
        
        Vector2 tPos = transform.position;
        transform.position = tPos + Random.insideUnitCircle * 1.5f;
        transform.position = tPos;
        Color col = sRend.color;
        sRend.color = Color.white;
        sRend.color = col;
    }

    private void OnDisable()
    {
        //GameManager.OnHitted -= ShakeAndFlash;
    }

    public void SetSpots()
    {
        float chanceApple = targetData.chanceForApple / 100;
        float chanceKnife = targetData.chanceForKnife / 100;

        if (Random.value <= chanceApple)
        {
            int countApples = Random.Range(1, 4);
            for (int i = 0; i < countApples; i++)
            {
                Spot s = GetEmptySpot();
                if (s != null)
                {
                    s.SetType(SpotType.apple, targetData.applePrefab);
                }
                else return;
            }
        }
        if (Random.value <= chanceKnife)
        {
            int countKnifes = Random.Range(1, 4);
            for (int i = 0; i < countKnifes; i++)
            {
                Spot s = GetEmptySpot();
                if (s != null)
                {
                    s.SetType(SpotType.knife, targetData.knifePrefab);
                }
                else return;
            }
        }
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
    //public void VariableRotate()
    //{
    //    float u = (Time.time - timeStart) / timeForAccelerate;
    //    if(u >= 1)
    //    {
    //        float tSpeed = minRotSpeed;
    //        minRotSpeed = maxRotSpeed;
    //        maxRotSpeed = tSpeed;
    //        timeStart = Time.time;
    //    }

    //    float tSpeedRot = (1 - u) * minRotSpeed + u * maxRotSpeed;
    //    angleRot += tSpeedRot * angle * Time.deltaTime;
    //    q = Quaternion.AngleAxis(angleRot, Vector3.back);
    //    transform.rotation = q;

    //    //angleRot += tSpeedRot * Time.deltaTime;
    //    //transform.rotation *= Quaternion.Euler(0, 0, angleRot);
    //}
}




