using System;
using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DINO
{ 
    public class RaycastInteraction : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] private LayerMask layerInteractions;
        [SerializeField] private float raycastDistance = 100f;
        [SerializeField] private InputActionReference mousePosition;
        #endregion

        #region private variables
        private Vector2 _senderPosition;
        private bool _isInteracting = false;
        #endregion
        
        #region unity methods
        private void OnEnable()
        {
            mousePosition.action.Enable();
        }

        #endregion

        #region public methods
        public void OnInteraction(InputAction.CallbackContext context)
        {
            _senderPosition = mousePosition.action.ReadValue<Vector2>();
            _isInteracting = context.started;
        
            if (_isInteracting)
            {
                SendRaycastFromMousePos();
            }
        
        }
        #endregion

        #region private methods
        
        private void SendRaycastFromMousePos()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_senderPosition);


            if (Physics.Raycast(ray, out hit, raycastDistance, layerInteractions))
            {
                Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 1f);
                if (hit.collider.gameObject.CompareTag("Torrets"))
                {
                    Cannon cannon = hit.collider.gameObject.GetComponent<Cannon>();
                    cannon.HandleClickCannon();
                    
                }
            }
        }
        #endregion

    }
}