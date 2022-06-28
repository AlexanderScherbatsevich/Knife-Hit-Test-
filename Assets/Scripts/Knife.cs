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
            this.rb.bodyType = RigidbodyType2D.Dynamic;
            this.rb.sleepMode = RigidbodySleepMode2D.StartAwake;
            GameManager.OnTouched += this.Move;
        }
    }

    public void Move()
    {       
        this.rb.AddForce(new Vector2(0, force));
        GameManager.OnTouched -= this.Move;
        UIManager.Instance.RemoveKnife(); 
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        GameManager.OnCollision?.Invoke(col);
        if (go.tag == "Target")
        {
            //GameManager.OnTouched -= Move;
            this.gameObject.tag = "Knife";
            this.gameObject.layer = 7;
            this.gameObject.transform.SetParent(go.transform);
            this.rb.bodyType = RigidbodyType2D.Static;
            this.rb.sleepMode = RigidbodySleepMode2D.StartAsleep;
            go.GetComponent<Target>().ItemsInTarget.Add(this.gameObject);
        }
        else if (go.tag == "Knife")
        {
            //GameManager.OnTouched -= Move;
            this.rb.AddForce(new Vector2(Random.Range(-1200, -1000), -force));
            this.rb.SetRotation(Random.Range(-180f, 180f));
        }
        else if(go.tag == "Wall")
        {
            GameManager.GameLost?.Invoke();
            Destroy(this.gameObject, 0.5f);
        }
    }
}
