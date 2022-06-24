using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public static SaveData Save;

    private void Awake()
    {
        Save = SaveData.Load();
        Debug.Log(Save.isSoundOff);
    }

    private void OnDisable()
    {
        SaveData.Save(Save);
    }
}
