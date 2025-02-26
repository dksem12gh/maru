using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Doozy.Runtime.UIManager.Containers;

public class MusicElementBar : MonoBehaviour
{    
    [SerializeField] Image _fillImage;
    [SerializeField] GameObject _free;
    [SerializeField] GameObject _mode;

    AudioSource targetObj;

    private void OnEnable()
    {
        _fillImage.fillAmount = 0;

        if (Managers.SelGameSet.rhythmMode == RhythmGameMode.FreeMode)
        {
            _free.SetActive(true);
            _mode.SetActive(false);
            StopCoroutine(MusicSelect());
        }
        else
        {
            _free.SetActive(false);
            _mode.SetActive(true);
            StartCoroutine(MusicSelect());
        }                
    }

    IEnumerator MusicSelect()
    {
        while (true)
        {
            yield return YieldInstructionCache.WaitUntil(() => Managers.SelGameSet.selectGameState == SelectGameState.Play);

            Scene targetScene = SceneManager.GetSceneByName("Rhythm Game");

            if (targetScene.isLoaded)
            {
                if (targetObj == null)
                {
                    targetObj = GameObject.Find("Audio Source")?.GetComponent<AudioSource>();
                    if (targetObj == null)
                        yield break;
                }

                while (_fillImage.fillAmount < 1)
                {
                    if (targetObj == null)
                        yield break;

                    var progress = targetObj.time / targetObj.clip.length;
                    _fillImage.fillAmount = progress;
                    yield return null;
                }

                _fillImage.fillAmount = 1;
                yield break;
            }
        }
    }
}