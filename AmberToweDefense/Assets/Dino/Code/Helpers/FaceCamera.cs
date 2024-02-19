using UnityEngine;

namespace DINO
{
    public class FaceCamera : MonoBehaviour
    {

        [SerializeField] private bool update = true;

        private Camera _camera;

        #region unity methods

        private void Start()
        {
            _camera = Camera.main;
            transform.LookAt(_camera.transform, Vector3.up);
        }

        private void Update()
        {
            if (update)
            {
                transform.LookAt(_camera.transform, Vector3.up);
            }
        }

        #endregion

    }
}