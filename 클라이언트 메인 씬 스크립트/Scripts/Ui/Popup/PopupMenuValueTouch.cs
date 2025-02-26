using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenuValueTouch : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    List<GameObject> _childList = new List<GameObject>();

    private void Start()
    {
        _childList.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            _childList.Add(this.transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        //Managers.Data.JsonLoad();

        _childList.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            _childList.Add(this.transform.GetChild(i).gameObject);
        }

        if (this.transform.parent.name == "Bgm")
        {
            _text.text = string.Format("{0}%", Managers.Sound._bgmAudioSource.volume * 100);
        }
        else if (this.transform.parent.name == "Sfx")
        {
            _text.text = string.Format("{0}%", Managers.Sound._sfxAudioSource.volume * 100);
        }
        else if (this.transform.parent.name == "Voice")
        {
            _text.text = string.Format("{0}%", Managers.Sound._voiceAudioSource.volume * 100);
        }

        foreach (var temp in _childList)
        {
            if (int.Parse(temp.gameObject.name) <= int.Parse(_text.text.Replace("%", "")))
            {
                temp.GetComponent<Image>().enabled = true;
            }
            else
            {
                temp.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void TouchValueChange(GameObject obj)
    {
        int num = 0;
        foreach(var temp in _childList)
        {
            num++;
            if (int.Parse(obj.gameObject.name) >= int.Parse(string.Format("{0}",num*10)))
            {                
                temp.GetComponent<Image>().enabled = true;                
            }
            else
            { 
                temp.GetComponent<Image>().enabled = false;
            }
        }        
        Managers.Sound.ChangedVolume(obj.name, _text,transform.parent.name);
        //Managers.Data.JsonSave();
    }

    public void TouchMute()
    {
        _text.text = string.Format("0%");

        foreach (var temp in _childList)
        {                
            temp.GetComponent<Image>().enabled = false;            
        }

        if (transform.parent.name == "Bgm")
        {
            Managers.Sound.BgmSoundMute();
        }
        else if(transform.parent.name == "Sfx")
        {
            Managers.Sound.SfxSoundMute();
        }
        else if (transform.parent.name == "Vo")
        {
            Managers.Sound.VoiceSoundMute();
        }
    }
}
