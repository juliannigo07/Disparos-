using UnityEngine;
using TMPro;

public class NombreJugador : MonoBehaviour
{
    public TMP_Text textoNombre;
    public string clavePlayerPrefs; 

    void Start()
    {
        string nombre = PlayerPrefs.GetString(clavePlayerPrefs, "Jugador");
        textoNombre.text = nombre;
    }
}