using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private GameObject player; 
    public Image [] powerUps;

    public CanvasUpdate canvas;

    // Player PowerUps
    private int healthPowerUps = 0;
    private int strengthPowerUps = 0;
    private int speedPowerUps = 0;
    private int attackSpeedPowerUps = 0;
    private int criticalHitPowerUps = 0;

    public Image GenerateRandomPowerUp()
    {
        // Pesos dos PowerUps
        float[] pesos = { 0.4f, 0.2f, 0.2f, 0.1f, 0.1f}; // Certifique-se de que somam 1.0
        float random = UnityEngine.Random.Range(0f, 1.0f);
        float acumulador = 0;

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
                        // IncreaseHealth();
                        return powerUps[0];

                    case 1:
                        strengthPowerUps++;
                        // Adiciona força ao player
                        canvas.UpdatePowerUps(i, strengthPowerUps);
                        // IncreaseStrength();
                        return powerUps[2];

                    case 2:
                        
                        // Adiciona velocidade ao player
                        speedPowerUps++;
                        canvas.UpdatePowerUps(i, speedPowerUps);
                        // IncreaseSpeed();
                        return powerUps[1];

                    case 3:
                        // Aumenta a chance de Crítico
                        criticalHitPowerUps++;
                        canvas.UpdatePowerUps(i, criticalHitPowerUps);
                        // IncreaseCriticalChance();
                        return powerUps[4];

                    case 4:
                        // Aumenta o attack Speed
                        attackSpeedPowerUps++;
                        canvas.UpdatePowerUps(i, attackSpeedPowerUps);
                        // IncreaseAttackSpeed();
                        return powerUps[3];

                    default:
                        // Caso não esperado (falha de segurança)
                        Debug.Log("Returning null");
                        return null;
                }
            }
        }

        return null;
    }

    private void IncreaseAttackSpeed()
    {
        throw new NotImplementedException();
    }

    private void IncreaseCriticalChance()
    {
        throw new NotImplementedException();
    }

    private void IncreaseSpeed()
    {
        throw new NotImplementedException();
    }

    private void IncreaseStrength()
    {
        throw new NotImplementedException();
    }

    private void IncreaseHealth()
    {
        PlayerHealth healthScript = player.GetComponent<PlayerHealth>();

    }

    public int GetHealthPowerUps(){
        return healthPowerUps;
    }

    public int GetstrengthPowerUps(){
        return strengthPowerUps;
    }

    public int getSpeedPowerUps(){
        return speedPowerUps;
    }

    public int getAttackSpeedPowerUps(){
        return attackSpeedPowerUps;
    }

    public int getCriticalHitPowerUps(){
        return criticalHitPowerUps;
    }

    public void SetPlayer (GameObject playerInstance){
        player = playerInstance;
    }

    public void SetCanvas (CanvasUpdate canvasUpdate) {
        canvas = canvasUpdate;
    }
}
