using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XUnityEngine.Joystick {

    public enum PS4Button {
        SQUARE,
        CROSS,
        CIRCLE,
        TRIANGLE,
        L1,
        R1,
        L2,
        R2,
        SHARE,
        OPTIONS,
        L3,
        R3,
        PS,
        TOUCHPAD
    }

    public enum PS4Axis {
        LSX,
        LSY,
        RSX,
        L2,
        R2,
        RSY,
        DPADX,
        DPADY
    }

    public enum PS4WirelessAxis {
        LSX,
        NULL,
        LSY,
        RSX,
        L2,
        R2,
        RSY,
        DPADX,
        DPADY
    }

    public class PS4Joystick : Joystick {

        public PS4Joystick (int index, string name) : base (index, name) {
            bool isWired = GetAxisRaw (2) != 0.0f; // Jank check to see if we're working with bluetooth (since, in that case, Axis2 should always return 0...)
            LoadConfig (isWired ? ((JoystickConfig) new PS4JoystickConfig ()) : ((JoystickConfig) new PS4WirelessJoystickConfig ()));
        }

        protected override float GetLSY () {
            return -base.GetLSY ();
        }

        protected override float GetRSY () {
            return -base.GetRSY ();
        }

        protected override float GetLT () {
            return (base.GetLT () + 1.0f) * 0.5f;
        }

        protected override float GetRT () {
            return (base.GetRT () + 1.0f) * 0.5f;
        }

    }

    public class PS4JoystickConfig : JoystickConfig {

        public override int B1 {
            get {
                return (int) PS4Button.CIRCLE;
            }
        }

        public override int B2 {
            get {
                return (int) PS4Button.TRIANGLE;
            }
        }

        public override int B3 {
            get {
                return (int) PS4Button.SQUARE;
            }
        }
        public override int B4 {
            get {
                return (int) PS4Button.CROSS;
            }
        }
        public override int B5 {
            get {
                return (int) PS4Button.SHARE;
            }
        }

        public override int B6 {
            get {
                return (int) PS4Button.OPTIONS;
            }
        }

        public override int B7 {
            get {
                return (int) PS4Button.PS;
            }
        }

        public override int LB {
            get {
                return (int) PS4Button.L1;
            }
        }

        public override int RB {
            get {
                return (int) PS4Button.R1;
            }
        }

        public override int LS {
            get {
                return (int) PS4Button.L3;
            }
        }

        public override int RS {
            get {
                return (int) PS4Button.R3;
            }
        }

        public override int LSX {
            get {
                return (int) PS4Axis.LSX;
            }
        }

        public override int LSY {
            get {
                return (int) PS4Axis.LSY;
            }
        }

        public override int RSX {
            get {
                return (int) PS4Axis.RSX;
            }
        }

        public override int RSY {
            get {
                return (int) PS4Axis.RSY;
            }
        }

        public override int DPADX {
            get {
                return (int) PS4Axis.DPADX;
            }
        }

        public override int DPADY {
            get {
                return (int) PS4Axis.DPADY;
            }
        }

        public override int LT {
            get {
                return (int) PS4Axis.L2;
            }
        }

        public override int RT {
            get {
                return (int) PS4Axis.R2;
            }
        }

    }

    public class PS4WirelessJoystickConfig : PS4JoystickConfig {

        public override int LSX {
            get {
                return (int) PS4WirelessAxis.LSX;
            }
        }

        public override int LSY {
            get {
                return (int) PS4WirelessAxis.LSY;
            }
        }

        public override int RSX {
            get {
                return (int) PS4WirelessAxis.RSX;
            }
        }

        public override int RSY {
            get {
                return (int) PS4WirelessAxis.RSY;
            }
        }

        public override int DPADX {
            get {
                return (int) PS4WirelessAxis.DPADX;
            }
        }

        public override int DPADY {
            get {
                return (int) PS4WirelessAxis.DPADY;
            }
        }

        public override int LT {
            get {
                return (int) PS4WirelessAxis.L2;
            }
        }

        public override int RT {
            get {
                return (int) PS4WirelessAxis.R2;
            }
        }

    }

}