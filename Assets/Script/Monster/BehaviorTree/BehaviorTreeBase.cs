using System.Collections;
using UnityEngine;

public class BehaviorTreeBase : MonoBehaviour
{
    //트리의 루트 노드는 항상 브런치노드에서 파생 되어야함
    protected BranchNode rootNode;
    protected Monster monster;
    private bool isRun = true;

    public Monster Monster { get => monster; set => monster = value; }

    public void RunTree()
    {
        if (isRun)
            rootNode.Tick();
    }

    public void ChangeTreeState()
    {
        rootNode.currentChild = 0;
        isRun = !isRun;
    }
    public bool GetRunState()
    {
        return isRun;
    }
}
