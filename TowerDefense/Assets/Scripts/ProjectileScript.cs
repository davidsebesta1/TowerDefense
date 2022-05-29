using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float damage;
    private ObjectPooler op;

    private void Start()
    {
        op = FindObjectOfType<ObjectPooler>();
    }

    public void Spawn()
    {
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
            this.gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void OnDisable()
    {
        if(op != null)
        {
            op.ReturnProjectile(this.gameObject);
        }
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
}
