using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI setNotification;
    [SerializeField] private TypewriterByCharacter _typewriterByCharacter;
    [Header("Notification Badge")]
    [SerializeField] private Image badgeImage;
    [SerializeField] private int flashAmount = 3;
    [SerializeField] private float timeBetweenFlashes = 0.05f;

    public static NotificationManager Instance;
    private Color _badgeInitialColour;

    private void Awake()
    {
        _badgeInitialColour = badgeImage.color;
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
        StartCoroutine(FlashBadge());
    }
    
    IEnumerator FlashBadge()
    {
        for (int i = 0; i < flashAmount; i++)
        {
            badgeImage.color = Color.white;
            yield return new WaitForSeconds(timeBetweenFlashes);
            badgeImage.color = _badgeInitialColour;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
    }
}