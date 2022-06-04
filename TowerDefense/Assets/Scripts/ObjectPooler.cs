using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject artyPrefab;

    [SerializeField] private GameObject enemyBasicPrefab;
    [SerializeField] private GameObject enemySpeedPrefab;
    [SerializeField] private GameObject enemyTankPrefab;

    [Header("Object Pools")]
    [SerializeField] private Queue<GameObject> projectilePool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> rocketPool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> artilleryPool = new Queue<GameObject>();

    [SerializeField] private Queue<GameObject> enemyBasicPool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> enemySpeedPool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> enemyTankPool = new Queue<GameObject>();

    [Header("Start Size")]
    [SerializeField] private int startSize = 5;

    private void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            //projectile
            GameObject prot = Instantiate(projectilePrefab);
            projectilePool.Enqueue(prot);
            prot.SetActive(false);

            //rocket
            GameObject rocket = Instantiate(rocketPrefab);
            rocketPool.Enqueue(rocket);
            rocket.SetActive(false);

            //arty
            GameObject arty = Instantiate(artyPrefab);
            artilleryPool.Enqueue(arty);
            arty.SetActive(false);

            //enemy basic
            GameObject enemyBasic = Instantiate(enemyBasicPrefab);
            enemyBasicPool.Enqueue(enemyBasic);
            enemyBasic.SetActive(false);

            //enemy speed
            GameObject enemySpeed = Instantiate(enemySpeedPrefab);
            enemySpeedPool.Enqueue(enemySpeed);
            enemySpeed.SetActive(false);

            //enemy tank
            GameObject enemyTank = Instantiate(enemyTankPrefab);
            enemyTankPool.Enqueue(enemyTank);
            enemyTank.SetActive(false);
        }
    }

    public GameObject GetProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject prot = projectilePool.Dequeue();
            prot.SetActive(true);
            return prot;
        }
        else
        {
            GameObject prot = Instantiate(projectilePrefab);
            return prot;
        }
    }

    public GameObject GetRocket()
    {
        if (rocketPool.Count > 0)
        {
            GameObject roc = rocketPool.Dequeue();
            roc.SetActive(true);
            return roc;
        }
        else
        {
            GameObject roc = Instantiate(rocketPrefab);
            return roc;
        }
    }

    public GameObject GetArty()
    {
        if (artilleryPool.Count > 0)
        {
            GameObject arty = artilleryPool.Dequeue();
            arty.SetActive(true);
            return arty;
        }
        else
        {
            GameObject arty = Instantiate(artyPrefab);
            return arty;
        }
    }

    public GameObject GetEnemyBasic()
    {
        if (enemyBasicPool.Count > 0)
        {
            GameObject enemy = enemyBasicPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyBasicPrefab);
            return enemy;
        }
    }

    public GameObject GetEnemySpeed()
    {
        if (enemySpeedPool.Count > 0)
        {
            GameObject enemy = enemySpeedPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemySpeedPrefab);
            return enemy;
        }
    }

    public GameObject GetEnemyTank()
    {
        if (enemyTankPool.Count > 0)
        {
            GameObject enemy = enemyTankPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyTankPrefab);
            return enemy;
        }
    }

    public void ReturnRocket(GameObject roc)
    {
        rocketPool.Enqueue(roc);
        roc.SetActive(false);

    }

    public void ReturnProjectile(GameObject prot)
    {
        projectilePool.Enqueue(prot);
        prot.SetActive(false);
    }

    public void ReturnArty(GameObject arty)
    {
        artilleryPool.Enqueue(arty);
        arty.SetActive(false);
    }

    public void ReturnEnemyBasic(GameObject enemy)
    {
        enemyBasicPool.Enqueue(enemy);
        enemy.SetActive(false);
    }

    public void ReturnEnemySpeed(GameObject enemy)
    {
        enemySpeedPool.Enqueue(enemy);
        enemy.SetActive(false);
    }

    public void ReturnEnemyTank(GameObject enemy)
    {
        enemyTankPool.Enqueue(enemy);
        enemy.SetActive(false);
    }
}
