using System.Collections.Generic;
using UnityEngine;


public class OutLine : MonoBehaviour
{
    [SerializeField]
    private List<Material> materials;
    private List<Material> originMat = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().GetMaterials(originMat);
    }

    // Update is called once per frame
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
