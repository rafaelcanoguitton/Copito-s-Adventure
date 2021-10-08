using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using static SpeechRecognizerPlugin;

public class ControladorVoz : MonoBehaviour
{


    public UnityEvent avanza;
    public UnityEvent derecha;
    public UnityEvent izquierda;
    public UnityEvent atras;
    public GameObject personaje;

    private SpeechRecognizerPlugin plugin = null;
    // Start is called before the first frame update
    /*
        "avanza",new Vector3Int(0,0,-100) );
        {"izquierda",new Vector3Int(0,-90,0) },
        {"derecha",new Vector3Int(0,90,0) },
        {"atr�s",new Vector3Int(0,180,0) },
     */
    void Start()
    {
        plugin = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        //configuraciones
        plugin.SetLanguageForNextRecognition("es-ES");
        plugin.SetMaxResultsForNextRecognition(1);
        plugin.SetContinuousListening(true);

        // startListeningBtn.onClick.AddListener(StartListening);
        //stopListeningBtn.onClick.AddListener(StopListening);
        //continuousListeningTgle.onValueChanged.AddListener(SetContinuousListening); ok 
        ///languageDropdown.onValueChanged.AddListener(SetLanguage); ok
        //maxResultsInputField.onEndEdit.AddListener(SetMaxResults); ok
        plugin.StartListening();
    }
    private void OnDestroy()
    {
        plugin.StopListening();
    }
    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        string[] result = recognizedResult.Split(delimiterChars);

        //resultsTxt.text = "";
        for (int i = 0; i < result.Length; i++)
        {
            switch (result[i]) {
                case "avanza":
                    avanza?.Invoke();
                    personaje.transform.Translate(0, 0,002);
                    break;
                case "izquierda":
                    izquierda?.Invoke();
                    personaje.transform.Rotate(0,-90,0);
                    break;
                case "derecha":
                    derecha?.Invoke();
                    personaje.transform.Rotate(0, 90, 0);
                    break;
                case "atr�s":
                    atras?.Invoke();
                    personaje.transform.Rotate(0, 180, 0);
                    break;
            }
            //resultsTxt.text += result[i] + '\n';
        }
    }
    public void mover1() {
        transform.Translate(0,20,0);
    }

}
