using UnityEngine;
using System.Collections;

// rotates particles 
public class ParticleRotator : MonoBehaviour
{
    // reference to vector to rotate transform per update
    private static Vector3 rotateVector = new Vector3(0, 0, 90);

    void Update()
    {
        transform.Rotate(rotateVector * Time.deltaTime);
    }
}
