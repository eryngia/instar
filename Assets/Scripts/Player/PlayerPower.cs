using UnityEngine;

public class PlayerPower : MonoBehaviour {

    private void Start() {
        // activate all player related powers
        Player player = GetComponent<Player>();
        foreach (PowerSO powerSO in GameManager.Instance.GetChosenPowers()) {
            powerSO.PlayerPower(player);
        }
    }
    
}
