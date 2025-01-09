using TMPro;
using UnityEngine;

public class PotionSlot : MonoBehaviour
{

    [SerializeField]
    private PlayerCharacter character;
    public TextMeshProUGUI dialogueText;
    private int potionCount = 0;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        EventManager.Instance.onPotionTriggerd += PotionAction;
    }
    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onPotionTriggerd -= PotionAction;
        }
    }

    /////////////////////////////// Public Method///////////////////////////////////
    
    //포션에 관련한 이벤트처리를 위한 메서드
    public void PotionAction(string actionName)
    {
        if (actionName == "USE")
        {
            if(PotionCount>0)
            {
                QuestManager.Instance.OnUseItem("Potion");
                UIManager.Instance.InventoryController.UsePotion();
                character.HP = Mathf.Clamp(character.HP + 100 , 0,character.MaxHP);

                PotionCount--;
            }
        }
        else if(actionName == "GET")
        {
            PotionCount++;
        }
        else if (actionName == "DROP")
        {
            PotionCount--;
        }
        dialogueText.text = PotionCount.ToString();
    }

    /////////////////////////////// Property /////////////////////////////////
    public int PotionCount { get => potionCount; set => potionCount = value; }
}
