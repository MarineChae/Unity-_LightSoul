using System;
using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{ 
    private Status entityStatus;


    /////////////////////////////// Overried Method///////////////////////////////////
    public abstract void TakeDamage(float damage);
    public abstract void UseStamina(float stamina);
    /////////////////////////////// Public Method///////////////////////////////////
    public void InitStatus()
    {
        HP = MaxHP;
        Stamina = MaxStamina;
        StartCoroutine("Recovery");
    }
    /////////////////////////////// Coroutine //////////////////////////
    private IEnumerator Recovery()
    {
        while (true)
        {
            if (Stamina < MaxStamina) Stamina += StaminaRecovery;
            yield return new WaitForSeconds(0.1f);
        }
    }
    /////////////////////////////// Property /////////////////////////////////
    public float HP
    {   
        get => entityStatus.hp;
        set => entityStatus.hp = Mathf.Clamp(value, 0, MaxHP); 
    }
    public float Stamina
    {
        get => entityStatus.stamina;
        set => entityStatus.stamina = Mathf.Clamp(value, 0, MaxStamina);
    }
    public abstract float MaxHP { get; }
    public abstract float MaxStamina { get; }
    public abstract float StaminaRecovery { get; }

}



[Serializable]
public struct Status
{
    public float hp;
    public float stamina;
}
