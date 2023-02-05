using UnityEngine;

namespace YetYalcin.Template
{
    public class GameManager : MonoBehaviour
    {
        public enum GameStates : byte
        {
            Start,
            Run,
            End
        }
        public GameStates GameState;

        #region UnityBuildinFunctions

        #region Singleton
        public static GameManager Instance = null;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        #endregion

        #endregion

        #region CustomMethods
        public void EndOfLevel(bool isWin)
        {
            if (GameState.Equals(GameStates.End))
                return;

            GameState = GameStates.End;

            if (isWin)
                Success();
            else
                Fail();
        }
        public void Success()
        {
            GuiManager.Instance.SetActiveUi(2);

            //Elephant.LevelCompleted(CurrentLevelIndex);
            // MMVibrationManager.Haptic(HapticTypes.Success);
        }
        public void Fail()
        {
            GuiManager.Instance.SetActiveUi(3);

            //Elephant.LevelFailed(CurrentLevelIndex);
            // MMVibrationManager.Haptic(HapticTypes.Warning);
        }
        #endregion
    }
}


