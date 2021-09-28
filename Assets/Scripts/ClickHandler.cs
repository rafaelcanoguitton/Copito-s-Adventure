using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class ClickHandler : MonoBehaviour{
    public UnityEvent upEvent;
    public UnityEvent downEvent;

    void OnMouseDown()
    {
        Debug.Log("Down");
        downEvent?.Invoke();
    }
    void OnMouseUp()
    {
        //transform.Translate(-100, 0, 0);
        Debug.Log("Up");
        upEvent?.Invoke();
    }
}
