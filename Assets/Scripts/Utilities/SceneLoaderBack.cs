using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderBack : MonoBehaviour {

    public GameObject rootGameObject;

   void Update()
    {
        if (rootGameObject != null && Game.sceneOperation != null && Game.sceneOperation.isDone)
            ActivateSceneObjects();
    } 

    public void ActivateSceneObjects()
    {
        if (!rootGameObject.activeSelf && SceneManager.GetActiveScene().name.Equals("roomScene"))
        {
            rootGameObject.SetActive(true);
        }
    }
}
