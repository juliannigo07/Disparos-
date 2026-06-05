using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField] float lifeTime = 5f;
    public System.Action OnProjectileEnd;
    public int jugadorQueDisparo;

    void Start()
    {
        Invoke("DestroyProjectileTime", lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Manzana") || collision.gameObject.CompareTag("Persona"))
        {
            CancelInvoke();
        }

        if (collision.gameObject.CompareTag("Manzana"))
        {
            GameManager.instance.HitApple(jugadorQueDisparo);
            GameManager.instance.SonidoManzana.Play();
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);     
            OnProjectileEnd?.Invoke();
            Destroy(gameObject);

        }
        else if (collision.gameObject.CompareTag("Persona"))
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            GameManager.instance.HitPerson(jugadorQueDisparo);
            GameManager.instance.SonidoPersona.Play();            
            OnProjectileEnd?.Invoke();
            Destroy(gameObject);
        }
    }

    void DestroyProjectileTime()
    {
        GameManager.instance.MissShot(jugadorQueDisparo);

        OnProjectileEnd?.Invoke();

        Destroy(gameObject);
    }
}
