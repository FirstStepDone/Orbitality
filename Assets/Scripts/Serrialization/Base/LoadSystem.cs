using UnityEngine;
/*
    Base class for handling Save/Load operations
*/
namespace Orbitality
{
    public abstract class LoadSystem : MonoBehaviour
    {
        //Events
        public event System.Action OnGameSaved;
        public event System.Action<bool> OnGameLoadCompleted;

        //Manipulate loaded data, eg. Feed into systems & behaviours
        public abstract void InitializeLoadedData();

        //Pull data that needs to be saved
        public abstract void PrepareSaveData();

        //Parse data from abstract to concrete LevelData
        public abstract void ParseSaveData(string json, System.Action<bool> OnComplete = null);

        //Basically destroy current game state
        public abstract void CleanGamestate();

        //Serializing save data, storing in PlayerPrefs 
        public void Save()
        {
            PrepareSaveData();

            string json = JsonUtility.ToJson(levelData);

            Debug.Log("Saving game, json: " + json);
            PlayerPrefs.SetString("Save", json);

            OnGameSaved.Fire();
        }

        //Full sequence of loading
        public void Load(System.Action<bool> OnComplete = null)
        {
            CleanGamestate();

            OnGameLoadCompleted += OnComplete;

            string json = PlayerPrefs.GetString("Save");
            Debug.Log("Loading game, json loaded: " + json);

            Debug.Log("Starting data parsing...");
            ParseSaveData(json, (success) =>
            {
                if (success)
                {
                    Debug.Log("Save parsed");
                    Debug.Log("Initializing loaded data...");
                    InitializeLoadedData();

                    OnGameLoadCompleted.Fire(true);
                }
                else
                {
                    Debug.LogError("Could not read provided save");

                    OnGameLoadCompleted.Fire(false);
                }

                OnGameLoadCompleted -= OnComplete;
            });
        }

        //Data that needs to become concrete based on the game
        protected abstract LevelData levelData { get; }

        [System.Serializable]
        public class LevelData { }
    }



}