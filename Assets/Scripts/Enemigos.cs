using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;

    [SerializeField]
    Transform startPoint, endPoint;

    [SerializeField]
    float changeDirectionDelay = 3.0f;

    [SerializeField]
    public int maxHealth=3, currHealth = 3;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    private Transform destinationTarget, departTarget;
    private float startTime;
    private float journeyLength;
    bool isWaiting;


    void Start()
    {
        departTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat =GetComponentInChildren<MeshRenderer>().material ;
    }


    void FixedUpdate()
    {
        Move();
    }
    #region Movimiento
    private void Move()
    {
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * speed;

                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                transform.Rotate(0,180,0);
                isWaiting = true;
                StartCoroutine(changeDelay());
            }
        }


    }

    void ChangeDestination()
    {

        if (departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }

    }

    IEnumerator changeDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }
    #endregion
    #region colisiones
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {

            Bullet bullet = other.GetComponent<Bullet>();
            currHealth -= bullet.damage;
            //Modificado debido a que cambiaba la posicion del enemigo
            //Vector3 reactVec = transform.position = other.transform.position;
            Vector3 reactVec = other.transform.position;
            if (currHealth < 0)
                Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }
    }


    IEnumerator OnDamage(Vector3 reactVec)
    {
        Debug.Log("Colision bala");
        mat.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        if (currHealth > 0){
            mat.color = Color.white;
        }

        else{
            mat.color = Color.gray;
            gameObject.layer = 12;

            //comentado porque se sale del mapa
            //reactVec = reactVec.normalized;
            //reactVec += Vector3.up;
            //rigid.AddForce(reactVec * 5, ForceMode.Impulse);            

            Destroy(gameObject, 1);
        }
    }
    #endregion
}
