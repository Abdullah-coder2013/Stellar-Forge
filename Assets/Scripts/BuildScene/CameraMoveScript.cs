using UnityEngine;
using UnityEngine.InputSystem;
public class CameraMoveScript : MonoBehaviour
{
    private PlayerInputs inputActions;
    [SerializeField] private float speed = 10;
    #region Variables

    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

    #endregion

#if UNITY_ANDROID
    private bool onMobile = true;
#endif

    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.PlayerInputActions.Enable();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 inputVector = inputActions.PlayerInputActions.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += direction * (Time.deltaTime * speed);
    }
   
    

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.position = _origin - _difference;
    }

    private Vector3 GetMousePosition
    {
        get
        {
            if (onMobile) if (Input.touchCount > 0) return _mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                else
                {
                    return _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                }
            return new Vector3();
        }
    }
}

