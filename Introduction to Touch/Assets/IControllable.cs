using UnityEngine;

interface IControllable
{
    void youve_been_touched();

    void youve_been_tapped();

    void dragTo(Vector3 new_position);
    Transform GetTransform();
    T GetComponent<T>();
    void Accellemoter(Vector3 dir);
    void MoveTo(Ray ourRay, Touch touch, Vector3 vector3);
}
