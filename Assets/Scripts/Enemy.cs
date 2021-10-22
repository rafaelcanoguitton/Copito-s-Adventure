using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Type { Enemigo, Bloque };
    public Type type;

    public int maxHealth;
    public int currHealth;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    public float speed;
    public Transform target;
    private Vector3 initialPosition;
    private Vector3 targetPosition;


    void Awake()
    {
        initialPosition = transform.position;
        targetPosition = target.position;

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = (type == Type.Enemigo) ? GetComponentInChildren<MeshRenderer>().material : GetComponent<MeshRenderer>().material ;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee" && type == Type.Bloque)
        {
            Weapon weapon = other.GetComponent<Weapon>();
            currHealth -= weapon.damage;
            Vector3 reactVec = transform.position = other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }

        else if (other.tag == "Bullet" && type == Type.Enemigo)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            currHealth -= bullet.damage;
            Vector3 reactVec = transform.position = other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }
    }


    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(currHealth > 0) {
            mat.color = Color.white;
        }

        else
        {
            mat.color = Color.gray;
            gameObject.layer = 12;

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);            

            Destroy(gameObject, 1);
        }
    }


    void Update()
    {
        if (type == Type.Enemigo)
        {
            Turn();

            //Debug.Log("Target : " + targetPosition);
            //Debug.Log("Inicio : " + initialPosition);

            if (transform.position == target.position) {
                targetPosition = initialPosition;
            }

            if (transform.position == initialPosition) {
                targetPosition = target.position;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }

    void Turn()
    {
        transform.LookAt(targetPosition);
    }
}
