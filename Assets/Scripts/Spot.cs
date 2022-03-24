using UnityEngine;


public enum SpotType
{
    none,
    knife,
    apple,
}

public class Spot : MonoBehaviour
{
    private SpotType _type = SpotType.none;
    private GameObject prefab;
    public SpotType type
    {
        get { return (_type); }
        set { SetType(value, prefab); }
    }

    public void SetType(SpotType st, GameObject prefab)
    {
        GameObject rootGO = transform.root.gameObject;
        _type = st;
        if (type == SpotType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
            GameObject go = Instantiate(prefab, this.transform);
            go.transform.SetParent(rootGO.transform);
            //go.tag = "Knife";
            //go.GetComponent<Knife>().enabled = false;
        }
        //else if (type == SpotType.knife)
        //{
        //    this.gameObject.SetActive(true);
        //    GameObject go = Instantiate(prefab, this.transform);
        //    go.transform.SetParent(rootGO.transform);
        //}
    }
}