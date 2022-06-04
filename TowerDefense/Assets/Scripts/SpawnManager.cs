using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    private bool isGameActive = false;
    private int wave = 0;

    private ObjectPooler op;

    public void StartGame()
    {
        isGameActive = true;
        op = FindObjectOfType<ObjectPooler>();
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
            yield return new WaitForSeconds(0.7f + Random.Range(-0.15f, 0.15f));

            int randomIndex = Random.Range(0, enemies.Length);
            if (randomIndex == 1 && wave >= 10)
            {

                //speedy
                GameObject enemy = op.GetEnemySpeed();
                enemy.GetComponent<EnemySpeedScript>().Spawn();
                enemy.transform.position = spawnPoint.transform.position;
                enemy.transform.rotation = enemies[1].transform.rotation;
                i += 2;
            }
            else if (randomIndex == 2 && wave >= 20)
            {
                //tank
                GameObject enemy = op.GetEnemyTank();
                enemy.GetComponent<EnemyTankScript>().Spawn();
                enemy.transform.position = spawnPoint.transform.position;
                enemy.transform.rotation = enemies[1].transform.rotation;
                i += 3;
            }
            else
            {
                //basic
                GameObject enemy = op.GetEnemyBasic();
                enemy.GetComponent<EnemyBasicScript>().Spawn();
                enemy.transform.position = spawnPoint.transform.position;
                enemy.transform.rotation = enemies[1].transform.rotation;
                i++;
            }

        }
    }

    IEnumerator AutomaticNextWaveCounter()
    {
        while (isGameActive)
        {
            if (wave < 10)
            {
                yield return new WaitForSeconds(wave * 2f);
            }
            else
            {
                yield return new WaitForSeconds(wave * 0.6f);
            }
            wave++;
            StartCoroutine(SpawnNextWave());
        }
    }

    public int GetWaveNumber()
    {
        return wave;
    }

}
