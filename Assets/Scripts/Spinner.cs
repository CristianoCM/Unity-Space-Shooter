using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
