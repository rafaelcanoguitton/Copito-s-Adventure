using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class FlechasPantalla : NetworkBehaviour
{
    public Material Up;
    public Material Left;
    public Material Right;
    public Material Down;

    public Material Default;

    public NetworkVariable<char> state_animacion;
    //public string state_animacion;
    public MeshRenderer my_renderer;

    // Start is called before the first frame update
    void Start()
    {
        my_renderer= GetComponent<MeshRenderer>();
        state_animacion.Value='N';
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_animacion.Value){
            case 'U':
            my_renderer.material = Up;
            break;
            case 'L':
            my_renderer.material = Left;
            break;
            case 'R':
            my_renderer.material = Right;
            break;
            case 'D':
            my_renderer.material = Down;
            break;
        }
        
    }
    [ServerRpc(RequireOwnership =false)]//cliente->servidor
    void ActivarPlataformaServerRpc(char f){
        state_animacion.Value=f;
        
    }
    [ClientRpc]//servidor->cliente
    void ActivarPlataformaClientRpc(char f){
        state_animacion.Value=f;
    }

    /*
    void Start()
    {
        my_renderer= GetComponent<MeshRenderer>();
        my_renderer.enabled=true;
        state_animacion.Value='N';
    }

    // Update is called once per frame
    void Update()
    {
        switch(state_animacion.Value){
            case 'U':
            my_renderer.material = Up;
            break;
            case 'L':
            my_renderer.material = Left;
            break;
            case 'R':
            my_renderer.material = Right;
            break;
            case 'D':
            my_renderer.material = Down;
            break;
            Default:
            my_renderer.material = Default;
            break;
        }
        
    }
    */
}
