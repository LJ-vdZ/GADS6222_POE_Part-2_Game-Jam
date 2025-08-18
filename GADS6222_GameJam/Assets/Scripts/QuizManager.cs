using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;

    public GameObject[] answerZones; //corner GameObjects for selection
    public TextMeshPro[] answerTexts; //array of TextMeshPro objects for answer text

    public int currentQuestion;
    public TextMeshPro questionText; //3D TextMeshPro for question

    public GameObject door; //door GameObject to open on correct answer

    public GameObject gameOverPanel; //UI panel for game over

    public int playerHealth = 3; //starting health, change in game, this is just test

    private int totalQuestions;

    private bool isQuestionAnsweredCorrectly;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        totalQuestions = QnA.Count;
        gameOverPanel.SetActive(false);
        //healthText.text = "Health: " + playerHealth;

        isQuestionAnsweredCorrectly = false; //initialize flag

        //select random question only on start
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            questionText.text = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else
        {
            Debug.Log("No questions available");
            GameOver();
        }


    }

    void SetAnswers()
    {

        for (int i = 0; i < answerZones.Length; i++)
        {
            AnswerScript answerScript = answerZones[i].GetComponent<AnswerScript>();

            answerScript.isCorrect = (QnA[currentQuestion].CorrectAnswer == i + 1);

            if (answerTexts[i] != null)
            {
                answerTexts[i].text = QnA[currentQuestion].Answers[i];
            }
            else
            {
                Debug.LogError($"AnswerText[{i}] is not assigned!");
            }
        }
    }

    public void Correct()
    {


        isQuestionAnsweredCorrectly = true; //question is answered correctly
        OpenDoor(); //open door
    }

    public void Wrong()
    {
        if (!isQuestionAnsweredCorrectly) //reduce health if question selected is wrong
        {
            playerHealth -= 1;
            //healthText.text = "Health: " + playerHealth;
            if (playerHealth <= 0)
            {
                GameOver();
            }

        }
    }

    void OpenDoor()
    {
        //move door 
        if (door != null)
        {
            door.SetActive(false); //use animation/// door.GetComponent<Animator>().SetTrigger("Open");
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);

    }
}
