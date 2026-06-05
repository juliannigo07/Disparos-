using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Camera cam;
    public Transform jugadorActual;
    public float zoomCerca = 5f;
    public float zoomLejos = 10f;
    public float velocidadZoom = 2f;
    public Vector3 offset;

    private Transform objetivoActual;

    void LateUpdate()
    {
        if (objetivoActual != null)
        {
            Vector3 pos = objetivoActual.position + offset;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5f);
        }
    }

    public void EnfocarJugador(Transform jugador)
    {
        jugadorActual = jugador;
        objetivoActual = jugadorActual;

        StopAllCoroutines();
        StartCoroutine(Zoom(zoomCerca));
    }

    public void SeguirProyectil(Transform proyectil)
    {
        if (proyectil == null) return;

        objetivoActual = proyectil;

        StopAllCoroutines();
        StartCoroutine(Zoom(zoomLejos));
    }

    IEnumerator Zoom(float objetivoZoom)
    {
        while (Mathf.Abs(cam.orthographicSize - objetivoZoom) > 0.1f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, objetivoZoom, Time.deltaTime * velocidadZoom);
            yield return null;
        }
    }

}