
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IHittable {

    private const float MAX_LIFETIME = 10f;
    private const float RAY_WARNING_WIDTH = 0.25f;
    [SerializeField] private ProjectileSO projectileSO;
    private Entity owner;
    private float distanceMoved;
    private Transform target;
    private List<IHittable> entitiesHit = new List<IHittable>();
    private bool canCollide = true;

    private void Start() {
        Destroy(gameObject, MAX_LIFETIME);
        if (projectileSO.ray) StartCoroutine(RayWarmUp());
    }

    public void Initialize(Entity owner, ProjectileSO projectileSO) {
        this.owner = owner;
        this.projectileSO = projectileSO;
        gameObject.layer = owner.GetProjectileLayer();
    }

    private void Update() {
        Move();
        RayMove();
        HomeIn();
    }

    private void Move() {
        if (projectileSO.ray) return;

        Vector3 delta = transform.up * projectileSO.speed * Time.deltaTime;
        transform.position += delta;

        distanceMoved += delta.magnitude;
        if (distanceMoved > projectileSO.range) Expire();
    }

    private IEnumerator RayWarmUp() {
        canCollide = false;

        Vector3 originalScale, newScale;
        originalScale = newScale = transform.localScale;
        newScale.x *= RAY_WARNING_WIDTH;
        transform.localScale = newScale;

        yield return new WaitForSeconds(projectileSO.rayWarmUp);

        transform.localScale = originalScale;
        canCollide = true;  
        Destroy(gameObject, projectileSO.rayDuration);
    }

    private void RayMove() {
        if (!projectileSO.ray || gameObject == null)
            return;
        if (owner.GetFirePoint() == null) {
            Destroy(gameObject);
            return;
        }
        transform.position = owner.GetFirePoint().position + transform.up * projectileSO.height / GameManager.Constants.pixelsPerUnit / 2;
    }

    private void HomeIn() {
        if (projectileSO.homingStrength <= 0) return;
        if (target == null) target = FindTarget();
        if (target == null) return;
        
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        float rotateSpeed = projectileSO.homingStrength * Time.deltaTime; 
        Vector3 newDirection = Vector3.RotateTowards(transform.up, directionToTarget, rotateSpeed * Mathf.Deg2Rad, 0.0f);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection); 
    }

    private void Expire() {
        if (projectileSO.explosionSO != null && !projectileSO.ray)
            ExplosionCreator.Create(owner, projectileSO.explosionSO, transform.position);
        Destroy(gameObject);
    }

    protected abstract Transform FindTarget();

    public void ReceiveHit(IHittable source, int damage) {
        if (!canCollide) return;
        if (projectileSO.explosionSO != null)
            ExplosionCreator.Create(owner, projectileSO.explosionSO, transform.position);
        if (!projectileSO.piercing) 
            Destroy(gameObject);
    }

    public int Hit(IHittable target) {
        if (!canCollide) return 0;
        if (entitiesHit.Contains(target)) {
            return 0;
        } else {
            entitiesHit.Add(target);
            return projectileSO.damage;
        }
    }

}
