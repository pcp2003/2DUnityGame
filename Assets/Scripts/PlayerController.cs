using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    // Variable related to Player's Health System
    public int maxHealth = 5; // Maximum Health Points
    int currentHealth; // Current Health Points


    // Variables related to Character Movement
    public float speed; // Character Speed
    public InputAction MoveAction; // Input to Move
    Rigidbody2D rigidbody2d; // Character RigidBody
    Vector2 move; // Vector of the MoveAction


    // Script that runs when the game starts
    void Start() {
        currentHealth = maxHealth;
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Script that runs every frame
    void Update() {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
    }

    // Script that runs in fixed time intervals
    void FixedUpdate() {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
