using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    public InputField AnswerField;
    private bool Doted = false;

    private bool MinusSym = false;
    private bool PositiveSym = true;

    // Start is called before the first frame update
    public void StartQuiz()
    {
        AnswerField.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (AnswerField.text.Contains("."))
        {
            Doted = true;
        }
        else if (!AnswerField.text.Contains("."))
        {
            Doted = false;
        }
    }

    public void InputAnswer(int Number)
    {
        if (AnswerField.text.Length < 12)
        {
            AnswerField.text = AnswerField.text + Number.ToString();
        }
    }

    public void DeleteAnswer()
    {
        if(AnswerField.text.Length > 0 && AnswerField.text != "-")
        {
            AnswerField.text = AnswerField.text.Remove(AnswerField.text.Length - 1, 1);
        }
    }

    public void Dot()
    {
        if (AnswerField.text != "")
        {
            if (Doted == false)
            {
                AnswerField.text = AnswerField.text + ".";
                Doted = true;
            }
        }
    }

    public void Minus()
    {
        if (MinusSym == false && PositiveSym == true)
        {
            AnswerField.text = "-" + AnswerField.text;
            MinusSym = true;
            PositiveSym = false;
        }

    } 

    public void Positive()
    {
        if (PositiveSym == false)
        {
            AnswerField.text = AnswerField.text.Remove(0, 1);
            PositiveSym = true;
            MinusSym = false;
        }
    }

    public void C()
    {
        AnswerField.text = "";
    }
}
