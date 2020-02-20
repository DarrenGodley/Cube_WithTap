using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    private float drag_distance;
    float timer;
    float MAX_TAP_TIME = 0.2f;
    bool has_moved = false;
    private float startingFingerAngle;
    private Quaternion startingobjectAngle;
    private float startingDistance;
    private Vector3 startingScale;
    private float newFingerDestination;
    public float speed = 0.1f;


    Controllable currently_selected_object;
    Camera my_camera = new Camera();
    // Start is called before the first frame update
    void Start()
    {
        my_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            bool A = Input.GetKey(KeyCode.Space);
            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];

            if ((touch1.phase == TouchPhase.Began) || (touch2.phase == TouchPhase.Began))
            {
                Vector2 diff = touch2.position - touch1.position;
                startingFingerAngle = Mathf.Atan2(diff.y, diff.x);
                startingobjectAngle = currently_selected_object.transform.rotation;
                startingDistance = Vector2.Distance(touch1.position, touch2.position);
                startingScale = currently_selected_object.transform.localScale;
            }

            if (currently_selected_object)
            {
                Vector2 diff = touch2.position - touch1.position;

                newFingerDestination = Mathf.Atan2(diff.y, diff.x);
                currently_selected_object.transform.rotation = startingobjectAngle * Quaternion.AngleAxis(Mathf.Rad2Deg * (newFingerDestination - startingFingerAngle), my_camera.transform.forward);
                currently_selected_object.transform.localScale = (Vector2.Distance(touch1.position, touch2.position) / startingDistance) * startingScale;
            }
        }

        else 

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            timer += Time.deltaTime;

            if (touch.phase == TouchPhase.Began)
            {
                if (currently_selected_object)
                    drag_distance = Vector3.Distance(currently_selected_object.transform.position, my_camera.transform.position);
                timer = 0f;
                has_moved = false;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                has_moved = true;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if ((timer < MAX_TAP_TIME) && !has_moved)
                    implement_tap(touch);
            }

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                if (currently_selected_object)
                {
                    Ray drag_ray = my_camera.ScreenPointToRay(touch.position);
                    Vector3 new_destination = drag_ray.GetPoint(drag_distance);
                    currently_selected_object.latestDragPosition(new_destination);
                }
            }
        }

        if (!currently_selected_object)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime, -touchDeltaPosition.y * speed * Time.deltaTime, 0);
            }
        }
    }

    private void implement_tap(Touch touch)
    {
        Ray our_ray = my_camera.ScreenPointToRay(touch.position);

        RaycastHit info;

        if (Physics.Raycast(our_ray, out info))
        {
            Controllable object_hit = info.transform.GetComponent<Controllable>();

            if (object_hit)
            {
                if (currently_selected_object)
                {
                    currently_selected_object.ChangeColour(Color.white);
                }
                object_hit.ChangeColour(Color.magenta);
                currently_selected_object = object_hit;
            }
        }
    }
}


