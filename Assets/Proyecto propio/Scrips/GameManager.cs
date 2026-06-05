using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int scoreJugador1 = 0;
    public int scoreJugador2 = 0;

    public int shotsLeft1 = 3;
    public int shotsLeft2 = 3;

    public bool gameOver = false;

    public TMP_Text shotsText;
    public TMP_Text resultText;

    public AudioSource Ambiente;
    public AudioSource SonidoPersona;
    public AudioSource SonidoManzana;

    public int turno = 1; 
    public bool puedeDisparar = false;

    public CameraShake camara;
    public Transform jugador1;
    public Transform jugador2;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        resultText.text = "";
        UpdateUI(1); 
        IniciarTurno();
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void IniciarTurno()
    {
        puedeDisparar = true;

        if (turno == 1)
            camara.EnfocarJugador(jugador1);
        else
            camara.EnfocarJugador(jugador2);
    }

    public void TerminarTurno()
    {
        puedeDisparar = false;

        turno = (turno == 1) ? 2 : 1;

        Invoke(nameof(IniciarTurno), 2f);
    }

    public void UseShot(int player)
    {
        if (player == 1)
            shotsLeft1--;
        else
            shotsLeft2--;

        UpdateUI(player);
        CheckLoseAfterShot(player);
    }

    public void CheckLoseAfterShot(int player)
    {
        if (gameOver) return;

        int shots = (player == 1) ? shotsLeft1 : shotsLeft2;

        if (shots <= 0)
        {
            Lose(player);
        }
    }

    public void Lose(int jugadorQuePerdio)
    {
        if (gameOver) return;

        gameOver = true;

        int ganador = (jugadorQuePerdio == 1) ? 2 : 1;

        resultText.text = "JUGADOR " + ganador + " GANA";
        resultText.color = Color.red;

        Time.timeScale = 0f;
    }

    public void HitApple(int jugadorQueDisparo)
    {
        if (gameOver) return;

        gameOver = true;

        resultText.text = "JUGADOR " + jugadorQueDisparo + " GANA";
        resultText.color = Color.green;

        Time.timeScale = 0f;
    }

    public void HitPerson(int jugadorQueDisparo)
    {
        if (gameOver) return;

        gameOver = true;

        int ganador = (jugadorQueDisparo == 1) ? 2 : 1;

        resultText.text = "JUGADOR " + ganador + " GANA";
        resultText.color = Color.red;

        Time.timeScale = 0f;
    }

    public void MissShot(int player)
    {
        if (gameOver) return;

        int shots = (player == 1) ? shotsLeft1 : shotsLeft2;

        if (shots <= 0)
        {
            Lose(player);
        }
    }

    void UpdateUI(int player)
    {
        int shots = (player == 1) ? shotsLeft1 : shotsLeft2;

        shotsText.text = "Jugador " + player + " - Balas: " + shots;

        if (shots == 1)
            shotsText.color = Color.red;
        else if (shots == 2)
            shotsText.color = Color.yellow;
        else
            shotsText.color = Color.white;
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}