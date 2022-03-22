using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefabKnife;


    public delegate void OnTouch();
    public static event OnTouch OnTouched;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnTouched?.Invoke();
    }

    public void CreateKnife()
    {
        GameObject go = Instantiate(prefabKnife, new Vector2(0, -3.5f), Quaternion.identity);
        var knife = go.GetComponent<Knife>();
    }
}
