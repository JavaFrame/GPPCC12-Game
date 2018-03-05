using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsPlayer : Player {
    public float speed = 5f;
    public float lookSensitivity = 3f;
    public float viewRange = 60f;


    //jumping stuff
    public float jumpForce = 2.5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultipler = 2f;
    public int maxJumpCounter = 1;
    public int jumpCounter = 0;
    private float distToGround;


    public override void GeneralUpdate()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumpCounter)
        {
            jumpCounter++;
            //rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            rigidbody.AddForce(Vector3.up * jumpForce);
        }

        if (IsOnGround())
        {
            jumpCounter = 0;
        }
    }

    public override void UpdateRotation()
    {
        var yRot = Input.GetAxisRaw("Mouse X");
        var rotation = new Vector3(0, yRot, 0) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));
    }

    public override void UpdateCameraRotation()
    {
        var xRot = Input.GetAxisRaw("Mouse Y");
        var cameraRotation = new Vector3(xRot, 0, 0) * lookSensitivity;
        if (camera != null)
        {
            camera.transform.Rotate(-cameraRotation);
            camera.transform.localEulerAngles =
                new Vector3(ClampAngle(camera.transform.localEulerAngles.x, -viewRange, viewRange), 0, 0);
        }

    }

    public override void UpdateMovement()
    {
        var xMov = Input.GetAxisRaw("Horizontal") * transform.right;
        var zMov = Input.GetAxisRaw("Vertical") * transform.forward;

        var velocity = (xMov + zMov).normalized * speed;
        if (velocity != Vector3.zero)
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

}
