using System.Collections.Generic;
using UnityEngine;


public class KnifeKeeper : MonoBehaviour
{
    public static KnifeKeeper Instance;

    public KnifeData[] knivesData;

    [HideInInspector] public KnifeData selectedKnife;
    [HideInInspector] public List<int> openedKnivesID;

    private int _selectedKnifeID = 0;

    private void Awake()
    {
        Instance = this;

        openedKnivesID = Save.Instance.openedKnivesID;

        _selectedKnifeID = Save.Instance.selectedKnifeID;
        SelectKnifeData(_selectedKnifeID);
    }

    public void AddOpenedKnife(int ID)
    {
        openedKnivesID.Add(ID);
        Save.Instance.openedKnivesID = openedKnivesID;
    }

    public KnifeData SelectKnifeData(int ID)
    {
        for (int i = 0; i < knivesData.Length; i++)
        {
            if (ID == knivesData[i].ID)
                selectedKnife = knivesData[i];           
        }
        return selectedKnife;
    }
}