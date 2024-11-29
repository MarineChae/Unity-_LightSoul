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
    private bool isRotate;
    
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
        Init();
        var itemCount = Random.Range(5, 10);


        var temp = canvas.GetComponentInChildren<InvectoryGrid>();
        for(int i = 0; i < itemCount; i++)
        {
            int randomId = 1000 + Random.Range(0,3);
            temp.InsertRandomItem(randomId);
        }


    }

    private void Init()
    {
        cehstTop = transform.Find("ChestTop");
        originRotation = cehstTop.transform.localRotation;
        targetRotation = originRotation;
        targetRotation *= Quaternion.Euler(-130, 0, 0);
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        inventoryUI = Camera.main.GetComponent<InventoryController>();
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (character != null)
        {
            if (character.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("test!");
                isRotate = true;
                isOpen = true;
                canvas.gameObject.SetActive(true);
                inventoryUI.ChangeInventoryState(true);
            }
            else if(character.CompareTag("Player") && Input.GetKeyDown(KeyCode.Escape))
            {
                canvas.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (isRotate)
            {
                cehstTop.transform.rotation = Quaternion.Slerp(cehstTop.transform.rotation, targetRotation, Time.deltaTime * 2.0f);
                if (Quaternion.Angle(cehstTop.transform.rotation, targetRotation) < 0.5f)
                {
                    isRotate = false;
                }

            }
        }


    }

    public void LateUpdateWork() { }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            character = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if(isOpen && other.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(false);
            inventoryUI.ChangeInventoryState(false);
            character = null;
        }
    }

}
