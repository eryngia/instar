
// Text display for total score.
public class TotalScoreTextDisplay : TextDisplay {

    protected override int GetValue() {
        return GameManager.Instance.GetTotalScore();
    }

    private void Awake() {
        GameManager.Instance.OnScoreChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnScoreChanged -= OnVariableChangedHandler;
    }

}
