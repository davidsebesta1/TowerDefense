using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private ParticleSystem impactParticlesRock;
    [SerializeField] private ParticleSystem impactParticlesDirt;

    private ObjectPooler op;

    private GameObject audioPlayer;

    private void Start()
    {
        op = FindObjectOfType<ObjectPooler>();
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
            ParticleSystem explosion = Instantiate(impactParticlesRock, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponent<ParticleSystem>().Play();

            //create sound player
            audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, 4, true, 10);
            audioPlayer.GetComponent<AudioSource>().volume = 0.1f;

            this.gameObject.SetActive(false);

        }

        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Tile")){

            //impact particle
            ParticleSystem explosion = Instantiate(impactParticlesDirt, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            explosion.GetComponent<ParticleSystem>().Play();

            //create sound player
            audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, 3, true, 10);
            audioPlayer.GetComponent<AudioSource>().volume = 0.1f;

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
            op.ReturnProjectile(this.gameObject);
        }
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
}
