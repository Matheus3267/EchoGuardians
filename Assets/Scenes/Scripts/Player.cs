using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    private CharacterController controller;

    public float walkSpeed = 5.0f;       // Velocidade no solo
    public float flightSpeed = 8.0f;     // Velocidade no ar
    public float gravity = -9.81f;       // Força da gravidade
    public float jumpHeight = 2.0f;      // Altura do pulo
    public float flightHeightSpeed = 3.0f;  // Velocidade de subir/descer no voo

    private Vector3 velocity;
    private bool isGrounded;
    private bool canWalk = true;
    private bool isFlying = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Verificar se está no chão
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Resetar a velocidade ao tocar o chão
        }

        // Alternar entre andar e voar ao pressionar "F"
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isGrounded) return;

            isFlying = !isFlying;
            if (isFlying)
            {
                velocity.y = 0;  // Resetar a velocidade vertical ao começar a voar
            }
        }

        // Movimentação e inputs
        if (!isFlying)
        {
            // Movimentação no chão
            MoveOnGround();
        }
        else
        {
            // Movimentação no voo
            Fly();
        }

        // Aplicar a movimentação no Character Controller
        controller.Move(velocity * Time.deltaTime);
    }

    void MoveOnGround()
    {
        if (canWalk)
        {
            // Entrada de movimentação no solo
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * walkSpeed * Time.deltaTime);

            // Pular
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Aplicar gravidade no chão
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void Fly()
    {
        // Entrada de movimentação no voo
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * flightSpeed * Time.deltaTime);

        // Controle da altura no voo
        if (Input.GetKey(KeyCode.Space)) // Subir
        {
            velocity.y = flightHeightSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) // Descer
        {
            velocity.y = -flightHeightSpeed;
        }
        else
        {
            velocity.y = 0;  // Manter a altura
        }
    }

}

