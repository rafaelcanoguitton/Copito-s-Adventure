
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//public class Player : MonoBehaviour
public class Player : NetworkBehaviour
{
    public float speed;
    public GameObject[] weapon;
    public bool[] hasWeapon;

    public int ammo;
    public int coin;
    public int health = 3;
    public int maxAmmo = 3;
    public int maxCoin = 999;
    public int maxHealth = 3;

    float hAxis; float vAxis;

    bool wDown; bool jDown;
    public bool fDown=false;
    public bool sDown1=false; 
    public bool sDown2=false;

    bool isJump;
    bool isSwap;
    bool isFireReady;
    //bool isBorder;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    float FireDelay;

    //control voz
    public Vector3 objetivoMoverse;
    public char estado = 'N';
    public NetworkVariable<char> state_animacion;
    public float angulo_salto;
    void Awake()
    {
        hasWeapon = new bool[2] { false, false };
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //Controlar por teclado
        /*
        state_animacion.Value='N';
        GetInput();
        Move();
        Turn();
        Jump();
        */
        //Controlar por teclado
        //Controlar por voz
        
        AnimacionServerRpc(estado);
        if(NetworkManager.Singleton.IsServer){
            Atack();
            Swap();
            moverse();
            saltar();
            rotar();
            AnimacionesRed();
        }
        
        //Controlar por voz
    }
    #region Movimiento por voz
    private void rotar(){
        if(estado=='I'){//izquierda
            transform.Rotate(0, -90, 0);
            estado='N';
        }
        if(estado=='D'){//izquierda
            transform.Rotate(0, 90, 0);
            estado='N';
        }
        if(estado=='A'){//izquierda
            transform.Rotate(0, 180, 0); 
            estado='N';
        }
        
    }
    private void moverse()
    {
        if (estado == 'M')
        {
            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse);
            estado = 'C';
        }
        if (estado == 'C')
        {
            if (transform.position.x != objetivoMoverse.x && transform.position.z != objetivoMoverse.z)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    objetivoMoverse, speed * Time.deltaTime);
                //anim.SetBool("isRun", true);
            }
            else
            {
                //anim.SetBool("isRun", false);
                estado = 'N';
            }
        }
    }
    private void saltar()
    {

        if (estado == 'S')
        {
            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse * 2);

            rigid.velocity = BallisticVelocity(objetivoMoverse, angulo_salto);
            //anim.SetBool("isJump", true);
            //anim.SetTrigger("doJump");
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

    void GetInput()
    {
        
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButton("Jump");

        fDown = Input.GetButton("Fire1");//ataca
        sDown1 = Input.GetButton("Swap1");//martillo
        sDown2 = Input.GetButton("Swap2");//pistola
        
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isSwap)
            moveVec = Vector3.zero;

        //if (!isBorder)
        transform.position += moveVec * speed * Time.deltaTime;
        /*
        if(moveVec != Vector3.zero)
            state_animacion.Value='C';
        */
        //anim.SetBool("isRun", moveVec != Vector3.zero);
        //
        //ActivarAnimMoveServerRpc();
    }


    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump && !isSwap)
        {
            
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            //state_animacion.Value='S';
            //anim.SetBool("isJump", true);
            //anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Atack()
    {
        if (equipWeapon == null)
            return;

        FireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < FireDelay;

        if (fDown && isFireReady && !isSwap && !isJump)
        {
            if (equipWeapon.gameObject.activeInHierarchy == true)
            {
                equipWeapon.Use();
                anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                FireDelay = 0;
            }
            fDown = false;
        }
    }

    void Swap()
    {
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if ((sDown1 || sDown2) && !isJump)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            if (hasWeapon[weaponIndex] == true)
            {
                equipWeapon = weapon[weaponIndex].GetComponent<Weapon>();
                equipWeapon.gameObject.SetActive(true);

                anim.SetTrigger("doSwap");
                isSwap = true;
                Invoke("SwapOut", 0.0f);
            }
            sDown1 = false;
            sDown2 = false;
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    /*void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }*/

    void AnimacionesRed(){
        switch(state_animacion.Value){
            case 'C':
                anim.SetBool("isRun", true);
            break;
            case 'S':
                anim.SetBool("isJump", true);
                anim.SetTrigger("doJump");
            break;
            case 'N':
                anim.SetBool("isRun", false);
                anim.SetBool("isJump", false);
                break;
        }
    }

    void fixedUpdate()
    {
        FreezeRotation();
        //StopToWall();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log("Floor");
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Coin:
                    //Debug.Log("MONEDA");
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;

                case Item.Type.Heart:
                    //Debug.Log("Corazon");
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
    //------------------------------------------------------------------
    [ServerRpc(RequireOwnership =false)]//cliente->servidor
    void AnimacionServerRpc(char estado_animacion){
        state_animacion.Value=estado_animacion;
    }
}

/*
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
    public int health = 3;
    public int maxAmmo = 3;
    public int maxCoin = 999;
    public int maxHealth = 3;

    float hAxis; float vAxis;

    bool wDown; bool jDown;
    public bool fDown=false;
    public bool sDown1=false; 
    public bool sDown2=false;

    bool isJump;
    bool isSwap;
    bool isFireReady;
    //bool isBorder;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    float FireDelay;

    //control voz
    public Vector3 objetivoMoverse;
    public char estado = 'N';
    public float angulo_salto;
    void Awake()
    {
        hasWeapon = new bool[2] { false, false };
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

        //control voz
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
            objetivoMoverse = transform.position + transform.TransformDirection(objetivoMoverse * 2);

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

    void GetInput()
    {
        
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButton("Jump");

        fDown = Input.GetButton("Fire1");//ataca
        sDown1 = Input.GetButton("Swap1");//martillo
        sDown2 = Input.GetButton("Swap2");//pistola
        
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isSwap)
            moveVec = Vector3.zero;

        //if (!isBorder)
        transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }


    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump && !isSwap)
        {
            
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Atack()
    {
        if (equipWeapon == null)
            return;

        FireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < FireDelay;

        if (fDown && isFireReady && !isSwap && !isJump)
        {
            if (equipWeapon.gameObject.activeInHierarchy == true)
            {
                equipWeapon.Use();
                anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                FireDelay = 0;
            }
            fDown = false;
        }
    }

    void Swap()
    {
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if ((sDown1 || sDown2) && !isJump)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            if (hasWeapon[weaponIndex] == true)
            {
                equipWeapon = weapon[weaponIndex].GetComponent<Weapon>();
                equipWeapon.gameObject.SetActive(true);

                anim.SetTrigger("doSwap");
                isSwap = true;
                Invoke("SwapOut", 0.0f);
            }
            sDown1 = false;
            sDown2 = false;
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    //void StopToWall()
    //{
      //  Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        //isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    //}

    void fixedUpdate()
    {
        FreezeRotation();
        //StopToWall();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log("Floor");
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Coin:
                    //Debug.Log("MONEDA");
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;

                case Item.Type.Heart:
                    //Debug.Log("Corazon");
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

*/