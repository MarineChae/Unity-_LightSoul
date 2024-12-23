using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class ClickMove : MonoBehaviour ,IUpdatable
{

    private NavMeshAgent navMeshAgent;
    [SerializeField]
    LayerMask ignorMask;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, ~ignorMask))
                {

                    navMeshAgent.SetDestination(hit.point);
                    var obj = GameManager.Instance.GetPoolingObject();
                    obj.Activate(hit.point);
                    GameManager.Instance.ReturnPoolingObject(obj);


                }

            }

        }

    }
    public void LateUpdateWork() { }

}
