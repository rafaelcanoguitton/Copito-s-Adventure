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
    public NetworkVariable<bool> vivo=new NetworkVariable<bool>();
    void Start()
    {
        if(personaje)
            script_personaje = personaje.GetComponent<Player>();
    }
    void OnMouseDown(){
        
        if (!NetworkManager.Singleton.IsServer){//client
            Debug.Log("cf");
            if(type==Type.Weapon){
                script_personaje.hasWeapon[value] = true;
                vivo.Value=false;
                //Destroy(this.gameObject);
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        if(vivo.Value==false)
            Destroy(this.gameObject);
    }
}
