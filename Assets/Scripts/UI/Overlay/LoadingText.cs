using UnityEngine;
using TMPro;
using System.Collections;

// Simple loading animation (unused becuase Unity async loading is not truly async and freezes this).
public class LoadingText : MonoBehaviour {

    private const int MAX_DOTS = 3;

    [SerializeField] private string baseText = "Loading";
    [SerializeField] private float delay = 0.5f;
   
    private TextMeshProUGUI textUI;
    private int dotCount = 0;


    void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText() {
        while (true) {
            dotCount = (dotCount + 1) % (MAX_DOTS + 1);
            textUI.text = baseText + new string('.', dotCount)  + new string('\u00A0', 3 - dotCount); 
            yield return new WaitForSeconds(delay);
        }
    }
}
