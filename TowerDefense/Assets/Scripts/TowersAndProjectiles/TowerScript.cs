using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerScript : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float fireRate;
    [SerializeField] protected float range;
    [SerializeField] protected int cost = 1;

    [Header("Game Objects")]
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject rotatingPart;
    [SerializeField] protected GameObject muzzleFlash;

    [Header("Overrides")]
    [SerializeField] protected float damageOverride;

    [SerializeField] protected float towerTierDamageMultiplier = 1;

    [SerializeField] protected int towerLevel = 1;


    protected GameObject[] enemiesAll;
    protected List<GameObject> enemiesInRadius = new List<GameObject>();

    protected bool canFire = false;

    protected ObjectPoolAdvanced op;

    protected GameObject audioPlayer;

    protected AudioManager au;

    protected void Start()
    {
        fireRate = 1 / fireRate;
        gameObject.GetComponent<SphereCollider>().radius = range;
        op = FindObjectOfType<ObjectPoolAdvanced>();
        StartCoroutine(FireCountdown());
        au = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    protected void GetGameObjectsInRadius()
    {
        if (enemiesAll != null)
        {
            enemiesInRadius.Clear();
            for (int i = 0; i < enemiesAll.Length; i++)
            {
                float distance = (transform.position - enemiesAll[i].transform.position).magnitude;
                if (distance < range)
                {
                    enemiesInRadius.Add(enemiesAll[i]);
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
    }

    public float GetRange()
    {
        return this.range;
    }

    public float GetDamageOverride()
    {
        return this.towerTierDamageMultiplier * damageOverride;
    }

    public float GetFireRate()
    {
        return this.fireRate;
    }

    public int GetTowerLevel()
    {
        return this.towerLevel;
    }

    public int GetCost()
    {
        return this.cost;
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
                //audio
                if (au.CurrentlyPlayingAudios() <= 30)
                {
                    audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 5, true, 10);
                    audioPlayer.GetComponent<AudioSource>().volume = 0.15f + UnityEngine.Random.Range(-0.1f, 0.1f);
                }

                //particles
                muzzleFlash.GetComponent<ParticleSystem>().Play();

                //projectile stuff
                GameObject proj = op.GetObject(projectile);

                proj.transform.position = muzzleFlash.transform.position;
                proj.GetComponent<ProjectileScript>().Spawn();
                proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                proj.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //direction vector3
                Vector3 dir = enemiesInRadius[0].transform.forward + 10f * (enemiesInRadius[0].transform.position - proj.transform.position).normalized;

                //making projectile look towards enemy
                proj.transform.LookAt(enemiesInRadius[0].transform);

                //rotation vectors
                Vector3 portDir = (enemiesInRadius[0].transform.position - rotatingPart.transform.position + new Vector3(0, 90, 0)).normalized;
                Quaternion portRot = Quaternion.LookRotation(portDir);

                //applying force and rotation
                rotatingPart.transform.rotation = portRot * Quaternion.Euler(0, 0, 90);
                proj.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            }
        }
    }
}
