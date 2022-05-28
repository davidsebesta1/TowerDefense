using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTowerScript : TowerScript
{
    protected override IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            enemiesAll = GameObject.Find("GameManager").GetComponent<GameManager>().GetEnemiesList();
            GetGameObjectsInRadius();
            if (canFire)
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    GameObject prot = Instantiate(projectile, gameObject.transform.position, projectile.transform.rotation);
                    GetGameObjectsInRadius();
                    try
                    {
                        Vector3 dir = (enemiesInRadius[0].transform.position - prot.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward * -0.25f;
                        prot.GetComponent<ProjectileScript>().SetDamage(damageOverride);
                        prot.transform.LookAt(enemiesInRadius[0].transform);
                        prot.gameObject.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
                    } catch(Exception e)
                    {
                        Destroy(prot);
                    }
                }
            }
        }
    }
}
