using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private Queue<GameObject> audioPool = new();

    private int playingAudios = 0;

    private void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject audioPlr = Instantiate(audioPlayer);
            audioPool.Enqueue(audioPlr);
            audioPlr.SetActive(false);

            StartCoroutine(CountCurrentlyActivePlayers());
        }
    }

    public GameObject SpawnClipPlayer(Vector3 spawnPosition, Quaternion rotation, int audioID, bool destroyOnClipEnd, float maxDistance)
    {
        GameObject audioPlayerInstance = GetAudioPlayerObject();
        audioPlayerInstance.transform.SetPositionAndRotation(spawnPosition, rotation);

        audioPlayerInstance.SetActive(true);

        audioPlayerInstance.GetComponent<AudioSource>().maxDistance = maxDistance;

        audioPlayerInstance.GetComponent<AudioSource>().PlayOneShot(audioClips[audioID]);

        if (destroyOnClipEnd)
        {
            StartCoroutine(ReturnCountdown(audioClips[audioID].length, audioPlayerInstance));
        }

        return audioPlayerInstance;
    }

    public GameObject GetAudioPlayerObject()
    {
        if(audioPool.Count > 0)
        {
            GameObject audioPlr = audioPool.Dequeue();
            audioPlr.SetActive(true);
            return audioPlr;
        } else
        {
            GameObject audioPlr = Instantiate(audioPlayer);
            return audioPlr;
        }
    }

    private IEnumerator ReturnCountdown(float lenght, GameObject audioPlayer)
    {
        yield return new WaitForSeconds(lenght);
        audioPlayer.SetActive(false);
        audioPool.Enqueue(audioPlayer);
    }

    private IEnumerator CountCurrentlyActivePlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject[] audios = GameObject.FindGameObjectsWithTag("AudioPlayer");
            playingAudios = 0;
            for(int i = 0; i < audios.Length; i++)
            {
                if (audios[i].active == true)
                {
                    playingAudios++;
                }
            }
        }
    }
    
    public int CurrentlyPlayingAudios()
    {
        return playingAudios;
    }
}
