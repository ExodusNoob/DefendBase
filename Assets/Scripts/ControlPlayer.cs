using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    private InputSystem_Actions controles;
    public Vector2 direccion;
    private Rigidbody2D _compRigidBody2;
    public int VelocidadPlayer = 10;

    private Animator _compAnimator;
    private SpriteRenderer _compSpriteRenderer;
    public bool IsMoving = false;
    public float horizontal;
    private void Awake()
    {
        controles = new();

        _compAnimator = GetComponentInChildren<Animator>();
        _compRigidBody2 = GetComponent<Rigidbody2D>();
        _compSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnEnable()
    {
        controles.Enable();
    }
    private void OnDisable()
    {
        controles.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = direccion.x;
        direccion = controles.Player.Move.ReadValue<Vector2>(); 

        // Verifica si el jugador se está moviendo
        IsMoving = direccion.sqrMagnitude > 0;

        // Actualiza el Animator solo si el jugador se está moviendo
        if (IsMoving)
        {
            if(horizontal != 0)
            {
                _compAnimator.SetInteger("SideMovement", horizontal < 0 ? -1 : 1);

            }

            // Cambia la dirección del sprite según el movimiento
            if (horizontal < 0)
            {
                _compSpriteRenderer.flipX = true;
            }
            else if (horizontal > 0)
            {
                _compSpriteRenderer.flipX = false;  
            }
        }
        else
        {
            // Detén la animación si no hay movimiento
            _compAnimator.SetInteger("SideMovement", 0);
        }
    }
    private void FixedUpdate()
    {
        _compRigidBody2.MovePosition(_compRigidBody2.position + Time.fixedDeltaTime * VelocidadPlayer * direccion.normalized);
    }
}
