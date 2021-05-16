using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundTypes
{
    playerHit, reloadStart,reloadEnd
}
public class audioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _as;
    [SerializeField] private AudioClip[] soundClips;
    public static audioManager i;
    private void Start()
    {
        if(i == null)
        {
            i = this;
        }
    }
    public void playSound(SoundTypes sound)
    {
        AudioClip clipToPlay = null;
        switch (sound)
        {
            case SoundTypes.playerHit:
                clipToPlay = soundClips[0];
                break;

            case SoundTypes.reloadStart:
                clipToPlay = soundClips[1];
                break;

            case SoundTypes.reloadEnd:
                clipToPlay = soundClips[2];
                break;
            default:
                break;
        }
        _as.PlayOneShot(clipToPlay);
    }
}
