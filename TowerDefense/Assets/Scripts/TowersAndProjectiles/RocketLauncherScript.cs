using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherScript : TowerScript
{
    [SerializeField] private int fireAmount = 1;
    protected override IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            enemiesAll = GameObject.Find("GameManager").GetComponent<GameManager>().GetEnemiesList();
            GetGameObjectsInRadius();
            if (canFire)
            {
                for (int i = 0; i < fireAmount; i++)
                {
                    yield return new WaitForSeconds(0.25f);
                    GameObject proj = op.GetObject(projectile);

                    proj.transform.position = gameObject.transform.position;
                    proj.GetComponent<RocketScript>().Spawn();
                    proj.GetComponent<RocketScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                    proj.GetComponent<RocketScript>().SetTarget(enemiesInRadius[0]);
                    proj.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    //create sound player
                    int rnd = UnityEngine.Random.Range(6,8);
                    audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, rnd, true, 10);
                    audioPlayer.GetComponent<AudioSource>().volume = 0.2f + UnityEngine.Random.Range(-0.1f, 0.1f);

                    //particles
                    muzzleFlash.GetComponent<ParticleSystem>().Play();

                    try
                    {
                        //direction vector3
                        Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 7f;

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
}
