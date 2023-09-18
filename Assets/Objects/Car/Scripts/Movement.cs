using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float weight = 10;
    public float speed = 2;
    public float accelerationFactor = 0.5f;
    public float turnSpeed = 2;
    public float stoppingFactor = 0.95f;
    public Vector2 moveDirection = Vector2.zero;
    Rigidbody2D _body;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection =  Vector2.Lerp(moveDirection, transform.up * vertical, turnSpeed);
        _body.AddForce(moveDirection * speed * Time.deltaTime, ForceMode2D.Force);
        _body.velocity *= stoppingFactor;
        transform.Rotate(0, 0, -horizontal * turnSpeed * Time.deltaTime * Vector2.Dot(transform.up, moveDirection));
    }
}
