using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [SerializeField]
    string mainSceneName;

    [SerializeField]
    GameObject mainPanel;

    MainMenuPanel mainMenuPanel;

	void Awake()
    {
        mainMenuPanel = mainPanel.GetComponent<MainMenuPanel>();
    }

    public void StartGame(AudioSource audioClip)
    {
        if (audioClip == null)
            SceneManager.LoadScene(mainSceneName);
        else
            StartCoroutine(StartAfterSound(audioClip));
    }

    public void Cancel(GameObject panel, bool returnToDefaultPanel, GameObject prevButton)
    {
        HideGameObject(panel);

        if (returnToDefaultPanel)
            ShowGameObject(mainPanel);

        if (prevButton != null)
            mainMenuPanel.StartCoroutine("SelectButton",prevButton);
    }

    public void ShowGameObject(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HideGameObject(GameObject panel)
    {
        panel.SetActive(false);
    }

    public IEnumerator ExitGameCoroutine(AudioSource audio)
    {
        yield return StartCoroutine("WaitUntilFinished", audio);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ExitGame(AudioSource audio)
    {
        StartCoroutine(ExitGameCoroutine(audio));
    }

    IEnumerator WaitUntilFinished(AudioSource audio)
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }

    IEnumerator StartAfterSound(AudioSource audioClip)
    {
        yield return WaitUntilFinished(audioClip);
        SceneManager.LoadScene(mainSceneName);
    }
}
