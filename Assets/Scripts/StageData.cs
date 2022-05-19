using UnityEngine;


[CreateAssetMenu(fileName = "New Stage", menuName = "Knife Hit/Stage")]
public class StageData : ScriptableObject
{
    public StageType type = StageType.stage;
    public string stageName;
    public TargetData targetData;
    public GameObject prefabDestroyedTarget;
    [Range(1, 10)]
    public int freeKnivesCount = 0;
    public KnifeData openedKnife;

    public enum StageType
    {
        stage,
        boss,
    }
}

