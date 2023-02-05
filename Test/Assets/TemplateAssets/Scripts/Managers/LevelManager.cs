using System.Collections.Generic;
using UnityEngine;

namespace YetYalcin.Template
{
    public class LevelManager : MonoBehaviour
    {
        public int CurrentLevelIndex;

        private GameObject _previousLevel;
        private bool _isStarting = false;
        [Space(10)]
        [Header("Level List")]
        public List<GameObject> LevelList;

        [Space(10)]
        [Header("Level Looping Settings")]

        //Set willLoop = false to disable the Looping system
        public bool WillLoop = true;
        public int EndOfLoop;
        public int StartOfLoop;

        #region UnityBuildinFunctions

        #region Singleton
        public static LevelManager Instance = null;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        #endregion

        private void Start()
        {
            LoadLevel();
        }
        #endregion

        #region CustomMethods
        //This method is for loading the curLevel
        public void LoadLevel()
        {
            if (_previousLevel != null)  
                Destroy(_previousLevel); 
            

            _previousLevel = Instantiate(LevelList[CurrentLevelIndex]);

            StartLevel();

            _isStarting = false;
        }

        //StartLevel() gets called when the user presses the screen first time
        public void StartLevel()
        {
            Debug.Log("Level started");

            GameManager.Instance.GameState = GameManager.GameStates.Start;
            //Elephant.LevelStarted(CurrentLevelIndex);

            _isStarting = false;
        }

        public void NextLevel()
        {
            if (WillLoop)
            {
                if (CurrentLevelIndex == EndOfLoop)
                {
                    CurrentLevelIndex = StartOfLoop;
                    LoadLevel();
                    return;
                }
                if (CurrentLevelIndex < EndOfLoop)
                {
                    CurrentLevelIndex++;
                    LoadLevel();
                }
            }
            else
            {
                CurrentLevelIndex++;
                LoadLevel();
            }
        }

        public void RepeatLevel()
        {
            Destroy(_previousLevel.transform.gameObject);
            LoadLevel();
        }
        #endregion
    }
}

