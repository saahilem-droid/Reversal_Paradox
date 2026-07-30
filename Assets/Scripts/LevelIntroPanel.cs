using System.Collections;
using UnityEngine;

public class LevelIntroPanel : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private float showDelay = 0.4f;

    [Header("Show Only Once")]
    [SerializeField] private bool showOnlyFirstTime = true;
    [SerializeField] private string saveKey = "Level1IntroShown";

    private IEnumerator Start()
    {
        bool alreadyShown = PlayerPrefs.GetInt(saveKey, 0) == 1;

        if (showOnlyFirstTime && alreadyShown)
        {
            infoPanel.SetActive(false);
            GameManager.Instance.SetGameplayEnabled(true);
            yield break;
        }

        GameManager.Instance.SetGameplayEnabled(false);

        infoPanel.SetActive(false);

        yield return new WaitForSecondsRealtime(showDelay);

        infoPanel.SetActive(true);
    }

    public void ClosePanel()
    {
         Debug.Log("ClosePanel called");
        infoPanel.SetActive(false);
        GameManager.Instance.SetGameplayEnabled(true);

        if (showOnlyFirstTime)
        {
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
        }
    }

    [ContextMenu("Reset Tutorial Popup")]
public void ResetTutorialPopup()
{
    PlayerPrefs.DeleteKey(saveKey);
    PlayerPrefs.Save();

    Debug.Log("Tutorial popup has been reset.");
}
}