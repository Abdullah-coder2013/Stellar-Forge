using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    private PlayerInputs inputActions;
    [SerializeField] private float speed = 10;
    
    private void Awake() {
        inputActions = new PlayerInputs();
        inputActions.PlayerInputActions.Enable();
    }
    private void Update() {
        Vector2 inputVector = inputActions.PlayerInputActions.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += direction * Time.deltaTime * speed;
    }
}
