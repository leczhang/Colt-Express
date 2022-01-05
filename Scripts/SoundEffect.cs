using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffect : MonoBehaviour
{
    public AudioSource backgroundMusic; 

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void playSoundEffect(){
        backgroundMusic.Play();
    }
}
