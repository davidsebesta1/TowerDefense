using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float damage;

    private void Start()
    {
        Destroy(gameObject, 3);
    }
    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
