using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour{

    private int hordas;
    private int kills;
    private int powerUps;
    public TextMeshProUGUI HordasText;
    public TextMeshProUGUI EliminacoesText;
    public TextMeshProUGUI PowerUpsText;
    private CanvasUpdate Canvas;
    

    public void Setup() {
        gameObject.SetActive(true);
        HordasText.text = "Hordas sobrevividas: " + Canvas.hordeCounter.text.Split(" ")[1];
        EliminacoesText.text = "Eliminações: " + Canvas.killCounterText.text;
        PowerUpsText.text = "Powerups Conseguidos: " + getAllPowerUps();
    }

    public void ClickPlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClickMainMenu() {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void SetCanvas(CanvasUpdate canvas) {
        this.Canvas = canvas;
    }

    private String getAllPowerUps() {
        int allPowerUps = Int32.Parse(Canvas.attackSpeedPowerUps.text) + Int32.Parse(Canvas.criticalHitPowerUps.text) + Int32.Parse(Canvas.healthPowerUps.text) + Int32.Parse(Canvas.speedPowerUps.text) + Int32.Parse(Canvas.strengthPowerUps.text);
        return allPowerUps.ToString();
    }
}
