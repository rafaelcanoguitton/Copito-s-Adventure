using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.UI;


public class MenuScene : MonoBehaviour
{
    public Text _WaitingText;
    // Start is called before the first frame update
    private CanvasGroup fadeGroup;
    private float fadeInSpeed=0.33f;
    public RectTransform menuContainer;
    public Transform levelPanel;
    private Vector3 desiredMenuPosition;
    private void Start(){
        fadeGroup=FindObjectOfType<CanvasGroup>();
        fadeGroup.alpha=1;
        InitLevel();
    }
    public void InitLevel(){
        int i=0;
        foreach(Transform t in levelPanel){
            Debug.Log("Tres veces.");
            Button b=t.GetComponent<Button>();
            b.onClick.AddListener(()=>OnLevelSelect(i));
            i++;
        }
    }
    public void NavigateTo(int menuIndex){
        switch (menuIndex)
        {
            case 1:
                desiredMenuPosition=Vector3.left*1920;
                break;
            case 2:
                desiredMenuPosition=Vector3.right*1920;
                break;
            case 3:
                desiredMenuPosition=Vector3.right*1920*2;
                _WaitingText.text= "Esperando a cliente...";
                //Debug.Log(NetworkManager.Singleton.IsServer);
                //Debug.Log(NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
                //NetworkManager.Singleton.StartHost();
                break;
            case 4:
                desiredMenuPosition=Vector3.right*1920*2;
                _WaitingText.text="Esperando a host...";
                //Temporarily using a fixed ip
                //NetworkManager.Singleton.StartClient();
                //NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
                break;
            default:
                desiredMenuPosition=Vector3.zero;
                break;
        }
    }
    public void OnLevelSelect(int currentIndex){
        Debug.Log(currentIndex);
        switch (currentIndex)
        {
            default:
                SceneManager.LoadScene("Lab nivel 1 - Completo");
                break;
        }
        
    }
    private void Update(){
        //fade in
        fadeGroup.alpha=1-Time.timeSinceLevelLoad*fadeInSpeed;
        //Menu navigation (smooth)
        menuContainer.anchoredPosition3D=Vector3.Lerp(menuContainer.anchoredPosition3D,desiredMenuPosition,0.1f);
    }
    //Button-section
    public void onPlayClick(){
        NavigateTo(1);
    }
    public void onExitClick(){
        Application.Quit();
    }
    public void onBack(){
        NavigateTo(0);
    }
    public void onMultiplayerClick(){
        NavigateTo(2);
    }
    public void onMultiplayerHost(){
        NavigateTo(3);
    }
    public void onMultiplayerClient(){
        NavigateTo(4);
    }
}
