using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager InstanceSound { get; private set; }

    private void Awake()
    {
        if (InstanceSound != null && InstanceSound != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceSound = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public AudioSource mainMenuSound;
    public AudioSource gameSound;
}
