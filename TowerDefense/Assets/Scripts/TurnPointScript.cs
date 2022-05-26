using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPointScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touched");
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.Rotate(rotation);
        }
    }
}
