using UnityEngine;
using TMPro;

public class PortalMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }

    public void ClearMessage()
    {
        messageText.text = "";
    }
}