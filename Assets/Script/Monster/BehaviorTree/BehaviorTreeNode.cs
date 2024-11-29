//behaviortree�� ����ϱ����ؼ� �� �̳Ѱ��� �����ؾ߸� ��밡��
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ReturnCode { FAILURE, SUCCESS, RUNNING };

//���� ���� ��ũ��Ʈ�� ���� �ʿ䰡 ���� ������ �⺻Ŭ������ ����
public class BaseNode
{
    // Start is called before the first frame update

    public virtual ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }


}
//�б⸦ ���� �� �ִ� ���� �� ��带 �Ļ��Ͽ� ����ؾ��Ѵ�
public class BranchNode : BaseNode
{

    public int currentChild;
    public List<BaseNode> childList;


    public override ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }

    public BranchNode()
    {
        childList = new List<BaseNode>();
    }

}

//����Ʈ ���� �Ѱ����� �����ߴٸ� ���ڸ����� ���� ����
//������ ��� ���� �������� �ݺ��Ѵ�
public class SelectNode : BranchNode
{

    public override ReturnCode Tick()
    {

        int icur = currentChild;
        int iListSize = childList.Count;

        for (int iSize = icur; iSize < iListSize; ++iSize)
        {
            ReturnCode State = childList[iSize].Tick();

            currentChild = iSize;

            //�̹� ƽ���� �׼��� �����߰� ������ �ʾҴٸ� 
            //���� ƽ���� �̾ �����ϱ�����
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //�������� �ڽ��� ���������� �����ٸ� ���� ƽ���� �������� ù ��° ���� ����
            else if (State == ReturnCode.SUCCESS)
            {
                currentChild = 0;
                return ReturnCode.SUCCESS;
            }

        }

        currentChild = 0;
        return ReturnCode.FAILURE;
    }
}

//������ ���� �������� �ڽĳ�带 ���� ����
//�ڽ��� �����Ѱ�� �� ���� ���� ����ó��
public class SequenceNode : BranchNode
{
    public override ReturnCode Tick()
    {
        int icur = currentChild;
        int iListSize = childList.Count;

        for (int iSize = icur; iSize < iListSize; ++iSize)
        {
            ReturnCode State = childList[iSize].Tick();

            currentChild = iSize;
            //�̹� ƽ���� �׼��� �����߰� ������ �ʾҴٸ� 
            //���� ƽ���� �̾ �����ϱ�����
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //������ �ڽ��� �����üũ�� ���� �� ��� �������� ����ó��
            //������ �������� ������ 0�� �ڽĺ��� �����ϱ� ����
            else if (State == ReturnCode.FAILURE)
            {
                currentChild = 0;
                return ReturnCode.FAILURE;
            }

        }
        currentChild = 0;
        return ReturnCode.SUCCESS;
    }






}

//���� �׼��� ó���ϴ� ���
public class TaskNode : BaseNode
{

    [SerializeField]
    public Func<ReturnCode> action;



    public override ReturnCode Tick()
    {
        return action();
    }
    public TaskNode(Func<ReturnCode> action)
    {
        this.action = action;
    }
}
//��尡 ���� �� �� �ִ��� Ȯ���ϴ� �Լ�, �����Ȯ��
public class DecoratorNode : BaseNode
{
    public Func<ReturnCode> condition;
    public BaseNode child;

    public override ReturnCode Tick()
    {
        if (condition() == ReturnCode.FAILURE)
        {
            return ReturnCode.FAILURE;
        }
        else
        {
            return child.Tick();
        }
  
    }

    public DecoratorNode(Func<ReturnCode> condition)
    {
        this.condition = condition;
    }

}
