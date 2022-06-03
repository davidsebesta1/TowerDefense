using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryRoundScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private ParticleSystem impactParticles;

    private ObjectPooler op;
    private Rigidbody rb;
    private GameObject target;

    private GameObject audioPlayer;

    private void Start()
    {
        op = FindObjectOfType<ObjectPooler>();
        rb = GetComponent<Rigidbody>();
    }

    public void Spawn(GameObject target)
    {
        this.target = target;
        StartCoroutine(ImpactTimer());
        StartCoroutine(DestroyTimer());
    }

    private void FixedUpdate()
    {
        if(audioPlayer != null)
        {
            audioPlayer.transform.position = gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Path") || other.gameObject.CompareTag("Tile"))
        {
            Debug.Log("impact");

            //impact particle
            ParticleSystem explosion = Instantiate(impactParticles, target.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90,Random.Range(-360,360),0));
            explosion.GetComponent<ParticleSystem>().Play();

            //damage to nearby enemies
            Collider[] collided = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider col in collided)
            {
                if (col.gameObject.CompareTag("Enemy"))
                {
                    col.gameObject.GetComponent<EnemyScript>().RemoveHealth(damage);
                }
            }
            this.gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    public void ImpactTarget()
    {
        if(target != null)
        {
            //create sound player
            audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().SpawnClipPlayer(transform.position, Quaternion.identity, 0, true, 25);
            audioPlayer.GetComponent<AudioSource>().volume = 0.4f + Random.Range(-0.1f, 0.1f);

            rb.velocity = Vector3.zero;
            transform.LookAt(Vector3.down);
            transform.position = target.transform.position + new Vector3(0, 60, 0);
            rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
        }
    }

    private void OnDisable()
    {
        if (op != null)
        {
            op.ReturnArty(this.gameObject);
        }
    }

    private IEnumerator ImpactTimer()
    {
        yield return new WaitForSeconds(10);
        ImpactTarget();
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(17);
        this.gameObject.SetActive(false);
    }
}
