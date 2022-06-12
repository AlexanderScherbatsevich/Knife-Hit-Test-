using UnityEngine;

[CreateAssetMenu(fileName = "New KnifeData", menuName = "Knife Hit/KnifeData")]
public class KnifeData : ScriptableObject
{
    public State state = State.isClosed;
    public int ID;
    public int cost;
    public Sprite knifeSkin;

    public enum State
    {
        isClosed,
        isSelected,
        isBought
    }
}

