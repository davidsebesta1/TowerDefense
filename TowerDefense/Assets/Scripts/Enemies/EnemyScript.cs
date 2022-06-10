using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speedDefault;
    [SerializeField] private float healthDefault;
    [SerializeField] private int moneyValueDefault;
    [SerializeField] private bool doSpawnEnemies;

    private float speed;
    private float health;
    private int moneyValue;

    private ObjectPoolAdvanced op;

    private void Start()
    {
        speed = speedDefault;
        health = healthDefault;
        moneyValue = moneyValueDefault;

        op = FindObjectOfType<ObjectPoolAdvanced>();

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

    private void Update()
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

    private void OnDisable()
    {
        if (op != null)
        {
            op.ReturnGameObject(this.gameObject);
        }
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (doSpawnEnemies)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

        }
    }
}