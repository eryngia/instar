using System.Collections;
using UnityEngine;

// An abstract framework for common entity visuals.
public abstract class Visuals : MonoBehaviour {

    protected Camera mainCamera;
    [SerializeField] protected Material flashMaterial;
    protected SpriteRenderer spriteRenderer;

    private bool flashing = false;

    private void Awake() {
        mainCamera = FindObjectOfType<Camera>();
    }

    // Repeatedly flash the alpha of the sprite back and forth.
    protected IEnumerator Flashing(float alpha, float duration, float interval) {
        Color alphaColor = spriteRenderer.color;
        alphaColor.a = alpha;
        yield return StartCoroutine(Flashing(alphaColor, duration, interval));
    }

    // Repeatedly flash the color of the sprite back and forth.
    protected IEnumerator Flashing(Color color, float duration, float interval) {
        if (flashing) yield break;
        else flashing = true;

        for (int i = 0; i < duration / interval / 2f; i++) {
            yield return StartCoroutine(Flash(color, interval));
            yield return new WaitForSeconds(interval);
        }

        flashing = false;
    }

    // Perform a single flash to a color and back.
    protected IEnumerator Flash(Color color, float duration) {
        if (flashing) yield break;
        else flashing = true;

        Material originalMaterial = spriteRenderer.material;
        Color originalColor = spriteRenderer.color;

        spriteRenderer.material = flashMaterial;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;
        spriteRenderer.color = originalColor;

        flashing = false;
    }

    // Play the given particles at this position.
    protected void PlayParticles(ParticleSystem particles) {
        ParticleSystem particlesInstance = Instantiate(particles, transform.position, transform.rotation);
        particlesInstance.Play();
        Destroy(particlesInstance.gameObject, particlesInstance.main.duration);
    }

    // Shake the screen randomly with the given magnitude and duration.
    protected IEnumerator ShakeScreen(float shakeMagnitude, float shakeDuration) {
        Vector3 initialCameraPosition = new Vector3(0, 0, -10);
        float elapsedTime = 0f;
        float unitsPerPixel = 1f / GameManager.Constants.pixelsPerUnit;

        while (elapsedTime < shakeDuration) {
            Vector2 rawOffset = UnseededRandomUtils.InsideUnitCircle() * shakeMagnitude;

            Vector2 pixelAlignedOffset = new Vector2(
                Mathf.Round(rawOffset.x / unitsPerPixel) * unitsPerPixel,
                Mathf.Round(rawOffset.y / unitsPerPixel) * unitsPerPixel
            );

            mainCamera.transform.position = initialCameraPosition + (Vector3) pixelAlignedOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = initialCameraPosition;
    }
}
