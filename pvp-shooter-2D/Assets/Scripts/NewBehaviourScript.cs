using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class NewBehaviourScript : MonoBehaviour
    {
        [SerializeField] InputField inputField;// Start is called before the first frame update
        [SerializeField] Button hostBtn;
        [SerializeField] Button joinBtn;
        public void Host()
        {
            inputField.interactable = false;
            hostBtn.interactable = false;
            joinBtn.interactable = false;
        }

        // Update is called once per frame
        public void Join()
        {
            inputField.interactable = false;
            hostBtn.interactable = false;
            joinBtn.interactable = false;
        }
    }
}

