using UnityEditor;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR

namespace LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        [SerializeField] private GameObject popUpPanel;
        [SerializeField] private GameObject saveConfirmPanel;

        private const int POPUPPANEL_TITLE_CHILD_NUMBER = 0;
        private const int POPUPPANEL_INPUTFIELD_CHILD_NUMBER = 1;
        private const int POPUPPANEL_LEFTBUTTON_CHILD_NUMBER = 2;
        private const int POPUPPANEL_RIGHTBUTTON_CHILD_NUMBER = 3;

        private const string LOADTITLE = "Which Level to Load";
        private const string SAVETITLE = "Which Level to Save";

        private bool popUpPanelFlag = false;

        private int levelNumber;

       


        #region Bottom Panel

        public void OnGridButtonClick()
        {
            GridEditorWindow gridEditorWindow = (GridEditorWindow)GridEditorWindow.GetWindow(typeof(GridEditorWindow), false);
            GridColorEditorWindow gridColorEditorWindow = (GridColorEditorWindow)GridColorEditorWindow.GetWindow(typeof(GridColorEditorWindow), false);
            gridEditorWindow.Show();
            gridColorEditorWindow.Show();
        }


        public void OnPreviewButtonClick()
        {
            // Load Level with animations
        }

        public void OnLoadButtonClick()
        {
            popUpPanelFlag = true;

            TextMeshProUGUI textmeshPro = popUpPanel.transform.GetChild(POPUPPANEL_TITLE_CHILD_NUMBER).GetComponent<TextMeshProUGUI>();
            textmeshPro.text = LOADTITLE;
            
            popUpPanel.SetActive(true);
        }

        public void OnSaveButtonClick()
        {
            popUpPanelFlag = false;

            TextMeshProUGUI textmeshPro = popUpPanel.transform.GetChild(POPUPPANEL_TITLE_CHILD_NUMBER).GetComponent<TextMeshProUGUI>();
            textmeshPro.text = SAVETITLE;

            popUpPanel.SetActive(true);
        }

        #endregion

        #region PopUp Panel

        public void OnPopUpPanelLoadButtonClick()
        {
            string inputString = popUpPanel.transform.GetChild(POPUPPANEL_INPUTFIELD_CHILD_NUMBER).GetComponent<TMP_InputField>().text;
            levelNumber = int.Parse(inputString);

            if (popUpPanelFlag)
            {
                LevelEditorManager.Instance.Grid?.DestroyAllPixels3D();
                LevelEditorManager.Instance.LoadGivenLevelFromAssets(levelNumber);
                
            }
            else
            {
                if (LevelEditorManager.Instance.IsLevelExist(levelNumber))
                {
                    saveConfirmPanel.SetActive(true);
                    
                }
                else
                {
                    LevelEditorManager.Instance.SaveCurrentLevelToAsset(levelNumber);
                }

                

            }

            popUpPanel.SetActive(false);
        }

        public void OnPopUpPanelCloseButtonClick()
        {
            popUpPanel.SetActive(false);
        }

        #endregion

        #region Save Confirm Panel

        public void OnSaveConfirmPanelYesButton()
        {
            LevelEditorManager.Instance.SaveCurrentLevelToAsset(levelNumber);
            saveConfirmPanel.SetActive(false);
        }


        public void OnSaveConfirmPanelNoButton()
        {
            saveConfirmPanel.SetActive(false);
            popUpPanel.SetActive(true);
        }


        #endregion

    }

}
#endif