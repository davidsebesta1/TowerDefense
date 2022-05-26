using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(transform.forward * -speed * Time.deltaTime);
    }
}
