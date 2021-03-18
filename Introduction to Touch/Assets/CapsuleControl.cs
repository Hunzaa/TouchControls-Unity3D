using UnityEngine;

public class CapsuleControl : MonoBehaviour, IControllable
{
    Renderer ourRenderer;
    Vector3 newPosition;
    float speed = 10f;

    void Start()
    {
        ourRenderer = GetComponent<Renderer>();
        ourRenderer.material.color = Color.black;
    }

    void Update()
    {
    }

    public void youve_been_touched()
    {
        //transform.position += Vector3.right;
    }

    public void youve_been_tapped()
    {
        //transform.position += Vector3.up;
        //ourRenderer.material.color = Color.green;

    }


    public void dragTo(Vector3 new_position)
    {
        transform.position = new_position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void GyroRotate(Vector3 acc)
    {
    }

    public void MoveTo(Ray ourRay, Touch touch, Vector3 vector3)
    {
        newPosition = vector3;
        transform.position = Vector3.Lerp(transform.position, newPosition, 1f);

    }

    public void Accellemoter(Vector3 dir)
    {
        transform.Translate(dir * speed);
    }
}
