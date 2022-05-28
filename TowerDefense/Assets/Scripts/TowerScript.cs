using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float range;
    [SerializeField] private GameObject projectile;

    List<GameObject> enemiesAll = new List<GameObject>();
    List<GameObject> enemiesInRadius = new List<GameObject>();

    private bool canFire = false;

    private void Start()
    {
        fireRate = 1 / fireRate;
        gameObject.GetComponent<SphereCollider>().radius = range;
        StartCoroutine(FireCountdown());
    }

    void GetGameObjectsInRadius()
    {
        enemiesInRadius.Clear();
        foreach (GameObject en in enemiesAll)
        {
            float distance = (transform.position - en.transform.position).magnitude;
            if (distance < range){
                enemiesInRadius.Add(en);
            }
        }

        Debug.Log("enemies in radius: " + enemiesInRadius.Count);

        if(enemiesInRadius.Count > 0)
        {
            canFire = true;
        } else
        {
            canFire = false;
        }
    }

    IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            enemiesAll = GameObject.Find("GameManager").GetComponent<GameManager>().getEnemiesList();
            GetGameObjectsInRadius();
            if (canFire)
            {
                Debug.Log("fired");
                GameObject prot = Instantiate(projectile, gameObject.transform.position, projectile.transform.rotation);
                Vector3 dir = (enemiesInRadius[0].transform.position - prot.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward * -0.25f;
                prot.transform.LookAt(enemiesInRadius[0].transform);
                prot.gameObject.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            }
        }
    }
}
