using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float speedDefault;
    [SerializeField] private float healthDefault;
    [SerializeField] private int moneyValueDefault;

    [Header("Spawning Properties")]
    [SerializeField] private bool doSpawnEnemies;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 5f;
    [SerializeField] private Vector3 spawnOffset;

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
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    public float GetHealth()
    {
        return health;
    }

    public void RemoveHealth(float removeAmount)
    {
        health -= removeAmount;

        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerController>().AddMoneyAmount(moneyValue);

            if(this.gameObject.name == "TheTon")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().WinGame();
            }
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
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            var en = op.GetObject(enemyToSpawn);
            en.GetComponent<EnemyScript>().Spawn();
            en.transform.SetPositionAndRotation(transform.position + spawnOffset, transform.rotation);
        }
    }
}