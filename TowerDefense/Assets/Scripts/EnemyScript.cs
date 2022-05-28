using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private int moneyValue;

    private void Update()
    {
        transform.Translate(transform.forward * -speed * Time.deltaTime);
    }

    public float GetHealth()
    {
        return health;
    }

    public void RemoveHealth(float removeAmount)
    {
        health -= removeAmount;

        if(health <= 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerController>().AddMoneyAmount(moneyValue);
            Destroy(gameObject);
        }
    }
}
