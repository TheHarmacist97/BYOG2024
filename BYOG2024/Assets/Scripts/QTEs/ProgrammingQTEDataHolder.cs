
using UnityEngine;

[CreateAssetMenu(menuName = "QTEs/ProgrammingQTEDataHolder")]
public class ProgrammingQTEDataHolder : ScriptableObject
{
    [System.Serializable]
    public class CodeBlock
    {
        public string fileName;
        public TextAsset codeFile;
    }
    [System.Serializable]
    public class ErrorBlock
    {
        [TextArea(10, 20)]
        public string errorText;
    }
    
    public CodeBlock[] codeBlocks;
    public ErrorBlock[] errorBlocks;
}