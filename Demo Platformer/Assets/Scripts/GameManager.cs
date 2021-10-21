using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator SceneTransitionAnimator;
    public bool loading;

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

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    public IEnumerator LoadScene(string sceneName)
    {
        // Avoid loading scene multiple times
        if (loading) yield break;
        loading = true;

        SceneTransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);
        SceneManager.LoadScene(sceneName);
        SceneTransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);

        loading = false;
    }
}
