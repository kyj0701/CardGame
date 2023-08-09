using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioMatch;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endWindow;

    public Text timeTxt;
    public Text nameTxt;
    public Text trialTxt;
    public Text endTxt;
    public Text scoreTxt;
    float time = 0.0f;

    public static gameManager I;

    public int numsOfCards = 12;

    private int numsOfMatches = 0;
    private int numsOfTrials = 0;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        numsOfMatches = 0;

        int[] pics = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

        pics = pics.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < numsOfCards; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 1.38f;
            float y = (i % 4) * 1.4f - 3.5f;
            newCard.transform.position = new Vector3(x, y, 0);

            string picName = "pic" + pics[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(picName);

        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time > 30.0f)
        {
            endWindow.SetActive(true);

            endTxt.text = "실패";
            endTxt.color = Color.red;
            scoreTxt.text = "0";

            Time.timeScale = 0.0f;
        }
        else
        {
            CalculateScore(); // 성공 시 (30초 미만으로 게임 종료 시) 점수 계산 함수
        }

    }

    public void isMatched()
    {
        numsOfTrials++;
        trialTxt.text = numsOfTrials.ToString();

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            numsOfMatches += 2;

            audioSource.PlayOneShot(audioMatch);
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();
            if (firstCardImage == "pic3" || secondCardImage == "pic3" || firstCardImage == "pic4" || secondCardImage == "pic4")
            {
                nameTxt.text = "최수용";
            }
            else if (firstCardImage == "pic1" || secondCardImage == "pic1" || firstCardImage == "pic2" || secondCardImage == "pic2")
            {
                nameTxt.text = "김도현";
            }
            else
            {
                nameTxt.text = "김예준";
            }

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            Debug.Log(cardsLeft);
            if (cardsLeft == 2)
            {
                endWindow.SetActive(true);

                endTxt.text = "성공";
                endTxt.color = Color.yellow;
                scoreTxt.text = (100 - numsOfTrials).ToString();

                Time.timeScale = 0.0f;
            }
            DecreaseTimeOnMatchSuccess(); // 매치 성공 시 시간 1초 감소 함수 호출
        }
        else
        {
            IncreaseTimeOnMatchFail(); // 매치 실패 시 시간 2초 증가 함수 호출

            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            nameTxt.text = "실패";
        }

        firstCard = null;
        secondCard = null;
    }




    public void IncreaseTimeOnMatchFail() // 매치 실패 시 시간 2초 증가
    {
        time += 2.0f; // 시간 2초 증가
        timeTxt.text = time.ToString("N2"); // UI에 시간 업데이트

        // 실패 처리
        if (time > 30.0f) // 시간이 30초 이상이면
        {
            endWindow.SetActive(true);
            endTxt.text = "실패"; 
            endTxt.color = Color.red; 
            scoreTxt.text = "0"; 
            Time.timeScale = 0.0f; 
        }
    }

    private void DecreaseTimeOnMatchSuccess() // 매치 성공 시 시간 1초 감소
    {
        if (time >= 1.0f) 
        {
            time -= 1.0f; // 시간 1초 감소
            timeTxt.text = time.ToString("N2"); // UI에 시간 업데이트
        }
    }

    void CalculateScore()     // 시간에 따라 점수를 계산

    {
        int baseScore = 100; // 기본점수
        int timeBonus = 0;

        if (time >= 25.0f)
        {
            timeBonus = -15; // 남은 시간이 25초 이상인 경우 15점 차감
        }
        else if (time >= 20.0f)
        {
            timeBonus = -10; // 20초 이상 10점 차감
        }
        else if (time >= 15.0f)
        {
            timeBonus = -5; // 15초 이상 5점 차감
        }
        // 15초 미만 차감 0

        int finalScore = Mathf.Max(0, baseScore + timeBonus - numsOfTrials); // 최종 점수
        scoreTxt.text = finalScore.ToString(); // 점수 UI에 적용
    }





    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
