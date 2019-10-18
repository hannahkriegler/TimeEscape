using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TE
{
    /// <summary>
    /// Handles Game Audio to play the different music tracks. Implements different sound 
    /// channels for BGM, SFX and other audio layers to regulate volume.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        [Header("Ambient")]
        public AudioClip level1Ambient;
        public AudioClip level2Ambient;
        public AudioClip bossAmbient;

        [Header("Combat")]
        public AudioClip slash;
        public AudioClip hit;
        public AudioClip die;

        [Header("Misc")]
        public AudioClip pickup;

        [Header("Player")]
        public AudioClip jump;
        public AudioClip land;
        public AudioClip dash;
        public AudioClip portal;
        public AudioClip timeTravel;

        [Header("Mixer Groups")]
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup pickUpGroup;

        AudioSource musicSource;
        AudioSource sfxSource;
        AudioSource pickupSource;
        AudioSource playerSource;

        public static SoundManager instance;
        private void Awake()
        {
            instance = this;

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            pickupSource = gameObject.AddComponent<AudioSource>();
            playerSource = gameObject.AddComponent<AudioSource>();

            musicSource.outputAudioMixerGroup = musicGroup;
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxSource.loop = false;
            pickupSource.outputAudioMixerGroup = pickUpGroup;
            pickupSource.loop = false;
            playerSource.outputAudioMixerGroup = sfxGroup;
            playerSource.loop = false;


            StartAudio();
        }

        void StartAudio()
        {
            musicSource.clip = level1Ambient;
            musicSource.loop = true;
            musicSource.Play();
        }
        private void Update()
        {
            musicSource.pitch = Game.instance.CalculateAmbientPitch();
        }

        public void PlayLevel1Ambient()
        {
            musicSource.clip = level1Ambient;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlayLevel2Ambient()
        {
            musicSource.clip = level2Ambient;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlayBossAmbient()
        {
            musicSource.clip = bossAmbient;
            musicSource.loop = true;
            musicSource.Play();
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

        public void PlayDie()
        {
            sfxSource.clip = die;
            sfxSource.Play();
        }

        public void PlayPickup()
        {
            pickupSource.clip = pickup;
            pickupSource.Play();
        }

        public void PlayJump()
        {
            playerSource.clip = jump;
            playerSource.volume = 0.3f;
            playerSource.Play();
        }

        public void PlayLand()
        {
            playerSource.clip = land;
            playerSource.volume = 1.0f;
            playerSource.Play();
        }

        public void PlayDash()
        {
            playerSource.clip = dash;
            playerSource.volume = 1.0f;
            playerSource.Play();
        }

        public void PlayPortal()
        {
            playerSource.clip = portal;
            playerSource.volume = 1.0f;
            playerSource.Play();
        }

        public void PlayTimeTravel()
        {
            playerSource.clip = timeTravel;
            playerSource.volume = 1.0f;
            playerSource.Play();
        }

    }
}
