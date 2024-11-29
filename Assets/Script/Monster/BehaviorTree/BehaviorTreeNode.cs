//behaviortree를 사용하기위해서 이 이넘값을 리턴해야만 사용가능
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ReturnCode { FAILURE, SUCCESS, RUNNING };

//노드는 따로 스크립트로 만들 필요가 없기 때문에 기본클래스로 생성
public class BaseNode
{
    // Start is called before the first frame update

    public virtual ReturnCode Tick()
    {
        return ReturnCode.SUCCESS;
    }


}
//분기를 나눌 수 있는 노드는 이 노드를 파생하여 사용해야한다
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

//셀렉트 노드는 한곳에서 성공했다면 그자리에서 실행 종료
//실패한 경우 다음 노드실행을 반복한다
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

            //이번 틱에서 액션이 성공했고 끝나지 않았다면 
            //다음 틱에서 이어서 실행하기위함
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //셀렉터의 자식이 성공적으로 끝났다면 다음 틱에서 셀렉터의 첫 번째 부터 시작
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

//시퀀스 노드는 시퀀스의 자식노드를 전부 실행
//자식이 실패한경우 이 노드는 전부 실패처리
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
            //이번 틱에서 액션이 성공했고 끝나지 않았다면 
            //다음 틱에서 이어서 실행하기위함
            if (State == ReturnCode.RUNNING)
            {
                return ReturnCode.RUNNING;
            }
            //시퀀스 자식의 컨디션체크가 실패 한 경우 시퀀스를 실패처리
            //다음에 시퀀스에 들어오면 0번 자식부터 실행하기 위함
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

//실제 액션을 처리하는 노드
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
//노드가 실행 될 수 있는지 확인하는 함수, 컨디션확인
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
