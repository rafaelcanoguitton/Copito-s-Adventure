using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoHorizontal : MonoBehaviour
{
    enum Directions{ x, y, z }
    [SerializeField]
    private Directions eje;
    [SerializeField]
    private GameObject Jugador;
    private Vector3[] vectorDirecciones=new Vector3[2];
    private int direccion = 1;
    public float speed = 0.1f;
    Rigidbody rigid;
    void Start()
    {

        rigid = GetComponent<Rigidbody>();

        if (eje == Directions.z) {
            vectorDirecciones[0] = new Vector3(0, 0, speed);
            vectorDirecciones[1] = vectorDirecciones[0] * -1;
        }
        if (eje == Directions.x)
        {
            vectorDirecciones[0] = new Vector3(-speed, 0, 0);
            vectorDirecciones[1] = vectorDirecciones[0] * -1;
        }
        if (eje == Directions.y)
        {
            vectorDirecciones[0] = new Vector3(0, -speed, 0);
            vectorDirecciones[1] = vectorDirecciones[0] * -1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(vectorDirecciones[direccion] * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Jugador) {
            Jugador.transform.parent = transform;
        }
    }
    private void OntriggerExit(Collider other)
    {
        if (other.gameObject == Jugador)
        {
            Jugador.transform.parent = null;
        }
    }

    
}
