
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages loading scenes.
public class SceneManager : MonoBehaviour {
    public static SceneManager Instance { get; private set; }
    private const string START_SCENE = "Start";
    private const string GAME_SCENE = "Level";
    private const string CONTINUE_SCENE = "Continue";
    private const string END_SCENE = "End";
    
    [SerializeField] private float loadDelay = 0.5f;
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private GameObject pausedScreenPrefab;

    private GameObject pausedScreenInstance;
    private PlayerInput thisInput;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            thisInput = GetComponent<PlayerInput>();
        } else {
            Destroy(gameObject);
        }
    }

    // Load the game level scene asynchronously with a loading screen.
    public void LoadGameScene() {
        LoadingScreen();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GAME_SCENE);
    }

    // Load the level complete scene.
    public void LoadContinueScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CONTINUE_SCENE);
    }

    // Load the game over scene after a short delay.
    public void LoadEndScene() {
        StartCoroutine(LoadSceneDelayed(END_SCENE));
    }

    // Load the start scene and restart.
    public void LoadStartScene() {
        GameManager.Instance.Restart();
        UnityEngine.SceneManagement.SceneManager.LoadScene(START_SCENE);        
    }

    // Display the loading screen over the scene.
    public void LoadingScreen() {
        Canvas canvas = FindObjectOfType<Canvas>();
        Instantiate(loadingScreenPrefab, canvas.transform, false);
    }

    // Load scene with short delay.
    private IEnumerator LoadSceneDelayed(string scene) {
        yield return new WaitForSeconds(loadDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    // Pause the game.
    private void OnPause() {
        Debug.Log("Pausing!");

        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>()) {
            input.actions.Disable();
        }

        thisInput.SwitchCurrentActionMap("Paused");
        thisInput.actions.Enable();


        Time.timeScale = 0f;
        AudioListener.pause = true;
        
        Canvas canvas = FindObjectOfType<Canvas>();
        pausedScreenInstance = Instantiate(pausedScreenPrefab, canvas.transform, false);
    }

    // Unpause the game.
    private void OnUnpause() {
        Debug.Log("Unpausing!");

        foreach (PlayerInput input in FindObjectsOfType<PlayerInput>()) {
            input.actions.Enable();
        }

        thisInput.actions.Disable();
        thisInput.SwitchCurrentActionMap("Menu");

        Time.timeScale = 1f;
        AudioListener.pause = false;

        StartCoroutine(ReenableInput());
        Destroy(pausedScreenInstance);
    }

    private IEnumerator ReenableInput() {
        yield return new WaitForSecondsRealtime(0.5f);
        thisInput.actions.Enable();
    }


}
