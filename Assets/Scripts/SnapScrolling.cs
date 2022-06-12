using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SnapScrolling : MonoBehaviour
{
    public static SnapScrolling Instance;
    public GameObject prefabKnifeKeeper;
    public GameObject prefabSellPanel;
    
    [Range (0,500)] public int offsetSP;
    [Range(0, 200f)] public float snapSpeed;

    [HideInInspector] public GameObject[] instSP;

    private int countSPs;
    private Vector2[] panelsPos;
    private RectTransform contentRect;
    private bool isScrolling = false;
    private Vector2 contentVector;
    private int selectedPanelID;
    private float timeStart;
    private KnifeKeeper _knifeKeeper;
    private List<int> _openedKnives;
    
    private void Start()
    {
        Instance = this;

        Debug.Log("i'm scrolling");
        int selectedKnifeID;
        if (PlayerPrefs.HasKey("SelectedKnife"))
        {
            selectedKnifeID = PlayerPrefs.GetInt("SelectedKnife");
        }
        else selectedKnifeID = 0;
        _openedKnives = KnifeKeeper.Instance.openedKnivesID;
        _knifeKeeper = prefabKnifeKeeper.GetComponent<KnifeKeeper>();
        countSPs = _knifeKeeper.knivesData.Length;
        contentRect = GetComponent<RectTransform>();
        float wigthSP = prefabSellPanel.GetComponent<RectTransform>().sizeDelta.x;       
        instSP = new GameObject[countSPs];

        panelsPos = new Vector2[countSPs];
        for (int i = 0; i < countSPs; i++)
        {
            instSP[i] = Instantiate(prefabSellPanel, transform, false);
            instSP[i].GetComponent<SellPanel>().CreateSP(_knifeKeeper.knivesData[i]);

            int ID = instSP[i].GetComponent<SellPanel>().ID;
            if (_openedKnives.Contains(ID))
            {
                instSP[i].GetComponent<SellPanel>().ChangeState(KnifeData.State.isBought);
            }

            instSP[i].GetComponent<SellPanel>().selectedToggle.group = this.GetComponent<ToggleGroup>();
            if (selectedKnifeID == ID)
            {
                instSP[i].GetComponent<SellPanel>().SelectKnife(true);
                instSP[i].GetComponent<SellPanel>().selectedToggle.isOn = true;
            }

            if (i == 0) continue;
            instSP[i].transform.localPosition = new Vector2(instSP[i-1].transform.localPosition.x
                + wigthSP + offsetSP, instSP[i - 1].transform.localPosition.y);
            panelsPos[i] = -instSP[i].transform.localPosition;
        }
    }

    private void Update()
    {
        float nearestPos = float.MaxValue;
        for (int i = 0; i < countSPs; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - panelsPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanelID = i;
            }
        }
        if (isScrolling) return;
        //contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, panelsPos[selectedID].x,
        //snapSpeed * Time.fixedDeltaTime);
        //contentRect.anchoredPosition = contentVector;
        Move(contentRect.anchoredPosition.x, panelsPos[selectedPanelID].x);

        
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }

    private void Move(float p0, float p1)
    {
        float u = (Time.time - timeStart) / snapSpeed;
        if(u > 1) return;
        u = 1 - Mathf.Pow(1 - u, 2);  
        contentVector.x = (1 - u) * p0 + u * p1;
        contentRect.anchoredPosition = contentVector;
    }

    public void Timereset()
    {
        timeStart = Time.time;
    }
}
