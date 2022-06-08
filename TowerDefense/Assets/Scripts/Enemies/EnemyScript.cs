using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected float speedDefault;
    [SerializeField] protected float healthDefault;
    [SerializeField] protected int moneyValueDefault;
    [SerializeField] protected bool doSpawnEnemies;

    protected float speed;
    protected float health;
    protected int moneyValue;


    protected ObjectPooler op;

    protected void Start()
    {
        speed = speedDefault;
        health = healthDefault;
        moneyValue = moneyValueDefault;

        op = FindObjectOfType<ObjectPooler>();

        if (doSpawnEnemies)
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }
    }

    public void Spawn()
    {
        speed = speedDefault;
        health = healthDefault;
        moneyValue = moneyValueDefault;
    }

    protected void Update()
    {
        transform.Translate(-speed * Time.deltaTime * transform.forward);
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
            this.gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        if (op != null)
        {
            op.ReturnEnemyBasic(this.gameObject);
        }
    }

    protected virtual IEnumerator SpawnEnemiesCoroutine()
    {
        while (doSpawnEnemies)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

        }
    }
}
