using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ProgrammingQTE : QuickTimeEvent
{
    [SerializeField]
    private TimerProgressBar timerProgressBar;
    [SerializeField]
    private Vector2 letterSpawnDelayRange = new Vector2(0.1f, 0.5f);
    [SerializeField]
    private ProgrammingQTELetter letterPrefab;
    [SerializeField]
    private Transform letterSpawnPoint;
    [SerializeField]
    private float spawnYOffset = 0.5f;

    [Header("Code Editor")]
    [SerializeField] private ProgrammingQTEDataHolder programmingQteData;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private ScrollRect codeScrollRect;
    
    private string[] _letterSequence;
    private ProgrammingQTELetter[] _letterObjs;
    private int _currentLetterIndex = 0;
    private int _currentLetterSpawned = 0;
    private ProgrammingQTEDataHolder.CodeBlock _currentCodeBlock;
    private string[] _codeChunks;
    
    private static readonly string Letters = "abcdefghijklmnopqrstuvwxyz";

    protected override void Initialize()
    {
        _letterSequence = new string[totalActionCount];
        _letterObjs = new ProgrammingQTELetter[totalActionCount];
        _codeChunks = new string[totalActionCount];
        _currentCodeBlock = programmingQteData.codeBlocks[Random.Range(0, programmingQteData.codeBlocks.Length)];
        int codeLength = _currentCodeBlock.codeFile.text.Length;
        int chunkSize = codeLength / totalActionCount;
        for (int i = 0, j = 0; i < codeLength && j< totalActionCount ; i += chunkSize, j++)
        {
            if (i + chunkSize > codeLength) chunkSize = codeLength  - i;
            _codeChunks[j] = _currentCodeBlock.codeFile.text.Substring(i, chunkSize);
        }
        for (int i = 0; i < totalActionCount; i++)
        {
            _letterSequence[i] = Letters[Random.Range(0, Letters.Length)].ToString();
        }
        
        StartCoroutine(SpawnLetters());
    }
    
    protected override void OnUpdate()
    {
        timerProgressBar.SetProgress(GetTimeLeftProgress());
        codeScrollRect.normalizedPosition = new Vector2(0, 0);
        if (_currentLetterIndex <= _currentLetterSpawned && _currentLetterIndex < totalActionCount)
        {
            if (Input.GetKeyDown(_letterSequence[_currentLetterIndex]))
            {
                IncrementSuccessAction();
                if(_letterObjs[_currentLetterIndex] != null)
                    _letterObjs[_currentLetterIndex].DestroyAsSuccess();
                codeText.text += "<color=green>" + _codeChunks[_currentLetterIndex] + "</color>";
                MoveToNextLetter();
            }
            else if (!Input.GetKeyDown(KeyCode.Escape) 
                     && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)
                     && !Input.GetMouseButtonDown(2)&& Input.anyKeyDown)
            {
                IncrementFailedAction();
                codeText.text += "\n<color=red>" + programmingQteData.errorBlocks[Random.Range(0, programmingQteData.errorBlocks.Length)].errorText+ "</color>";
                if(_letterObjs[_currentLetterIndex] != null)
                    _letterObjs[_currentLetterIndex].DestroyAsFailure();
                MoveToNextLetter();
            }
        }
    }

    private void MoveToNextLetter()
    {
        _currentLetterIndex++;
        if(_letterObjs[_currentLetterIndex] != null)
            _letterObjs[_currentLetterIndex].SetAsCurrentlyActive();
    }
    
    IEnumerator SpawnLetters()
    {
        float yOffset = 0f;
        while (_currentLetterSpawned < totalActionCount)
        {
            Vector2 pos = new Vector2(letterSpawnPoint.position.x, letterSpawnPoint.position.y + yOffset);
            _letterObjs[_currentLetterSpawned] = Instantiate(letterPrefab, pos, Quaternion.identity);
            _letterObjs[_currentLetterSpawned].transform.parent = letterSpawnPoint;
            yOffset += spawnYOffset;
            _letterObjs[_currentLetterSpawned].Init(_letterSequence[_currentLetterSpawned].ToUpper());
            if(_currentLetterSpawned == 0)
                _letterObjs[_currentLetterSpawned].SetAsCurrentlyActive();
            _currentLetterSpawned++;
            yield return new WaitForSeconds(Random.Range(letterSpawnDelayRange.x, letterSpawnDelayRange.y));
        }
    }

    protected override void OnComplete()
    {
        timerProgressBar.ResetTimer();
    }
}
