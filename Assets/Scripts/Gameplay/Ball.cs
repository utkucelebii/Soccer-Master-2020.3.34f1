using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        m_EulerAngleVelocity = new Vector3(100, 0, 0);
    }


    void FixedUpdate()
    {
        if (LevelManager.Instance.PowerUP && !transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (!LevelManager.Instance.PowerUP && !transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        if (InputManager.Instance.isSwipe)
        {
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
        }
    }
}
