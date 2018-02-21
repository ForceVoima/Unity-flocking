using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    public AgentController _controller;
    public int neighboursFound = 0;

    public void AddController(AgentController controller)
    {
        _controller = controller;
    }

    Vector3 GetSeparationVector(Transform target)
    {
        Vector3 diff = transform.position - target.transform.position;
        float diffLen = diff.magnitude;
        float scaler = Mathf.Clamp01(1.0f - diffLen / _controller._agentDistance);
        return diff * (scaler / diffLen);
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        float velocity = _controller.Velocity;

        Vector3 separation = Vector3.zero;
        Vector3 alignment = transform.forward;
        Vector3 cohesion = _controller.transform.position;

        Collider[] neighbours = Physics.OverlapSphere(currentPos, _controller._agentDistance, _controller._agentMask);
        neighboursFound = neighbours.Length - 1;

        foreach (Collider agent in neighbours)
        {
            if (agent.gameObject == this.gameObject)
                continue;

            separation += GetSeparationVector(agent.transform);
            alignment += agent.transform.forward;
            cohesion += agent.transform.position;
        }

        float average = 1.0f / (1.0f * neighbours.Length);

        alignment *= average;
        cohesion *= average;
        cohesion = (cohesion - currentPos).normalized;

        Vector3 newDirection = separation + alignment + cohesion;
        Quaternion newRotation = Quaternion.FromToRotation(Vector3.forward, newDirection.normalized);

        if (currentRot != newRotation)
        {
            transform.rotation = Quaternion.Slerp(currentRot, newRotation, Mathf.Exp(-4.0f * Time.deltaTime));
        }

        // Finally move
        transform.position += transform.forward * (_controller.Velocity * Time.deltaTime);

        Debug.DrawLine(currentPos, (currentPos + transform.forward * 3f), Color.red);
    }
}
