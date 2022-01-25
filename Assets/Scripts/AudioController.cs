using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire() {
        audioSource.PlayOneShot(audioClips[0], 0.4f);
    }

    public void SetTower() {
        audioSource.PlayOneShot(audioClips[1], 0.7f);
    }

    public void Click() {
        audioSource.PlayOneShot(audioClips[2], 0.7f);
    }

    public void Hit() {
        audioSource.clip = audioClips[3];
        audioSource.volume = 0.7f;
        audioSource.Play();
    }

    public void Die() {
        audioSource.PlayOneShot(audioClips[5], 0.4f);
    }


}
