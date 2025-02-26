using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/SoundList")]
public class SoundList : ScriptableObject
{
    [SerializeField] List<AudioClip> m_AudioClipList;

    Dictionary<string, AudioClip> m_AudioClipDictionary;
    public Dictionary<string, AudioClip> AudioClips
    {
        get
        {
            if (m_AudioClipDictionary == null || m_AudioClipList.Count != m_AudioClipDictionary.Count)
                m_AudioClipDictionary = GetSoundList();

            return m_AudioClipDictionary;
        }
    }

    private Dictionary<string,AudioClip> GetSoundList()
    {
        Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        foreach(AudioClip clip in m_AudioClipList)
        {
            if (audioDictionary.ContainsKey(clip.name))
                continue;
            audioDictionary.Add(clip.name, clip);
        }

        return audioDictionary;
    }
}
