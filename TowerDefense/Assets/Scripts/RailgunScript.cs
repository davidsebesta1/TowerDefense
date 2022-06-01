using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunScript : TowerScript
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
                GameObject proj = op.GetProjectile();
                proj.transform.position = gameObject.transform.position;
                proj.GetComponent<ProjectileScript>().Spawn();
                proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                try
                {
                    //direction vector3
                    Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 15f;

                    //making projectile look towards enemy
                    proj.transform.LookAt(enemiesInRadius[0].transform);

                    //rotation vectors
                    Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, -90, 0)).normalized;
                    Quaternion portRot = Quaternion.LookRotation(portDir);

                    //applying force and rotation
                    rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 90, -180);
                    proj.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
                }
                catch (Exception)
                {
                    if (op != null)
                    {
                        proj.SetActive(false);
                    }
                }
            }
        }
    }
}
