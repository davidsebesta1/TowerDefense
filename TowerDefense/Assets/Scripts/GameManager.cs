using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = false;

    [SerializeField] private GameObject spawnManager;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI waveLabel;

    private int wallHealth = 100;

    List<GameObject> enemies = new List<GameObject>();

    public void StartGame()
    {
        isGameActive = true;
        spawnManager.GetComponent<SpawnManager>().StartGame();
        player.GetComponent<PlayerController>().StartGame();

        StartCoroutine(TextChanger());
    }

    private void Update()
    {
        if (isGameActive)
        {
            GameObject[] enArray = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies != null)
            {
                enemies.Clear();
            }
            enemies.AddRange(enArray);
        }
    }

    public List<GameObject> GetEnemiesList()
    {
        return enemies;
    }

    public void WallDamage(int damage)
    {
        wallHealth -= damage;
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

