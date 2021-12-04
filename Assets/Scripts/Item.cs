using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

public class Item : MonoBehaviour
{
    public enum Type { Coin, Heart, Weapon, Key };
    public Type type;
    public int value;
    //-----------------------------------------------------
    void Start(){
    
    }
    //-----------------------------------------------------
    void Update(){
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);            
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

public class Item : NetworkBehaviour
{
    public enum Type { Coin, Heart, Weapon, Key };
    public Type type;
    public int value;
    //-----------------------
    public GameObject personaje=null;
    private Player script_personaje;
    //-----------------------
    public NetworkVariable<bool> vivo=new NetworkVariable<bool>(true);
    public override void OnNetworkSpawn(){
        if(IsOwner){
            Debug.Log("host");
        }
        else{
            Debug.Log("client");
        }
    }
    void Start()
    {
        if(personaje)
                script_personaje = personaje.GetComponent<Player>();        
    }
    void OnMouseDown(){
        if(type==Type.Weapon){
            script_personaje.hasWeapon[value] = true;
            //vivo.Value=false;
            SubmitPositionRequestServerRpc();
                
        }
    }
    void OnMouseUp(){

    }

    // Update is called once per frame
    void Update(){
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        if(!vivo.Value){
            Destroy(this.gameObject);
        }
    }
    [ClientRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default){
        vivo.Value = false;
    }
}

*/