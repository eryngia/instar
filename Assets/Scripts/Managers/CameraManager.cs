using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using static ColorMap;

// Manages camera behaviour across scenes.
public class CameraManager : MonoBehaviour {

    public static CameraManager Instance { get; private set; }

    public Palette palette {get; private set;}
    public int paletteIndex {get; private set;}

    private Palette DEFAULT_PALETTE = new Palette {
        whiteColor = Color.white,
        blackColor = Color.black,
        redColor = Color.red
    };

    private void OnEnable() {
        sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        sceneLoaded -= OnSceneLoaded;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            palette = DEFAULT_PALETTE;
            paletteIndex = -1;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // set to base position
        transform.position = new Vector3(0, 0, -10);
        // set canvas reference camera to this camera
        FindAnyObjectByType<Canvas>().worldCamera = GetComponent<Camera>();
    }

    // Set color palette to new palette.
    public void SetColorPalette(Palette palette, int index) {
        this.palette = new Palette(palette);  
        paletteIndex = index;
    }

    // Set color palette to default.
    public void SetDefaultPalette() {
        palette = DEFAULT_PALETTE;
        paletteIndex = -1;
    }
}