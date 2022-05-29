using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool isGameActive = false;
    private int wave = 0;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;

    public void StartGame()
    {
        isGameActive = true;
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
                Instantiate(enemies[1], spawnPoint.transform.position, enemies[1].transform.rotation);
                i += 2;
            }
            else if (randomIndex == 2 && wave >= 20)
            {
                //tank
                Instantiate(enemies[2], spawnPoint.transform.position, enemies[2].transform.rotation);
                i += 3;
            }
            else
            {
                //basic
                Instantiate(enemies[0], spawnPoint.transform.position, enemies[0].transform.rotation);
                i++;
            }

        }
    }

    IEnumerator AutomaticNextWaveCounter()
    {
        while (isGameActive)
        {
            if(wave < 10)
            {
                yield return new WaitForSeconds(wave * 2f);
            } else
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
