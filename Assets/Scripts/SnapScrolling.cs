using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    public KnifeData[] knivesData;
    public GameObject sellPanelPrefab;
    
    //private bool isChosenKnife;
    [Range (0,500)]
    public int offsetSP;
    [Range(0, 200f)]
    public float snapSpeed;
    
    private int countSPs;
    private GameObject[] instSP;
    //private KnifeData[] knivesData;
    private Vector2[] panelsPos;
    private RectTransform contentRect;
    private bool isScrolling = false;
    private Vector2 contentVector;
    private int selectedID;
    private float timeStart;

    private void Start()
    {       

        countSPs = knivesData.Length;
        contentRect = GetComponent<RectTransform>();
        float wigthSP = sellPanelPrefab.GetComponent<RectTransform>().sizeDelta.x;       
        instSP = new GameObject[countSPs];
        //knivesData = new KnifeData[countSPs];
        panelsPos = new Vector2[countSPs];
        for (int i = 0; i < countSPs; i++)
        {
            instSP[i] = Instantiate(sellPanelPrefab, transform, false);
            instSP[i].GetComponent<SellPanel>().CreateSP(knivesData[i]);
            instSP[i].GetComponent<SellPanel>().chosenToggle.group = this.GetComponent<ToggleGroup>();
            //instSP[i].GetComponent<Toggle>().group = this.GetComponent<ToggleGroup>();
            
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
                selectedID = i;
            }
        }
        if (isScrolling) return;
        //contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, panelsPos[selectedID].x,
        //snapSpeed * Time.fixedDeltaTime);
        //contentRect.anchoredPosition = contentVector;
        Move(contentRect.anchoredPosition.x, panelsPos[selectedID].x);

        
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
