using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour ,IUpdatable
{
    [SerializeField]
    PlayerCharacter player;
    [SerializeField]
    Monster monster;
    private TMP_Text text;
    private bool end;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.alpha = 0;
    }
    public void UpdateWork()
    {
        if (end) return;
        if (monster != null)
        {
            if (monster.HP <= 0)
            {
                end = true;
                StartCoroutine("Die");
            }
        }
        else
        {
            if (player.HP <= 0)
            {
                end = true;
                StartCoroutine("Die");
            }
        }
    }
    public void FixedUpdateWork() { }

    public void LateUpdateWork() { }

    /////////////////////////////// Coroutine //////////////////////////
    
    IEnumerator Die()
    {
        SoundManager.Instance.BGMStop();
        yield return new WaitForSeconds(1.0f);
        float duration = 4.0f;
        float elapsedTime = 0.0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime; 
            text.alpha = Mathf.Clamp01(elapsedTime/duration);
            yield return null;

        }
    }
}
