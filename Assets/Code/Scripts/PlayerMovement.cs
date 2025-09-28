using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private PlayerActions _playerActions;
    private Rigidbody2D _rbody;
    private Vector2 _moveInput;

    // Called once when the script instance is loaded
    void Awake()
    {
        _playerActions = new PlayerActions();

        _rbody = GetComponent<Rigidbody2D>();
        if ( _rbody is null )
        {
            Debug.LogError("Rigidbody2D is NULL!");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector2>();
        _moveInput.y = 0f;
        _rbody.linearVelocity = _moveInput * _speed;
    }
}
