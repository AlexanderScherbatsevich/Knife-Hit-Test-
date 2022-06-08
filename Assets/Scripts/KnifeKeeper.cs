using System.Collections.Generic;
using UnityEngine;


public class KnifeKeeper : MonoBehaviour
{
    public static KnifeKeeper Instance;

    public KnifeData[] knivesData;

    [HideInInspector] public KnifeData selectedKnife;
    [HideInInspector] public List<int> openedKnivesID;

    private int _selectedKnifeID = 0;
    private SavedList _savedList = new SavedList();

    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.HasKey("OpenedKnives"))
        {
            _savedList = JsonUtility.FromJson<SavedList>(PlayerPrefs.GetString("OpenedKnives"));
            openedKnivesID = _savedList.savedOpenedKnivesID;
        }
        else
            AddOpenedKnife(0);

        if (PlayerPrefs.HasKey("SelectedKnife"))
            _selectedKnifeID = PlayerPrefs.GetInt("SelectedKnife");
        else
            _selectedKnifeID = 0;

        SelectKnifeData(_selectedKnifeID);
    }

    public void AddOpenedKnife(int ID)
    {
        //if (openedKnivesID.Contains(ID)) return;
        //else
        //{
        //    openedKnivesID.Add(ID);
        //    _savedList.savedOpenedKnivesID = openedKnivesID;
        //    PlayerPrefs.SetString("OpenedKnives", JsonUtility.ToJson(_savedList));
        //}

        openedKnivesID.Add(ID);
        _savedList.savedOpenedKnivesID = openedKnivesID;
        PlayerPrefs.SetString("OpenedKnives", JsonUtility.ToJson(_savedList));
    }

    public void SelectKnifeData(int ID)
    {
        for (int i = 0; i < knivesData.Length; i++)
        {
            if (ID == knivesData[i].ID)
                selectedKnife = knivesData[i];           
        }
    }
}

[System.Serializable]
public class SavedList
{
    public List<int> savedOpenedKnivesID;
}