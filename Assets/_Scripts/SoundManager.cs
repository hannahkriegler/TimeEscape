using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TE
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Ambient")]
        public AudioClip musicClip;

        [Header("Mixer Groups")]
        public AudioMixerGroup musicGroup;

        AudioSource musicSource;

        public static SoundManager instance;
        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();

            musicSource.outputAudioMixerGroup = musicGroup;

            StartAudio();
        }

        void StartAudio()
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
