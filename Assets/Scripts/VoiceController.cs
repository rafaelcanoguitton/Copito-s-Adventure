using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;


public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "es-ES";
    public GameObject Personaje;
    //-------------------------Comandos voz
    KeyValuePair<string, Vector3Int> avanzar = new KeyValuePair<string, Vector3Int>(
         "avanza",new Vector3Int(0,0,-100) );
    Dictionary<string, Vector3Int> comandos_rotacion = new Dictionary<string, Vector3Int>() {
        {"izquierda",new Vector3Int(0,-90,0) },
        {"derecha",new Vector3Int(0,90,0) },
        {"atrás",new Vector3Int(0,180,0) },
    };
    //-------------------------
    [SerializeField]
    Text uiText;
    private void Start(){
        Setup(LANG_CODE);
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;

        CheckPermission();
    }
    void CheckPermission() {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
    #region Text to Speech
    public void StartSpeaking(string message) {
        TextToSpeech.instance.StartSpeak(message);
    }
    public void StopSpeaking() {
        TextToSpeech.instance.StopSpeak();
    }
    void OnSpeakStart() {
        Debug.Log("Empezando a hablar...");
    }
    void OnSpeakStop() {
        Debug.Log("Parando de hablar...");
    }
    #endregion

    #region Speech to Text
    public void StartListening() {
        SpeechToText.instance.StartRecording();
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();

    }
    void OnFinalSpeechResult(string result) {
        uiText.text = result;
        //Comandos avanzar
        if (result == avanzar.Key) {
            Personaje.transform.Translate(avanzar.Value);
        }
        //Comandos girar
        if (comandos_rotacion.ContainsKey(result)) {
            Personaje.transform.Rotate(comandos_rotacion[result]);
        }
    }
    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }
    #endregion
    void Setup(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
        SpeechToText.instance.Setting(code);
    }
}
