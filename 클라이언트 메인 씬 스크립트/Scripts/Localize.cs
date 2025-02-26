using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Localize
{

    public string TextString(string tableName,string keyName)
    {
        LocalizedString localString = new LocalizedString() { TableReference = tableName, TableEntryReference = keyName };
        var stringOperation = localString.GetLocalizedStringAsync();

        stringOperation.WaitForCompletion();

        if (stringOperation.Status == AsyncOperationStatus.Succeeded)
        {
            return stringOperation.Result;
        }
        else
        {
            return null;
        }        
    }

    public AudioClip LocalizeAudioClip(string tableName, string keyName)
    {        
        AudioClip localizedClip = null;
        
        var audioOperation = new LocalizedAsset<AudioClip>() { TableReference = tableName, TableEntryReference = keyName }.LoadAssetAsync();
        
        audioOperation.WaitForCompletion();

        if (audioOperation.Status == AsyncOperationStatus.Succeeded)
        {            
            localizedClip = audioOperation.Result;
        }
        else
        {            
            Debug.LogError($"Failed to load localized audio clip for tableName: {tableName}, keyName: {keyName}");
        }

        return localizedClip;
    }   
    
    public Sprite LocalizeSpriteImage(string tableName,string keyName)
    {
        Sprite sprite = null;

        var spriteOperation = new LocalizedAsset<Sprite>() { TableReference = tableName, TableEntryReference = keyName }.LoadAssetAsync();

        spriteOperation.WaitForCompletion();
        
        if (spriteOperation.Status == AsyncOperationStatus.Succeeded)
        {
            sprite = spriteOperation.Result;
        }
        else
        {
            Debug.LogError($"Failed to load localized sprite for tableName: {tableName}, keyName: {keyName}");
        }

        return sprite;
    }
}
