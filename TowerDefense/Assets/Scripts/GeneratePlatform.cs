using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlatform : MonoBehaviour
{
    [SerializeField] private GameObject Tile;
    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                Instantiate(Tile, new Vector3(1.05f * i, 0, 1.05f * j), Tile.transform.rotation);
            }
        }
    }
}
