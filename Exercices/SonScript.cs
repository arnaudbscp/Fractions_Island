using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonScript : MonoBehaviour
{
    public AudioClip son1;
    public AudioClip son2;
    public AudioClip son3;

    internal int choix = 1;

    AudioSource myAudioSource1;
    AudioSource myAudioSource2;
    AudioSource myAudioSource3;

    public void Start()
    {
        myAudioSource1 = AddAudio(false, false,1f);
        myAudioSource2 = AddAudio(false, false, 1f);
        myAudioSource3 = AddAudio(false, false, 1f);

        myAudioSource1.clip = son1;
        myAudioSource2.clip = son2;
        myAudioSource3.clip = son3;
    }
    public AudioSource AddAudio(bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

    public void JouerSon()
    {
        if (choix==1)
        {
            myAudioSource1.Play();
        }
        else
        {
            if (choix==2)
            {
                myAudioSource2.Play();
            }
            else
            {
                if (choix==3)
                {
                    myAudioSource3.Play();
                }
            }
        }
    }
     
}
