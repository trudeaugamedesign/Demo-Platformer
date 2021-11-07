using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoToScene : MonoBehaviour
{
    public void goToScene(string sceneName)
    {
        print(GameManager.instance);
        GameManager.instance.StartCoroutine(GameManager.instance.LoadScene(sceneName));
    }
}
