using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isCorrect)
            {
                Debug.Log("Correct Answer!");
                quizManager.Correct();
            }
            else
            {
                Debug.Log("Wrong Answer!");
                quizManager.Wrong();
            }
        }
    }
}
