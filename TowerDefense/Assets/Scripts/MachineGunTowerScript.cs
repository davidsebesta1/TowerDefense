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

                    //spawning projectile and changing stuff
                    GameObject proj = op.GetProjectile();
                    proj.transform.position = gameObject.transform.position;
                    proj.GetComponent<ProjectileScript>().Spawn();
                    proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                    GetGameObjectsInRadius();
                    try
                    {
                        //direction vector 3
                        Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward * -0.25f;

                        //look at enemy
                        proj.transform.LookAt(enemiesInRadius[0].transform);

                        //getting rotation vectors
                        Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, 180, 0)).normalized;
                        Quaternion portRot = Quaternion.LookRotation(portDir);

                        //applying rotation vectors
                        rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 90, 0);

                        //applying move force
                        proj.GetComponent<Rigidbody>().AddForce(dir * 1.5f, ForceMode.Impulse);
                    }
                    catch (Exception)
                    {
                        // in case something weird happens which I dont know what it is
                        if (op != null)
                        {
                            proj.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
