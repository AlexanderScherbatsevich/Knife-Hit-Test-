using UnityEngine;


[CreateAssetMenu(fileName = "New Stage", menuName = "Knife Hit/Stage")]
public class StageData : ScriptableObject
{
    public StageType type = StageType.stage;
    public string stageName;
    public GameObject Target;
    public GameObject prefabTargetDestroyed;
    [Range(1, 10)]
    public int freeKnivesCount = 0;
    public Sprite newKnifeSkin;

    public enum StageType
    {
        stage,
        boss,
    }
}

