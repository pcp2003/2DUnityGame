using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    private PlayerController player;

    // Referência ao Text (TMP)
    public TextMeshProUGUI killCounterText;
    public Image Slot1;
    public Image Slot2;
    public Image Slot3;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            if (player.getKills() != 0) UpdateKillCounter();

            UpdateSlotBars();

        }


    }

    public void SetPlayer(GameObject playerReference)
    {
        player = playerReference.GetComponent<PlayerController>();
        Debug.Log($"Canvas player: {player}");
    }

    public void UpdateKillCounter()
    {
        killCounterText.text = "" + player.getKills();
    }

    public void UpdateSlotBars()
    {
        Image[] slots = { Slot1, Slot2, Slot3 };

        for (int i = 0; i < slots.Length; i++)
        {
            // Verifica se o índice `i` existe na lista de chaves
            if (i < player.keys.Count && player.keys[i] != null)
            {
                Debug.Log($"Slot {i}: Chave encontrada.");
                slots[i].sprite = player.keys[i].GetSprite();
                slots[i].enabled = true; // Ativa o componente para exibir o sprite
            }
            else
            {
                slots[i].enabled = false; // Desativa o componente
            }
        }
    }



}
