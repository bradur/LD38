// Date   : 22.04.2017 09:02
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetY;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
    private float originalZ;

    [SerializeField]
    private Transform cameraTransform;
    bool targetSet = false;

    private void Start()
    {
        SetTarget(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        targetSet = true;
        if (newTarget == null)
        {
            targetSet = false;
        }
        m_LastTargetPosition = target.position;
        m_OffsetY = (cameraTransform.position - target.position).y;
        originalZ = cameraTransform.position.z;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (targetSet)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }
            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.up * m_OffsetY;
            Vector3 newPos = Vector3.SmoothDamp(cameraTransform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            newPos.z = originalZ;

            cameraTransform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }

}
