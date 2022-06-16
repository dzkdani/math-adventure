using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public Text MathematicQuestion;

    void Update()
    {
        MathematicQuestion.text = CSVFileConverter.myQuestion;
    }
}
