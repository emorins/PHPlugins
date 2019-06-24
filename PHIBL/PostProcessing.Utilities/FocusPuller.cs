// Utility scripts for the post processing stack
// https://github.com/keijiro/PostProcessingUtilities

using UnityEngine;

namespace PHIBL.PostProcessing.Utilities
{
    [RequireComponent(typeof(PostProcessingController))]
    public class FocusPuller : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] Transform _target;

        public Transform target {
            get { return _target; }
            set { _target = value; }
        }

        [SerializeField] float _offset = 0;

        public float offset {
            get { return _offset; }
            set { _offset = value; }
        }

        [SerializeField] float _speed = 6f;

        public float speed {
            get { return _speed; }
            set { _speed = Mathf.Max(0.01f, value); }
        }

        #endregion

        #region Private members

        PostProcessingController _controller;
        float _velocity;
        Vector3 lastDoFPoint;
        bool initialFrame = true;
        public GameObject FocusTarget = new GameObject("FocusTarget");
        public LayerMask hitLayer = Camera.main.cullingMask;
        public float maxDistance = Camera.main.farClipPlane;
        #endregion

        #region MonoBehaviour functions

        void Awake()
        {
            //Physics.queriesHitBackfaces = true;
            _controller = GetComponent<PostProcessingController>();
            _target = FocusTarget.transform;            
        }
        void OnEnable()
        {
            initialFrame = true;
            _controller.controlDepthOfField = true;
        } 
        void OnValidate()
        {
            speed = _speed;
        }
        //public void Reset()
        //{
        //    OnEnable();
        //}
        void OnPreRender()
        {
            if (initialFrame)
            {
                initialFrame = false;
                Focus(new Vector3(Screen.width / 2, Screen.height / 2));
            }
            else
                Focus(Input.mousePosition);

            // Retrieve the current value.
            var d1 = _controller.depthOfField.focusDistance;

            // Calculate the depth of the focus point.
            var d2 = Vector3.Dot(_target.position - transform.position, transform.forward);
            if (d2 < 0.1f)
                d2 = 0.1f;
            // Damped-spring interpolation.
            var dt = Time.deltaTime;
            var n1 = _velocity - (d1 - d2) * speed * speed * dt;
            var n2 = 1 + speed * dt;
            _velocity = n1 / (n2 * n2);
            var d = d1 + _velocity * dt;

            // Apply the result.
            _controller.depthOfField.focusDistance = d;            
        }

        #endregion

        void Focus(Vector3 PointOnScreen)
        {
            // our ray
            var ray = transform.GetComponent<Camera>().ScreenPointToRay(PointOnScreen);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitLayer))
            {
                // do we have a new point?					
                if (lastDoFPoint == hit.point)
                    return;
                _target.position = hit.point;
                // asign the last hit
                lastDoFPoint = hit.point;
            }
        }
    }
}
