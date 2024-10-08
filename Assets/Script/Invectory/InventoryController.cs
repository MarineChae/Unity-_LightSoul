using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public InvectoryGrid selectedItemGrid;




    private void Update()
    {
        if (selectedItemGrid == null) return;

        if(Input.GetMouseButton(0))
        {
            Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        }
     
    }




}
