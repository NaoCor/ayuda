using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // Tiempo para saltar después de salir del suelo
    private float coyoteCounter; // Contador de tiempo fuera del suelo

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;


    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private bool mirandoDerecha = true;
    private bool ground;

    private void Awake()
    {
        // Referencias a componentes
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Voltear al personaje al moverse
        GestionarOrientacion(horizontalInput);

        // Configurar animaciones
        animator.SetBool("isRunning", horizontalInput != 0f);
        animator.SetBool("grounded", isGrounded());

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Ajustar altura del salto
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        // Aplicar movimiento horizontal
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (isGrounded())
        {
            coyoteCounter = coyoteTime; // Reiniciar contador al tocar el suelo
            jumpCounter = extraJumps;  // Reiniciar saltos extra
        }
        else
        {
            coyoteCounter -= Time.deltaTime; // Reducir tiempo fuera del suelo
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;

        if (isGrounded() || coyoteCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if (jumpCounter > 0) // Saltos extra
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            jumpCounter--;
        }

        // Reiniciar contador para evitar dobles saltos no deseados
        coyoteCounter = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    private void GestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }
}