
// Text display for health.
public class HealthTextDisplay : TextDisplay {

    protected override int GetValue() {
        return GameManager.Instance.GetHealth();
    }
    
    private void Awake() {
        GameManager.Instance.OnHealthChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnHealthChanged -= OnVariableChangedHandler;
    }

}
