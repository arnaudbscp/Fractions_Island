using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehavior : MonoBehaviour
{
    //Fait le lien entre les données brutes et Unity.
    [SerializeField]
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    public int Score => _playerData.Score;
    public int Game => _playerData.Game;
    public int Level => _playerData.Level;
    //État actuek

    public UnityEvent OnPlayerUpdated = new UnityEvent();

    public void SetGame(int game)
    {
        if (game != Game)
        {
            _playerData.Game = game;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetLevel(int level)
    {
        if (level != Level)
        {
            _playerData.Level = level;
            OnPlayerUpdated.Invoke();
        }
    }

    public void SetScore(int score)
    {
        if (score != Score)
        {
            _playerData.Score = score;
            OnPlayerUpdated.Invoke();
        }
    }

    public void Set(int game, int level)
    {
        if (level != Level || game != Game)
        {
            _playerData.Level = level;
            _playerData.Game = game;
            OnPlayerUpdated.Invoke();
        }
    }

    public int GetGame()
    {
        return _playerData.Game;
    }

    public int GetLevel()
    {
        return _playerData.Level;
    }

    public int GetScore()
    {
        return _playerData.Score;
    }

    /// <summary>
    /// Met a jour l'état actuel
    /// </summary>
    /// <param name="playerData">Player data.</param>
    public void UpdatePlayer(PlayerData playerData)
    {
        if (!playerData.Equals(_playerData))
        {
            _playerData = playerData;
            OnPlayerUpdated.Invoke();
        }
    }
}
