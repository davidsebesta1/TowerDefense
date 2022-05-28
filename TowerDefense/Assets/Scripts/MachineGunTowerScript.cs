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
                        Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, 180, 0)).normalized;
                        Quaternion portRot = Quaternion.LookRotation(portDir);
                        rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 90, 0);
                        prot.gameObject.GetComponent<Rigidbody>().AddForce(dir * 1.5f, ForceMode.Impulse);
                    } catch(Exception e)
                    {
                        Destroy(prot);
                    }
                }
            }
        }
    }
}
