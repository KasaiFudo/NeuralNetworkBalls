using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{
    public string levelName;
    public Text t_Score;
    public int i_Score = 1;
    public GameObject GameManager;
    public void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Red") //gameObject.tag - объект, на который наложен код coll.gameObject.tag - объект, с которым наш соприкасается
        {
            GameManager.GetComponent<GameManager>().realScore+=1;
            i_Score += 5;
            //i_Score = GameManager.GetComponent<GameManager>().real_score;
            //t_Score.text = i_Score.ToString();
            //Destroy(coll.gameObject);
        }
    }
}