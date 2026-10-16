using UnityEngine;
using UnityEngine.UI;

// Bar for tracking score streak state.
public class ScoreBoostBar : MonoBehaviour {

    [SerializeField] private Sprite boostedSprite;
    [SerializeField] private Sprite unboostedSprite;
    [SerializeField] private float boostedHeight;
    [SerializeField] private float unboostedHeight;
    [SerializeField] private float maxWidth;

    private PlayerScoreBoost playerScoreBoost;
    private RectTransform rectTransform;
    private Image image;

    private void Start() {
        playerScoreBoost = FindObjectOfType<PlayerScoreBoost>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update() {
        image.sprite = playerScoreBoost.IsBoosted() ? boostedSprite : unboostedSprite; // colored highlight when boost active
        rectTransform.sizeDelta = new Vector2(
            playerScoreBoost.GetBoostProgress() * maxWidth, // width indicates boost progress
            playerScoreBoost.IsBoosted() ? boostedHeight : unboostedHeight // thicker when boost active
        );
    }

}