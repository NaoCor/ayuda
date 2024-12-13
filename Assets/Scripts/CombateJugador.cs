using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CombateJugador : MonoBehaviour
{
    [SerializeField] private int vida;
    public event EventHandler MuerteJugador;   
    public void TomarDaño(int cantidadDaño)
    {
        vida -= cantidadDaño;
        if (vida <= 0)
        {
            MuerteJugador?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        } 
    }
}