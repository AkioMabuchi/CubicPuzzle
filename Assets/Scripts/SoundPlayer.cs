using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISoundPlayer
{
    public void PlaySound(int index);
}

public class SoundPlayer : MonoBehaviour, ISoundPlayer
{
    [SerializeField] private GameObject prefabSound;
    [SerializeField] private AudioClip[] audioClips;

    public void PlaySound(int index)
    {
        ISound sound = Instantiate(prefabSound).GetComponent<ISound>();
        sound.Initialize(audioClips[index]);
    }
}
