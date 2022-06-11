using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject impactParticlesRock;
    [SerializeField] private GameObject impactParticlesDirt;

    private ObjectPoolAdvanced op;

    private GameObject audioPlayer;

    private AudioManager au;

    private void Start()
    {
        this.op = FindObjectOfType<ObjectPoolAdvanced>();
        this.au = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void Spawn()
    {
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
            this.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Path") || other.gameObject.CompareTag("Rock"))
        {
            //impact particle
            GameObject explosion = op.GetObject(impactParticlesRock);
            explosion.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponentInChildren<ParticleSystem>().Play();

            //create sound player
            if (au.CurrentlyPlayingAudios() <= 25)
            {
                audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 4, true, 10);
                audioPlayer.GetComponent<AudioSource>().volume = 0.1f;
            }

            this.gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Tile")){

            //impact particle
            GameObject explosion = op.GetObject(impactParticlesDirt);
            explosion.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponentInChildren<ParticleSystem>().Play();

            //create sound player
            if(au.CurrentlyPlayingAudios() <= 25)
            {
                audioPlayer = au.SpawnClipPlayer(transform.position, Quaternion.identity, 3, true, 10);
                audioPlayer.GetComponent<AudioSource>().volume = 0.1f;
            }

            this.gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    private void OnDisable()
    {
        if(op != null)
        {
            StopCoroutine(DestroyTimer());
            op.ReturnGameObject(this.gameObject);
        }
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
}
