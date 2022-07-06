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
        GameManager.OnTouched += this.Move;
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
            this.gameObject.layer = 7;
            this.gameObject.transform.SetParent(go.transform);
            this.rb.bodyType = RigidbodyType2D.Static;
        }
        else if (go.tag == "Knife")
        {
            this.rb.AddForce(new Vector2(Random.Range(-1300, -1100), -force));
            this.rb.SetRotation(Random.Range(-180f, 180f));
        }
        else if(go.tag == "Wall")
        {
            GameManager.GameLost?.Invoke();
            Destroy(this.gameObject, 0.5f);
        }
    }
}
