using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.Events;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class FireBaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    //Configuration du SDK Firebase pour l'éditor Unity
    void Start()
    {
        //Tâche asynchrone pour initialiser Firebase et on attend son succès sur le thread principal
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => { //méthode d'extension Firebase pour mélanger les tâches avec la logique de jeu de Unity
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dyscalculie-ensc.firebaseio.com/");
                OnFirebaseInitialized.Invoke();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }


}
