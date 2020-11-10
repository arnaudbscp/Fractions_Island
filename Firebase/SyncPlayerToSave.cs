using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerToSave : MonoBehaviour
{
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private PlayerBehavior _player;

    private void Reset()
    {
        _playerSaveManager = FindObjectOfType<PlayerSaveManager>();
    }

    private void Start()
    {
        _playerSaveManager.OnPlayerUpdated.AddListener(HandlePlayerSaveUpdated); //Listener pour quand les données enregistrées sont mises à jour
        _player.OnPlayerUpdated.AddListener(HandlePlayerUpdated);
        _player.UpdatePlayer(_playerSaveManager.LastPlayerData);//On définit les données sur les données du cache
    }

    private void HandlePlayerUpdated()
    {
        _playerSaveManager.SavePlayer(_player.PlayerData);
    }

    private void HandlePlayerSaveUpdated(PlayerData playerData)
    {
        _player.UpdatePlayer(playerData); //Transmission des données à PlayerBahavior
    }
}
