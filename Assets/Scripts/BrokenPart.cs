using UnityEngine;

public class BrokenPart : MonoBehaviour
{
    public int force;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0));
    }
}
