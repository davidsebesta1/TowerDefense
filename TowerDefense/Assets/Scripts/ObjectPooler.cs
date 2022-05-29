using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Queue<GameObject> projectilePool = new Queue<GameObject>();
    [SerializeField] private int startSize = 5;

    private void Start()
    {
        for(int i = 0; i < startSize; i++)
        {
            GameObject prot = Instantiate(projectilePrefab);
            projectilePool.Enqueue(prot);
            prot.SetActive(false);
        }
    }

    public GameObject GetProjectile()
    {
        if(projectilePool.Count > 0)
        {
            GameObject prot = projectilePool.Dequeue();
            prot.SetActive(true);
            return prot;
        } else
        {
            GameObject prot = Instantiate(projectilePrefab);
            return prot;
        }
    }


    public void ReturnProjectile(GameObject prot)
    {
        projectilePool.Enqueue(prot);
        prot.SetActive(false);
    }
}
