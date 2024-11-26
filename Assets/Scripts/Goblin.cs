using UnityEngine;

public class Goblin : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float speed = 2.0f; // Velocidade de movimento do Goblin
    public float attackRange = 1.0f; // Distância de ataque
    public float attackInterval = 1.0f; // Intervalo entre ataques

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private float distance;
    private float nextAttackTime = 0f; // Tempo para o próximo ataque

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogError("Player reference not set on Goblin.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calcula a direção para o jogador
            Vector2 direction = player.position - transform.position;
            distance = direction.magnitude;

            // Atualiza a direção de movimento se estiver fora da distância mínima
            if (distance > attackRange)
            {
                moveDirection = direction.normalized;
                animator.SetFloat("Speed", distance);
            }
            else
            {
                moveDirection = Vector2.zero; // Para de mover ao atingir a distância mínima
                animator.SetFloat("Speed", 0);

                // Verifica se está na range de ataque
                if (distance <= attackRange && Time.time >= nextAttackTime)
                {
                    Attack(); // Realiza o ataque
                    nextAttackTime = Time.time + attackInterval; // Atualiza o próximo tempo de ataque
                }
            }

            // Atualiza o sprite para olhar na direção correta
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void FixedUpdate()
    {
        // Move o Goblin apenas se estiver longe o suficiente do jogador
        if (moveDirection != Vector2.zero)
        {
            Vector2 position = rigidbody2d.position + moveDirection * speed * Time.fixedDeltaTime;
            rigidbody2d.MovePosition(position);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
    
    //teste de ataque
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Goblin teriou vida ao jogador");
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

    }
    
    
    
    
}
