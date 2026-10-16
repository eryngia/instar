using UnityEngine;
using System.Collections;

// Simple animator to switch sprites at runtime.
public class SpriteAnimator : MonoBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;   
    [SerializeField] private Sprite[] sprites;                
    [SerializeField] private float speed = 0.5f;

    private int currentFrame;

    public void Initialize(Sprite[] sprites, float speed) {
        this.sprites = sprites;
        this.speed = speed;
    }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimateSprites());
    }

    // Cycle between sprites and update sprite renderer.
    private IEnumerator AnimateSprites() {
        while (true) {
            spriteRenderer.sprite = sprites[currentFrame];
            currentFrame = (currentFrame + 1) % sprites.Length;
            yield return new WaitForSeconds(speed);
        }
    }
}
