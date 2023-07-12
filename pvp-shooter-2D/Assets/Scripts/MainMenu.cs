using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Project
{
    [System.Serializable]
    public class Match
    {
        public string ID;
        public readonly List<GameObject> players = new List<GameObject>(); 
        public Match(string ID, GameObject player)
        {
            this.ID = ID;
            players.Add(player);
        }
        public Match() { }
    }
    public class MainMenu : NetworkBehaviour
    {
        public static MainMenu instance;
        public SyncList<Match> matches = new SyncList<Match>();
        public SyncList<string> matchIDs = new SyncList<string>();
        public InputField joinInput;
        public Button hostButton;
        public Button joinButton;
        public Canvas loadingCanvas;
        public Transform UIPlayerParent;
        public GameObject UIPlayerPrefab;
        public Text IDText;
        public Button startButton;
        public GameObject turnManager; 
        public bool inGame;

        private void Start()
        {
            instance = this;
        }
        private void Update()
        {
            if (!inGame)
            {
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                for(int i = 0; i < players.Length; i++)
                {
                    players[i].gameObject.transform.localScale = Vector3.zero;
                }
            }
        }
        public void Host()
        {
            joinInput.interactable = false;
            hostButton.interactable = false;
            joinButton.interactable = false;

            PlayerController.localPlayer.HostGame();
        }
        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                loadingCanvas.gameObject.SetActive(true);
                SpawnPlayerPrefabUI(PlayerController.localPlayer);
                IDText.text = matchID;
                startButton.interactable = true;
            }
            else
            {
                joinInput.interactable = true;
                hostButton.interactable = true;
                joinButton.interactable = true;
            }
        }
        public void Join()
        {
            joinInput.interactable = false;
            hostButton.interactable = false;
            joinButton.interactable = false;

            PlayerController.localPlayer.JoinGame(joinInput.text.ToUpper());
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                loadingCanvas.gameObject.SetActive(true);
                SpawnPlayerPrefabUI(PlayerController.localPlayer);
                IDText.text = matchID;
                startButton.interactable = false;
            }
            else
            {
                joinInput.interactable = true;
                hostButton.interactable = true;
                joinButton.interactable = true;
            }
        }

        public void SpawnPlayerPrefabUI(PlayerController player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player);
        }

        public bool HostGame(string matchID, GameObject player)
        {
            if (!matchIDs.Contains(matchID))
            {
                matchIDs.Add(matchID);
                matches.Add(new Match(matchID, player));
                return true;
            }
            else return false;
        }
        public bool JoinGame(string matchID, GameObject player)
        {
            if (matchIDs.Contains(matchID))
            {
               for(int i = 0; i < matches.Count; i++)
                {
                    matches[i].players.Add(player);
                    break;
                }
                return true;
            }
            else return false;
        }
        public static string GetRandomID()
        {
            string ID = string.Empty;
            for(int i = 0; i < 5; i++)
            {
                int rand = UnityEngine.Random.Range(0, 36);
                if (rand < 26)
                {
                    ID += (char)(rand + 65);
                }
                else
                {
                    ID += (rand - 26).ToString();
                }
            }
            return ID;
        }
        public void BeginGame()
        {
            PlayerController.localPlayer.StartGame();
        }
        public void StartGame(string matchID)
        {
            GameObject tempTurnManager = Instantiate(turnManager);
            NetworkServer.Spawn(tempTurnManager);
            tempTurnManager.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
            TurnManager newturnManager = tempTurnManager.GetComponent<TurnManager>();
            for(int i=0; i < matches.Count; i++)
            {
                if (matches[i].ID == matchID)
                {
                    foreach (var player in matches[i].players)
                    {
                        PlayerController player1 = player.GetComponent<PlayerController>();
                        newturnManager.AddPlayer(player1);
                        player1.BeginGame();
                    }
                    break;
                }
            }
        }
    }
    public static class MatchExtension
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hasBytes = provider.ComputeHash(inputBytes);

            return new Guid(hasBytes);
        }

    }

}
