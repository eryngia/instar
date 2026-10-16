
// An interface for an object with a hitbox that can take or deal damage.
public interface IHittable {
    
    // Return the damage this hit deals to the target.
    int Hit(IHittable target);

    // Receive damage from a source.
    void ReceiveHit(IHittable source, int damage);
}