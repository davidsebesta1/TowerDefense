using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject impactParticles;

    private GameObject target;
    private GameObject audioPlayer;
    private ObjectPoolAdvanced op;

    private Rigidbody rb;

    private AudioManager au;

    private void Start()
    {
        this.op = FindObjectOfType<ObjectPoolAdvanced>();
        this.rb = gameObject.GetComponent<Rigidbody>();
        this.au = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void Spawn()
    {
        StartCoroutine(DestroyTimer());
    }

    private void FixedUpdate()
    {
        if (target != null && target.activeSelf == true)
        {
            Vector3 dir = 5f * Time.deltaTime * (target.transform.position - gameObject.transform.position).normalized;
            transform.up = dir;
            rb.AddForce(dir, ForceMode.Impulse);
        } else
        {
            Vector3 dir = 5f * Time.deltaTime * Vector3.forward;
            rb.AddForce(dir, ForceMode.Impulse);
        }
    } 

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject explosion = op.GetObject(impactParticles);
            explosion.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponentInChildren<ParticleSystem>().Play();

            if (au.CurrentlyPlayingAudios() <= 35)
            {
                audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 8, true, 10);
                audioPlayer.GetComponent<AudioSource>().volume = 0.2f + Random.Range(-0.1f, 0.1f);
            }

            Collider[] collided = Physics.OverlapSphere(transform.position, 0.5f);

            for (int i = 0; i < collided.Length; i++)
            {
                if (collided[i].gameObject.CompareTag("Enemy"))
                {
                    collided[i].gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
                }

            }

            this.gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    public void SetTarget(GameObject newTarget)
    {
        this.target = newTarget;
    }

    private void OnDisable()
    {
        if (op != null)
        {
            StopCoroutine(DestroyTimer());
            op.ReturnGameObject(this.gameObject);
        }
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(10);
        this.gameObject.SetActive(false);
    }
}
