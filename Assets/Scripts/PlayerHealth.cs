using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    private float fullHealth;
    private bool isDying; // Controle para evitar múltiplas chamadas

    public AudioSource hitAudioSource;

    public AudioSource deathAudioSource;

    public static float volume = 1.0f;

    public bool getIsDying () {
        return isDying;
    }
    

    void Start(){
         
        maxHealth = GetComponent<PlayerController>().health;
        currentHealth = maxHealth;
        fullHealth = maxHealth;
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

    public void TakeDamage(float damage)
    {
        if (!isDying) {

            Debug.Log($"Health = {currentHealth}, Damage = {damage}");
            currentHealth -= damage;

            if (currentHealth != 0){
                UIHandler.instance.SetHealthValue(currentHealth / maxHealth);
            }
            if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
        }
        

    }

    public void addHealth (float healthToAdd)
    {
        if (!isDying) {
            fullHealth = fullHealth + healthToAdd;
            currentHealth = fullHealth;
            UIHandler.instance.SetHealthValue(currentHealth / maxHealth);
            Debug.Log($"Health = {currentHealth}");
        }
    }
}
