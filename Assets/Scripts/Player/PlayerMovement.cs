using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("The player's rigidbody used for movement and physics")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Parameters")]
    [Tooltip("The multiplier for adjusting movement speed")]
    [SerializeField] private float movementMultiplier = 5;
    [Tooltip("The layers considered ground for raycasting")]
    [SerializeField] private LayerMask groundLayers;

    private Vector3 playerMovementVector;
    private Vector3 groundNormal;
    private RaycastHit[] groundHit = new RaycastHit[1];

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        UpdateGroundNormal();
    }

    private void Initialize()
    {
        if (PlayerInputHandler.Instance != null)
        {
            PlayerInputHandler.Instance.MovementAction += ReadPlayerMovement;
            if (rb == null)
            { 
                if(TryGetComponent(out rb)) Debug.Log("No rigidbody assigned, found rigidbody on script");
                else Debug.LogError("No rigidbody found for player movement");
            }
        }
        else StartCoroutine(WaitForInputHandler());
    }

    private IEnumerator WaitForInputHandler()
    {
        yield return new WaitUntil(() => PlayerInputHandler.Instance != null);
        Initialize();
    }

    private void MovePlayer()
    {
        rb.AddForce(Vector3.ProjectOnPlane(playerMovementVector, groundNormal).normalized * movementMultiplier);
    }

    public void UpdateGroundNormal()
    {
        if(Physics.RaycastNonAlloc(transform.position, transform.up * -1, groundHit, 5.0f, groundLayers.value) > 0)
        {
            groundNormal = groundHit[0].normal;
        }
    }
    public void UpdateGroundNormal(Vector3 normal)
    {
        groundNormal = normal;
    }

    public void ReadPlayerMovement(Vector2 movement)
    {
        playerMovementVector.x = movement.x;
        playerMovementVector.z = movement.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.ProjectOnPlane(playerMovementVector, groundNormal).normalized * movementMultiplier);
    }
}