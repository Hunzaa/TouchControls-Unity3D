// Accelemoter script that works when attached to an object 

using UnityEngine;

public class AccellerometerControl : MonoBehaviour
{
    private Rigidbody rigid;
    public bool isFlat = true;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 tilt = Input.acceleration;
        if (isFlat)
            tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rigid.AddForce(tilt);
        Debug.DrawRay(transform.position + Vector3.up, tilt, Color.cyan);

    }
}
