using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuInicio : MonoBehaviour
{
    public TMP_InputField inputJugador1;
    public TMP_InputField inputJugador2;

    public void EmpezarJuego()
    {
        PlayerPrefs.SetString("Jugador1", inputJugador1.text);
        PlayerPrefs.SetString("Jugador2", inputJugador2.text);

        SceneManager.LoadScene("Juego");
    }
}