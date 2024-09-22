using System;
using System.Collections.Generic;
using UnityEngine;
namespace Pacman
{
    public class PacmanManager : MonoBehaviour
    {
        public static PacmanManager Instance;
        
        [SerializeField] private Material _glitchMatGhost;
        [SerializeField] private Material _glitchMatPacman;
        //[SerializeField] private 
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

            _totalPellets = _pelletUtil.GetPelletCount();
            _currentPellets = 0;
            _currentTime = _timeToComplete;
        }
        private void SkinPacman()
        {

            _pacman._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Pacman, _defaultSprite);
            _ghostAI1._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Ghost1, _defaultSprite);
            _ghostAI2._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Ghost2, _defaultSprite);
            _ghostAI3._spriteRenderer.sprite = PacmanConfig.Drawings.GetValueOrDefault(PictureIDs.Food, _defaultSprite);
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
        }

        public void PelletEaten()
        {
            _currentScore += _pelletScoreIncrement;
            
        }

        public void GameOver()
        {
            _gameOver = true;
        }
    }
}
