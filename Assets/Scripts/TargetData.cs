using UnityEngine;

[CreateAssetMenu(fileName ="New Target", menuName ="Knife Hit/Target")]
public class TargetData : ScriptableObject
{
    public Sprite sprite;
    [Range(-1f, 1f)]
    public float minSpeedRot;
    [Range(-1f, 1f)]
    public float maxSpeedRot;
    [Range(0.01f, 10f)]
    public float timeForAccelerate;
    public bool isRandomSpeed;
}
