using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWallScript : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameManager.GetComponent<GameManager>().WallDamage((int) Mathf.Round(other.gameObject.GetComponent<EnemyScript>().GetHealth()));
            other.gameObject.SetActive(false);
        }
    }
}
