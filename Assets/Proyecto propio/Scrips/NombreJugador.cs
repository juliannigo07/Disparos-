using UnityEngine;
using TMPro;

public class NombreJugador : MonoBehaviour
{
    public TMP_Text textoNombre;
    public string nombreJugador;

    void Start()
    {
        if (!string.IsNullOrEmpty(nombreJugador))
        {
            textoNombre.text = $"Jugador \"{nombreJugador}\"";
        }
        else
        {
            textoNombre.text = "Jugador";
        }
    }
}