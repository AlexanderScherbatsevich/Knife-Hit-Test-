using UnityEngine;

public class BrokenPart : MonoBehaviour
{
    public int forceX;
    public int forceY;
    public int angle;

    private BoundsCheck bndCheck;
    void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();

        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, angle),
                    0.3f).setDelay(0.1f).setEase(LeanTweenType.easeOutQuint);
        Destroy(this.gameObject, 3f);
    }

    private void Update()
    {
        if (bndCheck != null && this.bndCheck.offDown)
        {
            Destroy(this.gameObject);               
        }
    }
}
