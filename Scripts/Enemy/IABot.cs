using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IABot : MonoBehaviour
{
    public NavMeshAgent ai;

    public GameObject target;

    public GameObject enemyDeath;

    public float vidaEnemy = 100f;

    public int stateBot = 1;

    public bool atacando = false;
    public bool idle = true;
    public bool apuntando = false;
    public bool randPos = false;
    public bool siendoAtacado = false;

    public GameObject barraVida;

    public Renderer materialACambiar;

    public GameObject punteroArmaEnemy;
    public GameObject headEnemy;

    public GameObject balaEnemy;

    public GameObject humoDisparo;

    void Start()
    {
        ai = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        materialACambiar = barraVida.GetComponent<Renderer>();
    }

    private void ControlVida()
    {
        if (vidaEnemy > 0)
        {
            barraVida.transform.localScale = new Vector3(vidaEnemy / 20, 0.5f, 0.2f);
        }
        
        if (vidaEnemy <= 100)
        {
            materialACambiar.material.SetColor("_Color", Color.green);
        }
        
        if (vidaEnemy <= 66)
        {
            materialACambiar.material.SetColor("_Color", Color.yellow);
        }
        
        if (vidaEnemy <= 33)
        {
            materialACambiar.material.SetColor("_Color", Color.red);
        }
        
        if (vidaEnemy <= 0)
        {
            vidaEnemy = 0;
            Instantiate(enemyDeath, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        ControlVida();
        IAEnemigo();
    }

    void IAEnemigo()
    {
        if (target != null)
        {
            Vector3
                    distPropia = this.transform.position,
                    distTarget = target.transform.position;
            ;

            float distancia = Vector3.Distance(distPropia, distTarget);

            if(apuntando)
            {
                headEnemy.transform.LookAt(target.transform.position);
            }

            if (!siendoAtacado)
            {
                if (idle && randPos)
                {
                    randPos = false;
                    InvokeRepeating("RandomPos", 5f, 5f);
                }

                if ((distancia > 25 && !idle) || idle)
                {
                    idle = true;
                    atacando = false;
                    apuntando = false;
                    randPos = true;
                    CancelInvoke("Atacar");
                }

                if (distancia <= 25 && idle)
                {
                    idle = false;
                    atacando = false;
                    apuntando = true;
                    randPos = false;
                    ai.destination = target.transform.position;
                    CancelInvoke("RandomPos");
                    CancelInvoke("Atacar");
                }

                if (distancia <= 20 && !atacando)
                {
                    idle = false;
                    atacando = true;
                    apuntando = true;
                    randPos = false;
                    InvokeRepeating("Atacar", 1f, 1f);
                    ai.stoppingDistance = 10;
                    ai.destination = target.transform.position;
                    CancelInvoke("RandomPos");
                }
            }
            else
            {
                if (distancia > 20)
                {
                    idle = false;
                    atacando = false;
                    apuntando = true;
                    randPos = false;
                    ai.destination = target.transform.position;
                    CancelInvoke("RandomPos");
                }

                if (distancia <= 20 && !atacando)
                {
                    idle = false;
                    atacando = true;
                    apuntando = true;
                    randPos = false;
                    Invoke("AtacarSiendoAtacado", 1f);
                    ai.stoppingDistance = 10;
                    ai.destination = target.transform.position;
                }
            }
        }
        else
        {
            
        }
    }

    void Atacar()
    {
        if(atacando)
        {
            GameObject a;
            a = Instantiate(balaEnemy, punteroArmaEnemy.transform.position, headEnemy.transform.rotation);
            a.GetComponent<Rigidbody>().AddForce(headEnemy.transform.forward * 30, ForceMode.Impulse);
            ai.destination = target.transform.position;
            Instantiate(humoDisparo, punteroArmaEnemy.transform.position, transform.rotation);
        }
    }

    void AtacarSiendoAtacado()
    {
        if (atacando)
        {
            GameObject a;
            a = Instantiate(balaEnemy, punteroArmaEnemy.transform.position, headEnemy.transform.rotation);
            a.GetComponent<Rigidbody>().AddForce(headEnemy.transform.forward * 30, ForceMode.Impulse);
            ai.destination = target.transform.position;
            Instantiate(humoDisparo, punteroArmaEnemy.transform.position, transform.rotation);
            atacando = false;
        }
    }

    void RandomPos()
    {
        if (idle)
        {
            ai.stoppingDistance = 0;

            var pos = transform.position;
            var rPos = Random.Range(-1, 1);

            if (rPos >= 0)
            {
                var rangoX = Random.Range(2, 5);
                var rangoZ = Random.Range(2, 5);
                ai.destination = new Vector3(pos.x + rangoX, pos.y, pos.z + rangoZ);
            }
            else if (rPos < 0)
            {
                var rangoX = Random.Range(2, 5);
                var rangoZ = Random.Range(2, 5);
                ai.destination = new Vector3(pos.x - rangoX, pos.y, pos.z - rangoZ);
            }
        }
    }
}
