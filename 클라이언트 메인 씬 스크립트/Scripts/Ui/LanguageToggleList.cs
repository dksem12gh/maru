using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class LanguageToggleList : MonoBehaviour
{
    public Transform container;
    public GameObject languageTogglePrefab;
    public Sprite[] flagSprite;

    AsyncOperationHandle m_InitializeOperation;
    Dictionary<Locale, TouchEventHandler> m_Toggles = new Dictionary<Locale, TouchEventHandler>();    

    void Start()
    {
        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
        if (m_InitializeOperation.IsDone)
        {
            InitializeCompleted(m_InitializeOperation);
        }
        else
        {
            m_InitializeOperation.Completed += InitializeCompleted;
        }
    }

    void InitializeCompleted(AsyncOperationHandle obj)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        
        for (int i = 0; i < locales.Count; ++i)
        {
            var locale = locales[i];            
            var languageToggle = Instantiate(languageTogglePrefab, container);
            languageToggle.name = locale.Identifier.CultureInfo != null ? locale.Identifier.CultureInfo.NativeName : locale.ToString();
            var flag = languageToggle.GetComponentInChildren<Image>();
            flag.sprite = flagSprite[i];            

            var touch = languageToggle.GetComponentInChildren<TouchEventHandler>();

            m_Toggles[locale] = touch;

            touch.m_PressedEvent.AddListener(val =>
            {
                LocalizationSettings.SelectedLocale = locale;

                //Managers.Data.JsonLocaleSave(locale);            
            });
        }
    }
}
