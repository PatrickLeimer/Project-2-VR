// BonsaiTrimming.cs — attach to each bonsai tree
using UnityEngine;

public class BonsaiTrimming : MonoBehaviour
{
    public ParticleSystem leafParticles; // assign leaf burst effect
    public AudioSource snipSound;       // assign clip sound
    public GameObject[] growthStages;   // 3 meshes: overgrown, trimmed, perfect
    private int currentStage = 0;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetMouseButtonDown(0))
        {
            var toolMgr = FindObjectOfType<PlayerToolManager>();
            if (toolMgr != null && toolMgr.currentTool == "Scissors")
            {
                Trim();
            }
        }
    }

    void Trim()
    {
        if (currentStage >= growthStages.Length - 1) return;

        // Play effects
        snipSound.Play();
        leafParticles.Play();

        // Hide current, show next stage
        growthStages[currentStage].SetActive(false);
        currentStage++;
        growthStages[currentStage].SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}