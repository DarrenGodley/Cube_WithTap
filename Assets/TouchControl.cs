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


