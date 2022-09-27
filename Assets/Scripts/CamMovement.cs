using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [SerializeField] Camera C_Camera;
    [SerializeField] Rigidbody R_Camera;
    [SerializeField] Transform T_Camera;
    [SerializeField] float Speed;
    Vector3 AxisMovment;

    private void Start()
    {
        T_Camera.position += new Vector3(0, WorldGen.ChunkSize.y + 10, 0);
    }
    void Update()
    {
        AxisMovment = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        R_Camera.velocity = new Vector3(AxisMovment.x * Speed, R_Camera.velocity.y, AxisMovment.z * Speed);
        C_Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel")/5 * 20f * 100f * Time.deltaTime;
        C_Camera.orthographicSize = Mathf.Clamp(C_Camera.orthographicSize, 1, 7);
    }
}
