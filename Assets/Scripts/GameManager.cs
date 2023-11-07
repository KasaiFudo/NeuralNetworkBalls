using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject protoSphere;
    public GameObject goal;
    static int popul = 100;
    public GameObject[] spheres = new GameObject[popul];
    public double[,] goalDistance = new double[3,popul];
    public Text timerTxt;
    float timer;
    public float[,,] weightInput = new float[popul, 5, 5];
    public float[,,,] weightMass = new float[popul, 5, 5, 5];
    public float[,,] weightOutput = new float[popul, 5, 4];
    public Slider timeSlider;

    public Text tScore;
    int i_Score = 0;
    public int realScore;

    void Start()
    {
        realScore = 1;
        timer = 60;
        for (int p = 0; p < popul; p++)
        {
            spheres[p] = Instantiate(protoSphere);
            spheres[p].transform.localPosition = new Vector3(-400 + Random.value * 100 - 50, 40, 400 + Random.value * 100 - 50);
        }
    }
    void Update()
    {
        timer -= Time.deltaTime;
        timerTxt.text = timer.ToString();
        if(timer <= 0)
        {
            StopPopulation();
            NextPopulation();
        }
        Time.timeScale = timeSlider.value;
    }

    public void StopPopulation()
    {
        for (int p = 0; p < popul; p++)
        {
            goalDistance[0, p] = p;
            goalDistance[1,p] = System.Math.Pow(((double)goal.transform.localPosition.x - (double)spheres[p].transform.localPosition.x), 2)
                + System.Math.Pow(((double)goal.transform.localPosition.y - (double)spheres[p].transform.localPosition.y), 2)
                + System.Math.Pow(((double)goal.transform.localPosition.z - (double)spheres[p].transform.localPosition.z), 2);
            goalDistance[1, p] = System.Math.Pow(goalDistance[1, p],5)/10;
            goalDistance[2, p] = spheres[p].GetComponent<Next>().i_Score;
            goalDistance[1, p] /= spheres[p].GetComponent<SphereMoveScript>().goalArea;   
        }

        Sort();

        for (int p = 0; p < popul; p++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    weightInput[p, i, j] = spheres[System.Convert.ToInt32(goalDistance[0, p])].GetComponent<SphereMoveScript>().w_i[i, j];  //получаем матрицу рандомных весовых коэффициентов
                }
            }
            for (int k = 0; k < 5; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        weightMass[p, k, i, j] = spheres[System.Convert.ToInt32(goalDistance[0, p])].GetComponent<SphereMoveScript>().w_m[k, i, j];  //получаем матрицу рандомных весовых коэффициентов
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    weightOutput[p, i, j] = spheres[System.Convert.ToInt32(goalDistance[0, p])].GetComponent<SphereMoveScript>().w_o[i, j];  //получаем матрицу рандомных весовых коэффициентов
                }
            }
            
        }

    }

    void Sort()
    {
        for (int p = 0; p < popul - 1; p++) //сортировка
        {
            for (int pp = 0; pp < popul - 1; pp++)
            {
                if (goalDistance[1, pp] / goalDistance[2, pp] > goalDistance[1, pp + 1] / goalDistance[2, pp])
                {
                    double t = goalDistance[1, pp + 1];
                    goalDistance[1, pp + 1] = goalDistance[1, pp];
                    goalDistance[1, pp] = t;

                    double tn = goalDistance[0, pp + 1];
                    goalDistance[0, pp + 1] = goalDistance[0, pp];
                    goalDistance[0, pp] = tn;

                    double ts = goalDistance[2, pp + 1];
                    goalDistance[2, pp + 1] = goalDistance[2, pp];
                    goalDistance[2, pp] = ts;
                }
            }
        }
    }
    public void NextPopulation()
    {
        timer = 60;
        i_Score += 1;
        tScore.text = i_Score.ToString();
        
        for (int p = 0; p < popul; p++)
        {
            spheres[p].transform.localPosition = new Vector3(-400 + Random.value * 100 - 50, 40, 400 + Random.value * 100 - 50);
            spheres[p].GetComponent<Next>().i_Score = 1;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    spheres[p].GetComponent<SphereMoveScript>().w_i[i, j] = weightInput[p/4, i, j] + (Random.value * 2 - 1)/20;  //получаем матрицу рандомных весовых коэффициентов
                }
            }
            for (int k = 0; k < 5; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        spheres[p].GetComponent<SphereMoveScript>().w_m[k, i, j] = weightMass[p / 4, k, i, j] + (Random.value * 2 - 1) / 20;  //получаем матрицу рандомных весовых коэффициентов
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    spheres[p].GetComponent<SphereMoveScript>().w_o[i, j] = weightOutput[p/ 4, i, j] + (Random.value * 2 - 1) / 20;  //получаем матрицу рандомных весовых коэффициентов
                }
            }
        }
    }
}
