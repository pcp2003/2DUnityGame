using UnityEngine;
using System.Collections;
using System.Data.Common;

public class EnemiesHealth : MonoBehaviour
{
    private int currentHealth;
    private bool isDying; // Controle para evitar múltiplas chamadas

    public AudioSource hitAudioSource;

    public AudioSource deathAudioSource;

    public static float volume = 1.0f;

    void Start(){
        
        if (name.Equals("Goblin(Clone)")) currentHealth = GetComponent<Goblin>().health;
        if (name.Equals("Soldier(Clone)")) currentHealth = GetComponent<Soldier>().health;

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
            Debug.Log("Iniciando cooldown para destruir " + gameObject.name);
            GetComponent<Animator>().SetBool("isDead", true);
             
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
    }
}
