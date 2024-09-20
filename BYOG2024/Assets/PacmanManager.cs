using Pacman;
using UnityEngine;

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
        //_pacman._spriteRenderer.sprite =
        //_ghostAI1._spriteRenderer.sprite = 
        //_ghostAI2._spriteRenderer.sprite = 
        //_ghostAI3._spriteRenderer.sprite = 
        //_pelletPrefab._spriteRenderer.sprite =
        _pelletUtil.GetPellets(_pelletPrefab);
    }
}
