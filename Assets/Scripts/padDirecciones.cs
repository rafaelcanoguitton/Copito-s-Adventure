using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class padDirecciones : MonoBehaviour
{

    private FlechasPantalla rend;
    string direccion;
    string name;
    // Start is called before the first frame update
    void Start(){
        
    }
    public void setActive(GameObject xd){
        rend=xd.GetComponent<FlechasPantalla>();
    }
    
    public void nose(GameObject xd, string name){

    }
    void Update()
    {
        Debug.Log(name);
        switch(direccion){
            case "U":
            rend.state_animacion="U";
            break;
            case "D":
            rend.state_animacion="D";
            break;
            case "L":
            rend.state_animacion="L";
            break;
            case "R":
            rend.state_animacion="R";
            break;
        }
        //ActivarPlataformaServerRpc(rend.state_animacion);
    }
    /*
    [ServerRpc(RequireOwnership =false)]//cliente->servidor
    void ActivarPlataformaServerRpc(char estado){
        //Debug.Log("ENVIANDO");
        rend.state_animacion=estado;
        
    }
    [ClientRpc]//servidor->cliente
    void ActivarPlataformaClientRpc(char estado){
        rend.state_animacion=estado;
    }
*/
}
