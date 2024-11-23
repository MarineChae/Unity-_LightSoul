using System.Collections;
using UnityEngine;

public class BehaviorTreeBase : MonoBehaviour
{
    //Ʈ���� ��Ʈ ���� �׻� �귱ġ��忡�� �Ļ� �Ǿ����
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
