using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XUnityEngine.Joystick {

    /* TODO
     *
     * - WebGL: Figure out some way to recognize that a joystick is no longer connected so that we can stop reading in input (this is particular to XBOX it seems.)
     *          I think WebGL must use some other way to represent a DC'd controller besides an empty string.
     *          
     * - General: Handle the case that an amount of joysticks greater than MAX_JOYSTICKS is connected.
     * 
     */

    public class JoystickManager : MonoBehaviour {

        public const int MAX_JOYSTICKS = 10;

        public event Action<Joystick> OnRegister;
        public event Action<Joystick> OnConnect;
        public event Action<Joystick> OnDisconnect;

        public int JoystickCount {
            get {
                return readonlyJoyCount;
            }
        }

        private Joystick[] joysticks;
        private int readonlyJoyCount = 0;

        private string[] joyNames;
        private string[] prevJoyNames;

        private int joyNameCount = 0;
        private int prevJoyNameCount = 0;

        private void Awake () {
            joyNames = new string[MAX_JOYSTICKS];
            prevJoyNames = new string[MAX_JOYSTICKS];
            joysticks = new Joystick[MAX_JOYSTICKS];
            PollForJoysticks ();
            print ("Detected " + joyNameCount + " joystick" + (joyNameCount == 1 ? "." : "s."));
            for (int i = 0; i < Mathf.Min (joyNameCount, MAX_JOYSTICKS); i++)
                StartCoroutine (AddJoystick (joyNames, i + 1));
            joyNames.CopyTo (prevJoyNames, 0);
            prevJoyNameCount = joyNameCount;
        }

        private void Update () {
            PollForJoysticks ();
            for (int i = 0; i < Mathf.Min (prevJoyNameCount, joyNameCount); i++) {
                if (prevJoyNames[i].Length == 0 && joyNames[i].Length > 0)
                    StartCoroutine (AddJoystick (joyNames, i + 1));
            }
            for (int i = prevJoyNameCount; i < joyNameCount; i++) {
                StartCoroutine (AddJoystick (joyNames, i + 1));
            }
            for (int i = 0; i < joyNameCount; i++) {
                Joystick j = GetJoystickByID (i + 1);
                if (j == null)
                    continue;
                if (joyNames[i].Length == 0)
                    DisableJoystick (i + 1);
                else
                    EnableJoystick  (i + 1);
            }
            joyNames.CopyTo (prevJoyNames, 0);
            prevJoyNameCount = joyNameCount;
        }

        private void PollForJoysticks () {
            string[] newJoyNames = Input.GetJoystickNames ();
            newJoyNames.CopyTo (joyNames, 0);
            joyNameCount = newJoyNames.Length;
        }

        private IEnumerator AddJoystick (string[] joystickNames, int joystickIndex) {
            if (GetJoystickByID (joystickIndex) != null) {
                Debug.LogWarning ("Tried adding a joystick where there was one already!");
                yield break;
            }
            string name = joystickNames[joystickIndex - 1];
            // Check if the joystick is inactive...
            if (name.Length == 0) {
                print ("Inactive joystick detected at index " + joystickIndex + ". Ignoring.");
                yield break;
            }
            Joystick joystick = null;
            string joyID = "Joystick" + joystickIndex;
            print ("Attempting to bind joystick " + joystickIndex + ": " + name);
            Func<bool> IsPollingForInput = delegate () {
                bool isPolling = true;
                int i = 0;
                while ((isPolling = Input.GetAxisRaw (joyID + "Axis" + (++i)) == 0.0f) && i < Joystick.MAX_AXES);
                return isPolling;
            };
            while (IsPollingForInput ())
                yield return null;
            switch (name) {
                // Assume that the default joystick is configured to work like an XBOX 360 controller.
                default:
                    // case "Controller (Xbox One For Windows)":                    // Standalone (Win)
                    // case "Controller (XBOX 360 For Windows)":                    // Standalone (Win)
                    // case "Controller (Xbox 360 Wireless Receiver for Windows)    // Standalone (Win)
                    // case "xinput":                                               // WebGL
                    joystick = new XBOX360Joystick (joystickIndex, name);
                    break;
                // PS4 controllers, although I'm not sure if they hold these names exclusively.
                // Would be nice if Unity gave me a better way to read into a joystick's info...
                case "Wireless Controller":                                         // Standalone (Win)
                case "054c-05c4-Wireless Controller":                               // WebGL
                    bool isWired = Input.GetAxisRaw (joyID + "Axis2") != 0.0f; // Jank check to see if we're working with bluetooth (since, in that case, Axis2 should always return 0...)
                    joystick = new PS4Joystick (joystickIndex, name, isWired);
                    break;
            }
            print ("Successfully bound joystick " + joystickIndex + " of type " + joystick + " with config " + joystick.Config + '.');
            joysticks[readonlyJoyCount++] = joystick;
            if (OnRegister != null)
                OnRegister (joystick);
            if (OnConnect != null)
                OnConnect (joystick);
            yield break;
        }

        public Joystick GetJoystick (int joystick) {
            if (joystick < 1 || joystick > readonlyJoyCount)
                return null;
            int i = 0;
            Joystick potential = null;
            while (joystick > 0 && i < readonlyJoyCount) {
                potential = joysticks[i++];
                if (potential.IsActive)
                    joystick--;
            }
            return joystick == 0 ? potential : null;
        }

        public Joystick GetJoystickByID (int joystickID) {
            for (int i = 0; i < readonlyJoyCount; i++) {
                Joystick potentialJoystick = joysticks[i];
                if (potentialJoystick.Index == joystickID)
                    return potentialJoystick;
            }
            return null;
        }

        private void DisableJoystick (int joystickIndex) {
            Joystick joy = GetJoystickByID (joystickIndex);
            if (joy == null || !joy.IsActive)
                return;
            Debug.LogWarning ("Joystick " + joystickIndex + " has been disconnected!");
            joy.Deactivate ();
            if (OnDisconnect != null)
                OnDisconnect (joy);
        }

        private void EnableJoystick (int joystickIndex) {
            Joystick joy = GetJoystickByID (joystickIndex);
            if (joy == null || joy.IsActive)
                return;
            Debug.LogWarning ("Joystick " + joystickIndex + " has been reconnected!");
            joy.Activate ();
            if (OnConnect != null)
                OnConnect (joy);
        }

    }

}