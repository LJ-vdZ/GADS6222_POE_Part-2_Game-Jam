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

   // public GameObject gameOverPanel; UI panel for game over

    public int playerHealth = 3; //starting health, change in game, this is just test

    private int totalQuestions;

    private bool isQuestionAnsweredCorrectly;

    public Transform currentRoomSpawnPoint; //spawn point for current room

    public Transform nextRoomSpawnPoint; //spawn point for next room

    public GameObject player;

    [Header("Objects to Destroy")]
    [SerializeField] private GameObject objectToDestroy1; 
    [SerializeField] private GameObject objectToDestroy2; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        totalQuestions = QnA.Count;
        //gameOverPanel.SetActive(false);
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
        if (isQuestionAnsweredCorrectly) return; //prevents multiple triggers

        isQuestionAnsweredCorrectly = true; //question is answered correctly
        
        Debug.Log("Correct answer!");

        Destroy(objectToDestroy1);
        Destroy(objectToDestroy2);

        /*
        //move player to next room's spawn point
        if (nextRoomSpawnPoint != null && player != null)
        {
            player.transform.position = nextRoomSpawnPoint.position;
            player.transform.rotation = nextRoomSpawnPoint.rotation;
        }
        else
        {
            Debug.LogError("Next room spawn point or player not assigned!");
        }*/

        //remove the current question and load a new one
        QnA.RemoveAt(currentQuestion);
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            questionText.text = QnA[currentQuestion].Question;
            SetAnswers();
            isQuestionAnsweredCorrectly = false;
            
        }

       
        
    }

    public void Wrong()
    {
        if (isQuestionAnsweredCorrectly) return; //prevent multiple triggers
        playerHealth -= 1;
        //healthText.text = "Health: " + playerHealth;
        Debug.Log("Wrong Answer!");

        if (playerHealth <= 0)
        {
            GameOver();
            return;
        }

        //respawn player in current room
        if (currentRoomSpawnPoint != null && player != null)
        {
            player.transform.position = currentRoomSpawnPoint.position;
            player.transform.rotation = currentRoomSpawnPoint.rotation;
        }
        else
        {
            Debug.LogError("Current room spawn point not assigned");
        }
    }

    void GameOver()
    {
       // gameOverPanel.SetActive(true);

    }
}
