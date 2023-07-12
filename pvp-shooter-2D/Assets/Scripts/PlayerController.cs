using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

namespace Project
{
    public class PlayerController : NetworkBehaviour
    {
        public static PlayerController localPlayer;
        [SyncVar] public string matchID;
        private NetworkMatch networkMatch;

        public bool isDead;
        private Joystick joystick;
        [SerializeField]
        //Button shootButton;
        private Rigidbody2D rb;
        private SpriteRenderer sprite;
        [SerializeField]
        float speed;
        private float moveInput;
        private Vector2 dir;
        private Quaternion m_MyQuaternion;
        private Vector2 startPos;
        private Vector3 startScale;
        // Start is called before the first frame update
        void Start()
        {
            networkMatch=GetComponent<NetworkMatch>();
            if (isOwned)
            {
                Debug.Log("Autority");
                if (isLocalPlayer)
                {
                    localPlayer = this;
                }
                else
                {
                    MainMenu.instance.SpawnPlayerPrefabUI(this);
                } 
                isDead = false;
                rb = GetComponent<Rigidbody2D>();
                joystick = FindObjectOfType<Joystick>();
                joystick = GetComponent<DynamicJoystick>();
                //joystick = Find
                sprite = GetComponent<SpriteRenderer>();
                startScale = transform.localScale;
            }
            else Debug.Log("No Autority");
        }

        // Update is called once per frame
        void Update()
        {
            if (isOwned)
            {
                //Debug.Log(joystick.name);
                //Debug.Log("Autority");
                // startPos = transform.position;
                //m_MyQuaternion = transform.rotation;
                //if (!isLocalPlayer)
                    dir = transform.right * joystick.Horizontal + transform.up * joystick.Vertical;
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
            else Debug.Log("No Autority");
        }
        private void FixedUpdate()
        {
            if (isOwned)
            {
                Debug.Log("Autority");
                startPos = rb.position;
                rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
                //Vector2 vector = transform.position - startPos;
                //float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90f;
                //rb.rotation = angle;
            }
            else Debug.Log("No Autority");
        }
        //public void HostGame()
        //{
        //    string roomName = "";
        //    CmdHostGame(roomName);
        //}

        //[Command]
        //void CmdHostGame(string roomName)
        //{

        //}
        public void HostGame()
        {
            string ID = MainMenu.GetRandomID();
            CmdHostGame(ID);
        }

        [Command]
        public void CmdHostGame(string ID)
        {
            matchID = ID;
            if (MainMenu.instance.JoinGame(ID, gameObject))
            {
                Debug.Log("Успешное подключение к лобби");
                networkMatch.matchId = ID.ToGuid();
                TargetJoinGame(true, ID);
            }
            else
            {
                Debug.Log("Не удалось подключиться");
                TargetJoinGame(false, ID);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string ID)
        {
            matchID = ID;
            Debug.Log($"ID {matchID} == {ID}");
            MainMenu.instance.HostSuccess(success, ID);
        }

        public void JoinGame(string inputID)
        {
            CmdJoinGame(inputID);
        }

        [Command]
        public void CmdJoinGame(string ID)
        {
            matchID = ID;
            if (MainMenu.instance.JoinGame(ID, gameObject))
            {
                Debug.Log("Успешное подключение к лобби");
                networkMatch.matchId = ID.ToGuid();
                TargetJoinGame(true, ID);
            }
            else
            {
                Debug.Log("Не удалось подключиться");
                TargetJoinGame(false, ID);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string ID)
        {
            matchID = ID;
            Debug.Log($"ID {matchID} == {ID}");
            MainMenu.instance.JoinSuccess(success, ID);
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        public void CmdBeginGame()
        {
            MainMenu.instance.StartGame(matchID);
            Debug.Log("Игра начилась");
        }

        public void StartGame()
        {
            TargetBeginGame();
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log($"ID {matchID} | начало");
            DontDestroyOnLoad(gameObject);
            MainMenu.instance.inGame = true;
            transform.localScale = startScale; //Размер вашего игрока (x, y, z)
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }
    }
   
}

