using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerScript : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float fireRate;
    [SerializeField] protected float range;

    [Header("Game Objects")]
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject rotatingPart;
    [SerializeField] protected GameObject muzzleFlash;

    [Header("Overrides")]
    [SerializeField] protected float damageOverride;
    [SerializeField] protected float towerTierDamageMultiplier;

    protected List<GameObject> enemiesAll = new List<GameObject>();
    protected List<GameObject> enemiesInRadius = new List<GameObject>();

    protected bool canFire = false;

    protected ObjectPooler op;

    protected GameObject audioPlayer;

    protected void Start()
    {
        fireRate = 1 / fireRate;
        gameObject.GetComponent<SphereCollider>().radius = range;
        op = FindObjectOfType<ObjectPooler>();
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
                //audio
                audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, 5, true, 10);
                audioPlayer.GetComponent<AudioSource>().volume = 0.15f + UnityEngine.Random.Range(-0.1f, 0.1f);

                //particles
                muzzleFlash.GetComponent<ParticleSystem>().Play();

                //projectile stuff
                GameObject proj = op.GetProjectile();

                proj.transform.position = gameObject.transform.position;
                proj.GetComponent<ProjectileScript>().Spawn();
                proj.GetComponent<ProjectileScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //direction vector3
                Vector3 dir = (enemiesInRadius[0].transform.position - proj.transform.position).normalized * 10f + enemiesInRadius[0].transform.forward * -0.25f;

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
