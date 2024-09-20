using UnityEngine;
namespace Pacman
{
    public class PacmanManager : MonoBehaviour
    {
        [SerializeField] private PelletUtil _pelletUtil;
        [SerializeField] private PacmanLogic _pacman;
        [SerializeField] private GhostAI _ghostAI1, _ghostAI2, _ghostAI3;
        [SerializeField] private Pellet _pelletPrefab;
        private Pellet changedPellet;
    
        // Start is called before the first frame update
        void Start()
        {
            _pacman._spriteRenderer.sprite = PacmanConfig.Drawings[PictureIDs.Pacman];
            _ghostAI1._spriteRenderer.sprite = PacmanConfig.Drawings[PictureIDs.Ghost2]; 
            _ghostAI2._spriteRenderer.sprite = PacmanConfig.Drawings[PictureIDs.Ghost2];  
            _ghostAI3._spriteRenderer.sprite = PacmanConfig.Drawings[PictureIDs.Ghost3];
            _pelletPrefab._spriteRenderer.sprite = PacmanConfig.Drawings[PictureIDs.Food];
            _pelletUtil.GetPellets(_pelletPrefab);
        }
    }
}
