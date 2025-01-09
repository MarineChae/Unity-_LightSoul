using UnityEngine;

public class ProjectileObject : MonoBehaviour ,IUpdatable
{
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private Vector3 direction = Vector3.zero;
    [SerializeField]
    private float projectileSpeed=1.0f;
    [SerializeField]
    private float damage;
    private Rigidbody body;


    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);
    }
    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, false, true, false);
    }
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Init();
    }
    public void FixedUpdateWork() 
    {
        body.Move(body.position + direction * Time.fixedDeltaTime * projectileSpeed, Quaternion.identity);
    }
    public void UpdateWork() { }

    public void LateUpdateWork() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<PlayerCharacter>();
            player.HP -= damage;
            Destroy(this.gameObject);
        }
    }
    /////////////////////////////// Private Method///////////////////////////////////
    private void Init()
    {
        var eff = Instantiate(effect);
        eff.SetActive(false);
        eff.transform.SetParent(transform, false);
        eff.SetActive(true);
    }
    /////////////////////////////// Property /////////////////////////////////
    public Vector3 Direction { get => direction; set => direction = value; }
}
