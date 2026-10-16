
// Text display for score.
public class ScoreTextDisplay : TextDisplay {

    protected override int GetValue() {
        return GameManager.Instance.GetScore();
    }

    private void Awake() {
        GameManager.Instance.OnScoreChanged += OnVariableChangedHandler;
    }

    private void OnDisable() {
        GameManager.Instance.OnScoreChanged -= OnVariableChangedHandler;
    }

}
