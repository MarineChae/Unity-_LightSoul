using TMPro;
using UnityEngine;

public class PotionSlot : MonoBehaviour
{
    private int potionCount = 0;
    [SerializeField]
    private PlayerCharacter character;
    public TextMeshProUGUI dialogueText;

    public int PotionCount { get => potionCount; set => potionCount = value; }
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

}
