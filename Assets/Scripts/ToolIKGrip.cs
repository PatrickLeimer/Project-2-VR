// ToolIKGrip.cs — attach to JapaneseFarmer
using UnityEngine;

public class ToolIKGrip : MonoBehaviour
{
    public Transform rightHandTarget;  // empty on rake handle (top)
    public Transform leftHandTarget;   // empty on rake handle (bottom)
    private Animator animator;
    private PlayerToolManager toolMgr;
    private float ikWeight = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        toolMgr = GetComponent<PlayerToolManager>();
    }

    void Update()
    {
        // Smoothly blend IK on/off when holding a tool
        float target = (toolMgr != null && toolMgr.currentTool != "") ? 1f : 0f;
        ikWeight = Mathf.Lerp(ikWeight, target, Time.deltaTime * 8f);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // Right hand
        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }

        // Left hand
        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
}