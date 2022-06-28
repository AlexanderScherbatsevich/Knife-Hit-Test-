using UnityEngine;


[CreateAssetMenu(fileName = "New Stage", menuName = "Knife Hit/Stage")]
public class StageData : ScriptableObject
{
    public StageType type = StageType.stage;
    public string stageName;
    public TargetData targetData;
    public GameObject prefabDestroyedTarget;
    public KnifeData openedKnife;
    [Range(1, 10)] public int freeKnivesCount = 0;

    public enum StageType
    {
        stage,
        boss,
    }
}

