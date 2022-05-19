using UnityEngine;
using UnityEngine.UI;

public class SellPanel : MonoBehaviour
{
    //public bool isOpenedKnife = false;
    //public bool isChosenKnife = false;
    //public bool isBuying = false;
    public KnifeData knife;
    public KnifeData.State state;
    public Toggle chosenToggle;
    public Button buyButton;
    public Image knifeImage;
    public Image lockImage;    
    public Text num;
    public Text cost;
    public int ID;

    //public Color greenCol;
    //public Color orangeCol;

    private bool isOpenedKnife = false;
    //private Color greenCol = new Color(113, 105, 3, 255);
    //private Color orangeCol = new Color(208, 122, 12, 255);

    //private Image imageSP

    //private void Start()
    //{
    //    imageSP = GetComponent<Image>();
    //    greenCol = new Color(113, 105, 3, 255);
    //    orangeCol = new Color(208, 122, 12, 255);
    //}
    //private void Update()
    //{
    //    if (chosenToggle) ChangeState(KnifeData.State.isChosen);
    //    else ChangeState(KnifeData.State.isBought);

    //}
    public void Buy()
    {
        ChangeState(KnifeData.State.isBought);
    }

    public void ChooseKnife(bool toggle)
    {
        if(toggle)
        ChangeState(KnifeData.State.isSelected);
        PlayerPrefs.SetInt("SelectedKnife", this.ID);
    }


    public void CreateSP(KnifeData knifeData)
    {
        //knife = knifeData;
        state = knifeData.state;
        knifeImage.sprite = knifeData.knifeSkin;
        cost.text = knifeData.cost.ToString();
        int number = knifeData.ID + 1;
        num.text = number.ToString();
        ID = knifeData.ID;
        ChangeState(state);
    }
    public void ChangeState(KnifeData.State newState)
    {
        switch (newState)
        {
            case KnifeData.State.isClosed:
                break;
            case KnifeData.State.isBought:
                isOpenedKnife = true;
                //this.GetComponent<Image>().color = greenCol;
                knifeImage.color = Color.white;
                knifeImage.gameObject.transform.SetParent(chosenToggle.transform);
                LeanTween.moveLocal(knifeImage.gameObject, new Vector3(0, 0f, 0f),
                0.3f).setEase(LeanTweenType.easeOutCubic);
                LeanTween.scale(knifeImage.gameObject, new Vector3(1.3f, 1.3f, 1f),
                0.3f).setEase(LeanTweenType.easeOutCubic);

                lockImage.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                chosenToggle.gameObject.SetActive(true);
                break;
            case KnifeData.State.isSelected:
                //this.GetComponent<Image>().color = orangeCol;
                knifeImage.gameObject.transform.SetParent(chosenToggle.transform);
                LeanTween.moveLocal(knifeImage.gameObject, new Vector3(0, 0f, 0f),
                0).setEase(LeanTweenType.easeOutCubic);
                LeanTween.scale(knifeImage.gameObject, new Vector3(1.3f, 1.3f, 1f),
                0).setEase(LeanTweenType.easeOutCubic);

                knifeImage.color = Color.white;                
                lockImage.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                chosenToggle.gameObject.SetActive(true);
                chosenToggle.isOn = true;
                break;
        }
    }
}
