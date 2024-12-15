using System;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private String color;
    private CanvasUpdate canvas;
    private PowerUpManager powerUpManager;

    void Start(){

        color = gameObject.name.Split(' ')[0];

    }

    void OnCollisionEnter2D(Collision2D other)
    {

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {   

             if (other.gameObject.tag.Equals("Player")){

                Key key = player.GetKeyByColor(color);
            
                if ( key != null && !gameObject.GetComponent<Animator>().GetBool("isOpen")){

                    OpenChest(key, player);

                }
            }
            
        }

    }

    public void OpenChest (Key key, PlayerController player ) {
        
        player.UseKey(key);
        gameObject.GetComponent<Animator>().SetBool("isOpen", true);
        Image PowerUp = powerUpManager.GenerateRandomPowerUp();
        canvas.ShowMessage(PowerUp);
    }

    public void setCanvas (CanvasUpdate canvasUpdate) {
        canvas = canvasUpdate;
    }

    public void SetPowerUpManager (PowerUpManager powerUpManagerScript){
        powerUpManager = powerUpManagerScript;
    }

    public String GetColor (){
        return color;
    }

}
