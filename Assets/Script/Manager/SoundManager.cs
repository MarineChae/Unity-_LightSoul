using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Sound
{
    Bgm,
    Effect,
    MaxCount,  
}

public class SoundManager : SingleTon<SoundManager>
{
    private float masterVolumeSFX = 0.2f;
    private float masterVolumeBGM = 0.05f;

    Dictionary<string,AudioClip> audioClipDic = new Dictionary<string,AudioClip>();

    AudioSource sfxPlayer;
    AudioSource bgmPlayer;

    public float MasterVolumeSFX { get => masterVolumeSFX; set => masterVolumeSFX = value; }
    public float MasterVolumeBGM { get => masterVolumeBGM; set => masterVolumeBGM = value; }

    private void Awake()
    {
        sfxPlayer = this.AddComponent<AudioSource>();
        bgmPlayer = this.AddComponent<AudioSource>();

        bgmPlayer.clip = Resources.Load<AudioClip>("Sound/20 - Heavy Combat - Imminent Ambush (loop)");
        SetBGMVolume(MasterVolumeBGM);

    }
    public void PlayBGM()
    {
        bgmPlayer.loop = true;
        bgmPlayer.Play();
    }
    public void SetBGMVolume(float volume)
    {
        bgmPlayer.volume = volume;
    }
    public void BGMStop()
    {
        bgmPlayer.Stop();
    }
    public void PlaySFXSound(string soundName)
    {
        if(!audioClipDic.ContainsKey(soundName))
        {
            var clip = Resources.Load<AudioClip>(soundName);
            audioClipDic.Add(soundName, clip);
        }
        sfxPlayer.PlayOneShot(audioClipDic[soundName], MasterVolumeSFX);

    }
    public void PlaySFXSound(AudioClip audioClip)
    {

        sfxPlayer.PlayOneShot(audioClip, MasterVolumeSFX);

    }





}
