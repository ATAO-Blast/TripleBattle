using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    public class AudioSvc : MonoBehaviour
    {
        private static AudioSvc instance;
        public static AudioSvc Instance => instance;

        public AudioSource bgAudio;
        public AudioSource uiAudio;
        public void InitSvc()
        {
            instance = this;
        }

        public void PlayBGMusic(string name, bool isLoop = true)
        {
            AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" +  name,true);
            if(bgAudio.clip == null || bgAudio.clip.name != audio.name)
            {
                bgAudio.clip = audio;
                bgAudio.loop = isLoop;
                bgAudio.Play();
            }
        }
        public void PlayUIMusic(string name)
        {
            AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name);
            uiAudio.clip = audio;
            uiAudio.Play();
        }
        public void SetBGVolume(float volume)
        {
            bgAudio.volume = volume;
        }
        public float GetBGVolume()
        {
            return bgAudio.volume;
        }
        public void SetUIVolume(float volume)
        {
            uiAudio.volume = volume;
        }
        public float GetUIVolume()
        {
            return uiAudio.volume;
        }
    }
}