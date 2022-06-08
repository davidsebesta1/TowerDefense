using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = false;

    [SerializeField] private GameObject spawnManager;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI waveLabel;
    [SerializeField] private TextMeshProUGUI deadWaveLabel;

    private int wallHealth = 100;
    private GameObject[] enemies;

    public void StartGame()
    {
        isGameActive = true;
        spawnManager.GetComponent<SpawnManager>().StartGame();
        player.GetComponent<PlayerController>().StartGame();

        StartCoroutine(TextChanger());
    }

    public void EndGame()
    {
        isGameActive = false;

        spawnManager.GetComponent<SpawnManager>().EndGame();
        player.GetComponent<PlayerController>().EndGame();

        deadWaveLabel.text = "Waves Survived: " + spawnManager.GetComponent<SpawnManager>().GetWaveNumber();

        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                enemies[i].SetActive(false);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isGameActive)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
    }

    public GameObject[] GetEnemiesList()
    {
        return enemies;
    }

    public void WallDamage(int damage)
    {
        wallHealth -= damage;

        if(wallHealth <= 0)
        {
            wallHealth = 0;
            EndGame();
        }
    }

    IEnumerator TextChanger()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(0.1f);

            moneyLabel.text = "Money: " + player.GetComponent<PlayerController>().GetMoneyAmount();
            healthLabel.text = "Wall Health: " + wallHealth + "%";
            waveLabel.text = "Wave: " + spawnManager.GetComponent<SpawnManager>().GetWaveNumber();
        }
    }
}

