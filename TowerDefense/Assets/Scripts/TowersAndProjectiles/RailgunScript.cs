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
                GameObject proj = op.GetObject(projectile);
                proj.transform.position = muzzleFlash.transform.position;
                proj.GetComponent<ProjectileScript>().Spawn();
                proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                proj.GetComponent<Rigidbody>().velocity = Vector3.zero;


                //create sound player
                if (au.CurrentlyPlayingAudios() <= 30)
                {
                    audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 1, true, 12);
                    audioPlayer.GetComponent<AudioSource>().volume = 0.3f;
                }

                //particles
                muzzleFlash.GetComponent<ParticleSystem>().Play();

                try
                {

                    //direction vector
                    Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 15f;

                    //look at enemy
                    proj.transform.LookAt(enemiesInRadius[0].transform);

                    //getting rotation vectors
                    Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, -90, 0)).normalized;
                    Quaternion portRot = Quaternion.LookRotation(portDir);

                    //applying rotation vectors
                    rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 90, -180);
                    Debug.Log("rotated");
                    //applying move force
                    proj.GetComponent<Rigidbody>().AddForce(dir * 1.5f, ForceMode.Impulse);
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
