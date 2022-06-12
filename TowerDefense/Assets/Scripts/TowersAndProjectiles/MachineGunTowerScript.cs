using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTowerScript : TowerScript
{
    [SerializeField] private int fireCycleAmount = 3;
    protected override IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            enemiesAll = GameObject.Find("GameManager").GetComponent<GameManager>().GetEnemiesList();
            GetGameObjectsInRadius();
            if (canFire)
            {

                for (int i = 0; i < fireCycleAmount; i++)
                {
                    yield return new WaitForSeconds(0.05f);

                    //spawning projectile and changing stuff
                    var proj = op.GetObject(projectile);
                    proj.transform.position = muzzleFlash.transform.position;
                    proj.GetComponent<ProjectileScript>().Spawn();
                    proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);

                    //create sound player
                    if (au.CurrentlyPlayingAudios() <= 20)
                    {
                        audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 2, true, 10);
                        audioPlayer.GetComponent<AudioSource>().volume = 0.2f + UnityEngine.Random.Range(-0.1f, 0.1f);
                    }

                    //direction vector 3
                    try
                    {
                        Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward;

                        //look at enemy
                        proj.transform.LookAt(enemiesInRadius[0].transform);

                        //getting rotation vectors
                        Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, 180, 0)).normalized;
                        Quaternion portRot = Quaternion.LookRotation(portDir);

                        //applying rotation vectors
                        rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 90, 0);

                        //applying move force
                        proj.GetComponent<Rigidbody>().AddForce(dir * 2f, ForceMode.Impulse);
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

                //particles
                muzzleFlash.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
