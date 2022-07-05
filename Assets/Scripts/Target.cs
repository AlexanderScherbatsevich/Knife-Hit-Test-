using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private bool isReversible;
    [SerializeField] private bool isVariable;
    [SerializeField] private bool withDelay;
    [SerializeField] private float speed = 100;
    [SerializeField] private float timeForConstRot = 0.01f;
    [SerializeField] private float delayBetweenconstRot = 0.1f;
    private float timeForConstRotStart;
    private float constSpeed;
    private float constDelay;
    private float constTime;
    //private Rigidbody2D rb;
    private SpriteRenderer sRend;
    //float constPosY;
    private Animation anim;

    [HideInInspector] public TargetData targetData;
    [SerializeField] private List<Transform> spotTrans;
    //[HideInInspector] public List<GameObject> ItemsInTarget;
    //private float minRotSpeed;
    //private float maxRotSpeed; 
    //private bool isRandomSpeed;    
    //[SerializeField] private float timeForSmooth = 0.5f;
    //private float timeForSmoothStart;
    //private float timeForChangeSpeed = 3f;
    //private float timeForChangeSpeedStart;
    //private float angle = 1000;
    //private float angleRot;
    //private Quaternion q;    
    //private float minSpeedT;
    //private float maxSpeedT;

    private void Start()
    {
        sRend = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animation>();

        timeForConstRotStart = Time.time;

        speed = targetData.speed;
        isReversible = targetData.isReversible;
        isVariable = targetData.isVariable;
        withDelay = targetData.withDelay;
        delayBetweenconstRot = targetData.delay;
        timeForConstRot = targetData.timeForConstRot;

        constSpeed = speed;
        constDelay = delayBetweenconstRot;
        constTime = timeForConstRot;
        
        //ItemsInTarget = new List<GameObject>();
        sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = targetData.sprite;

        //minRotSpeed = targetData.minSpeedRot;
        //maxRotSpeed = targetData.maxSpeedRot;
        //minSpeedT = minRotSpeed;
        //maxSpeedT = maxRotSpeed;
        //timeForAccelerate = targetData.timeForAccelerate;
        //isRandomSpeed = targetData.isRandomSpeed;
        //timeForAccelerateStart = Time.time;

        GameManager.OnCollision += ShakeAndFlash;

        SetItem(targetData.chanceForKnife, targetData.knifePrefab);
        SetItem(targetData.chanceForApple, targetData.applePrefab);

        LeanTween.scale(this.gameObject, new Vector3(1.5f, 1.5f, 1.5f),
        0.5f).setEase(LeanTweenType.easeOutCubic);

        //constPosY = this.GetComponent<Transform>().position.y;
        //StartCoroutine(VariableRotate());
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float u = (Time.time - timeForConstRotStart) / timeForConstRot;
        if (u >= 1)
        {
            ChangeSpeed();          
        }
        this.transform.Rotate(0, 0, speed * Time.deltaTime);
    }
    //private IEnumerator VariableRotate()
    //{
    //    while (true)
    //    {
    //        float u = (Time.time - timeForAccelerateStart) / timeForAccelerate;
    //        float u1 = (Time.time - timeForChangeSpeedStart) / timeForChangeSpeed;

    //        if (u >= 1)
    //        {
    //            if (isRandomSpeed && u1 >= 1)
    //                ChangeSpeed();

    //            float tSpeed = minRotSpeed;
    //            minRotSpeed = maxRotSpeed;
    //            maxRotSpeed = tSpeed;
    //            timeForAccelerateStart = Time.time;
    //        }

    //        float tSpeedRot = (1 - u) * minRotSpeed + u * maxRotSpeed;
    //        angleRot += tSpeedRot * angle * Time.deltaTime;
    //        q = Quaternion.AngleAxis(angleRot, Vector3.back);
    //        transform.rotation = q;
    //        yield return null;
    //        //yield return new  WaitForFixedUpdate();
    //    }
    //}

    private void ChangeSpeed()
    {
        //timeForChangeSpeed = Random.Range(2f, 5f);
        //float minSpeed = Random.Range(minSpeedT/2, minSpeedT*1.2f);

        ////float speed = Random.Range(minSpeedT, maxSpeedT);
        //minRotSpeed = minSpeed;
        ////maxRotSpeed = Random.Range(maxRotSpeed, maxRotSpeed / 2);
        //timeForAccelerate = Random.Range(0.1f, timeForAccelerate);
        //timeForChangeSpeedStart = Time.time;
        
        if (isReversible)
        {
            speed *= -1;
            constSpeed *= -1;
        }           
        if (isVariable)
        {
            speed = Random.Range(constSpeed / 1.5f, constSpeed * 1.5f);
            timeForConstRot = Random.Range(constTime / 1.5f, constTime * 1.5f);
            delayBetweenconstRot = Random.Range(constDelay / 1.5f, constDelay * 1.5f);
        }     
        if(withDelay)
            StartCoroutine(SpeedDelay(delayBetweenconstRot));

        timeForConstRotStart = Time.time;
    }
    //private float Smooth(float from, float to, float u)
    //{
    //    float res = (1 - u) * from + u * to;
    //    return (res);
    //}

    private void SetItem(int chance, GameObject prefab)
    {
        int randomValue = Random.Range(1, 100);
        if (randomValue <= chance)
        {
            //int countItems = Mathf.FloorToInt(chance / randomValue);    
            int countItems = Random.Range(1, 4);
            for (int i = 0; i < countItems; i++)
            {
                int index = Random.Range(0, spotTrans.Count);
                Instantiate(prefab, spotTrans[index]);
                spotTrans.Remove(spotTrans[index]);
                //ItemsInTarget.Add(prefab);
            }
        }
        else return;
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

    private IEnumerator SpeedDelay(float delay)
    {
        float tSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = tSpeed;
    }

    //private void ShakeAndFlash()
    //{
    //    LeanTween.moveLocal(this.gameObject, new Vector3(0, 1.65f, 0f),
    //                0.05f).setEase(LeanTweenType.easeOutQuint);

    //    LeanTween.moveLocal(this.gameObject, new Vector3(0, 1.5f, 0f),
    //                0.05f).setDelay(0.1f).setEase(LeanTweenType.easeOutQuint);

    //    Color col = this.gameObject.GetComponent<SpriteRenderer>().color;
    //    LeanTween.color(this.gameObject, Color.white, 0.05f);
    //    LeanTween.color(this.gameObject, col, 0.05f).setDelay(0.1f);
    //}

    //private IEnumerator ShakeAndFlash()
    //{
    //    rb.MovePosition(new Vector2(0, 1.65f));
    //    //rb.AddForce(1f, ForceMode2D.Impulse);
    //    Color col = sRend.color;
    //    sRend.color = Color.white;
    //    yield return new WaitForSeconds(0.1f);
    //    rb.MovePosition(new Vector2(0, constPosY));
    //    sRend.color = col;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Knife"))
    //    {
    //        //StartCoroutine(ShakeAndFlash());
    //        anim.Play("ShakeAndFlash");
    //        Debug.Log("i'm shaking");
    //        //ShakeAndFlash();
    //    }            
    //}
    private void ShakeAndFlash(Collider2D col)
    {
        if (col.CompareTag(this.tag))
            anim.Play("ShakeAndFlash");
    }

    private void OnDisable()
    {
        GameManager.OnCollision -= ShakeAndFlash;
    }
}




