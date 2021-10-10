using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
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
            default:
                desiredMenuPosition=Vector3.zero;
                break;
        }
    }
    public void OnLevelSelect(int currentIndex){
        switch (currentIndex)
        {
            
            default:
                SceneManager.LoadScene("Novaborn");
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
}
