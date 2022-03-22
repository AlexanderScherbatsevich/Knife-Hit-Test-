using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetData targetData;
    //[Range(-1f, 1f)]
    private float minRotSpeed;
    //[Range(-1f, 1f)]
    private float maxRotSpeed;
    //[Range(0.01f, 10f)]
    private float timeForAccelerate;
    private bool isRandomSpeed;

    private SpriteRenderer sRend;
    private float timeStart;
    private float angle = 1000;
    private float angleRot;
    private Quaternion q;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();

        sRend.sprite = targetData.sprite;       
        minRotSpeed = targetData.minSpeedRot;
        maxRotSpeed = targetData.maxSpeedRot;
        timeForAccelerate = targetData.timeForAccelerate;
        isRandomSpeed = targetData.isRandomSpeed;

        timeStart = Time.time;

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
        minRotSpeed = Random.Range(-0.8f, 0.8f);
        timeForAccelerate = Random.Range(0.5f, 3f);
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




