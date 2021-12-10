using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class botonOscurecer : NetworkBehaviour
{

    private Renderer rend;
    private Renderer rend2;
    public GameObject pantalla;

    public GameObject canvas;

    public GameObject flechas;
    public bool AccionOscurecer;
        // Start is called before the first frame update
    void Start()
    {
        rend=pantalla.GetComponent<Renderer>();
        rend2=flechas.GetComponent<Renderer>();
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(NetworkManager.Singleton.IsServer){
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("JUGADOR BOTON");
                rend.enabled=AccionOscurecer;
                rend2.enabled=AccionOscurecer;
            }
        }
        else{
            if (other.gameObject.tag == "Player")
                canvas.SetActive(AccionOscurecer);
        }
    }
}
