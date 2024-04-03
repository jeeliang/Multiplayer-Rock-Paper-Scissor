using UnityEngine;
using UnityEngine.UI;

public class ButtonScriptSP : MonoBehaviour
{
    public int choiceValue;
    public GameManagerSP gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManagerSP>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        gameManager.SetChoice(choiceValue);
    }
}
