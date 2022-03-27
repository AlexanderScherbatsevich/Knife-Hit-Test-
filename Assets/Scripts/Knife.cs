using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [Range(100,5000)]
    public float force;

    private Rigidbody2D rb;
    private BoundsCheck bndCheck;

    void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rb = GetComponent<Rigidbody2D>();
        if(this.gameObject.tag == "New Knife") 
            GameManager.OnTouched += Move;
        //GameManager.GameLost += DestroyKnife;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
    }

    private void Update()
    {
        if (bndCheck != null) 
        {
            if (this.bndCheck.offRight || this.bndCheck.offLeft || this.bndCheck.offUp)
            {
                GameManager.GameLost?.Invoke();
            }       
        }            
    }

    public void Move()
    {
        
        rb.AddForce(new Vector2(0, force));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject go = collision.gameObject;
        
        GameManager.OnHitted?.Invoke(collision);
        //Debug.Log(go.name);
        if (go.tag == "Target")
        {
            GameManager.OnTouched -= Move;
            this.gameObject.tag = "Knife";
            this.gameObject.transform.SetParent(go.transform);
            rb.bodyType = RigidbodyType2D.Static;
            rb.sleepMode = RigidbodySleepMode2D.StartAsleep;            
        }
        else if (go.tag == "Knife")
        {
            GameManager.OnTouched -= Move;
            rb.AddForce(new Vector2(Random.Range(-1000, -800), -force * 1));
            rb.SetRotation(Random.Range(-180f, 180f));            
        }
    }
    //public void DestroyKnife()
    //{
    //    Destroy(this.gameObject, 0.5f);
    //}

    private void OnDisable()
    {
        //GameManager.GameLost -= DestroyKnife;
    }
}
