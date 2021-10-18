using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator SceneTransitionAnimator;

    [Header("Scene Transition")]
    public float sceneTransitionTime;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator LoadScene(string sceneName)
    {
        SceneTransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);
        SceneManager.LoadScene(sceneName);
        SceneTransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);
    }
}
