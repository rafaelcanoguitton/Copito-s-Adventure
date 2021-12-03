using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccionar : MonoBehaviour
{
    //private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        //renderer=GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter(){
        Debug.Log("encimaobjeto");
    }
    private void OnMouseExit(){
        Debug.Log("dedjoobjeto");
        //renderer.material.color=Color.white;
    }
}
