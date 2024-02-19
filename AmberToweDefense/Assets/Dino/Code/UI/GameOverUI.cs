using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    void Start()
    {
        GameplayController.Instance.OnGameStateChanged += UpdateGameOverUI;
        UpdateGameOverUI(GameplayController.Instance.CurrentGameState);
    }

    private void UpdateGameOverUI(GameState obj)
    {
        gameOverPanel.SetActive(obj == GameState.GameOver);
    }
    
   

}
