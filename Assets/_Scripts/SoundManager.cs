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

        [Header("Combat")]
        public AudioClip slash;
        public AudioClip hit;

        [Header("Misc")]
        public AudioClip pickup;

        [Header("Mixer Groups")]
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup pickUpGroup;

        AudioSource musicSource;
        AudioSource sfxSource;
        AudioSource pickupSource;

        public static SoundManager instance;
        private void Awake()
        {
            instance = this;

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            pickupSource = gameObject.AddComponent<AudioSource>();

            musicSource.outputAudioMixerGroup = musicGroup;
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxSource.loop = false;
            pickupSource.outputAudioMixerGroup = pickUpGroup;
            pickupSource.loop = false;

            StartAudio();
        }

        void StartAudio()
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        private void Update()
        {
            musicSource.pitch = Game.instance.CalculateAmbientPitch();
        }

        public void PlayHit()
        {
            sfxSource.clip = hit;
            sfxSource.Play();
        }

        public void PlaySlash()
        {
            sfxSource.clip = slash;
            sfxSource.Play();
        }

        public void PlayPickup()
        {
            pickupSource.clip = pickup;
            pickupSource.Play();
        }

    }
}
