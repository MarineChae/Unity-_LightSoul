using System;
using System.Collections;
using UnityEngine;


public abstract class Entity : MonoBehaviour
{ 
    private Status entityStatus;

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


    public void InitStatus()
    {
        HP = MaxHP;
        Stamina = MaxStamina;
        StartCoroutine("Recovery");
    }
   
    private IEnumerator Recovery()
    {
        while(true)
        {
            if (Stamina < MaxStamina) Stamina += StaminaRecovery;
            yield return new WaitForSeconds(0.1f);
        }

    }
    public abstract void TakeDamage(float damage);
    public abstract void UseStamina(float stamina);

}



[Serializable]
public struct Status
{
    public float hp;

    public float stamina;

}
