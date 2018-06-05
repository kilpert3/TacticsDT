using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour
{
    //get the camera to follow the tile selection indicator
    public float speed = 3f;
    public Transform follow;
    Transform _transform;

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        //LERP for smooth approach
        if (follow)
            _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
    }
}
