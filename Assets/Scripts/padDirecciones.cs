using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class padDirecciones : MonoBehaviour
{

    private FlechasPantalla rend;
    string direccion;
    string name;
    void Update()
    {
        
        switch(direccion){
            case "U":
            rend.state_animacion.Value='U';
            break;
            case "D":
            rend.state_animacion.Value='D';
            break;
            case "L":
            rend.state_animacion.Value='L';
            break;
            case "R":
            rend.state_animacion.Value='R';
            break;
        }
    }

}
