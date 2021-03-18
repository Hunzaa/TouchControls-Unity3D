using UnityEngine;

public class TouchManagerScript : MonoBehaviour
{
    IControllable selectedObject;
    float tapTime;

    private float starting_distance_to_selected_object;
    private float _startingPosition;

    Quaternion startingOreintation;
    float startingAngle;

    const float pinchRatio = 0;
    const float minPinchDistance = 0;
    static public float pinchDistanceDelta;
    static public float pinchDistance;

    float preiviousDistance;
    float zoomSpeed = 1.0f;

    Ray newPosition;

    public Camera cam;

    bool accel;

    void Start()
    {
        cam = Camera.main;
        accel = false;
    }

    public void myAccelerometer()
    {
        accel = !accel;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Two finger touch 

            // PINCH/ZOOM OBJECT and CAMERA
            if (Input.touchCount == 2)
            {
                if ((Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetTouch(1).phase == TouchPhase.Began))
                {
                    preiviousDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                }
                if ((Input.GetTouch(0).phase == TouchPhase.Moved) || (Input.GetTouch(1).phase == TouchPhase.Moved))
                {
                    float distance;
                    Vector2 touch1 = Input.GetTouch(0).position;
                    Vector2 touch2 = Input.GetTouch(1).position;
                    distance = Vector2.Distance(touch1, touch2);

                    float pinchAmount = (preiviousDistance - distance) * zoomSpeed * Time.deltaTime;
                    if (selectedObject != null)
                    {
                        Debug.Log("PINCHING Object");
                        selectedObject.GetTransform().Translate(0, 0, pinchAmount);
                    }
                    else
                    {
                        Debug.Log("PINCHING Camera");
                        Camera.main.transform.Translate(0, 0, pinchAmount);
                    }
                    preiviousDistance = distance;
                }
            }

            // ROTATE OBJECT and ROTATE CAMERA
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];

                if ((touch2.phase == TouchPhase.Began) || (touch1.phase == TouchPhase.Began))
                {
                    if (selectedObject != null)
                    {
                        startingOreintation = selectedObject.GetTransform().rotation;
                    }
                    else
                    {
                        startingOreintation = Camera.main.transform.rotation;
                    }
                    startingAngle = Mathf.Atan2(touch2.position.y - touch1.position.y, touch2.position.x - touch1.position.x);
                }

                if ((touch1.phase == TouchPhase.Moved) || (touch2.phase == TouchPhase.Moved))
                {
                    float currentAngle = Mathf.Atan2(touch2.position.y - touch1.position.y, touch2.position.x - touch1.position.x);
                    float angle = Mathf.Rad2Deg * (currentAngle - startingAngle);

                    if (selectedObject != null)
                    {
                        Debug.Log("ROTATING Object");
                        selectedObject.GetTransform().rotation = startingOreintation * Quaternion.AngleAxis(angle, Camera.main.transform.forward);
                    }
                    else
                    {
                        Debug.Log("ROTATING Camera");
                        Camera.main.transform.rotation = startingOreintation * Quaternion.AngleAxis(angle, Camera.main.transform.forward);
                    }

                }
            }

            float timer = Time.time - tapTime;

            // One finger - Camera Drag 
            if (Input.touchCount == 1 && selectedObject == null)
            {
                Touch touch = Input.GetTouch(0);
                cam.transform.Translate(touch.deltaPosition * -0.1f);
            }

            // One finger - Object Drag 
            if (Input.touchCount == 1)
            {
                Ray ourRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                Debug.DrawRay(ourRay.origin, 30 * ourRay.direction);
                RaycastHit info;

                Touch touch = Input.touches[0];

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        tapTime = Time.time;
                        _startingPosition = touch.position.x;
                        break;

                    case TouchPhase.Moved:
                        Ray ourFingerRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                        if (selectedObject != null)
                        {
                            // DRAG 1 - Distance 
                            selectedObject.dragTo(ourFingerRay.GetPoint(starting_distance_to_selected_object));
                            Debug.Log("DRAGGING");
                            // DRAG 3 - Horizontal Plane - not working
                            newPosition = ourFingerRay;
                            selectedObject.MoveTo(ourRay, Input.touches[0], newPosition.GetPoint(starting_distance_to_selected_object));
                        }
                        break;

                    case TouchPhase.Ended:
                        if (Physics.Raycast(ourRay, out info))
                        {
                            IControllable object_hit = info.transform.GetComponent<IControllable>();

                            if (object_hit != null)
                            {
                                // TAP
                                if (timer < 0.2f)
                                {
                                    object_hit.youve_been_tapped();
                                    Debug.Log("You've been TAPPED");

                                    // Checks if an object is already selected and clears the selection it is 
                                    if (selectedObject != null)
                                    {
                                        if (object_hit == selectedObject)
                                            return;
                                        Deselect();
                                    }

                                    selectedObject = object_hit;

                                    // Changes the color of selected object to red 
                                    Renderer ourRenderer = selectedObject.GetComponent<Renderer>();
                                    Material m = ourRenderer.material;
                                    m.color = Color.red;
                                    ourRenderer.material = m;

                                    starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);

                                }

                                // TOUCH
                                else if (timer > 0.2f)
                                {
                                    object_hit.youve_been_touched();
                                    Debug.Log("You've been TOUCHED");
                                }
                            }
                        }
                        break;
                }
            }

            // ACCELEMOTER - not working
            if (Input.touchCount == 0 && selectedObject != null && accel == true)
            {
                Vector3 dir = Vector3.zero;
                dir.x = Input.acceleration.x;
                dir.z = Input.acceleration.y;
                if (dir.sqrMagnitude > 1)
                {
                    dir.Normalize();
                }
                dir *= Time.deltaTime;
                selectedObject.Accellemoter(dir);
            }
        }
    }

    // Clears object selection and changes the color back to default
    public void Deselect()
    {
        if (selectedObject == null)
            return;

        Renderer ourRenderer = selectedObject.GetComponent<Renderer>();
        Material m = ourRenderer.material;
        m.color = default;
        ourRenderer.material = m;
    }


} // END

