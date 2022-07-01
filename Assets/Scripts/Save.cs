using UnityEngine;
using System.Collections.Generic;
using System;

public class Save : MonoBehaviour
{
    public static SaveData Instance;

    private void Awake()
    {
        Instance = LoadObject();
    }

    private void OnDisable()
    {
        SaveObject();
    }

    private static void SaveObject()
    {       
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(Instance));
        PlayerPrefs.Save();
    }

    private static SaveData LoadObject()
    {
        SaveData save = new SaveData();   

        if (PlayerPrefs.HasKey("SaveData"))
        {
            save = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
        }
        return save;
    }
}

[Serializable]
public class SaveData
{
    public int highScore;
    public int appleCount;
    public int maxStage;
    public string lastStage = "Stage 1";
    public bool isSoundOff;
    public bool isVibrationOff;
    public int selectedKnifeID;
    public List<int> openedKnivesID = new List<int>() {0};
}
