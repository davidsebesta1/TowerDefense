using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryScript : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject rotatingPart;
    [SerializeField] private float damageOverride;
    [SerializeField] private float towerTierDamageMultiplier;

    private bool canFire = false;
    private bool editMode = false;

    private ObjectPooler op;

    private GameObject target;

    private void Start()
    {
        fireRate = 1 / fireRate;
        op = FindObjectOfType<ObjectPooler>();
        StartCoroutine(FireCountdown());
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

    public bool GetEditMode()
    {
        return this.editMode;
    }

    public void SetTarget(GameObject newTarget)
    {
        Destroy(target);
        this.target = newTarget;

        //rotation vectors
        Vector3 portDir = (target.transform.position - rotatingPart.transform.position + new Vector3(0, 90, 0)).normalized;
        Quaternion portRot = Quaternion.LookRotation(portDir);

        //applying rotation
        rotatingPart.transform.rotation = portRot * Quaternion.Euler(-65, 0, 0);
    }
}