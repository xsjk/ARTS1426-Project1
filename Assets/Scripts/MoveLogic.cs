using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : MonoBehaviour
{
    const float SPEED = 5.0f;

    Vector3 m_input;

    Vector3 m_movement;

    CharacterController m_characterController;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_input.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_input.x = Input.GetAxis("Horizontal");
        m_input.z = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        m_movement = m_input * SPEED * Time.deltaTime;
        m_characterController.Move(m_movement);
    }

}
