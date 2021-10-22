using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PausaMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject todoElCanvas;
    void Start()
    {
        setdeActive();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void setActive(){
        todoElCanvas.SetActive(true);
    }
    public void setdeActive(){
        todoElCanvas.SetActive(false);
    }
    public void goToMenu(){
        SceneManager.LoadScene("Menu Principal");
    }
}