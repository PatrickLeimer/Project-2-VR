using UnityEngine;

public class ToolPickup : MonoBehaviour
{
    public string toolName;
    public Transform holdPosition;
    private bool isPickedUp = false;
    private bool playerInRange = false;
    private Transform playerTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isPickedUp)
        {
            if (!IsClosestPickup()) return;
            PickUp();
        }
        else if (isPickedUp && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    bool IsClosestPickup()
    {
        if (playerTransform == null) return true;
        float myDist = Vector3.Distance(transform.position, playerTransform.position);
        foreach (var other in FindObjectsOfType<ToolPickup>())
        {
            if (other == this || !other.playerInRange || other.isPickedUp) continue;
            if (Vector3.Distance(other.transform.position, playerTransform.position) < myDist) return false;
        }
        return true;
    }

    void PickUp()
    {
        isPickedUp = true;
        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Collider>().enabled = false;
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        FindObjectOfType<PlayerToolManager>().EquipTool(toolName);
    }

    void Drop()
    {
        isPickedUp = false;
        transform.SetParent(null);

        // Return to original spot instead of dropping with physics
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        GetComponent<Collider>().enabled = true;
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true; // keep kinematic so it doesn't fly

        FindObjectOfType<PlayerToolManager>().UnequipTool();

        playerInRange = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}