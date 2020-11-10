using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PlayerSaveManager : MonoBehaviour
{
    private const string PLAYER_KEY = "Joueurs/Joueur1"; //Chemin des données
    private FirebaseDatabase _database;

    public PlayerData LastPlayerData { get; private set; }  //dernière données reçues
    public PlayerUpdatedEvent OnPlayerUpdated = new PlayerUpdatedEvent(); //pour dire aux listeners que quelque jour a changé dans les données du joueur
    //En faisant un listening sur les changements de valeurs, on est notifié si un des champs change. Pas besoin de faire un premier appel pour obtenir une valeur asychrone
    private DatabaseReference _ref;


    public void Lancer()
    {
        _database = FirebaseDatabase.DefaultInstance;
        _ref = _database.GetReference(PLAYER_KEY);
        _ref.ValueChanged += HandleValueChanged;
    }

    private void OnDestroy() //Très important car différence entre C# et Unity sur la façon de gérer la mémoire. On évite les erreur NoReferenceException
    {
        if (_ref!=null)
        {
            _ref.ValueChanged -= HandleValueChanged;
            _ref = null;
            _database = null;
        }
    }


    public void SavePlayer(PlayerData player)
    {
        if (!player.Equals(LastPlayerData)) //Pas d'écriture redondante. Pour éviter la bocule infinie lors de la sychronisation avec le serveur
        {
            _database.GetReference(PLAYER_KEY).SetRawJsonValueAsync(JsonUtility.ToJson(player));
        }
        //PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
    }

    public async Task<PlayerData?> LoadPlayer()
    {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> SaveExists()
    {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void EraseSave()
    {
        PlayerPrefs.DeleteKey(PLAYER_KEY);
        _database.GetReference(PLAYER_KEY).RemoveValueAsync();
    }

    /// <summary>
    /// Implémen
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    private void HandleValueChanged(object sender, ValueChangedEventArgs e) //Rappel à chaque fois que la valeur du noeud est modifiée
    {
        var json=e.Snapshot.GetRawJsonValue(); //On récupère la valeur JSON
        if(!string.IsNullOrEmpty(json)) //Au cas où le cache local renvoie une valeur nulle
        {
            var playerData = JsonUtility.FromJson<PlayerData>(json);
            LastPlayerData = playerData;
            OnPlayerUpdated.Invoke(playerData); //On dit aux listeners qu'il y a des nouvelles données
        }
    }

    [System.Serializable]
    public class PlayerUpdatedEvent : UnityEvent <PlayerData>
    {

    }
}
