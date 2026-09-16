using UnityEngine;

[System.Serializable]
public class QuestionData
{
    [TextArea(2, 4)]
    public string questionText;

    public string[] answers = new string[4];

    public int correctIndex; // 0=A, 1=B, 2=C, 3=D
}