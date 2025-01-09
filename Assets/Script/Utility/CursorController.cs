using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    /////////////////////////////// Life Cycle ///////////////////////////////////
    void Start()
    {
        //게임 시작 시 메인화면에서 커서를 보이도록
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
