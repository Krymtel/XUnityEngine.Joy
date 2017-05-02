using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XUnityEngine.Joystick;
using System;

public class JoystickTester : MonoBehaviour {

    public float speed = 4.0f;
    public Color color = Color.white;

    private JoystickListener joy;
    private SpriteRenderer sprite;

    private Vector3 velocity;
    private Vector3 rotation;

    private void Awake () {
        joy = GetComponent<JoystickListener> ();
        sprite = transform.Find ("Sprite").GetComponent<SpriteRenderer> ();
        sprite.color = color;
        sprite.sortingOrder = JoystickManager.MAX_JOYSTICKS - joy.playerID;
    }

    private void Update () {
        if (joy.GetButtonDown (JoystickButton.B4))
            print ("ayy lmao");
        velocity.x = joy.GetAxis (JoystickAxis.LSX);
        velocity.y = joy.GetAxis (JoystickAxis.LSY);
        rotation.x = joy.GetAxis (JoystickAxis.RSX);
        rotation.y = joy.GetAxis (JoystickAxis.RSY);
        transform.position += velocity * (speed + speed * joy.GetAxis (JoystickAxis.RT)) * Time.deltaTime;
        if (rotation.magnitude > Joystick.DEADZONE)
            transform.rotation = Quaternion.AngleAxis (Mathf.Atan2 (rotation.y, rotation.x) * Mathf.Rad2Deg, Vector3.forward);
    }

}
