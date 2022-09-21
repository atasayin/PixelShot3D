using UnityEngine;
using DG.Tweening;

public class DeathSound : IDeathEffect
{
    private Vector3 _position;
    private AudioStruct _deathAudio;

    public DeathSound(AudioStruct deathAudio, Vector3 position)
    {
        _deathAudio = deathAudio;
        _position = position;
    }

    public void InvokeDeathEffect()
    {
        AudioSource audioSource = ObjectManager.Instance.GetAudioObjectFromPool(_position).GetComponent<AudioSource>();
        SetAudioSource(audioSource);
        audioSource.Play();
    }

    private void SetAudioSource(AudioSource audioSource)
    {
        audioSource.clip = _deathAudio.audioClip;
        audioSource.playOnAwake = _deathAudio.isPlayOnAwake;
        audioSource.volume = _deathAudio.volume;
        audioSource.pitch = _deathAudio.pitch;

    }

}
