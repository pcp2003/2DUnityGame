using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour{

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
