using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    // Variáveis relacionadas ao ataque
    public float attackRange = 1.0f; // Alcance do ataque
    public int attackDamage = 1;     // Dano do ataque
    public LayerMask enemyLayer;     // Camada dos inimigos
    public Transform attackPoint;    // Ponto de origem do ataque (crie um objeto vazio na frente do jogador)
    public float attackCooldown = 0.7f; // Duração do ataque (igual ao HasExitTime)

    private bool isAttacking = false; // Controle se o jogador está atacando

    Animator animator;
    Vector2 moveDirection = new Vector2(0, 0);

    //Variaveis de vida/comida
    public int maxHealth = 5;
    int currentHealth;
    
    
    // Variables related to temporary invincibility
    public float timeInvincible = 1.0f;   // 1 segundo invincivel
    bool isInvincible;
    float damageCooldown;
    
    public int health { get { return currentHealth; } }
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leitura do movimento
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.magnitude, 0.0f) && !isAttacking) // Permite mudar de direção somente fora do ataque
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Detectar ataque
        if (Mouse.current.leftButton.wasPressedThisFrame && !isAttacking) // Apenas permite atacar se não estiver atacando
        {
            Debug.Log("Mouse Left Button Pressed");
            Attack();
        }
        
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
                isInvincible = false;
        }
        
        
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    void Attack()
    {
        isAttacking = true; // Impede que outros ataques sejam iniciados
        animator.SetTrigger("Attack");

        // Opcional: Chamar método para detectar e aplicar dano aqui

        StartCoroutine(ResetAttackState()); // Espera o cooldown antes de permitir outro ataque
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown); // Tempo do HasExitTime
        isAttacking = false; // Permite um novo ataque
    }
    
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }


        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
     //   UIHandler2.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
    
    
    
}
