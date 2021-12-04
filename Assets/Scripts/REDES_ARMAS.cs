using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class REDES_ARMAS : NetworkBehaviour
{
    public NetworkVariable<bool> vivo=new NetworkVariable<bool>();
    //public bool vivo=true;
    public GameObject personaje=null;
    private Player script_personaje;
    public int value;
    public override void OnNetworkSpawn(){
        if(IsOwner){
            Debug.Log("host");
            if(personaje)
                script_personaje = personaje.GetComponent<Player>(); 
        }
        else{
            Debug.Log("client");
        }
    }
    void OnMouseDown(){
        if(!IsOwner)//si es cliente
            shootServerRpc();
    }
    void Start(){
           
        vivo.Value=true;
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);  
        if(!vivo.Value){
            script_personaje.hasWeapon[value] = true;
            Destroy(this.gameObject);
        }
    }
    
    [ServerRpc(RequireOwnership =false)]//cliente->servidor
    void shootServerRpc(){
        //Debug.Log("ENVIANDO");
        vivo.Value=false;
        selec_armaClientRpc();
        
    }
    [ClientRpc]//servidor->cliente
    void selec_armaClientRpc(){
        vivo.Value=false;
    }
    
}
