using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    public static SE instance;

    [SerializeField]
    private List<AudioClip> soundList; 

    private AudioSource audioSource;
    
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(SEID seID)
    {
        audioSource.PlayOneShot(soundList[(int)seID]);
    }
}
