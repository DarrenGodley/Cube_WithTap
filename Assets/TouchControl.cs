using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour


{
    float timer;
    float MAX_TAP_TIME = 0.2f;
    bool has_moved = false;

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
                object_hit.ChangeColour(Color.magenta);
            }

        }
    }
}


