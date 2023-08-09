using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class gameManager : MonoBehaviour
{
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
    private int bgmStatus = 0; // 게임 상태에 따라서 BGM 이 빠르게 재생되었다가 종료되도록 설정하기 위한 숫자
    
    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 게임 시작시 오디오 BGM 플레이 default = 1f, 원하는 속도가 있는 경우에는 flaot 값을 넣으면 작동
        AudioManager.Instance.PlayBGM();
        Time.timeScale = 1.0f;
        numsOfMatches = 0;

        int[] pics = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5};

        pics = pics.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < numsOfCards; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f -1.38f;
            float y = (i % 4) * 1.4f -3.5f;
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

        if ( time >= 20.0f )
        {
            // 20초 경과시 BGM의 재생속도를 2로 플레이 한번만 작동하도록  bgm Status 를 넣음
            if( bgmStatus != 1)
            {
                bgmStatus = 1;
                AudioManager.Instance.PlayBGM(2.0f);
            }
        }

        if (time > 30.0f)
        {
            endWindow.SetActive(true);

            endTxt.text = "실패";
            endTxt.color = Color.red;
            scoreTxt.text = "0";

            Time.timeScale = 0.0f;
            // BGM은 따로 사운드 종료 한번만 작동하도록 bgm Status 를 넣음
            if (bgmStatus != 2) 
            {
                bgmStatus = 2;
                AudioManager.Instance.StopBGM();
            }
            
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
            //매칭 성공시 사운드
            AudioManager.Instance.PlayClip(audioMatch);

            numsOfMatches += 2;

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
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            nameTxt.text = "실패";
        }

        firstCard = null;
        secondCard = null;
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }


    public void PauseGame()
    {
        AudioManager.Instance.PauseSound();
        Time.timeScale = 0.0f;
    }

    public void UnPauseGame()
    {
        AudioManager.Instance.UnPauseSound();
        Time.timeScale = 1.0f;
    }
}
