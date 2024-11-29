using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{

    private Rigidbody body;
    [SerializeField]
    private Vector3 direction = Vector3.zero;
    [SerializeField]
    private float projectileSpeed=1.0f;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private float damage;
    public Vector3 Direction { get => direction; set => direction = value; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();

    }
    private void Start()
    {
        var eff = Instantiate(effect);
        eff.SetActive(false);
        eff.transform.SetParent(transform, false);
        eff.SetActive(true);
    }
    void FixedUpdate()
    {
        body.Move(body.position + direction * Time.fixedDeltaTime * projectileSpeed, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<PlayerCharacter>();
            player.HP -= damage;
            Destroy(this.gameObject);
        }
    }

    //public ProjectileObject(Vector3 dest,float projectionSpeed)
    //{
    //    destination = dest;
    //    this.projectileSpeed = projectionSpeed;
    //}
}
