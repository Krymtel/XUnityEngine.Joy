using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace XUnityEngine.Joystick {

    public enum JoystickButton {
        B1,
        B2,
        B3,
        B4,
        B5,
        B6,
        B7,
        LB,
        RB,
        LS,
        RS
    }

    public enum JoystickAxis {
        LSX,
        LSY,
        RSX,
        RSY,
        DPADX,
        DPADY,
        LT,
        RT
    }

    public class Joystick {

        public const int    MAX_BUTTONS = 20;
        public const int    MAX_AXES    = 27;
        public const float  DEADZONE    = 1.0f / 8.0f;

        public string Name {
            get {
                return readonlyName;
            }
        }

        public int Index {
            get {
                return readonlyIndex;
            }
        }

        public bool IsConnected {
            get {
                return readonlyIsConnected;
            }
        }

        public bool IsNeutral {
            get {
                bool noInput = true;
                int i;
                i = 0;
                while ((noInput &= !GetButton (i++)) && i < Joystick.MAX_BUTTONS) ;
                if (noInput) {
                    i = 1;
                    while ((noInput &= GetAxisRaw (i++) == 0.0f) && i <= Joystick.MAX_AXES) ;
                }
                return noInput;
            }
        }

        public JoystickConfig Config {
            get {
                return readonlyConfig;
            }
        }

        private int             readonlyIndex;
        private string          readonlyName;
        private JoystickConfig  readonlyConfig;
        private bool            readonlyIsConnected;
        private string[]        axes;

        public Joystick (int index, string name) {
            string axisBase = "Joystick" + index + "Axis";
            readonlyIndex = index;
            readonlyName = name;
            readonlyIsConnected = true;
            axes = new string[MAX_AXES];
            for (int i = 0; i < MAX_AXES; i++) {
                axes[i] = axisBase + (i + 1);
            }
        }

        protected void LoadConfig (JoystickConfig config) {
            readonlyConfig = config;
        }

        private KeyCode GlobalizeButton (int buttonID) {
            if (buttonID >= 0 && buttonID < MAX_BUTTONS)
                return (KeyCode) ((int) KeyCode.Joystick1Button0 + (Index - 1) * MAX_BUTTONS + buttonID);
            else
                return KeyCode.None;
        }

        private string GlobalizeAxis (int axisID) {
            return axes[axisID];
        }

        private KeyCode MapButton (JoystickButton button) {
            switch (button) {
                case JoystickButton.B1:
                    return GlobalizeButton (readonlyConfig.B1);
                case JoystickButton.B2:
                    return GlobalizeButton (readonlyConfig.B2);
                case JoystickButton.B3:
                    return GlobalizeButton (readonlyConfig.B3);
                case JoystickButton.B4:
                    return GlobalizeButton (readonlyConfig.B4);
                case JoystickButton.B5:
                    return GlobalizeButton (readonlyConfig.B5);
                case JoystickButton.B6:
                    return GlobalizeButton (readonlyConfig.B6);
                case JoystickButton.B7:
                    return GlobalizeButton (readonlyConfig.B7);
                case JoystickButton.LB:
                    return GlobalizeButton (readonlyConfig.LB);
                case JoystickButton.RB:
                    return GlobalizeButton (readonlyConfig.RB);
                case JoystickButton.LS:
                    return GlobalizeButton (readonlyConfig.LS);
                case JoystickButton.RS:
                    return GlobalizeButton (readonlyConfig.RS);
            }
            Debug.LogError ("How did you manage to print this?");
            return 0;
        }

        public bool GetButton       (JoystickButton button) {
            return IsConnected ? Input.GetKey      (MapButton (button)) : false;
        }

        public bool GetButtonDown   (JoystickButton button) {
            return IsConnected ? Input.GetKeyDown  (MapButton (button)) : false;
        }

        // I could see it being an issue that a joystick's button will be held down, its joystick unplugged, and it never reporting a release during.

        public bool GetButtonUp     (JoystickButton button) {
            return IsConnected ? Input.GetKeyUp    (MapButton (button)) : false;
        }

        public bool GetButton      (int buttonID) {
            return Input.GetKey     (GlobalizeButton (buttonID));
        }

        public bool GetButtonDown  (int buttonID) {
            return Input.GetKeyDown (GlobalizeButton (buttonID));
        }

        public bool GetButtonUp    (int buttonID) {
            return Input.GetKeyUp   (GlobalizeButton (buttonID));
        }

        public bool AnyButtonDown () {
            bool output = false;
            int i = 0;
            while (!(output |= GetButton ((JoystickButton) i)) && ++i < 11) ;
            return output;
        }

        public float GetAxis (JoystickAxis axis) {
            if (!IsConnected)
                return 0.0f;
            switch (axis) {
                case JoystickAxis.LSX:
                    return GetLSX ();
                case JoystickAxis.LSY:
                    return GetLSY ();
                case JoystickAxis.RSX:
                    return GetRSX ();
                case JoystickAxis.RSY:
                    return GetRSY ();
                case JoystickAxis.DPADX:
                    return GetDPADX ();
                case JoystickAxis.DPADY:
                    return GetDPADY ();
                case JoystickAxis.LT:
                    return GetLT ();
                case JoystickAxis.RT:
                    return GetRT ();
            }
            Debug.LogError ("How did you manage to print this?");
            return 0;
        }

        // Maybe deadzones for triggers should be established...

        public float GetAxisRaw (JoystickAxis axis) {
            if (!IsConnected)
                return 0.0f;
            switch (axis) {
                case JoystickAxis.LSX:
                    return GetLSXRaw ();
                case JoystickAxis.LSY:
                    return GetLSYRaw ();
                case JoystickAxis.RSX:
                    return GetRSXRaw ();
                case JoystickAxis.RSY:
                    return GetRSYRaw ();
                case JoystickAxis.DPADX:
                case JoystickAxis.DPADY:
                case JoystickAxis.LT:
                case JoystickAxis.RT:
                    return GetAxis (axis);
            }
            Debug.LogError ("How did you manage to print this?");
            return 0;
        }

        public float GetAxisRaw (int axisID) {
            return Input.GetAxisRaw (GlobalizeAxis (axisID - 1));
        }

        private float GetLSXRaw () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.LSX));
        }

        private float GetLSYRaw () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.LSY));
        }

        private float GetRSXRaw () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.RSX));
        }

        private float GetRSYRaw () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.RSY));
        }

        protected virtual float GetLSX () {
            float output = GetLSXRaw ();
            return Mathf.Abs (output) > DEADZONE ? output : 0.0f;
        }

        protected virtual float GetLSY () {
            float output = GetLSYRaw ();
            return Mathf.Abs (output) > DEADZONE ? output : 0.0f;
        }

        protected virtual float GetRSX () {
            float output = GetRSXRaw ();
            return Mathf.Abs (output) > DEADZONE ? output : 0.0f;
        }

        protected virtual float GetRSY () {
            float output = GetRSYRaw ();
            return Mathf.Abs (output) > DEADZONE ? output : 0.0f;
        }

        protected virtual float GetDPADX () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.DPADX));
        }

        protected virtual float GetDPADY () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.DPADY));
        }

        protected virtual float GetLT () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.LT));
        }

        protected virtual float GetRT () {
            return Input.GetAxisRaw (GlobalizeAxis (readonlyConfig.RT));
        }

        public void Activate () {
            readonlyIsConnected = true;
        }

        public void Deactivate () {
            readonlyIsConnected = false;
        }

        public override string ToString () {
            return base.ToString ().Substring (GetType ().Namespace.Length + 1);
        }

    }

    public abstract class JoystickConfig {

        public abstract int B1 { get; }
        public abstract int B2 { get; }
        public abstract int B3 { get; }
        public abstract int B4 { get; }
        public abstract int B5 { get; }
        public abstract int B6 { get; }
        public abstract int B7 { get; }

        public abstract int LB { get; }
        public abstract int RB { get; }

        public abstract int LS { get; }
        public abstract int RS { get; }

        public abstract int LSX { get; }
        public abstract int LSY { get; }
        public abstract int RSX { get; }
        public abstract int RSY { get; }

        public abstract int DPADX { get; }
        public abstract int DPADY { get; }

        public abstract int LT { get; }
        public abstract int RT { get; }

        public override string ToString () {
            return base.ToString ().Substring (GetType ().Namespace.Length + 1);
        }

    }

}