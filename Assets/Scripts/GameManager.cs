using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static bool isKnifeInTarget = false;
    public static bool isGameOver = false;
    public static GameManager S;
    public GameObject prefabKnife;

    

    public delegate void OnTouch();
    public static event OnTouch OnTouched;
    public static event OnTouch OnHitted;

    void Start()
    {
        OnHitted += CreateKnife;

        CreateKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTouched?.Invoke();

        if (isKnifeInTarget)
            OnHitted?.Invoke();
            
    }

    public void CreateKnife()
    {
        isKnifeInTarget = false;
        GameObject go = Instantiate(prefabKnife, new Vector2(0, -6.5f), Quaternion.identity);
        go.transform.position = Vector2.Lerp(transform.position, new Vector2(0, -4.5f), 0.9f);
    }


    private void OnDisable()
    {
        OnHitted -= CreateKnife;
    }
}
