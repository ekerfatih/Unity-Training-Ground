using System.Collections.Generic;
using UnityEngine;

namespace YetYalcin.Template
{
    public class GuiManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _uiPanels;
        private GameObject _activeUi;
        
        #region UnityBuildinFunctions

        #region Singleton
        public static GuiManager Instance = null;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        #endregion

        private void Start()
        {
            SetActiveUi(0);
        }
        #endregion

        #region CustomMethods
        public GameObject GetActiveUi()//returns currently active ui
        {
            return _activeUi;
        }
        public GameObject GetUiPanelInList(int index)
        {
            return _uiPanels[index];
        }
        public void SetActiveUi(int index)
        {
            foreach (GameObject uiPanels in _uiPanels)
            {
                uiPanels.SetActive(false);
            }

            GetUiPanelInList(index).SetActive(true);
            _activeUi = GetUiPanelInList(index);
        }//using on buttons also
        public void OnClickWin()
        {
            LevelManager.Instance.NextLevel();
        }
        public void OnClickLoose()
        {
            LevelManager.Instance.RepeatLevel();
        }
        #endregion
    }

}
