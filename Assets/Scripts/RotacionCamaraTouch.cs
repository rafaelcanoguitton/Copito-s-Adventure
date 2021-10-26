using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionCamaraTouch : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotacionY;
    private float velocidadRotacion = 0.2f;

    // Update is called once per frame
    void Update(){
        if (Input.touchCount > 0){
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved){
                /*rotacionY = Quaternion.Euler(
                        0.0f,
                        -touch.deltaPosition.x * velocidadRotacion,
                        0.0f
                    );
                */
                //transform.rotation = rotacionY * transform.rotation;
                transform.RotateAround(
                    new Vector3(0,0,0),
                    Vector3.up,
                    -touch.deltaPosition.x * velocidadRotacion);
            }
        }
    }
}
