using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private ParticleSystem impactParticles;

    private GameObject target;
    private ObjectPooler op;

    private Rigidbody rb;

    private void Start()
    {
        this.op = FindObjectOfType<ObjectPooler>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Spawn()
    {
        StartCoroutine(DestroyTimer());
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 dir = 5f * Time.deltaTime * (target.transform.position - gameObject.transform.position).normalized;
            transform.up = dir;
            rb.AddForce(dir, ForceMode.Impulse);
        } else
        {
            Vector3 dir = 5f * Time.deltaTime * transform.up;
            rb.AddForce(dir, ForceMode.Impulse);
        }
    } 

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ParticleSystem explosion = Instantiate(impactParticles, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponent<ParticleSystem>().Play();

            Collider[] collided = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider col in collided)
            {
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

    public void SetTarget(GameObject newTarget)
    {
        this.target = newTarget;
    }

    private void OnDisable()
    {
        if (op != null)
        {
            op.ReturnRocket(this.gameObject);
        }
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(10);
        this.gameObject.SetActive(false);
    }
}
