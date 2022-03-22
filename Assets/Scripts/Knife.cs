using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [Range(100,1000)]
    public float force;

    private Rigidbody2D rb;

    public delegate void OnHit();
    public event OnHit OnHitted;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.OnTouched += Move;
    }

    public void Move()
    {
        rb.AddForce(new Vector2(0, force));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Target")
        {
            Debug.Log(go.name);
            this.gameObject.transform.SetParent(go.transform);
            rb.bodyType = RigidbodyType2D.Static;
            this.gameObject.transform.localPosition = new Vector3( 0,-0.6f,0);
            this.gameObject.transform.localRotation = Quaternion.identity;

            OnHitted?.Invoke();
        }
    }

    public void OnDisable()
    {
        GameManager.OnTouched -= Move;
    }
}
