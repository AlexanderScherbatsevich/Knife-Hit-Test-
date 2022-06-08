using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [Range(100,5000)] public float force;

    [HideInInspector] public Sprite skin;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(this.gameObject.tag == "New Knife")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
            GameManager.OnTouched += Move;
        }
    }

    public void Move()
    {       
        rb.AddForce(new Vector2(0, force));
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        GameObject go = col.gameObject;
        
        GameManager.OnCollision?.Invoke(col);
        if (go.tag == "Target")
        {
            GameManager.OnTouched -= Move;
            this.gameObject.tag = "Knife";
            this.gameObject.layer = 7;
            this.gameObject.transform.SetParent(go.transform);
            go.GetComponent<Target>().ItemsInTarget.Add(this.gameObject);
            rb.bodyType = RigidbodyType2D.Static;
            rb.sleepMode = RigidbodySleepMode2D.StartAsleep;
        }
        else if (go.tag == "Knife")
        {
            GameManager.OnTouched -= Move;
            rb.AddForce(new Vector2(Random.Range(-1000, -800), -force));
            rb.SetRotation(Random.Range(-180f, 180f));
        }
        else if(go.tag == "Wall")
        {
            GameManager.GameLost?.Invoke();
            Destroy(this.gameObject, 0.5f);
        }
    }
}
