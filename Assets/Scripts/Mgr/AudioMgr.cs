using UnityEngine;

namespace Assets.Scripts.Mgr
{
    class AudioMgr : Singleton<AudioMgr>
    {
        public void PlayAudioClip(AudioClip clip, GameObject hostObj)
        {
            if (clip == null)
                return;

            AudioSource source = (AudioSource)hostObj.GetComponent<AudioSource>();
            if (source == null)
                source = (AudioSource)hostObj.AddComponent<AudioSource>();

            source.clip = clip;
            source.minDistance = 1.0f;
            source.maxDistance = 50;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.transform.position = hostObj.transform.position;

            source.Play();
        }
    }
}
