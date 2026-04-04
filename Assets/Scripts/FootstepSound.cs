using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] walkClips;
    public AudioClip[] runClips;

    [Header("Timing Settings")]
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f; // Faster interval for running

    private float stepTimer;

    void Update()
    {
        bool isMoving = IsMoving();
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (isMoving && IsOnSand())
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                // Pass the 'isRunning' state to the play function
                PlayFootstep(isRunning);

                // Set the timer based on movement speed
                stepTimer = isRunning ? runInterval : walkInterval;
            }
        }
        else
        {
            stepTimer = 0f; // Instant start when we begin moving
        }
    }

    bool IsMoving()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

    bool IsOnSand()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 0.6f))
        {
            return hit.collider.CompareTag("Sand");
        }
        return false;
    }

    void PlayFootstep(bool isRunning)
    {
        // Choose the correct array based on movement state
        AudioClip[] currentClips = isRunning ? runClips : walkClips;

        if (currentClips == null || currentClips.Length == 0) return;

        AudioClip clip = currentClips[Random.Range(0, currentClips.Length)];
        
        if (clip == null) return;

        // Make running footsteps slightly higher pitch for "intensity"
        float pitchMin = isRunning ? 1.1f : 0.9f;
        float pitchMax = isRunning ? 1.3f : 1.1f;

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip);
    }
}