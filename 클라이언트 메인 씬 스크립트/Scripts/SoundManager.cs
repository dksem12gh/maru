using UnityEngine;
using TMPro;

public class SoundManager : MonoBehaviour
{    
    public AudioSource _bgmAudioSource;
    public AudioSource _sfxAudioSource;
    public AudioSource _voiceAudioSource;
    public AudioSource _RhythmMusicSelectSource;

    public void Awake()
    {
        _bgmAudioSource = transform.Find("BGM").GetComponent<AudioSource>();
        _sfxAudioSource = transform.Find("SFX").GetComponent<AudioSource>();
        _voiceAudioSource = transform.Find("VOCAL").GetComponent<AudioSource>();
        _RhythmMusicSelectSource = transform.Find("MUSIC").GetComponent<AudioSource>();
    }

    public void SoundUnPause()
    {
        _bgmAudioSource.UnPause();
        _sfxAudioSource.UnPause();
        _voiceAudioSource.UnPause();
        _RhythmMusicSelectSource.UnPause();
    }

    public void SoundPause()
    {
        _bgmAudioSource.Pause();
        _sfxAudioSource.Pause();
        _voiceAudioSource.Pause();
        _RhythmMusicSelectSource.Pause();
    }

    public void MusicSoundMute()
    {
        _RhythmMusicSelectSource.mute = true;
    }

    public void BgmSoundMute()
    {
        _bgmAudioSource.volume = 0f;
    }    
    public void SfxSoundMute()
    {
        _sfxAudioSource.volume = 0f;
    }
    public void VoiceSoundMute()
    {
        _voiceAudioSource.volume = 0f;
    }

    public void StopAllSound()
    {
        _bgmAudioSource.Stop();
        _sfxAudioSource.Stop();
        _voiceAudioSource.Stop();
    }

    public void MuteAllSound()
    {
        _bgmAudioSource.volume = 0f;
        _sfxAudioSource.volume = 0f;
        _voiceAudioSource.volume = 0f;
    }

    public void playBgmSound(AudioClip clip , bool isLoop = true)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.loop = isLoop;
        _bgmAudioSource.Play();
    }

    public void playMusicSound(AudioClip clip, bool isLoop = false)
    {
        _RhythmMusicSelectSource.mute = false;
        //_RhythmMusicSelectSource.volume = Managers.Data.BgmVolumeLoad();
        _RhythmMusicSelectSource.clip = clip;
        _RhythmMusicSelectSource.loop = isLoop;
        _RhythmMusicSelectSource.Play();
    }

    public void playSfxSound(AudioClip clip, bool isLoop = false)
    {
        _sfxAudioSource.clip = clip;
        _sfxAudioSource.loop = isLoop;
        _sfxAudioSource.Play();
    }

    public void playVoiceSound(AudioClip clip, bool isLoop = false)
    {
        _voiceAudioSource.clip = clip;
        _voiceAudioSource.loop = isLoop;
        _voiceAudioSource.Play();
    }

    public void ChangedVolume(string num, TMP_Text text = null, string name = null)
    {        
        text.text = int.Parse(num).ToString() + "%"; // Text에 표시
        if (name == "Bgm")
        {
            _bgmAudioSource.volume = float.Parse(num) / 100;
        }
        else if(name == "Sfx")
        {
            _sfxAudioSource.volume = float.Parse(num) / 100;
        }
        else if (name == "Voice")
        {
            _voiceAudioSource.volume = float.Parse(num) / 100;
        }
    }
}
