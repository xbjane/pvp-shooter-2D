using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Project
{
    public class TurnManager : MonoBehaviour
    {
        private List<PlayerController> players = new List<PlayerController>();
        public void AddPlayer(PlayerController player)
        {
            players.Add(player);
        }
    }
}

