using UnityEngine;

// An abstract fighter entity.
public abstract class Entity : MonoBehaviour, IHittable {

    [SerializeField] private Transform firePoint;

    // Get the entity layer.
    public abstract int GetLayer();

    // Get the entity projectile layer.
    public abstract int GetProjectileLayer();

    public void Initialize(Transform firePoint) {
        this.firePoint = firePoint;
    }

    // Get the firing point of the entity.
    public Transform GetFirePoint() {
        return firePoint;
    }

    public abstract int Hit(IHittable target);

    public abstract void ReceiveHit(IHittable source, int damage);

}
