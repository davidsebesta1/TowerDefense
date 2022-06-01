using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject artyPrefab;
    [SerializeField] private Queue<GameObject> projectilePool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> rocketPool = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> artilleryPool = new Queue<GameObject>();
    [SerializeField] private int startSize = 5;

    private void Start()
    {
        for(int i = 0; i < startSize; i++)
        {
            GameObject prot = Instantiate(projectilePrefab);
            projectilePool.Enqueue(prot);
            prot.SetActive(false);
        }

        for(int i = 0; i < startSize; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab);
            rocketPool.Enqueue(rocket);
            rocket.SetActive(false);
        }

        for (int i = 0; i < startSize; i++)
        {
            GameObject arty = Instantiate(artyPrefab);
            artilleryPool.Enqueue(arty);
            arty.SetActive(false);
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

    public GameObject GetRocket()
    {
        if(rocketPool.Count > 0)
        {
            GameObject roc = rocketPool.Dequeue();
            roc.SetActive(true);
            return roc;
        } else
        {
            GameObject roc = Instantiate(rocketPrefab);
            return roc;
        }
    }

    public GameObject GetArty()
    {
        if (artilleryPool.Count > 0)
        {
            GameObject arty = artilleryPool.Dequeue();
            arty.SetActive(true);
            return arty;
        }
        else
        {
            GameObject arty = Instantiate(artyPrefab);
            return arty;
        }
    }

    public void ReturnRocket(GameObject roc)
    {
        rocketPool.Enqueue(roc);
        roc.SetActive(false);

    }

    public void ReturnProjectile(GameObject prot)
    {
        projectilePool.Enqueue(prot);
        prot.SetActive(false);
    }

    public void ReturnArty(GameObject arty)
    {
        artilleryPool.Enqueue(arty);
        arty.SetActive(false);
    }
}
