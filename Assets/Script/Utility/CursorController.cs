using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    /////////////////////////////// Life Cycle ///////////////////////////////////
    void Start()
    {
        //���� ���� �� ����ȭ�鿡�� Ŀ���� ���̵���
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
