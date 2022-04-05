using UnityEngine;

public enum SpotType
{
    none,
    knife,
    apple,
}

public class Spot : MonoBehaviour
{
    public SpotType type = SpotType.none;

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
