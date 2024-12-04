using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth;

    private int maxHealth;

    private bool isDying; // Controle para evitar múltiplas chamadas

    public AudioSource hitAudioSource;

    public AudioSource deathAudioSource;

    public static float volume = 1.0f;
    

    void Start(){
         
        maxHealth = GetComponent<PlayerController>().health;
        currentHealth = maxHealth;
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
        if (!isDying) {

            Debug.Log($"Health = {currentHealth}, Damage = {damage}");
            currentHealth -= damage;

            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

            if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
        }
        

    }

    public void RestoreHealth(int restore)
    {
        if (!isDying) {
            
            currentHealth += restore;

            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

            if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
        }
    }
}
