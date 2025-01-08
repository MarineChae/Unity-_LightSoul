using UnityEditor;
using UnityEngine;

/// <summary>
/// 몬스터 시야를 확인하기위한 디버깅용 스크립트
/// </summary>
#if DEBUG

[CustomEditor(typeof(MonsterRangeChecker))]
public class FOVDebuger : Editor
{

    private void OnSceneGUI()
    {

        MonsterRangeChecker monster = (MonsterRangeChecker)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(monster.transform.position, Vector3.up, Vector3.forward, 360, monster.ViewRadius);
        Vector3 viewAngleA = monster.DirectionFromAngle(-monster.ViewAngle / 2 , false);
        Vector3 viewAngleB = monster.DirectionFromAngle(monster.ViewAngle / 2, false);

        Handles.DrawLine(monster.transform.position,monster.transform.position + viewAngleA * monster.ViewRadius);
        Handles.DrawLine(monster.transform.position, monster.transform.position + viewAngleB * monster.ViewRadius);
        Handles.color = Color.red;


    }




}

#else
public class FOVDebuger 
{
}
#endif