using System;
using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Spawner spawner;
    public float destroyCooldown;
    private String color;

    void Start(){
        color = gameObject.name.Split(' ')[0];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision Detected (Chest)");

         PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {   

             if (other.gameObject.tag.Equals("Player")){

                Key key = player.GetKeyByColor(color);
            
                if ( key != null && !gameObject.GetComponent<Animator>().GetBool("isOpen")){

                    Debug.Log($"Key {key.GetColor()}. NOT NULL");

                    player.UseKey(key);
                    gameObject.GetComponent<Animator>().SetBool("isOpen", true);
                    // spawner.GetComponent<Spawner>().decreaseNumberOfEntities();
                    StartCoroutine(DestroyCooldown());
                }
            }
            
        }

    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(destroyCooldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    public String GetColor (){
        return color;
    }

}
