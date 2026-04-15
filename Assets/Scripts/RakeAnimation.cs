// RakeAnimation.cs - attach to the rake object
using UnityEngine;

public class RakeAnimation : MonoBehaviour
{
    private bool isRaking = false;
    private float rakeTimer = 0f;

    public float rakeSpeed = 3f;
    public float rakeSideAngle = 28f;
    public float rakeForwardTilt = 10f;
    public float rakeRollAngle = 6f;
    public float animationResponsiveness = 12f;
    public float returnSpeed = 8f;

    private Quaternion startRot;
    private PlayerToolManager toolManager;

    void Start()
    {
        startRot = transform.localRotation;

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

        bool shouldRake = toolManager != null
            && Input.GetMouseButton(0)
            && toolManager.GetCurrentTool() == "Rake";

        if (shouldRake)
        {
            isRaking = true;
            rakeTimer += Time.deltaTime * rakeSpeed;

            float primary = Mathf.Sin(rakeTimer);
            float secondary = Mathf.Sin(rakeTimer * 2f + Mathf.PI * 0.25f);

            Quaternion targetRot = startRot * Quaternion.Euler(
                -Mathf.Abs(primary) * rakeForwardTilt,
                primary * rakeSideAngle,
                secondary * rakeRollAngle
            );

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRot,
                Time.deltaTime * animationResponsiveness
            );
        }
        else
        {
            isRaking = false;
            rakeTimer = 0f;
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                startRot,
                Time.deltaTime * returnSpeed
            );
        }
    }

    public bool IsRaking()
    {
        return isRaking;
    }
}
