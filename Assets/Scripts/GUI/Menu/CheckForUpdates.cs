using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using SmartLocalization;

public class CheckForUpdates : MonoBehaviour {

    const string ITCH_UPDATES_API = "https://itch.io/api/1/x/wharf/latest?game_id=123343&channel_name=";
    const string CHANNEL_WINDOWS = "windows";
    const string CHANNEL_OSX = "osx";

    Text gameVersionText;

    protected LanguageManager m_LanguageManagerInstance;

    public class GameVersionInfo
    {
        public string latest;
    }

    void Awake()
    {
        gameVersionText = GetComponent<Text>();
        gameVersionText.text = "V: " + Application.version;
        m_LanguageManagerInstance = LanguageManager.Instance;
       
        string systemChannel;

#if UNITY_STANDALONE_WIN
        systemChannel = CHANNEL_WINDOWS;
#elif UNITY_STANDALONE_OSX
        systemChannel = CHANNEL_OSX;
#endif

        StartCoroutine(GetLatestVersion(ITCH_UPDATES_API + systemChannel, CheckGameVersion));

    }

    IEnumerator GetLatestVersion(string apiCall, Action<GameVersionInfo> onSuccess)
    {
        using(UnityWebRequest req = UnityWebRequest.Get(String.Format(apiCall)))
        {
            yield return req.Send();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string versionJSON = System.Text.Encoding.Default.GetString(result);
            GameVersionInfo info = JsonUtility.FromJson<GameVersionInfo>(versionJSON);
            onSuccess(info);
        }
    }

    public void CheckGameVersion(GameVersionInfo gameVersion)
    {
        if (gameVersionText != null && gameVersion != null && gameVersion.latest != Application.version && IsBiggerVersion(gameVersion.latest))
            gameVersionText.text = m_LanguageManagerInstance.GetTextValue("Main.UpdateAvailable");
       
    }

    bool IsBiggerVersion(string version)
    {
       
        switch (String.CompareOrdinal(version, Application.version))
        {
            case 1: return true;
            case 0: return false;
            
            default: return false;
        }
    }
}
