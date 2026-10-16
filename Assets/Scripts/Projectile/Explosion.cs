
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Expanding explosion projectile.
public class Explosion : MonoBehaviour, IHittable {

    const float INTERVAL = 0.1f;

    private Entity owner;
    private ExplosionSO explosionSO;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private float currentSize = 0f;
    private List<IHittable> entitiesHit = new List<IHittable>();

    private void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        spriteRenderer.sprite = explosionSO.sprite;
        spriteRenderer.transform.localScale = Vector3.zero;

        StartCoroutine(Explode());

        AudioUtils.PlayRandom(GameManager.Constants.explosionAudio);
    }

    public void Initialize(Entity owner, ExplosionSO explosionSO) {
        this.owner = owner;
        this.explosionSO = explosionSO;
    }

    // Continutally increase in size until maximum reached, then delete.
    private IEnumerator Explode() {
        while (currentSize < explosionSO.size) {
            currentSize += explosionSO.rate;

            circleCollider.radius = currentSize / GameManager.Constants.pixelsPerUnit;

            float spriteSize = currentSize / GameManager.Constants.pixelsPerUnit / (explosionSO.sprite.texture.width / explosionSO.sprite.pixelsPerUnit);
            spriteRenderer.transform.localScale = new Vector3(spriteSize * 2, spriteSize * 2, 1);

            yield return new WaitForSeconds(INTERVAL);
        }
        Destroy(gameObject);
    }

    public int Hit(IHittable target) {
        // track entities hit so each is only damaged once
        if (entitiesHit.Contains(target)) {
            return 0;
        } else {
            entitiesHit.Add(target);
            return explosionSO.damage;
        }
    }

    public void ReceiveHit(IHittable source, int damage) {
        // cannot take damage
    }

}