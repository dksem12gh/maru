using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


[System.Serializable]
public class GameSceneData
{
    public string _gameName;
    public string _gameHowto;
    public string _gameText;
    public Sprite _gameIconSprite;    
}

[System.Serializable]
public class MusicData
{
    public string _musicPathStr;
    public string _musicName;
    public string _atist;
    public int _level;
    public int _musicNum;
    public Sprite _albumImg;
    public Sprite _voidLevelIcon;
    public Sprite _fillLevelIcon;
    public AudioClip _clip;    
}

[System.Serializable]
public class GameData
{    
    public string _gameName;
    public string _atist;
    public int _level;
    public Sprite _albumImg;
    public Sprite _voidLevelIcon;
    public Sprite _fillLevelIcon;
    public AudioClip _clip;
}

[System.Serializable]
public class SaveData
{
    public float _bgmValue;
    public float _sfxValue;
    public float _voiceValue;    
    /*
     * 각 게임별 점수 기록 필요하면 추가
     */
}

[System.Serializable]
public class TeacherData
{
    public string _id;
    public string _schoolName;
    public string _name;
    public int Grade;
    public int Class;
}

[System.Serializable]
public class GameRankData
{
    public string name;
    public Timestamp _dayTime;
    public int _score;    
}

[System.Serializable]
public class StudentData
{
    IDictionary<string, GameRankData> _gameData;
    public string _id;
    public string _schoolName;
    public string _name;
    public int Grade;
    public int Class;
}

public class DataManager : MonoBehaviour
{
    string path;
    string localePath;

    [System.Obsolete]
    private void Start()
    {
            path = Path.Combine(Application.persistentDataPath, "gameData.json");
            localePath = Path.Combine(Application.persistentDataPath, "gameLocaleData.json");
            if (!File.Exists(path))
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                JsonSave();
#elif UNITY_ANDROID
            string streamingPath = Path.Combine(Application.streamingAssetsPath, "gameData.json");
            string streamingLocalePath = Path.Combine(Application.streamingAssetsPath, "gameLocaleData.json");
            StartCoroutine(CopyAndroidStreamingAsset(streamingPath, path));
            StartCoroutine(CopyAndroidStreamingAsset(streamingLocalePath, localePath));
#endif
        }

        JsonLoad();

        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];//en
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];//ko

        //JsonLocaleLoad();
    }

    [System.Obsolete]
    private IEnumerator CopyAndroidStreamingAsset(string sourcePath, string destinationPath)
    {
        using (WWW www = new WWW(sourcePath))
        {
            yield return www;

            File.WriteAllBytes(destinationPath, www.bytes);
        }
    }

    public float BgmVolumeLoad()
    {
        path = Path.Combine(Application.persistentDataPath, "gameData.json");

        string load_json = File.ReadAllText(path);

        SaveData data = JsonUtility.FromJson<SaveData>(load_json);

        return data._bgmValue;
    }

    public void JsonLoad()
    {
        path = Path.Combine(Application.persistentDataPath, "gameData.json");
        
        string load_json = File.ReadAllText(path);

        SaveData data = JsonUtility.FromJson<SaveData>(load_json);        

        Managers.Sound._bgmAudioSource.volume = data._bgmValue;
        Managers.Sound._sfxAudioSource.volume = data._sfxValue;
        Managers.Sound._voiceAudioSource.volume = data._voiceValue;                
    }

    public void JsonLocaleLoad()
    {
        localePath = Path.Combine(Application.persistentDataPath, "gameLocaleData.json");

        if (File.Exists(localePath))
        {
            string load_json = File.ReadAllText(localePath);

            Locale loadedLocale = ScriptableObject.CreateInstance<Locale>();
            JsonUtility.FromJsonOverwrite(load_json, loadedLocale);

            /*LocaleData loadedLocale = JsonUtility.FromJson<LocaleData>(load_json);
            Locale locale = new Locale
            {
                Identifier = loadedLocale.m_Identifier.m_Code
            };*/

            LocalizationSettings.SelectedLocale = loadedLocale;            
            //loadedLocale.Identifier.Code.ToString();
        }
    }
    
    public Locale LocalSaveString()
    {
        localePath = Path.Combine(Application.persistentDataPath, "gameLocaleData.json");
        Locale loadedLocale = new Locale();

        if (File.Exists(localePath))
        {
            string load_json = File.ReadAllText(localePath);

            loadedLocale = ScriptableObject.CreateInstance<Locale>();
            JsonUtility.FromJsonOverwrite(load_json, loadedLocale);
            return loadedLocale;
        }
        else
            return null;
    }

    public void JsonSave()
    {
        SaveData save_data = new SaveData()
        {
            _bgmValue = Managers.Sound._bgmAudioSource.volume,
            _sfxValue = Managers.Sound._sfxAudioSource.volume,
            _voiceValue = Managers.Sound._voiceAudioSource.volume
        };        

        string json = JsonUtility.ToJson(save_data, true);

        File.WriteAllText(path, json);
    }

    public void JsonLocaleSave(Locale locale)
    {
        //LocalizationSettings.SelectedLocale = locale;
        string json = JsonUtility.ToJson(locale);
        File.WriteAllText(localePath, json);
    }


    private void OnApplicationQuit()
    {
        //JsonSave();
    }

}
