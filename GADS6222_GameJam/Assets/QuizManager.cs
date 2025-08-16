using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public TextMeshProUGUI QuestionTxt;

    private void Start()
    {
        generateQuestion();
    }

    void SetAnswers() 
    {
        for(int i = 0; i < options.Length; i++) 
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1) 
            {
                options[i].GetComponent <AnswerScript>().isCorrect = true;
            }
        
        
        }
    }

    void generateQuestion() 
    {
        currentQuestion = Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestion].Question;

        SetAnswers();

        
    }

    public void Correct() 
    {
        QnA.RemoveAt(currentQuestion);

        generateQuestion();


    }
}
