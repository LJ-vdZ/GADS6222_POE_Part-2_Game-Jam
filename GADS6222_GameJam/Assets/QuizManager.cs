using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public TextMeshProUGUI QuestionTxt;

    public GameObject QuizPanel;
    public GameObject GOPanel;

    public TextMeshProUGUI ScoreTxt;

    int totalQuestions = 0;

    public int score;

    private void Start()
    {
        totalQuestions = QnA.Count;
        GOPanel.SetActive(false);
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
        if(QnA.Count > 0) 
        {
            currentQuestion = Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;

            SetAnswers();

        }
        else 
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
        
        
    }

    public void Correct() 
    {
        score += 1;

        QnA.RemoveAt(currentQuestion);

        generateQuestion();


    }

    public void GameOver() 
    {
        QuizPanel.SetActive(false);
        GOPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;
    }

    public void retry() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void wrong() 
    {
        QnA.RemoveAt(currentQuestion);

        generateQuestion();
    }
}
