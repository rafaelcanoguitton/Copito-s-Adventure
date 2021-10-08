using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown=true;
    bool isJump=false;
    Rigidbody rigid;
    Animator anim;
    //
    public Vector3 objetivoMoverse;
    public Vector3 objetivoSaltar;
    public char estado = 'N';
    public float initialAngle=20;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        moverse();
        saltar();

    }

    #region Movimiento por voz
    private void moverse(){
        if (estado == 'M') {
            
            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse);
            estado = 'c';
        }
        if (estado == 'c')
        {
            if (transform.position != objetivoMoverse)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    objetivoMoverse, speed * Time.deltaTime);
                anim.SetBool("isRun", true);
            }
            else {
                anim.SetBool("isRun", false);
                estado = 'N';
            }   
        }
    }
    private void saltar() {

        if (estado == 'S')
        {
            objetivoMoverse = transform.position +
                transform.TransformDirection(objetivoMoverse);
            rigid.velocity = BallisticVelocity(objetivoMoverse,80.0f);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            estado = 'N';
            /*
            objetivoMoverse = transform.position + 
                transform.TransformDirection(objetivoMoverse);
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            estado = 's';
            anim.SetTrigger("doJump");
            */
        }
        if (estado == 's')
        {
            /*
            //Si se mueve
            if (transform.position.y >=objetivoSaltar.y) {
                transform.position = Vector3.MoveTowards(transform.position,
                    objetivoMoverse, speed * Time.deltaTime);    
                return;
            }
            estado = 'N';
            */
        }

    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    #endregion
    #region Movimiento por teclado
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButton("Jump");
    }
    void Move()
    {
        Vector3 moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec* speed * (wDown? 0.3f: 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }
    
        
    void Turn() 
    {
        //transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    #endregion
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
