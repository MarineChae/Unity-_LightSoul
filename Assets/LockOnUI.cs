using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUI : MonoBehaviour
{


    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
