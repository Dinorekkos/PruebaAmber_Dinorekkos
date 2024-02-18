using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DINO
{
    public class GameplayController : MonoBehaviour
    {
        
        #region public variables

        public static GameplayController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameplayController>();
                }

                return _instance;
            }
        }

        #endregion

        private static GameplayController _instance;

        void Start()
        {
            _instance = this;
        }


    }
    
    public enum GameState
    {
        None,
        Countdown,
        Playing,
        GameOver
    }
}