using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMoveScript : MonoBehaviour
{
    public float[,] w_i = new float[5, 5]; //i-input
    public float[,,] w_m = new float[5,5,5]; // m-mass
    public float[,] w_o = new float[5, 4]; //o-output
    public GameObject goal;
    public bool goalComplete = false;
    public float goalArea = 1;
    public int speed = 250;
    Rigidbody rb;
    bool grounded = false;
    bool walled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < w_i.GetLength(0); i++)
        {
            for (int j = 0; j < w_i.GetLength(1); j++)
            {
                w_i[i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
            }
        }
        for (int k = 0; k < w_m.GetLength(0); k++)
        {
            for (int i = 0; i < w_m.GetLength(1); i++)
            {
                for (int j = 0; j < w_m.GetLength(2); j++)
                {
                    w_m[k, i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
                }
            }
        }
        for (int i = 0; i < w_o.GetLength(0); i++)
        {
            for (int j = 0; j < w_o.GetLength(1); j++)
            {
                w_o[i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
            }
        }
    }

    void FixedUpdate()
    {
        FindWay(CalculateRes());
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        if (collision.gameObject.tag == "wall")
        {
            walled = true;
        }
        
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        if (collision.gameObject.tag == "wall")
        {
            walled = false;
        }
    }
    void FindWay(float[] res)
    {
        float moveHorizontal = res[0] + 0.1f;
        float moveVertical = res[1] + 0.1f;
        float rndJump = res[2] + 0.1f;
        float rndNitro = res[3] + 0.1f;
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        if (grounded && rndJump > 0)
        {
            rb.AddForce(new Vector3(0f, speed * 20f, 0f));
        }
        if (grounded && rndNitro > 0)
        {
            rb.AddForce(movement * speed * 2);
        }
        rb.AddForce(movement * speed);

        if ((System.Math.Abs(goal.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x) + System.Math.Abs(goal.GetComponent<Transform>().position.z - GetComponent<Transform>().position.z)) < 50)
        {
            goalArea += Time.deltaTime * 100;
        }
    }
    float[] CalculateRes()
    {
        float xPos = GetComponent<Transform>().position.x / 1000f;
        float zPos = GetComponent<Transform>().position.z / 1000f;
        float xPosG = (goal.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x) / 1000f;
        float zPosG = (goal.GetComponent<Transform>().position.z - GetComponent<Transform>().position.z) / 1000f;

        System.Random rnd = new System.Random();

        float[] res_i = new float[5];
        for (int i = 0; i < w_i.GetLength(1); i++)
        {
            res_i[i] = 0;
        }
        for (int i = 0; i < w_i.GetLength(1); i++)
        {
            res_i[i] += xPos * w_i[0, i] + zPos * w_i[1, i] + System.Convert.ToInt32(walled) * w_i[2, i] + xPosG * w_i[3, i] + zPosG * w_i[4, i];
        }
        float[,] res_m = new float[5, 5];
        for (int k = 0; k < res_m.GetLength(0); k++)
        {
            for (int i = 0; i < res_m.GetLength(1); i++)
            {
                res_m[k, i] = 0;
            }
            if (k == 0)
            {
                for (int i = 0; i < res_m.GetLength(1); i++)
                {
                    for (int j = 0; j < res_m.GetLength(1); j++)
                    {
                        res_m[k, i] += res_i[j] * w_m[k, i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < res_m.GetLength(1); i++)
                {
                    for (int j = 0; j < res_m.GetLength(1); j++)
                    {
                        res_m[k, i] += res_m[k - 1, j] * w_m[k, i, j];
                    }
                }
            }
        }
        float[] res_out = new float[4];
        for (int i = 0; i < res_out.Length; i++)
        {
            res_out[i] = 0;
        }
        for (int i = 0; i < res_out.Length; i++)
        {
            for (int j = 0; j < res_m.GetLength(1); j++)
            {
                res_out[i] += res_m[4, j] * w_o[j, i];
            }
        }
        return res_out;
    }
    
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SphereMoveScript : MonoBehaviour
//{
//    private Rigidbody rb;
//    int speed = 250;
//    bool grounded = false;
//    float[,] w1 = new float[2, 4];
//    float[,] w2 = new float[4, 4];
//    float[,] w3 = new float[4, 4];
//    public Text timer_text;
//    public GameObject proto_Player;
//    GameObject[] Player = new GameObject[20];
//    float timer_life;
//    void Start()
//    {
//        for (int p = 0; p < 20; p++)
//        {
//            Player[p] = Instantiate(proto_Player);
//            rb[p] = Player[p].GetComponent<Rigidbody>();
//            timer_life = 20;
//            for (int i = 0; i < w1.GetLength(0); i++)
//            {
//                for (int j = 0; j < w1.GetLength(1); j++)
//                {
//                    w1[i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
//                }
//            }
//            for (int i = 0; i < w2.GetLength(0); i++)
//            {
//                for (int j = 0; j < w2.GetLength(1); j++)
//                {
//                    w2[i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
//                }
//            }
//            for (int i = 0; i < w3.GetLength(0); i++)
//            {
//                for (int j = 0; j < w3.GetLength(1); j++)
//                {
//                    w3[i, j] = Random.value * 2 - 1;  //получаем матрицу рандомных весовых коэффициентов
//                }
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        System.Random rnd = new System.Random();
//        timer_life -= Time.deltaTime;
//        timer_text.text = timer_life.ToString();
//        if(timer_life <= 0)
//        {
//            Start();
//        }
//        float moveHorizontal;
//        float moveVertical;
//        //moveHorizontal = rnd.Next(-100, 101)/10f;
//        //moveVertical = rnd.Next(-100, 101)/10f;
//        float moveUp = speed * 50f;

//        float rnd_jump;
//        //rnd_jump = rnd.Next(-10, 11);

//        float rnd_nitro;
//       // rnd_nitro = rnd.Next(-10, 11);

//        //нормирование
//        //float N_moveHorizontal, N_moveVertical, N_rnd_jump, N_rnd_nitro;
//        //N_moveHorizontal = moveHorizontal / 10f;
//        //N_moveVertical = moveVertical / 10f;
//        //N_rnd_jump = rnd_jump / 10f;
//        //N_rnd_nitro = rnd_nitro / 10f;

//        float Xpos, Zpos;
//        Xpos = Player.GetComponent<Transform>().position.x / 2500f; //позиция героя, нормированная от -1 до 1
//        Zpos = Player.GetComponent<Transform>().position.z / 2500f;



//        float[] res1 = new float[4];
//        for (int i = 0; i < w1.GetLength(1); i++)
//        {
//            res1[i] = 0;
//        }
//        for (int i = 0; i < w1.GetLength(1); i++)
//        {
//            res1[i] += Xpos * w1[0, i] + Zpos * w1[1, i];
//        }
//        //добавил вторую весовую матрицу
//        float[] res2 = new float[4];
//        for (int i = 0; i < w2.GetLength(1); i++)
//        {
//            res2[i] = 0;
//        }
//        for (int i = 0; i < w2.GetLength(0); i++)
//        {
//            for (int j = 0; j < w2.GetLength(1); j++)
//            {
//                res2[i] += res1[j] * w2[i, j] + w3[i, j];
//            }
//        }

//        float N_moveHorizontal, N_moveVertical, N_rnd_jump, N_rnd_nitro;
//        N_moveHorizontal = res2[0];
//        N_moveVertical = res2[1];
//        N_rnd_jump = res2[2];
//        N_rnd_nitro = res2[3];

//        moveHorizontal = N_moveHorizontal * 1f;
//        moveVertical = N_moveVertical * 1f;
//        rnd_jump = N_rnd_jump * 1f;
//        rnd_nitro = N_rnd_nitro * 1f;
//        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
//        if (grounded && rnd_jump > 0)
//        {
//            rb.AddForce(new Vector3(0f, moveUp, 0f));
//        }
//        if (grounded && rnd_nitro > 0) //прыжог
//        {
//            rb.AddForce(movement * speed * 5);
//        }
//        rb.AddForce(movement * speed);


//    }
//    public void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.tag == "Ground")
//        {
//            grounded = true; 
//        }
//    }
//    public void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.tag == "Ground")
//        {
//            grounded = false;
//        }
//    }
//}