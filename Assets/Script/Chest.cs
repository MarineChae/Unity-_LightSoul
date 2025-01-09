using UnityEngine;

public class Chest : MonoBehaviour 
{
    [SerializeField]
    private int[] itemList;
    [SerializeField]
    private InventoryController inventoryUI;
    private Transform character;
    private Transform cehstTop;
    private Canvas canvas;
    private Quaternion targetRotation;
    private Quaternion originRotation;
    private bool isRotate;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Start()
    {
        Init();
        var grid = canvas.GetComponentInChildren<InvectoryGrid>();
        foreach(var num in itemList)
        {
            grid.InsertItem(num);
        }
    }

    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if (isRotate)
        {
            cehstTop.transform.rotation = Quaternion.Slerp(cehstTop.transform.rotation, targetRotation, Time.deltaTime * 2.0f);
            if (Quaternion.Angle(cehstTop.transform.rotation, targetRotation) < 0.5f)
            {
                isRotate = false;
            }
        }
    }
    /////////////////////////////// Private Method///////////////////////////////////
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
    /////////////////////////////// Public Method///////////////////////////////////
    public void OpenChest()
    {
        UIManager.Instance.AddCanvas(canvas);
        canvas.gameObject.SetActive(true);
        inventoryUI.ChangeInventoryState(true);
    }

}
