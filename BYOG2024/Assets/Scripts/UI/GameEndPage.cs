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

        private const string _gameTitle = "-man's\nAdventure";

        public void ShowGameEnd()
        {
            gameObject.SetActive(true);
            _titleText.SetText(PacmanConfig.PacmanName + _gameTitle);
            SetGameIcon();
            CalculateScore();
            SetReview();
        }

        private void CalculateScore()
        {
            var overallScore = PacmanConfig.OverallSuccessRate;
            _currentScoreText.SetText($"{overallScore * 10f}");
            _scoreFillImage.fillAmount = overallScore;
        }

        private void SetReview()
        {
            //How much they liked the game
            var overallScore = PacmanConfig.OverallSuccessRate;
            //How much bug free the game was
            var programmingScore = PacmanConfig.ProgrammingSuccess;
            //How much they liked the sound
            var soundScore = PacmanConfig.SoundSuccess;
            bool likedGame = overallScore > 0.5f;
            bool bugFree = programmingScore > 0.5f;
            bool likedSound = soundScore > 0.5f;
            //Break the feedback into 3 parts
            //1. How much they liked the game
            //2. How much bug free the game was
            //3. How much they liked the sound
            
            string feedback = likedGame ? "I liked the game" : "I didn't like the game";
            feedback += bugFree ? ", I really liked that it did not brick my PC" : ", the game had bugs";
            feedback += likedSound ? ", Also I really liked the music" : ", Also I didn't like the sound";
            _feedBackText.SetText(feedback);
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
        public void HomeButtonClicked()
        {
            SceneTransitionManager.Instance.LoadScene(0);
        }

        public void RetryButtonClicked()
        {
            SceneTransitionManager.Instance.LoadScene(1);
        }
        [ContextMenu("Show Game End")]
        public void ShowGameEndContextMenu()
        {
            ShowGameEnd();
        }
    }
}