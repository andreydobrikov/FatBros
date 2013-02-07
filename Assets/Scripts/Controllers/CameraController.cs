using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float delta_X = 0f, delta_Y = 16f, delta_Z = 0f;
    private Transform target = null;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        //this.transform.LookAt(target);
        this.transform.localPosition = new Vector3(target.position.x + delta_X, target.position.y + delta_Y, target.position.z + delta_Z);
    }
}
