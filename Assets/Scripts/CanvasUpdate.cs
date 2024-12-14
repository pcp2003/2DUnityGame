using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{

    // Referência ao Text (TMP)
    public TextMeshProUGUI killCounterText;
    public TextMeshProUGUI healthPowerUps;
    public TextMeshProUGUI strengthPowerUps;
    public TextMeshProUGUI speedPowerUps;
    public TextMeshProUGUI attackSpeedPowerUps;
    public TextMeshProUGUI criticalHitPowerUps;
    public Text hordeCounter;
    public Image Slot1;
    public Image Slot2;
    public Image Slot3;
    public Image PowerUpImage;
    public GameObject MessagePanel;
    public GameObject HordePanel;


    private Coroutine hidePanelCoroutine; // Referência à corrotina em execução

    public void UpdatePowerUps(int whichPowerUp, int powerUpValue)
    {  
        switch (whichPowerUp)
        {
            case 0:

                healthPowerUps.text = "" + powerUpValue;
                return;

            case 1:

                strengthPowerUps.text = "" + powerUpValue;
                return;
            case 2:

                speedPowerUps.text = "" + powerUpValue;
                return;
            case 3:

                criticalHitPowerUps.text = "" + powerUpValue;
                return;
            case 4:

                attackSpeedPowerUps.text = "" + powerUpValue;
                return;
            default:
                Debug.LogError("Error updating powerUps.");
                return;
        }

    }

    public void UpdateHordePanel(int horda ){

        HordePanel.SetActive(true);

        hordeCounter.text = $"Horda: {horda}";

    }

    public void ShowMessage(Image powerUpImage)
    {
        PowerUpImage.sprite = powerUpImage.sprite;
        MessagePanel.SetActive(true); // Exibe o painel

        // Reseta o timer se já houver uma corrotina rodando
        if (hidePanelCoroutine != null)
        {
            StopCoroutine(hidePanelCoroutine);
        }

        // Inicia uma nova corrotina
        hidePanelCoroutine = StartCoroutine(HidePanelAfterDelay(3f));
    }

    private System.Collections.IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Aguarda pelo tempo especificado
        MessagePanel.SetActive(false); // Desativa o painel
        hidePanelCoroutine = null; // Limpa a referência, já que a corrotina terminou
    }

    public void UpdateKillCounter(int kills)
    {
        killCounterText.text = "" + kills;
    }

    public void UpdateSlotBars(int keysCount, List<Key> keys )
    {
        Image[] slots = { Slot1, Slot2, Slot3 };

        for (int i = 0; i < slots.Length; i++)
        {
            // Verifica se o índice `i` existe na lista de chaves
            if (i < keysCount && keys != null)
            {
                slots[i].sprite = keys[i].GetSprite();
                slots[i].enabled = true; // Ativa o componente para exibir o sprite
            }
            else
            {
                slots[i].enabled = false; // Desativa o componente
            }
        }
    }


}
