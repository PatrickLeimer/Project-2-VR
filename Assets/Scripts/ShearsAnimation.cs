using UnityEngine;

public class ShearsAnimation : MonoBehaviour
{
    [Header("Blades")]
    [SerializeField] private Transform bladeL;
    [SerializeField] private Transform bladeR;
    [SerializeField] private Vector3 bladeRotationAxis = Vector3.up;

    [Header("Cut Motion")]
    public float cutSpeed = 6f;
    public float maxOpenAngle = 25f;
    public float animationResponsiveness = 15f;
    public float returnSpeed = 8f;

    [Header("Effects")]
    [SerializeField] private AudioSource cutSound;
    [SerializeField] private ParticleSystem cutParticles;

    [Header("Score")]
    public int pointsPerCut = 10;
    public static int Score { get; private set; }

    private bool isCutting = false;
    private float cutTimer = 0f;
    private bool wasClosedLastFrame = false;

    private Quaternion bladeLStartRot;
    private Quaternion bladeRStartRot;
    private PlayerToolManager toolManager;

    void Start()
    {
        if (bladeL != null) bladeLStartRot = bladeL.localRotation;
        if (bladeR != null) bladeRStartRot = bladeR.localRotation;

        if (toolManager == null)
        {
            toolManager = FindObjectOfType<PlayerToolManager>();
        }
    }

    void Update()
    {
        if (toolManager == null)
        {
            toolManager = FindObjectOfType<PlayerToolManager>();
        }

        bool shouldCut = toolManager != null
            && Input.GetMouseButton(0)
            && toolManager.GetCurrentTool() == "Shears";

        if (shouldCut)
        {
            isCutting = true;
            cutTimer += Time.deltaTime * cutSpeed;

            // |sin| gives a 0 -> 1 -> 0 motion that snips twice per cycle
            float t = Mathf.Abs(Mathf.Sin(cutTimer));
            float angle = t * maxOpenAngle;

            if (bladeL != null)
            {
                Quaternion targetL = bladeLStartRot * Quaternion.AngleAxis(angle, bladeRotationAxis);
                bladeL.localRotation = Quaternion.Slerp(
                    bladeL.localRotation,
                    targetL,
                    Time.deltaTime * animationResponsiveness
                );
            }

            if (bladeR != null)
            {
                Quaternion targetR = bladeRStartRot * Quaternion.AngleAxis(-angle, bladeRotationAxis);
                bladeR.localRotation = Quaternion.Slerp(
                    bladeR.localRotation,
                    targetR,
                    Time.deltaTime * animationResponsiveness
                );
            }

            // trigger snip on the frame the blades reach closed position
            bool isClosedNow = t < 0.1f;
            if (isClosedNow && !wasClosedLastFrame && cutTimer > 0.2f)
            {
                TriggerSnip();
            }
            wasClosedLastFrame = isClosedNow;
        }
        else
        {
            isCutting = false;
            cutTimer = 0f;
            wasClosedLastFrame = false;

            if (bladeL != null)
            {
                bladeL.localRotation = Quaternion.Slerp(
                    bladeL.localRotation,
                    bladeLStartRot,
                    Time.deltaTime * returnSpeed
                );
            }
            if (bladeR != null)
            {
                bladeR.localRotation = Quaternion.Slerp(
                    bladeR.localRotation,
                    bladeRStartRot,
                    Time.deltaTime * returnSpeed
                );
            }
        }
    }

    void TriggerSnip()
    {
        if (cutSound != null) cutSound.Play();
        if (cutParticles != null) cutParticles.Play();
        Score += pointsPerCut;
        Debug.Log($"Snip! Score: {Score}");
    }

    public bool IsCutting()
    {
        return isCutting;
    }
}