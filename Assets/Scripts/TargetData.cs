using UnityEngine;

[CreateAssetMenu(fileName ="New Target", menuName ="Knife Hit/Target")]
public class TargetData : ScriptableObject
{
    [Range(0, 100)] public int chanceForApple;
    [Range(0, 100)] public int chanceForKnife;

    public Sprite sprite;
    public GameObject applePrefab;
    public GameObject knifePrefab;

    [Header("Rotation")]
    public bool isReversible;
    public bool isVariable;
    public bool withDelay;
    public float speed = 100;
    [Range(1f, 100f)] public float timeForConstRot = 5f;
    [Range(0.01f, 5f)] public float delay = 0.1f;
}
