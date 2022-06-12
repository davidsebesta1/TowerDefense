using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

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

    private StringBuilder sb = new StringBuilder();

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

    public void WinGame()
    {
        isGameActive = false;

        spawnManager.GetComponent<SpawnManager>().EndGame();
        player.GetComponent<PlayerController>().WinGame();

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
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

            sb.Clear();
            moneyLabel.text = sb.Append("Money: ").Append(player.GetComponent<PlayerController>().GetMoneyAmount()).ToString();
            sb.Clear();
            healthLabel.text = sb.Append("Wall Health: ").Append(wallHealth).Append("%").ToString();
            sb.Clear();
            waveLabel.text = sb.Append("Wave: ").Append(spawnManager.GetComponent<SpawnManager>().GetWaveNumber()).ToString();
        }
    }
}

