using UnityEngine;
using UnityEngine.InputSystem;

// Receive player input and transmit to other components.
public class PlayerController : MonoBehaviour {

    private PlayerInput playerInput;
    private PlayerMove playerMove;
    private PlayerDash playerDash;
    private PlayerFire playerFire;
    private PlayerAbility playerAbility;
    public Vector2 moveDirectionInput {private set; get;}

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        playerDash = GetComponent<PlayerDash>();
        playerFire = GetComponentInChildren<PlayerFire>();
        playerAbility = GetComponentInChildren<PlayerAbility>();

    }

    private void Update() {
        playerMove.Move(moveDirectionInput);
    }

    private void OnMove(InputValue value) {
        moveDirectionInput = value.Get<Vector2>();
    }

    private void OnDash() {
        playerDash.Dash(moveDirectionInput);
    }

    private void OnFire(InputValue value) {
        if (value.isPressed) {
            playerFire.StartFire();
        } else {
            playerFire.StopFire();
        }
    }

    private void OnAbility() {
        playerAbility.Ability();
    }

    public Vector2 GetMoveDirectionInput() {
        return moveDirectionInput;
    }

    public void EnableInput() {
        playerInput.actions.Enable();
    }

    public void DisableInput() {
        playerFire.StopFire();
        playerInput.actions.Disable();
    }
}
