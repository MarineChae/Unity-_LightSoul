using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed = 5.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private FollowCamera followCamera;
    private Rigidbody      playerRigidbody;
    private PlayerCharacter character;
    private Vector2        moveInput;
    private Animator       animator;

    private bool isMove;
    private bool canMove = true;
    private RaycastHit hit;
    private int stepLayer;
    private float maxSlopeAngle = 45.0f;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    void Start()
    {
        character = GetComponent<PlayerCharacter>();
        stepLayer = 1 << LayerMask.NameToLayer("Step");
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        followCamera.CamTarget.position = playerRigidbody.position + followCamera.OffSet;
        if(canMove)
        {
            isMove = moveInput.magnitude != 0;
            animator.SetBool("Walk", isMove);
 
            Vector3 lookForward = new Vector3(followCamera.CamTarget.forward.x, 0.0f, followCamera.CamTarget.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCamera.CamTarget.right.x, 0.0f, followCamera.CamTarget.right.z).normalized;
            Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;
            if (isMove)
            {
                var look = Quaternion.LookRotation(moveDirection);
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, look, Time.fixedDeltaTime * rotSpeed);

            }
            //���� �Ǻ�
            bool onslope = false;
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.black);
            if (Physics.Raycast(ray, out hit, 1.5f, stepLayer))
            {
                //upVector�� ������ normal���� �̿��Ͽ� �����Ǻ�
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                onslope = angle < maxSlopeAngle;
            }

            Vector3 gravity = onslope ? Vector3.zero : Vector3.down * Mathf.Abs(playerRigidbody.velocity.y);

            if (onslope)
            {
                Vector3 dir = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;

                playerRigidbody.velocity = dir * MoveSpeed + gravity;
            }
            else
            {
                playerRigidbody.velocity = moveDirection * MoveSpeed + gravity;
            }

            if (character.IsRoll) 
            {
                if (onslope)
                {
                    Vector3 dir = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
                    Debug.Log(gravity);
                    playerRigidbody.velocity = dir * 7.0f + gravity;
                }
                else
                {
                    playerRigidbody.velocity = transform.forward * 7.0f + gravity;
                }
            }
        }

    }

    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            moveSpeed = 5.0f;
        }
        if (value.canceled)
        {
            moveSpeed = 2.0f;
        }
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        if (input != null)
        {
            moveInput = new Vector2(input.x,input.y);
        }

    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mousePosition = value.ReadValue<Vector2>();
        followCamera.CamLook = mousePosition;
    }

    public void StopMovement()
    {
        canMove = false;
    }
    public void AllowMovement()
    {
        canMove = true;
    }
    public void RotateToTarget(Vector3 target)
    {

        playerRigidbody.rotation = Quaternion.LookRotation(target - playerRigidbody.position);
    }

}
