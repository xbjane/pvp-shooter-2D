using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Project
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] Text text;
        public NetworkManager networkManager;
        public InputField createField;
        public InputField enterField;
        private void Start()
        {
            if (!Application.isBatchMode)
            {
               // Debug.Log("Client connected");
                networkManager.StartClient();
            }
           // else Debug.Log("Server strted");
        }
        public void JoinClient()
        {
            try
            {
                networkManager.networkAddress = "localhost";
                networkManager.StartClient();
            }
            catch
            {
                text.gameObject.SetActive(true);
            }
        }
        //public void StartClientMy()
        //{
        //    networkManager.networkAddress = createField.text;
        //    networkManager.pl
        //}
    }
}

