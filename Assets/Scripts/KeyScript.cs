using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class Key : MonoBehaviour
{
    private String color;

    void Start(){
        color = gameObject.tag.Split(' ')[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && !player.GetIsInventoryFull())
        {   
            player.AddKeyToInventory(this);
            Destroy(gameObject);
            
        }



    }

    public String GetColor (){
        return color;
    }
}
