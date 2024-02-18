using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private Vector3[] path;
    [SerializeField] private int [] rotationWaypoints;
    
    
    #endregion
    

    #region public variables

    public static PathManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PathManager>();
            }

            return _instance;
        }
    }
    
    public Vector3[] Path
    {
        get => path;
    }

    
    #endregion

    #region private variables

    private static PathManager _instance;

    #endregion
    
    #region unity methods
    
    void Start()
    {
        _instance = this;
    }
    
    #endregion

    public int GetRotationWaypoint(int index)
    {
        if (index >= rotationWaypoints.Length)
        {
            return 0;
        }
        return rotationWaypoints[index];
    }
}
