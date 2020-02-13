﻿using System;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    [SerializeField]
    private bool bounce = false;
    private Material _material;
    private int _direction = 1;
    private Vector3 drag_destination;

    public bool GetBounce()
    {
        return bounce;
    }

    private void Start()
    {
        drag_destination = transform.position;
        if (GetComponent<Renderer>() && GetComponent<Renderer>().material)
        {
            _material = GetComponent<Renderer>().material;
        }
    }


    private void Update()
    {
        if (Vector3.Distance(transform.position, drag_destination) > 0.01f)
            transform.position = Vector3.Lerp(transform.position, drag_destination, 0.1f);

    }

    public void BounceCube()
    {
        gameObject.transform.position += Vector3.up * _direction;

        if (gameObject.transform.position.y >= 6.0f)
        {
            _direction = -1;
        }

        else if (gameObject.transform.position.y <= -4.0f)
        {
            _direction = 1;
        }
    }

    public void ChangeColour(Color newColour)
    {
        _material.color = newColour;
    }

    internal void move(Vector3 offset)
    {
        transform.position += offset;
    }

    internal void latestDragPosition(Vector3 desired_destination)
    {
        drag_destination = desired_destination;
    }
}