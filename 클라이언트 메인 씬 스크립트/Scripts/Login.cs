using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Profiling;

public class Login : MonoBehaviour
{
    [SerializeField] BoxCollider _start;
    [SerializeField] GameObject touchObj;    

    public void Start()
    {
        //LoadLocale("ko");

        touchObj.SetActive(false);

        Application.runInBackground = true;

        StartCoroutine(StartDel());
    }

    public void LoadLocale(string languageIdentifier)
    {
        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if (anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }        
    }

    IEnumerator StartDel()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        _start.enabled = true;
        touchObj.SetActive(true);
    }
}
