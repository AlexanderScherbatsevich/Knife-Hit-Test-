using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public bool isSoundOff;
    public bool isVibrationOff;
    public int selectedKnifeID = 0;
    public List<int> savedOpenedKnivesID;

    public static void Save(SaveData sv)
    {
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(sv));
    }

    public static SaveData Load()
    {
        SaveData save = new SaveData();

        if (PlayerPrefs.HasKey("SaveData"))
        {
             save = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
        }

        return save;
    }
}
