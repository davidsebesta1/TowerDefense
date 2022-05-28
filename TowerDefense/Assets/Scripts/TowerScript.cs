using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerScript : MonoBehaviour
{
    [SerializeField] protected float fireRate;
    [SerializeField] protected float range;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject rotatingPart;
    [SerializeField] protected float damageOverride;

    protected List<GameObject> enemiesAll = new List<GameObject>();
    protected List<GameObject> enemiesInRadius = new List<GameObject>();

    protected bool canFire = false;

    protected void Start()
    {
        fireRate = 1 / fireRate;
        gameObject.GetComponent<SphereCollider>().radius = range;
        StartCoroutine(FireCountdown());
    }

    protected void GetGameObjectsInRadius()
    {
        enemiesInRadius.Clear();
        foreach (GameObject en in enemiesAll)
        {
            float distance = (transform.position - en.transform.position).magnitude;
            if (distance < range)
            {
                enemiesInRadius.Add(en);
            }
        }

        if (enemiesInRadius.Count > 0)
        {
            canFire = true;
        }
        else
        {
            canFire = false;
        }
    }

    protected virtual IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            enemiesAll = GameObject.Find("GameManager").GetComponent<GameManager>().GetEnemiesList();
            GetGameObjectsInRadius();
            if (canFire)
            {
                GameObject prot = Instantiate(projectile, gameObject.transform.position, projectile.transform.rotation);
                prot.GetComponent<ProjectileScript>().SetDamage(damageOverride);
                try
                {
                    Vector3 dir = (enemiesInRadius[0].transform.position - prot.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward * -0.25f;
                    prot.transform.LookAt(enemiesInRadius[0].transform);
                    Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, 90, 0)).normalized;
                    Quaternion portRot = Quaternion.LookRotation(portDir);
                    rotatingPart.transform.rotation = portRot * Quaternion.Euler(0,0,90);
                    prot.gameObject.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
                }
                catch (Exception e)
                {
                    Destroy(prot);
                }
            }
        }
    }
}
