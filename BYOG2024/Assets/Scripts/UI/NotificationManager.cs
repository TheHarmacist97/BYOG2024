using Febucci.UI;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI setNotification;
    [SerializeField] private TypewriterByCharacter _typewriterByCharacter;

    public static NotificationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetNotification(string notification)
    {
        _typewriterByCharacter.ShowText(notification);
    }
}