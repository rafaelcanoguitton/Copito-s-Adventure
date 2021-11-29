using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Item : MonoBehaviour
{
    public enum Type { Coin, Heart, Weapon, Key };
    public Type type;
    public int value;
    //-----------------------
    public GameObject personaje=null;
    private Player script_personaje;
    //-----------------------
    void Start()
    {
        if(personaje)
            script_personaje = personaje.GetComponent<Player>();
    }
    void OnMouseDown(){
        if(type==Type.Weapon){
            Debug.Log("xd");
            script_personaje.hasWeapon[value] = true;
            Destroy(this.gameObject);
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
