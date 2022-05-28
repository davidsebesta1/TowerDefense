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

    IEnumerator SpawnNextWave()
    {
        Debug.Log("Wave:" + wave);
        for (int i = 0; i < wave; i++)
        {
            yield return new WaitForSeconds(0.7f);
            Instantiate(enemies[0], spawnPoint.transform.position, enemies[0].transform.rotation);
        }
    }

    IEnumerator AutomaticNextWaveCounter()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(wave * 1.1f);
            wave++;
            StartCoroutine(SpawnNextWave());
        }
    }

    public int getWaveNumber()
    {
        return wave;
    }

}
