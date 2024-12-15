using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private GameObject player; 
    public Image [] powerUps;
    private CanvasUpdate canvas;
    public AudioSource getPowerUp;

    // Player PowerUps
    private int healthPowerUps = 0;
    private int strengthPowerUps = 0;
    private int speedPowerUps = 0;
    private int knockBackPowerUps = 0;
    private int criticalHitPowerUps = 0;

    public Image GenerateRandomPowerUp()
    {
        // Pesos dos PowerUps
        float[] pesos = { 0.4f, 0.1f, 0.2f, 0.1f, 0.2f}; // Certifique-se de que somam 1.0
        float random = UnityEngine.Random.Range(0f, 1.0f);
        float acumulador = 0;
        getPowerUp.Play();

        for (int i = 0; i < pesos.Length; i++)
        {
            acumulador += pesos[i];
            if (random < acumulador)
            {
                switch (i)
                {
                    case 0:
                        healthPowerUps++;
                        // Restaura vida e aumenta o tamanho
                        canvas.UpdatePowerUps(i, healthPowerUps);
                        IncreaseHealth();
                        return powerUps[0];

                    case 1:
                        strengthPowerUps++;
                        // Adiciona força ao player
                        canvas.UpdatePowerUps(i, strengthPowerUps);
                        IncreaseStrength();
                        return powerUps[2];

                    case 2:
                        
                        // Aumenta o KnockBack
                        knockBackPowerUps++;
                        canvas.UpdatePowerUps(i, knockBackPowerUps);
                        IncreaseKnockBack();
                        return powerUps[3];

                    case 3:
                        // Aumenta a chance de Crítico
                        criticalHitPowerUps++;
                        canvas.UpdatePowerUps(i, criticalHitPowerUps);
                        IncreaseCriticalChance();
                        return powerUps[4];

                    case 4:

                        // Adiciona velocidade ao player
                        speedPowerUps++;
                        canvas.UpdatePowerUps(i, speedPowerUps);
                        IncreaseSpeed();
                        return powerUps[1];
                        

                    default:
                        // Caso não esperado (falha de segurança)
                        Debug.Log("Returning null");
                        return null;
                }
            }
        }

        return null;
    }

    private void IncreaseKnockBack()
    {
        player.GetComponent<PlayerController>().addKnockBack(2);
    }

    private void IncreaseCriticalChance()
    {
        player.GetComponent<PlayerController>().addCriticalHitChance(0.05f);
    }

    private void IncreaseSpeed()
    {
        player.GetComponent<PlayerController>().addSpeed(0.5f);
    }

    private void IncreaseStrength()
    {
        player.GetComponent<PlayerController>().addStrength(5);
    }

    private void IncreaseHealth()
    {
        player.GetComponent<PlayerHealth>().addHealth(30);

    }

    public void SetPlayer (GameObject playerInstance){
        player = playerInstance;
    }

    public void SetCanvas (CanvasUpdate canvasUpdate) {
        canvas = canvasUpdate;
    }
}
