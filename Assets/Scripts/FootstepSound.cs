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

    [Header("Timing")]
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private float stepTimer;

    void Update()
    {
        bool isMoving = IsMoving();
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            // Detect which surface is beneath KyleRobot
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
            stepTimer = 0f; // Reset so the next step is instant
        }
    }

    string GetSurfaceType()
    {
        RaycastHit hit;
        // Start ray slightly above pivot to avoid missing the floor
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 1.0f))
        {
            if (hit.collider.CompareTag("Sand")) return "Sand";
            if (hit.collider.CompareTag("Terrain")) return "Terrain";
        }

        return "None";
    }

    void PlayFootstep(string surface, bool isRunning)
    {
        AudioClip[] clips = null;

        // Pick the right array based on tag and speed
        if (surface == "Sand")
        {
            clips = isRunning ? sandRun : sandWalk;
        }
        else if (surface == "Terrain")
        {
            clips = isRunning ? terrainRun : terrainWalk;
        }

        if (clips == null || clips.Length == 0) return;

        // Pitch shift for variety
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