using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deprecated
/// Change to ThridPersonView from TopView
/// </summary>
public class OutLine : MonoBehaviour
{
    [SerializeField]
    private List<Material> materials;
    private List<Material> originMat = new List<Material>();

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().GetMaterials(originMat);
    }
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        gameObject.GetComponent<MeshRenderer>().SetMaterials(materials);
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().SetMaterials(originMat);
    }
}
