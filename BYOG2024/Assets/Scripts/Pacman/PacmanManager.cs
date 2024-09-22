using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Pacman
{
    public class PacmanManager : MonoBehaviour
    {
        public static PacmanManager Instance;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Material _glitchMatGhost;
        [SerializeField] private Material _glitchMatPacman;
        [SerializeField] private AudioClip _perfectSoundDesign;
        [SerializeField] private AudioClip _ehSoundDesign;
        [SerializeField] private AudioClip _badSoundDesign;
        [SerializeField] private bool _gameOver;
        [SerializeField] private int _pelletScoreIncrement;
        [SerializeField] private float _timeToComplete;
        [SerializeField] private bool _debug;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private PelletUtil _pelletUtil;
        [SerializeField] private PacmanLogic _pacman;
        [SerializeField] private GhostAI _ghostAI1, _ghostAI2, _ghostAI3;
        [SerializeField] private int _currentScore;
        [SerializeField] private int _totalPellets, _currentPellets;

        [SerializeField] private GameEndPage _gameEndPage;
        private float _currentTime;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            SkinPacman();
            if (PacmanConfig.SoundSuccess > 0.7)
            {
                AudioManager.instance.PlayMusic(_perfectSoundDesign, 1f);
            }
            else if (PacmanConfig.SoundSuccess > 0.4)
            {
                AudioManager.instance.PlayMusic(_ehSoundDesign, 1f);
            }
            else
            {
                AudioManager.instance.PlayMusic(_badSoundDesign, 1f);
            }

            if (PacmanConfig.ProgrammingSuccess < 0.7)
            {
                _pacman._spriteRenderer.material = _glitchMatPacman;
                _ghostAI1._spriteRenderer.material = _glitchMatGhost;
                _ghostAI2._spriteRenderer.material = _glitchMatGhost;
                _ghostAI3._spriteRenderer.material = _glitchMatGhost;
            }
            
            _totalPellets = _pelletUtil.GetPelletCount();
            _currentPellets = 0;
            _currentTime = _timeToComplete;
        }
        private void SkinPacman()
        {
            _pacman._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Pacman, _defaultSprite);
            _ghostAI1._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Ghost1, _defaultSprite);
            _ghostAI2._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Ghost2, _defaultSprite);
            _ghostAI3._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Ghost3, _defaultSprite);
            _pelletUtil.SkinPellets(PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Food, _defaultSprite));
        }

        private void Update()
        {
            if(_gameOver) 
                return;
            
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0&&!_gameOver)
            {
                GameOver();
            }
        }

        public void PacmanDeath()
        {
            _pacman.KillSelf();
            _ghostAI1._gameOver = true;
            _ghostAI2._gameOver = true;
            _ghostAI3._gameOver = true;
            GameOver();
        }

        public void AllPelletsEaten()
        {
            _pacman.KillSelf();
            _ghostAI1._gameOver = true;
            _ghostAI2._gameOver = true;
            _ghostAI3._gameOver = true;
            GameOver();
        }

        public void PelletEaten()
        {
            _currentPellets++;
            _currentScore += _pelletScoreIncrement;
            _scoreText.text = "Score:  " + string.Format(_currentScore.ToString("D2"));
            if (_currentPellets >= _totalPellets)
            {
                AllPelletsEaten();
            }
        }

        public void GameOver()
        {
            _gameOver = true;
            _gameEndPage.ShowGameEnd();
        }
    }
}
