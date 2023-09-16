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
    Rigidbody2D _body;
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_body.velocity, transform.up);
        _body.velocity = forwardVelocity * stoppingFactor;
        _body.AddForce(transform.up * speed * vertical * Time.deltaTime * accelerationFactor, ForceMode2D.Force);
        _body.MoveRotation(_body.rotation - horizontal *  Time.deltaTime * turnSpeed);
    }
}
