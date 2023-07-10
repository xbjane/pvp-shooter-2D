using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class PlayerController : NetworkBehaviour
{
    public bool isDead;
    [SerializeField]
    Joystick joystick;
    [SerializeField]
    Button shootButton;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    [SerializeField]
    float speed;
    private float moveInput;
    private Vector2 dir;
    private Quaternion m_MyQuaternion;
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       // startPos = transform.position;
        //m_MyQuaternion = transform.rotation;
        if(!isLocalPlayer)
        dir = transform.right*joystick.Horizontal + transform.up * joystick.Vertical;
        //if (dir.x != 0 || dir.y != 0)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        //    float angle = Quaternion.Angle(m_MyQuaternion, transform.rotation);
        //    transform.rotation = new Quaternion(0,0,angle,0);
        //}
        //if (!isDead)
        //{
        //    moveInput = joystick.Horizontal;
        //    rb.velocity = new Vector2(moveInput * speed, rb.velocity.x);
        //    moveInput = joystick.Vertical;
        //    rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        //}       
    }
    private void FixedUpdate()
    {
        startPos = rb.position;
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        //Vector2 vector = transform.position - startPos;
        //float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
    }
}
