// Gyroscope script that works when attached to an object 
using System.Collections;
using UnityEngine;

public class GyroscopeControl : MonoBehaviour
{
    private Quaternion Rotation_Origin;
    private Gyroscope Gyroscope_Reference;

    private void Awake()
    {
        Gyroscope_Reference = Input.gyro;
        Gyroscope_Reference.enabled = true;
        StartCoroutine(Coroutine_Method());
    }

    private IEnumerator Coroutine_Method()
    {
        yield return null;
        Quaternion Rotation_Origin_Addend = Quaternion.Euler(0, 0, 180);
        Rotation_Origin = Gyroscope_Reference.attitude * Rotation_Origin_Addend;

        Quaternion Gyroscope_Attitude_Difference_Addend = Quaternion.Euler(180, 180, 0);

        while (true)
        {
            Quaternion Gyroscope_Attitude_Difference = Quaternion.Inverse(Rotation_Origin) * Gyroscope_Reference.attitude;
            Gyroscope_Attitude_Difference *= Gyroscope_Attitude_Difference_Addend;

            Quaternion Lerped_Quaternion = Quaternion.Lerp(transform.rotation, Gyroscope_Attitude_Difference, 8 * Time.deltaTime);
            transform.rotation = Lerped_Quaternion;

            yield return null;
        }
    }
}