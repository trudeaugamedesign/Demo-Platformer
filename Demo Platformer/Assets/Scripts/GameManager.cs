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

    [Header("Level Management")]
    private Vector2 spawnPosition;

    [Header("Pausing")]
    private bool paused = false;
    public GameObject pauseMenu;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Set object to do not destroy on load
    }

    private void Update()
    {
        Time.timeScale = paused ? 0 : 1;
        pauseMenu.SetActive(paused);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
    }
    public void ReloadScene()
    {
        Debug.Log("Reloading Scene");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }
    public void ReturnToMainMenu()
    {
        print("bruh");
        paused = false;
        StartCoroutine(LoadScene("Title Screen"));
    }
    public IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);

        Scene lastScene = SceneManager.GetActiveScene(); // Record last scene

        // Avoid loading scene multiple times
        if (loading)
        {
            yield break;
        }
        loading = true;

        SceneTransitionAnimator.SetTrigger("Transition"); // Transition out
        yield return new WaitForSeconds(sceneTransitionTime); // Wait for animation time
        SceneManager.LoadScene(sceneName); // Load scene
        SceneTransitionAnimator.SetTrigger("Transition"); // Transition in

        loading = false; // Re enable scene loading

        // Check conditions are right for respawing the player at a checkpoint
        if (spawnPosition != Vector2.zero && SceneManager.GetActiveScene() == lastScene)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = spawnPosition;
        }
        else spawnPosition = Vector2.zero;

        paused = false;
    }

    public void TriggerPause()
    {
        paused = !paused;
    }
}
