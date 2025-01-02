using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField]
    PlayerCharacter player;
    [SerializeField]
    Monster monster;
    private TMP_Text text;
    private bool end;
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.alpha = 0;
    }
    private void Update()
    {
        if (end) return;
        if (monster != null)
        {

            if (monster.Hp <= 0)
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
