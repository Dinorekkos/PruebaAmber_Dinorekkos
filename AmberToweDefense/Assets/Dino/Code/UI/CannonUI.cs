using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonUI : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private GameObject iconBuild;
    [SerializeField] private GameObject iconUpgrade;
    [SerializeField] private Cannon cannonController;
    
    #endregion
    void Start()
    {
        cannonController.OnCannonStateChange += UpdateCannonUI;
        iconUpgrade.SetActive(false);
    }

    private void UpdateCannonUI(CannonState state)
    {
        switch (state)
        {
            case CannonState.UnBuild:
                iconBuild.SetActive(true);
                break;
            case CannonState.Idle:
                iconBuild.SetActive(false);
                break;
            case CannonState.Shooting:
                iconBuild.SetActive(false);
                break;
        }
    }
    
}
