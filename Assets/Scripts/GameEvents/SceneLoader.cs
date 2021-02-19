using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {

    [SerializeField]
    string m_SceneName;

    [SerializeField]
    string m_MenuSceneName;

    [SerializeField]
    GameObject m_GameOver;
    [SerializeField]
    GameObject m_BattleCanvas;

    public void ChangeScene()
    {
        SceneManager.LoadScene(m_SceneName);
    }

    public void ShowGameOver()
    {
        m_BattleCanvas.SetActive(false);
        m_GameOver.SetActive(true);
    }

    public void ReturnToMenu()
    {
        Game.ResetGame();
        SceneManager.LoadScene(m_MenuSceneName);
    }
}
