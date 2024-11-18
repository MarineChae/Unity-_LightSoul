using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour ,IUpdatable
{

    private Transform character;
    private Transform cehstTop;
    private Quaternion targetRotation;
    private Quaternion originRotation;
    private bool isOpen;
    
    private Canvas canvas;

    [SerializeField]
    private InventoryController inventoryUI;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    private void Start()
    {
        cehstTop = transform.Find("ChestTop");
        originRotation = cehstTop.transform.localRotation;
        targetRotation = originRotation;
        targetRotation *= Quaternion.Euler(-130, 0, 0);
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (character != null)
        {
            if (character.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("test!");
                isOpen = true;
                canvas.gameObject.SetActive(true);
                inventoryUI.ChangeInventoryState(true);
            }
            else if(character.CompareTag("Player") && Input.GetKeyDown(KeyCode.Escape))
            {
                canvas.gameObject.SetActive(false);
                inventoryUI.ChangeInventoryState(false);
            }
        }
        if (isOpen)
        {
            cehstTop.transform.rotation = Quaternion.Slerp(cehstTop.transform.rotation, targetRotation, Time.deltaTime * 2.0f);
            if (Quaternion.Angle(cehstTop.transform.rotation, targetRotation) < 0.5f)
            {
                isOpen = false;
            }

        }

    }

    public void LateUpdateWork() { }


    private void OnTriggerEnter(Collider other)
    {
        character = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
        inventoryUI.ChangeInventoryState(false);
        character = null;

    }

}
