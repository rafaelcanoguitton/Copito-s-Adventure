using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

using static SpeechRecognizerPlugin;

public class ControladorVoz : MonoBehaviour
{
    private Player script_personaje;
    public GameObject personaje;
    public float cantidadMoverse;
    private SpeechRecognizerPlugin plugin = null;
    //public Text uiTexto;
    void Start()
    {
        //uiTexto.text = personaje.transform.position.ToString();
        //script Player
        script_personaje = personaje.GetComponent<Player>();
        //plugin
        plugin = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        //configuraciones
        plugin.SetLanguageForNextRecognition("es-PE");
        plugin.SetMaxResultsForNextRecognition(2);
        plugin.SetContinuousListening(true);
        plugin.StartListening();
    }
    private void Update()
    {
        
        //uiTexto.text = personaje.transform.position.ToString();
    }
    private void OnDestroy()
    {
        plugin.StopListening(); 
    }
    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        string[] result = recognizedResult.Split(delimiterChars);

        for (int i = 0; i < result.Length; i++)
        {
            switch (result[i]) {
                case "avanza":
                    script_personaje.objetivoMoverse =new Vector3(0,0,cantidadMoverse);
                    script_personaje.estado = 'M';
                    break;
                case "izquierda":
                    personaje.transform.Rotate(0,-90,0);
                    break;
                case "derecha":
                    personaje.transform.Rotate(0, 90, 0);
                    break;
                case "atrás":
                    personaje.transform.Rotate(0, 180, 0);
                    break;
                case "salta":
                    script_personaje.objetivoMoverse = new Vector3(0, 0, cantidadMoverse);
                    script_personaje.estado = 'S';
                    break;
                case "ataca":
                    script_personaje.fDown = true;
                    break;
                case "martillo":
                    script_personaje.sDown1=true;
                    break;
                case "pistola":
                    script_personaje.sDown2 = true;
                    break;
            }
        }
    }
}
