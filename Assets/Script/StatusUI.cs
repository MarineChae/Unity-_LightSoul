using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour, IUpdatable
{

    [SerializeField]
    private Entity entity;
    [SerializeField]
    private Slider sliderHP;
    [SerializeField]
    private Slider sliderStamina;


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
       if(sliderHP != null) { sliderHP.value = Utility.Percent(entity.HP,entity.MaxHP); };
       if(sliderStamina != null) { sliderStamina.value = Utility.Percent(entity.Stamina,entity.MaxStamina); };
    }
    public void LateUpdateWork() { }
}
