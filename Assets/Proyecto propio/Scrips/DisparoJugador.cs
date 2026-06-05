using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject puntoPrefab;
    public Transform shootPoint;
    public AudioSource SonidoDisparo;

    public float maxDistance = 5f;
    public int numeroPuntos = 15;
    public float tiempoEntrePuntos = 0.1f;

    public bool puedeDisparar = true;

    private List<GameObject> puntos = new List<GameObject>();
    public CameraShake cameraController;

    [Header("Disparo")]
    public float fuerzaHorizontal = 10f;
    public float fuerzaCurva = 8f; 

    [Header("Trayectoria")]
    public float gravedad = -9.81f;

    public int idJugador;

    void Start()
    {
        for (int i = 0; i < numeroPuntos; i++)
        {
            GameObject p = Instantiate(puntoPrefab);
            puntos.Add(p);
        }

        GameManager.instance.IniciarTurno();
    }

    void Update()
    {
        if (GameManager.instance.turno != idJugador) return;
        if (!GameManager.instance.puedeDisparar) return;
        if (GameManager.instance.gameOver) return;

        if (idJugador == 1 && GameManager.instance.shotsLeft1 <= 0) return;
        if (idJugador == 2 && GameManager.instance.shotsLeft2 <= 0) return;

        if (Input.GetMouseButtonDown(0) && puedeDisparar)
        {
            Shoot();
            GameManager.instance.UseShot(idJugador);
        }

        DibujarTrayectoria();
    }

    Vector3 CalcularVelocidadInicial()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        Vector3 dir = (worldPos - shootPoint.position).normalized;

        float distancia = Vector3.Distance(worldPos, shootPoint.position);
        float distanciaNormalizada = Mathf.Clamp01(distancia / maxDistance);

        float fuerza = distanciaNormalizada * fuerzaHorizontal;

        Vector3 velocidad = dir * fuerza;

        float lateral = Mathf.Abs(dir.x);
        float curva = fuerzaCurva * (1f - distanciaNormalizada) * (1f - lateral);

        velocidad.y += curva;

        return velocidad;
    }

    void Shoot()
    {
        Vector3 velocidad = CalcularVelocidadInicial();

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = velocidad; 

        SonidoDisparo.Play();

        puedeDisparar = false;
        MostrarPuntos(false);
        Invoke(nameof(ActivarPuntos), 0.5f);

        cameraController.SeguirProyectil(projectile.transform);

        Proyectil proj = projectile.GetComponent<Proyectil>();
        proj.OnProjectileEnd += FinTurno;
        proj.jugadorQueDisparo = idJugador;
    }

    void DibujarTrayectoria()
    {
        if (!puntos[0].activeSelf) return;

        Vector3 velocidad = CalcularVelocidadInicial();
        Vector3 gravedadVec = new Vector3(0, gravedad, 0);

        for (int i = 0; i < puntos.Count; i++)
        {
            float t = i * tiempoEntrePuntos;

            Vector3 pos = shootPoint.position +
                          velocidad * t +
                          0.5f * gravedadVec * t * t;

            pos.z = 0f;
            puntos[i].transform.position = pos;
        }
    }

    void FinTurno()
    {
        GameManager.instance.TerminarTurno();
        puedeDisparar = true;
        MostrarPuntos(true);
    }

    void ActivarPuntos()
    {
        MostrarPuntos(true);
    }

    void MostrarPuntos(bool estado)
    {
        foreach (var p in puntos)
            p.SetActive(estado);
    }
}