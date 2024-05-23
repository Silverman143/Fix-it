using UnityEngine;

namespace FixItGame
{
    public class DragObject : MonoBehaviour
    {
        public LayerMask m_DragLayers;

        [Range(0.0f, 100.0f)]
        public float m_Damping = 1.0f;

        [Range(0.0f, 100.0f)]
        public float m_Frequency = 5.0f;

        public bool m_DrawDragLine = true;
        public Color m_Color = Color.cyan;

        [SerializeField] private bool useOffset;
        [SerializeField] private float _maxOffset = 3.0f;
        [SerializeField] private float _offsetChangingSpeed = 3.0f;

        private TargetJoint2D m_TargetJoint;
        private Vector2 _startPosition;

        private bool _isMobile;


        private void Start()
        {
            useOffset = false;
            _isMobile = SettingsManager.Instance.isMobile;
        }

        void Update()
        {
            if (_isMobile)
            {
                MobileUpdate();
            }
            else
            {
                DesktopUpdate()
;
            }
        }

        private void DesktopUpdate()
        {
            // Calculate the world position for the mouse.
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                // Fetch the first collider.
                // NOTE: We could do this for multiple colliders.
                var collider = Physics2D.OverlapPoint(worldPos, m_DragLayers);
                if (!collider)
                    return;

                // Fetch the collider body.
                var body = collider.attachedRigidbody;
                if (!body)
                    return;

                if (useOffset)
                    _startPosition = worldPos;

                // Add a target joint to the Rigidbody2D GameObject.
                m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D>();
                m_TargetJoint.dampingRatio = m_Damping;
                m_TargetJoint.frequency = m_Frequency;

                // Attach the anchor to the local-point where we clicked.
                m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint(worldPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Destroy(m_TargetJoint);
                m_TargetJoint = null;
                return;
            }

            // Update the joint target.
            if (m_TargetJoint)
            {
                Vector3 targetPosition = worldPos;

                if (useOffset)
                {
                    float yFromStart = _startPosition.y - worldPos.y;
                    yFromStart = Mathf.Abs(yFromStart) * _offsetChangingSpeed;

                    targetPosition.y += Mathf.Clamp(yFromStart, 0, _maxOffset);
                }

                m_TargetJoint.target = targetPosition;

                // Draw the line between the target and the joint anchor.
                if (m_DrawDragLine)
                    Debug.DrawLine(m_TargetJoint.transform.TransformPoint(m_TargetJoint.anchor), worldPos, m_Color);
            }
        }

        private void MobileUpdate()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    // Fetch the first collider.
                    // NOTE: We could do this for multiple colliders.
                    var collider = Physics2D.OverlapPoint(worldPos, m_DragLayers);
                    if (!collider)
                        return;

                    // Fetch the collider body.
                    var body = collider.attachedRigidbody;
                    if (!body)
                        return;

                    if (useOffset)
                        _startPosition = worldPos;

                    // Add a target joint to the Rigidbody2D GameObject.
                    m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D>();
                    m_TargetJoint.dampingRatio = m_Damping;
                    m_TargetJoint.frequency = m_Frequency;

                    // Attach the anchor to the local-point where we clicked.
                    m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint(worldPos);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Destroy(m_TargetJoint);
                    m_TargetJoint = null;
                    return;
                }

                if (m_TargetJoint)
                {
                    Vector3 targetPosition = worldPos;

                    //if (useOffset)
                    //{
                    //    float yFromStart = _startPosition.y - worldPos.y;
                    //    yFromStart = Mathf.Abs(yFromStart) * _offsetChangingSpeed;

                    //    targetPosition.y += Mathf.Clamp(yFromStart, 0, _maxOffset);
                    //}

                    m_TargetJoint.target = targetPosition;

                    // Draw the line between the target and the joint anchor.
                    if (m_DrawDragLine)
                        Debug.DrawLine(m_TargetJoint.transform.TransformPoint(m_TargetJoint.anchor), worldPos, m_Color);
                }
            }
        }
    }
}
