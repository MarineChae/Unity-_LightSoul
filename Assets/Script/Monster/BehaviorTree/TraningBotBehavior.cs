using UnityEngine;


public class TraningBotBehavior : BehaviorTreeBase
{

    private void Awake() 
    {
        Debug.Log("Traning");
        rootNode = new SelectNode();

    }

}
