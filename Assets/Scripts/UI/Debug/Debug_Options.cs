using UnityEngine;

// Debug options to set test seed and reduce game speed.
public class Debug_Options : MonoBehaviour {

    [SerializeField] private int seed1 = 1;
    [SerializeField] private int seed2 = 2;
    [SerializeField] private int seed3 = 3;
    [SerializeField] private float timeScale = 0.25f;

    private void On_1() {
        Random.InitState(seed1);
        Debug.Log("Set seed " + seed1);
    }

    private void On_2() {
        Random.InitState(seed2);
        Debug.Log("Set seed " + seed2);
    }

    private void On_3() {
        Random.InitState(seed3);
        Debug.Log("Set seed " + seed3);
    }

    private void On_0() {
        Debug.Log("Set time scale " + timeScale);
        if (Time.timeScale < 1) 
            Time.timeScale = 1;
        else 
            Time.timeScale = timeScale;
    }

}