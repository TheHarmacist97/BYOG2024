using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEndPage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _titleText;

        [Header("Review")]
        [SerializeField]
        private TextMeshProUGUI _feedBackText;

        [Header("Game Icon")]
        [SerializeField]
        private Image _pacManImage;

        [SerializeField]
        private Image _ghost1Image;

        [SerializeField]
        private Image _ghost2Image;

        [SerializeField]
        private Image _ghost3Image;

        [SerializeField]
        private Image _foodImage;

        [Header("Score")]
        [SerializeField]
        private TextMeshProUGUI _currentScoreText;

        [SerializeField]
        private Image _scoreFillImage;


        public void ShowGameEnd()
        {
            gameObject.SetActive(true);
        }


        private void CalculateScore()
        {
            var overallScore = PacmanConfig.OverallSuccessRate;
        }

        private void SetGameIcon()
        {
            SetSprite(PictureIDs.Pacman, _pacManImage);
            SetSprite(PictureIDs.Ghost1, _ghost1Image);
            SetSprite(PictureIDs.Ghost2, _ghost2Image);
            SetSprite(PictureIDs.Ghost3, _ghost3Image);
            SetSprite(PictureIDs.Food, _foodImage);
        }

        private void SetSprite(PictureIDs pictureId, Image image)
        {
            if (PacmanConfig.Drawings.TryGetValue(pictureId, out var drawing))
            {
                image.sprite = drawing;
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}