using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_Player : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] SwordSound;
    [SerializeField] AudioClip[] PunchSound;
    [SerializeField] AudioClip[] JumpSound;

    [SerializeField] AudioClip[] walkSound;
    [SerializeField] AudioClip rollSound;
    [SerializeField] AudioClip hitSound;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    public void playSword(int i)
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(SwordSound[i - 1]);
    }

    public void playPunch(int i)
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(PunchSound[i - 1]);
    }

    public void playJump(int i)
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(JumpSound[i - 1]);
    }

    public void playWalk(int i)
    {
        audioSource.volume = 0.07f;
        audioSource.PlayOneShot(walkSound[i - 1]);
    }

    public void playRoll()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(rollSound);
    }

    public void playHit()
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(hitSound);
    }

    public void stopPlaySound()
    {
        audioSource.Stop();
    }
}
