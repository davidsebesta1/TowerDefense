using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    private GameObject[] clouds;

    private void Start()
    {
        clouds = GameObject.FindGameObjectsWithTag("Cloud");
    }

    private void Update()
    {
        for(int i = 0; i < clouds.Length; i++)
        {
            clouds[i].transform.Translate(Vector3.right * Time.deltaTime);

            if(clouds[i].transform.position.x > 60)
            {
                Vector3 newPos = new(-65f + Random.Range(-15f, 15f), clouds[i].transform.position.y + Random.Range(-5f,5f), Random.Range(-20f, 30f));
                clouds[i].transform.position = newPos;
            }
        }
    }
}
