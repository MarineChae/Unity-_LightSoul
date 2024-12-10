using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class TraningBotBehavior : BehaviorTreeBase
{

    private void Awake() 
    {
        Debug.Log("Traning");
        rootNode = new SelectNode();

    }

}
