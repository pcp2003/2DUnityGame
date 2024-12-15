using UnityEngine;
using System.Collections;
using System.Data.Common;

public class EnemiesHealth : MonoBehaviour
{
    private float currentHealth;
    private bool isDying; // Controle para evitar múltiplas chamadas

    public AudioSource hitAudioSource;

    public AudioSource deathAudioSource;

    public static float volume = 1.0f;

    void Start(){
        
        if (name.Equals("Goblin(Clone)") ) currentHealth = GetComponent<Goblin>().getHealth();
        if (name.Equals("Soldier(Clone)")) currentHealth = GetComponent<Soldier>().getHealth();

        isDying = false;
        deathAudioSource.volume *= volume;
        hitAudioSource.volume *= volume;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDying)
        {
            if (deathAudioSource != null) deathAudioSource.Play();
            isDying = true; // Marca que o objeto está em processo de destruição
            GetComponent<Animator>().SetBool("isDead", true);
             
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log( $" Damage taken by enemy: {damage}");

        if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
    }
}
