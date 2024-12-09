using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private bool hasQuest = false;
    [SerializeField]
    private int dialogueBase;
    private int dialogueIndex = 0;



    public int DialogueBase { get => dialogueBase; set => dialogueBase = value; }

    public int DialogueIndex { get => dialogueIndex; set => dialogueIndex = value; }
    public bool HasQuest { get => hasQuest; set => hasQuest = value; }
}
