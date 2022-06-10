using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryScript : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] protected int cost = 1;

    [Header("Game Objects")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject rotatingPart;
    [SerializeField] private GameObject muzzleFlash;

    [Header("Overrides")]
    [SerializeField] private float damageOverride;
    [SerializeField] private float towerTierDamageMultiplier;
    [SerializeField] protected int towerLevel = 1;

    private bool canFire = false;
    private bool editMode = false;

    private ObjectPooler op;

    private GameObject target;

    private GameObject audioPlayer;

    private void Start()
    {
        fireRate = 1 / fireRate;
        op = FindObjectOfType<ObjectPooler>();
    }

    private IEnumerator FireCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            if (canFire && !editMode && target != null)
            {
                //Getting projectile from queue
                GameObject proj = op.GetArty();

                //create sound player
               // audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, 1, true, 10);
               // audioPlayer.GetComponent<AudioSource>().volume = 0.2f + UnityEngine.Random.Range(-0.1f, 0.1f);

                //particles
                muzzleFlash.GetComponent<ParticleSystem>().Play();

                //applying intial position & rotation
                proj.transform.position = gameObject.transform.position;
                proj.GetComponent<ArtilleryRoundScript>().Spawn(target);

                proj.transform.Rotate(0, 0, 90);

                //setting up stuff
                proj.GetComponent<ArtilleryRoundScript>().SetDamage(damageOverride * towerTierDamageMultiplier);
                proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                proj.GetComponent<Rigidbody>().AddForce(transform.forward * 52f, ForceMode.Impulse);
            }

        }
    }

    public void SetEditMode(bool newMode)
    {
        this.editMode = newMode;

        if (editMode)
        {
            canFire = false;
        }
        else
        {
            canFire = true;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        StopAllCoroutines();
        Destroy(target);
        this.target = newTarget;

        //rotation vectors
        Vector3 portDir = (target.transform.position - rotatingPart.transform.position + new Vector3(0, 90, 0)).normalized;
        Quaternion portRot = Quaternion.LookRotation(portDir);


        Debug.Log(portRot * Quaternion.Euler(-65, 0, 0));
        //applying rotation
        rotatingPart.transform.rotation = portRot * Quaternion.Euler(-65, 0, 0);
        StartCoroutine(FireCountdown());
    }

    public bool GetEditMode()
    {
        return this.editMode;
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

    public void DestroyTarget()
    {
        Destroy(this.target);
    }
}
