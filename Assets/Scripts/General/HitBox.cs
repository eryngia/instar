using UnityEngine;

// The behvaiour of a collider attached to an IHittable object.
public class HitBox : MonoBehaviour {
   
    private IHittable owner;

    private void Start() {
        owner = GetComponentInParent<IHittable>();
    }

    // transfer hit to owner
    public void ReceiveHit(IHittable source, int damage) {
        owner.ReceiveHit(source, damage);
    }

    // send hit to other collider
    public void OnTriggerEnter2D(Collider2D other) {
        HitBox otherHitBox = other.GetComponent<HitBox>();
        otherHitBox.ReceiveHit(owner, owner.Hit(otherHitBox.GetOwner()));
    }

    public IHittable GetOwner() {
        return owner;
    }

}
