using UnityEngine;

public class Chest : MonoBehaviour 
{

    private Transform character;
    private Transform cehstTop;
    private Quaternion targetRotation;
    private Quaternion originRotation;
    private bool isOpen;
    private bool isRotate;
    [SerializeField]
    private int[] itemList;
    
    private Canvas canvas;

    [SerializeField]
    private InventoryController inventoryUI;


    private void Start()
    {
        Init();

        var grid = canvas.GetComponentInChildren<InvectoryGrid>();
        foreach(var num in itemList)
        {
            grid.InsertItem(num);
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
        //if (character != null)
        //{
        //    if (character.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        //    {
        //        isRotate = true;
        //        isOpen = true;
        //        canvas.gameObject.SetActive(true);
        //        inventoryUI.ChangeInventoryState(true);
        //    }
        //    else if(character.CompareTag("Player") && Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        canvas.gameObject.SetActive(false);
        //        Cursor.visible = false;
        //        Cursor.lockState = CursorLockMode.Locked;
        //    }
        //    
        //    
        //    

        //    
        //}
        if (isRotate)
        {
            cehstTop.transform.rotation = Quaternion.Slerp(cehstTop.transform.rotation, targetRotation, Time.deltaTime * 2.0f);
            if (Quaternion.Angle(cehstTop.transform.rotation, targetRotation) < 0.5f)
            {
                isRotate = false;
            }
        }
    }

    public void OpenChest()
    {
        UIManager.Instance.AddCanvas(canvas);
        canvas.gameObject.SetActive(true);
        inventoryUI.ChangeInventoryState(true);
    }

}
