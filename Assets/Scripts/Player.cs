using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapon;
    public bool[] hasWeapon;

    public int ammo;
    public int coin;
    public int health;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;

    float hAxis; float vAxis;

    bool wDown; bool jDown;
    bool sDown1; bool sDown2;
    bool fDown;

    bool isJump;
    bool isSwap;
    bool isFireReady;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    float FireDelay;
    //CONTROL VOZ
    public Vector3 objetivoMoverse;
    public char estado = 'N';
    public float angulo_salto = 90;
    void Awake()
    {
        weapon = new GameObject[2];
        hasWeapon = new bool[2];
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        
        GetInput();
        Move();
        Turn();
        Jump();
        Atack();
        Swap();
        
        //
        moverse();
        saltar();
    }
    #region Movimiento por voz
    private void moverse()
    {
        if (estado == 'M')
        {

            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse);
            estado = 'c';
        }
        if (estado == 'c')
        {
            if (transform.position.x != objetivoMoverse.x && transform.position.z != objetivoMoverse.z)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    objetivoMoverse, speed * Time.deltaTime);
                anim.SetBool("isRun", true);
            }
            else
            {
                anim.SetBool("isRun", false);
                estado = 'N';
            }
        }
    }
    private void saltar()
    {

        if (estado == 'S')
        {
            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse*2);

            rigid.velocity = BallisticVelocity(objetivoMoverse, angulo_salto);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            estado = 'N';

        }
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; 
        float height = dir.y; 
        dir.y = 0; 
        float dist = dir.magnitude; 
        float a = angle * Mathf.Deg2Rad; 
        dir.y = dist * Mathf.Tan(a); 
        dist += height / Mathf.Tan(a); 
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; 
    }

    #endregion

    #region Movimiento por teclado
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButton("Jump");
        fDown = Input.GetButton("Fire1");
        sDown1 = Input.GetButton("Swap1");
        sDown2 = Input.GetButton("Swap2");
    }
        
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isSwap)
            moveVec = Vector3.zero;

        transform.position += moveVec* speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }
    
        
    void Turn() 
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump && !isSwap) {
            rigid.AddForce(Vector3.up * 7, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    #endregion
    void Atack()
    {
        if (equipWeapon == null)
            return;

        FireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < FireDelay;

        if(fDown && isFireReady && !isSwap){
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            FireDelay = 0;
        }
    }

    void Swap()
    {
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if ((sDown1 || sDown2) && !isJump) {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            if (hasWeapon[weaponIndex] == true) {
                equipWeapon = weapon[weaponIndex].GetComponent<Weapon>();
                equipWeapon.gameObject.SetActive(true);

                anim.SetTrigger("doSwap");
                isSwap = true;
                Invoke("SwapOut", 0.0f);
            }
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor") {
            Debug.Log("Floor");
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            Debug.Log("Floor2");
        }
            if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type) {
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;

                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
            }
            Destroy(other.gameObject);

        }

        if (other.tag == "Weapon")
        {
            Item item = other.GetComponent<Item>();
            int weaponIndex = item.value;
            hasWeapon[weaponIndex] = true;
            Destroy(other.gameObject);
        }
    }

    
}
