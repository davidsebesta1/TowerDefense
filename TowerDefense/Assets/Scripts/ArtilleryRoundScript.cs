using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryRoundScript : MonoBehaviour
{
    [SerializeField] private float damage;
    private ObjectPooler op;
    private Rigidbody rb;
    private GameObject target;

    private void Start()
    {
        op = FindObjectOfType<ObjectPooler>();
        rb = GetComponent<Rigidbody>();
    }

    public void Spawn(GameObject target)
    {
        this.target = target;
        StartCoroutine(ImpactTimer());
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Path") || other.gameObject.CompareTag("Tile"))
        {
            Debug.Log("impact");
            Collider[] collided = Physics.OverlapSphere(transform.position, 2f);
            foreach(Collider col in collided){
                if (col.gameObject.CompareTag("Enemy"))
                {
                    col.gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
                }
            }
            this.gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    public void ImpactTarget()
    {
        rb.velocity = Vector3.zero;
        transform.position = target.transform.position + new Vector3(0, 50, 0);
        rb.AddForce(Vector3.down * 16f, ForceMode.Impulse);
        transform.LookAt(target.transform);
    }

    private void OnDisable()
    {
        if (op != null)
        {
            op.ReturnArty(this.gameObject);
        }
    }

    private IEnumerator ImpactTimer()
    {
        yield return new WaitForSeconds(10);
        ImpactTarget();
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(15);
        this.gameObject.SetActive(false);
    }
}
