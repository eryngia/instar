using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float movePaddingX = 0.5f;
    [SerializeField] private float movePaddingY = 0.5f;

    private bool isMovingRight, isMovingLeft = false;

    private float minXPos, maxXPos, minYPos, maxYPos;
    private int lowHealthMaxBonusSpeed;
    private float lowHealthCurveExponent;
    private float extraLowHealthSpeed;

    private void Start() {
        // calculate bounds
        maxXPos = GameManager.Constants.screenWidth / 2 - movePaddingX;
        minXPos = - maxXPos;
        maxYPos = GameManager.Constants.screenHeight / 2 - movePaddingY;
        minYPos = - maxYPos;

        PowerSO_LowHealth_Speed lowHealthPower = GameManager.Instance.GetChosenPower<PowerSO_LowHealth_Speed>();
        if (lowHealthPower != null) {
            lowHealthMaxBonusSpeed = lowHealthPower.maxBonusSpeed;
            lowHealthCurveExponent = lowHealthPower.curveExponent;
            GetComponent<Player>().OnPlayerDamaged += LowHealthPower;
        }
    }

    // Move in direction based on speed within bounds.
    public void Move(Vector2 moveDirection) {
        Vector3 delta = moveDirection * moveSpeed * Time.deltaTime;

        float newXPos = Mathf.Clamp(transform.position.x + delta.x, minXPos, maxXPos);
        float newYPos = Mathf.Clamp(transform.position.y + delta.y, minYPos, maxYPos);
        transform.position = new Vector3(newXPos, newYPos, transform.position.z);

        isMovingRight = moveDirection.x > 0;
        isMovingLeft = moveDirection.x < 0;
    }

    // Is currently moving to the right.
    public bool IsMovingRight() {
        return isMovingRight;
    }

    // Is currently moving to the left.
    public bool IsMovingLeft() {
        return isMovingLeft;
    }

    // Change movement speed based on powers at low health.
    private void LowHealthPower(object sender, EventArgs e) {
        moveSpeed -= extraLowHealthSpeed;

        float normalizedHealth = (float) GameManager.Instance.GetHealth() / GameManager.Instance.GetMaxHealth();
        float curve = Mathf.Pow(1 - normalizedHealth, lowHealthCurveExponent);
        extraLowHealthSpeed = (int)(curve * lowHealthMaxBonusSpeed);        
        
        moveSpeed += extraLowHealthSpeed;
    }
}