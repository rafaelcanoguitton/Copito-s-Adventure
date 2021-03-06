using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Unity.Netcode;
public class Plataformas : NetworkBehaviour
{

    [SerializeField]
    float speed=0.5f;

    [SerializeField]
    Transform startPoint, endPoint;

    

    [SerializeField]
    float changeDirectionDelay=0.0f;


    private Transform destinationTarget, departTarget;

    private float startTime;

    private float journeyLength;

    bool isWaiting;

    #region  MOVER
    public NetworkVariable<bool> activador=new NetworkVariable<bool>();
    public float distancia_arreglada =0.02f;
    void OnMouseDown(){
        if(!NetworkManager.Singleton.IsServer){
            if(activador.Value){
                return;
            }
            ActivarPlataformaServerRpc(true);
        }
        //activador.Value=true;
    }   
    
    void OnMouseUp(){
        ActivarPlataformaServerRpc(false);
        //activador.Value=false;
    }

    public override void OnNetworkSpawn(){
        if(IsOwner){//host
            //Debug.Log("host");
        }
        else{//client
            //Debug.Log("client");
        }
    }
    
    #endregion

    void Start()
    {
        

        activador.Value=false;
        departTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }

    void FixedUpdate(){
        
        if(activador.Value){
            Move();
        } 
    }

    private void Move()
    {

        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                Debug.Log(Time.time - startTime);
                //float distCovered = (Time.time - startTime) * speed;
                float distCovered = (distancia_arreglada) * speed;
                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
                distancia_arreglada+=0.02f;
            }
            else
            {
                isWaiting = true;
                StartCoroutine(changeDelay());

            }
        }


    }

    void ChangeDestination()
    {

        if (departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }

    }
    IEnumerator changeDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        startTime = Time.time;
        distancia_arreglada = 0.0f;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    [ServerRpc(RequireOwnership =false)]//cliente->servidor
    void ActivarPlataformaServerRpc(bool estadoPlataforma){
        //Debug.Log("ENVIANDO");
        activador.Value=estadoPlataforma;
        ActivarPlataformaClientRpc(estadoPlataforma);
        
    }
    [ClientRpc]//servidor->cliente
    void ActivarPlataformaClientRpc(bool estadoPlataforma){
        activador.Value=estadoPlataforma;
    }

}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataformas : MonoBehaviour
{

    [SerializeField]
    float speed=0.5f;

    [SerializeField]
    Transform startPoint, endPoint;

    

    [SerializeField]
    float changeDirectionDelay=3.0f;


    private Transform destinationTarget, departTarget;

    private float startTime;

    private float journeyLength;

    bool isWaiting;


    void Start()
    {
        departTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * speed;

                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(changeDelay());
            }
        }


    }

    void ChangeDestination()
    {

        if (departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }

    }
    IEnumerator changeDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
*/