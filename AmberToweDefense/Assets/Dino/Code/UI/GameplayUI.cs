using System;
using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject currencyPanel;
    [SerializeField] private TMPro.TextMeshProUGUI waveText;

    private GameplayController _gameplayController;
    
    public void StartGame()
    {
        _gameplayController.ChangeGameState(GameState.Playing);
        EnemySpawner.Instance.OnWaveChange += UpdateWaveText;

    }

    private void Start()
    {
        _gameplayController = GameplayController.Instance;
        _gameplayController.OnGameStateChanged += UpdateGameUI;
    }

    private void UpdateGameUI(GameState state)
    { 
        startGameButton.SetActive(!(state == GameState.Playing || state == GameState.GameOver)); 
        currencyPanel.SetActive(state != GameState.GameOver);
    }
    private void UpdateWaveText(int wave)
    {
        waveText.text = "Wave " + wave + " / " + EnemySpawner.Instance.MaxWave;
    }
}
