using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaScript : MonoBehaviour
{
    public void OnContinueClick(){
        Debug.Log("Close menu");
    }
    public void OnOptionsClick(){
        Debug.Log("Opciones");
    }
    public void OnSalirClick(){
        SceneManager.LoadScene("Menu Principal");
    }
    
}
