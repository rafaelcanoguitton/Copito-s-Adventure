using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class NOSE : MonoBehaviour
{
    public Button yourButton;	
    private FlechasPantalla rend;

    public GameObject personaje;
    public char nombre;
    void Start () {
		Button btn = yourButton.GetComponent<Button>();
		
        btn.onClick.AddListener(TaskOnClick);
        rend = personaje.GetComponent<FlechasPantalla>();
	}	
    void TaskOnClick(){
		Debug.Log (nombre);
        rend.state_animacion.Value=nombre;
	}
}
