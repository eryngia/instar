using UnityEngine;

// Scrolls an environment upwards at a steady rate.
public class Scrolling : MonoBehaviour {

    [SerializeField] private float scrollSpeed = 0.5f;

    private SpriteRenderer spriteRenderer;
    private float startPositionY;

    private void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startPositionY = transform.position.y;
    }

    // Continue shifting the environment upwards at the given speed.
    private void Update() {
        if (spriteRenderer.sprite == null) return;

        float deltaY = - scrollSpeed * Time.deltaTime;

        if (transform.position.y + deltaY < startPositionY - spriteRenderer.sprite.bounds.size.y + GameManager.Constants.screenHeight) {
           Debug.LogWarning("Reached end of scrolling environment!");
        } else {
            transform.position += new Vector3(0, deltaY, 0);
        }
    }


}
