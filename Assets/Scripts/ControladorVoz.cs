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
        plugin.SetLanguageForNextRecognition("es-ES");
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
                    //uiTexto.text = "Avanza";
                    script_personaje.objetivoMoverse =new Vector3(0,0,cantidadMoverse);
                    script_personaje.estado = 'M';
                    break;
                case "izquierda":
                    //uiTexto.text = "Izquierda";
                    personaje.transform.Rotate(0,-90,0);
                    break;
                case "derecha":
                    //uiTexto.text = "Derecha";
                    personaje.transform.Rotate(0, 90, 0);
                    break;
                case "atrás":
                    //uiTexto.text = "Atrás";
                    personaje.transform.Rotate(0, 180, 0);
                    break;
                case "salta":
                    //uiTexto.text = "Salta";
                    script_personaje.objetivoMoverse = new Vector3(0, 0, cantidadMoverse);
                    script_personaje.estado = 'S';
                    break;
            }
        }
    }
}
