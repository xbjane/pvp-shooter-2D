using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class PlayerUI : MonoBehaviour
    {
        public Text playerText;
        private PlayerController player;
        public void SetPlayer(PlayerController player)
        {
            this.player = player;
            playerText.text = "Name";
        }
    }
}

