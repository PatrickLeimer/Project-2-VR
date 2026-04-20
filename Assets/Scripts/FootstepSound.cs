using UnityEngine;
public class FootstepSound : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sand Sounds (Tagged 'Sand')")]
    public AudioClip[] sandWalk;
    public AudioClip[] sandRun;

    [Header("Terrain Sounds (Tagged 'Terrain')")]
    public AudioClip[] terrainWalk;
    public AudioClip[] terrainRun;

    [Header("Bridge Sounds (Tagged 'Bridge')")]
    public AudioClip[] bridgeWalk;
    public AudioClip[] bridgeRun;

    [Header("Water Sounds (Tagged 'Water')")]
    public AudioClip[] waterWalk;
    public AudioClip[] waterRun;

    [Header("Wood Sounds (Tagged 'Wood')")]  // ← Added
    public AudioClip[] woodWalk;              // ← Added
    public AudioClip[] woodRun;              // ← Added

    [Header("Timing")]
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private float stepTimer;
    private bool inWater = false;

    void Update()
    {
        bool isMoving = IsMoving();
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            string surface = GetSurfaceType();
            if (surface != "None")
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    PlayFootstep(surface, isRunning);
                    stepTimer = isRunning ? runInterval : walkInterval;
                }
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water")) inWater = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water")) inWater = false;
    }

    string GetSurfaceType()
    {
        if (inWater) return "Water";

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 1.0f))
        {
            if (hit.collider.CompareTag("Sand"))    return "Sand";
            if (hit.collider.CompareTag("Terrain")) return "Terrain";
            if (hit.collider.CompareTag("Bridge"))  return "Bridge";
            if (hit.collider.CompareTag("Wood"))    return "Wood";  // ← Added
        }
        return "None";
    }

    void PlayFootstep(string surface, bool isRunning)
    {
        AudioClip[] clips = null;

        if (surface == "Sand")
        {
            clips = isRunning ? sandRun : sandWalk;
        }
        else if (surface == "Terrain")
        {
            clips = isRunning ? terrainRun : terrainWalk;
        }
        else if (surface == "Bridge")
        {
            clips = isRunning ? bridgeRun : bridgeWalk;
        }
        else if (surface == "Water")
        {
            clips = isRunning ? waterRun : waterWalk;
        }
        else if (surface == "Wood")                   // ← Added
        {
            clips = isRunning ? woodRun : woodWalk;   // ← Added
        }

        if (clips == null || clips.Length == 0) return;

        audioSource.pitch = isRunning ? Random.Range(1.1f, 1.3f) : Random.Range(0.9f, 1.1f);
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    bool IsMoving()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }
}