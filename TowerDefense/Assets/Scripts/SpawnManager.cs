using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    private bool isGameActive = false;
    private int wave = 0;

    private bool spawningInProgress = false;

    private ObjectPoolAdvanced op;

    public void StartGame()
    {
        isGameActive = true;
        op = FindObjectOfType<ObjectPoolAdvanced>();
        StartCoroutine(SpawnNextWave());
        StartCoroutine(AutomaticNextWaveCounter());
    }

    public void EndGame()
    {
        isGameActive = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnNextWave()
    {
        Debug.Log("Wave:" + wave);
        for (int i = 0; i < wave;)
        {
            spawningInProgress = true;
            yield return new WaitForSeconds(0.7f + Random.Range(-0.15f, 0.15f));

            int randomIndex = Random.Range(0, enemies.Length);

            switch (randomIndex)
            {
                case 1:
                    if (wave >= 10)
                    {
                        //speedy
                        var enemy = op.GetObject(enemies[1]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[1].transform.rotation);
                        i += 2;
                    } else
                    {
                        //basic
                        var enemy = op.GetObject(enemies[0]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                        i++;
                    }
                    break;

                case 2:
                    if (wave >= 20)
                    {
                        //tank
                        var enemy = op.GetObject(enemies[2]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[2].transform.rotation);
                        i += 3;
                    }
                    else
                    {
                        //basic
                        var enemy = op.GetObject(enemies[0]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                        i++;
                    }
                    break;

                case 3:
                    if (wave >= 30)
                    {
                        //spawner
                        var enemy = op.GetObject(enemies[3]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[3].transform.rotation);
                        i += 5;
                    }
                    else
                    {
                        //basic
                        var enemy = op.GetObject(enemies[0]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                        i++;
                    }
                    break;

                case 4:
                    if (wave >= 35)
                    {
                        //even speeder
                        var enemy = op.GetObject(enemies[4]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[4].transform.rotation);
                        i += 4;
                    }
                    else
                    {
                        //basic
                        var enemy = op.GetObject(enemies[0]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                        i++;
                    }
                    break;

                case 5:
                    if (wave >= 40)
                    {
                        //bigger tank
                        var enemy = op.GetObject(enemies[5]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[5].transform.rotation);
                        i += 5;
                    }
                    else
                    {
                        //basic
                        var enemy = op.GetObject(enemies[0]);
                        enemy.GetComponent<EnemyScript>().Spawn();
                        enemy.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                        i++;
                    }
                    break;

                default:
                    //basic
                    var en = op.GetObject(enemies[0]);
                    en.GetComponent<EnemyScript>().Spawn();
                    en.transform.SetPositionAndRotation(spawnPoint.transform.position, enemies[0].transform.rotation);
                    i++;
                    break;
            }

            if (wave == 100)
            {
                var enemy = op.GetObject(enemies[7]);
                enemy.GetComponent<EnemyScript>().Spawn();
                enemy.transform.SetPositionAndRotation(spawnPoint.transform.position - new Vector3(0, 0.21f, 0), enemies[7].transform.rotation);
                isGameActive = false;
                StopAllCoroutines();
                break;
            }
        }

        spawningInProgress = false;
    }

    IEnumerator AutomaticNextWaveCounter()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1.5f);
            if (spawningInProgress == false)
            {
                yield return new WaitForSeconds(5);
                wave++;
                StartCoroutine(SpawnNextWave());
            }
        }
    }

    public int GetWaveNumber()
    {
        return wave;
    }

}
