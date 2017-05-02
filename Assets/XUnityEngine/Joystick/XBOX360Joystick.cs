using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XUnityEngine.Joystick {

    public enum XBOX360Button {
        A,
        B,
        X,
        Y,
        LB,
        RB,
        SELECT,
        START,
        LS,
        RS
    }

    public enum XBOX360Axis {
        LSX,
        LSY,
        TRIGGER,
        RSX,
        RSY,
        DPADX,
        DPADY,
        NULL,
        LT,
        RT
    }

    public class XBOX360Joystick : Joystick {

        public XBOX360Joystick (int index, string name) : base (index, name) {
            LoadConfig (new XBOX360Config ());
        }

        protected override float GetLSY () {
            return -base.GetLSY ();
        }

        protected override float GetRSY () {
            return -base.GetRSY ();
        }

    }


    public class XBOX360Config : JoystickConfig {

        public override int B1 {
            get {
                return (int) XBOX360Button.B;
            }
        }

        public override int B2 {
            get {
                return (int) XBOX360Button.Y;
            }
        }

        public override int B3 {
            get {
                return (int) XBOX360Button.X;
            }
        }
        public override int B4 {
            get {
                return (int) XBOX360Button.A;
            }
        }
        public override int B5 {
            get {
                return (int) XBOX360Button.SELECT;
            }
        }

        public override int B6 {
            get {
                return (int) XBOX360Button.START;
            }
        }

        public override int B7 {
            get {
                return -1;
            }
        }

        public override int LB {
            get {
                return (int) XBOX360Button.LB;
            }
        }

        public override int RB {
            get {
                return (int) XBOX360Button.RB;
            }
        }

        public override int LS {
            get {
                return (int) XBOX360Button.LS;
            }
        }

        public override int RS {
            get {
                return (int) XBOX360Button.RS;
            }
        }

        public override int LSX {
            get {
                return (int) XBOX360Axis.LSX;
            }
        }

        public override int LSY {
            get {
                return (int) XBOX360Axis.LSY;
            }
        }

        public override int RSX {
            get {
                return (int) XBOX360Axis.RSX;
            }
        }

        public override int RSY {
            get {
                return (int) XBOX360Axis.RSY;
            }
        }

        public override int DPADX {
            get {
                return (int) XBOX360Axis.DPADX;
            }
        }

        public override int DPADY {
            get {
                return (int) XBOX360Axis.DPADY;
            }
        }

        public override int LT {
            get {
                return (int) XBOX360Axis.LT;
            }
        }

        public override int RT {
            get {
                return (int) XBOX360Axis.RT;
            }
        }

    }

}