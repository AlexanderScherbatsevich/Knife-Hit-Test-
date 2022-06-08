using UnityEngine;
using UnityEngine.UI;

public class SellPanel : MonoBehaviour
{
    //public bool isOpenedKnife = false;
    //public bool isChosenKnife = false;
    //public bool isBuying = false;
    public KnifeData knife;
    public KnifeData.State state;
    public Toggle selectedToggle;
    public Button buyButton;
    public Image knifeImage;
    public Image lockImage;    
    public Text num;
    public Text cost;
    public int ID;
    public Text notEnoughApplesText;

    private int _cost;

    //private bool isOpenedKnife = false;

    public void Buy()
    {       
        int appleCount = UIManager.AppleCount;        
        if (appleCount >= _cost )
        {
            ChangeState(KnifeData.State.isBought);
            KnifeKeeper.Instance.AddOpenedKnife(this.ID);
            GetComponentInParent<UIManager>().SellKnife(this._cost);
        }
        else
        {
            var notEnoughText = Instantiate(notEnoughApplesText, transform, false);
            LeanTween.moveLocal(notEnoughText.gameObject, new Vector3(0, 530f, 0f),
                2f).setDelay(0.1f).setEase(LeanTweenType.easeOutSine);
            Destroy(notEnoughText.gameObject, 2f);
        }
    }

    public void SelectKnife(bool toggle)
    {
        if (toggle)
        {
            ChangeState(KnifeData.State.isSelected);
            PlayerPrefs.SetInt("SelectedKnife", this.ID);

            //KnifeKeeper.selectedKnife
        }
    }


    public void CreateSP(KnifeData knifeData)
    {
        //knife = knifeData;
        state = knifeData.state;
        knifeImage.sprite = knifeData.knifeSkin;
        cost.text = knifeData.cost.ToString();
        _cost = knifeData.cost;
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
                //isOpenedKnife = true;
                //this.GetComponent<Image>().color = greenCol;
                knifeImage.color = Color.white;
                knifeImage.gameObject.transform.SetParent(selectedToggle.transform);
                LeanTween.moveLocal(knifeImage.gameObject, new Vector3(0, 0f, 0f),
                0.3f).setEase(LeanTweenType.easeOutCubic);
                LeanTween.scale(knifeImage.gameObject, new Vector3(1.3f, 1.3f, 1f),
                0.3f).setEase(LeanTweenType.easeOutCubic);

                lockImage.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectedToggle.gameObject.SetActive(true);
                break;
            case KnifeData.State.isSelected:
                //this.GetComponent<Image>().color = orangeCol;
                knifeImage.gameObject.transform.SetParent(selectedToggle.transform);
                LeanTween.moveLocal(knifeImage.gameObject, new Vector3(0, 0f, 0f),
                0).setEase(LeanTweenType.easeOutCubic);
                LeanTween.scale(knifeImage.gameObject, new Vector3(1.3f, 1.3f, 1f),
                0).setEase(LeanTweenType.easeOutCubic);

                knifeImage.color = Color.white;                
                lockImage.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectedToggle.gameObject.SetActive(true);
                selectedToggle.isOn = true;
                break;
        }
    }
}
