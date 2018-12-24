using UnityEngine;

public class QuaternionMultiply : MonoBehaviour
{
    void Start()
    {
        var q1 = Quaternion.Euler(30, 0, 0);
        var q2 = Quaternion.Euler(0, 30, 0);
        var q3 = Quaternion.Euler(0, 0, 30);

        // 按照q1，q2，q3的序列进行旋转
        transform.rotation = q3 * q2 * q1 * transform.rotation;
    }
}
