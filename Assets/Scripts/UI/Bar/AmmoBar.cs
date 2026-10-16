using UnityEngine;

// Bar for tracking ammo.
public class AmmoBar : Bar {    

    protected override int GetCurrent() {
        return GameManager.Instance.GetAmmo();
    }

    protected override int GetMax() {
        return GameManager.Instance.GetMaxAmmo();
    }

    private void Awake() {
        GameManager.Instance.OnAmmoChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnAmmoChanged -= OnVariableChangedHandler;
    }
}
