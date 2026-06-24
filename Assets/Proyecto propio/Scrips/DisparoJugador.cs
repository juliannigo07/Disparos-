using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject puntoPrefab;
    public Transform shootPoint;
    public AudioSource SonidoDisparo;

    public int numeroPuntos = 15;
    public float tiempoEntrePuntos = 0.1f;



    public bool puedeDisparar = true;

    private List<GameObject> puntos = new List<GameObject>();
    public CameraShake cameraController;


    [Header("Trayectoria")]
    public float minFuerza = 6f;
    public float maxFuerza = 20f;
    public float fuerzaCurva = 8f;
    public float maxDistance = 8f;
    public float gravedadPersonalizada = -9.8f;
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
        bool esMiTurno = GameManager.instance.turno == idJugador;

        MostrarPuntos(esMiTurno && puedeDisparar);
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
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        Vector2 origen = shootPoint.position;
        Vector2 destino = new Vector2(world.x, world.y);

        Vector2 desplazamiento = destino - origen;

        float distancia = desplazamiento.magnitude;

        Vector2 dir = desplazamiento.normalized;

        float distanciaNormalizada = Mathf.Clamp01(distancia / maxDistance);
        float fuerza = Mathf.Lerp(minFuerza, maxFuerza, distanciaNormalizada);

        Vector2 velocidad = dir * fuerza;

        float gravedad = Mathf.Abs(gravedadPersonalizada);

        float curva = (1f - distanciaNormalizada) * fuerzaCurva;

        velocidad.y += curva;

        if (distanciaNormalizada > 0.9f)
        {
            curva = 0f;
        }

        return new Vector3(velocidad.x, velocidad.y, 0f);
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
        //Invoke(nameof(ActivarPuntos), 0.5f);

        cameraController.SeguirProyectil(projectile.transform);

        Proyectil proj = projectile.GetComponent<Proyectil>();
        proj.OnProjectileEnd += FinTurno;
        proj.jugadorQueDisparo = idJugador;
    }

    void DibujarTrayectoria()
    {
        if (!puntos[0].activeSelf) return;
        Vector3 velocidadInicial = CalcularVelocidadInicial();

        Vector3 gravedad = new Vector3(0, gravedadPersonalizada, 0);

        for (int i = 0; i < puntos.Count; i++)
        {
            float t = i * tiempoEntrePuntos;

            Vector3 pos = shootPoint.position +
                          velocidadInicial * t +
                          0.5f * gravedad * t * t;

            pos.z = 0f;
            puntos[i].transform.position = pos;
        }
    }

    void FinTurno()
    {
        GameManager.instance.TerminarTurno();
        puedeDisparar = true;
        //MostrarPuntos(true);
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