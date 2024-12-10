using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public int maxSaltos = 2; // Cantidad de saltos permitidos

    [Header("Componentes")]
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private bool mirandoDerecha = true;
    private int saltosRestantes;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // Inicializa los saltos restantes
        saltosRestantes = maxSaltos;
    }

    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
    }

    void ProcesarSalto()
    {
        // Comprobamos si se presiona la tecla de salto y quedan saltos disponibles
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, fuerzaSalto);
            animator.SetTrigger("Jump");
            saltosRestantes--; // Reduce el número de saltos restantes
        }

        // Reinicia los saltos al tocar el suelo
        if (EstaEnSuelo())
        {
            saltosRestantes = maxSaltos;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");

        // Movimiento horizontal continuo
        rigidbody2D.velocity = new Vector2(inputMovimiento * velocidad, rigidbody2D.velocity.y);

        // Animación de correr
        animator.SetBool("isRunning", inputMovimiento != 0);

        // Gestionar orientación
        GestionarOrientacion(inputMovimiento);
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }


    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            LayerMask.GetMask("Ground")
        );
        return raycastHit.collider != null;
    }
}
