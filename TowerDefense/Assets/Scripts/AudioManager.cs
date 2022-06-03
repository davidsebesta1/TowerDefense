using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private GameObject audioPlayer;

    public GameObject SpawnClipPlayer(Vector3 spawnPosition, Quaternion rotation, int audioID, bool destroyOnClipEnd, float maxDistance)
    {
        GameObject audioPlayerInstance = Instantiate(audioPlayer, spawnPosition, rotation);
        audioPlayerInstance.SetActive(true);

        audioPlayerInstance.GetComponent<AudioSource>().maxDistance = maxDistance;

        audioPlayerInstance.GetComponent<AudioSource>().PlayOneShot(audioClips[audioID]);

        if (destroyOnClipEnd)
        {
            Destroy(audioPlayerInstance, audioClips[audioID].length);

        }

        return audioPlayerInstance;


    }
}
