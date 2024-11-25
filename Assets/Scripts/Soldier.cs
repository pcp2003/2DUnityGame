using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D enemy;
    public Vector2 move;
    public bool Vertical;

    //Variavais de tempo
    public float changeTime = 3.0f;
    private float timer;
    private int direction = 1;

    // animacoes
    Animator animator;
    //projeticil
    bool broken = true;

    

    // Start is called before the first frame update
    void Start()
    {
        timer = changeTime;
        enemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

    }





    void FixedUpdate() {
        Vector2 position = enemy.position;
        if (!broken)
        {
            return;
        }

        if (Vertical)
        {
            position.y = position.y + moveSpeed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + moveSpeed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);

        }
    
        enemy.MovePosition(position);
    }



    public void Fix()
    {
        broken = false;
        enemy.simulated = false;
        Destroy(gameObject);
    }




}

