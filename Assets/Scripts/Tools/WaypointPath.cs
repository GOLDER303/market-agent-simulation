using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public int ControlPointsCount
    {
        get => controlPoints.Length;
    }

    [SerializeField]
    private Vector3[] controlPoints = null;

    [SerializeField]
    private Color linesColor = Color.white;

    private void OnDrawGizmos()
    {
        if (controlPoints.Length == 0)
        {
            return;
        }

        Vector3? previousPoint = null;

        for (int i = 0; i < controlPoints.Length; i++)
        {
            Vector3 point = GetControlPoint(i);

            Gizmos.DrawWireSphere(point, .1f);

            if (previousPoint != null)
            {
                Color prevGizmosColor = Gizmos.color;

                Gizmos.color = linesColor;
                Gizmos.DrawLine(previousPoint.Value, point);

                Gizmos.color = prevGizmosColor;
            }

            previousPoint = point;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return transform.TransformPoint(controlPoints[index]);
    }

    public void SetControlPoint(int index, Vector3 position)
    {
        controlPoints[index] = transform.InverseTransformPoint(position);
    }
}
