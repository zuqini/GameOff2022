using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSentence
{
    public float sentenceCharDelayInSec = -1;

    [TextArea(3, 10)]
    public string sentence;
}

