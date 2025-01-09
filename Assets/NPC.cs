using UnityEngine;

public class NPC : MonoBehaviour ,IUpdatable
{
    [SerializeField]
    private bool hasQuest;
    [SerializeField]
    private int npcID;
    [SerializeField]
    private int[] questList;
    [SerializeField]
    private int dialogueBase;
    private int questIndex = 0;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }
    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (questList.Length >= 0 && QuestIndex < questList.Length)
        {
            if (DataManager.Instance.dicQuestDatas[questList[QuestIndex]].isCleared)
            {
                QuestIndex++;
            }
        }
    }
    public void LateUpdateWork() { }

    /////////////////////////////// Property /////////////////////////////////
    public int DialogueBase { get => dialogueBase; set => dialogueBase = value; }
    public int[] QuestList { get => questList; set => questList = value; }
    public int QuestIndex { get => questIndex; set => questIndex = value; }
    public bool HasQuest { get => hasQuest; set => hasQuest = value; }
}
