using UnityEngine;
using UnityEngine.UI;
public class MonsterHpUI : MonoBehaviour, IUpdatable
{
    [SerializeField]
    private Entity entity;
    [SerializeField]
    private Slider sliderHP;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, true, false);
    }
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        sliderHP = GetComponent<Slider>();
    }
    public void FixedUpdateWork()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void LateUpdateWork()
    {

    }

    public void UpdateWork()
    {

        if (sliderHP != null) { sliderHP.value = Utility.Percent(entity.HP, entity.MaxHP); };
    }
}
