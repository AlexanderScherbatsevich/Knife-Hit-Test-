using UnityEngine;

public enum SpotType
{
    none,
    knife,
    apple,
}

public class Spot : MonoBehaviour
{
    //private SpotType _type = SpotType.none;
    public SpotType type = SpotType.none;
    //public SpotType type
    //{
    //    get { return (_type); }
    //    set { SetType(value, prefab); }
    //}

    public void SetType(SpotType st, GameObject prefab)
    {
        GameObject rootGO = transform.root.gameObject;
        type = st;
        if (type == SpotType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(false);
            GameObject go = Instantiate(prefab,transform);
            go.transform.SetParent(rootGO.transform);
        }
    }
}
