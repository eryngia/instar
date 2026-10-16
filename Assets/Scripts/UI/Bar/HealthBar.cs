
// Bar for tracking health.
public class HealthBar : Bar {    

    protected override int GetCurrent() {
        return GameManager.Instance.GetHealth();
    }

    protected override int GetMax() {
        return GameManager.Instance.GetMaxHealth();
    }

    private void Awake() {
        GameManager.Instance.OnHealthChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnHealthChanged -= OnVariableChangedHandler;
    }
}
