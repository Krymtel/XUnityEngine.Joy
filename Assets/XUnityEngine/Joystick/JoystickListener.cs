using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XUnityEngine.Joystick {

    public class JoystickListener : MonoBehaviour {

        public bool IsConnected {
            get {
                return joystick != null;
            }
        }

        public int playerID = -1;

        // Whenever we connect to a joystick.
        public UnityEvent OnConnect;
        // Whenever we disconnect from a joystick.
        public UnityEvent OnDisconnect;

        private JoystickManager joyManager;
        private Joystick joystick;

        private void Awake () {
            if (!GetManager ()) return;
            if (!GetPlayerID ()) return;
            // Try to bind to the nth most active joystick, where n = playerID. The argument "joy" is not used at the moment.
            joyManager.OnActivate += delegate (Joystick joy) {
                Joystick potentialJoystick = joyManager.GetJoystick (playerID);
                if (potentialJoystick != null && !IsConnected) {
                    joystick = potentialJoystick;
                    OnConnect.Invoke ();
                }
            };
            // If the joystick being deactivated is equal to the joystick we've connected to, disconnect it.
            joyManager.OnDeactivate += delegate (Joystick joy) {
                if (joystick == joy) {
                    joystick = null;
                    OnDisconnect.Invoke ();
                }
            };
        }

        private bool GetManager () {
            joyManager = GameObject.FindObjectOfType<JoystickManager> ();
            if (joyManager == null) {
                Debug.LogError ("Couldn't find a JoystickManager in this scene to listen from. Have you forgotten to place one?");
                return false;
            }
            return true;
        }

        private bool GetPlayerID () {
            if (playerID < 1 || playerID > JoystickManager.MAX_JOYSTICKS) {
                Debug.LogError ("Invalid player ID of " + playerID + ". A valid player ID must be within the range of 1-10, inclusively, so that this JoystickListener can connect to a Joystick.");
                return false;
            }
            return true;
        }

        /*private void GetPlayerID () {
            int i = name.Length;
            while (--i >= 0 && name[i] >= '0' && name[i] <= '9') ;
            if (i == name.Length - 1) {
                Debug.LogWarning ("Couldn't determine what this GameObject's PlayerID is. Make sure to append a valid int to the end of its name. Defaulting to Player 01.");
                readonlyPlayerID = 1;
            } else {
                // Set this player's id to the integer at the end of its name... (e.g. Player01's id is 1).
                // This determines the priority in which joysticks connect (lower id = oldest connected joysticks, higher id = most recently connected joysticks)
                readonlyPlayerID = Int32.Parse (name.Substring (i + 1));
            }
        }*/

        public bool GetButton       (JoystickButton button) {
            return IsConnected ? joystick.GetButton     (button) : false;
        }

        public bool GetButtonDown   (JoystickButton button) {
            return IsConnected ? joystick.GetButtonDown (button) : false;
        }

        public bool GetButtonUp     (JoystickButton button) {
            return IsConnected ? joystick.GetButtonUp   (button) : false;
        }

        public float GetAxis        (JoystickAxis axis) {
            return IsConnected ? joystick.GetAxis       (axis) : 0.0f;
        }

        public float GetAxisRaw     (JoystickAxis axis) {
            return IsConnected ? joystick.GetAxisRaw    (axis) : 0.0f;
        }

    }

}
