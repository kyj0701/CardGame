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
    public float time = 0.0f;
    public float openLimitTime;

    public static gameManager I;

    public int numsOfCards = 12;

    private int numsOfTrials = 0;
    private int bgmStatus = 0; // ���� ���¿� ���� BGM �� ������ ����Ǿ��ٰ� ����ǵ��� �����ϱ� ���� ����
    
    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���� ���۽� ����� BGM �÷��� default = 1f, ���ϴ� �ӵ��� �ִ� ��쿡�� flaot ���� ������ �۵�
        AudioManager.Instance.PlayBGM();
        Time.timeScale = 1.0f;

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
            // 20�� ����� BGM�� ����ӵ��� 2�� �÷��� �ѹ��� �۵��ϵ���  bgm Status �� ����
            if( bgmStatus != 1)
            {
                bgmStatus = 1;
                AudioManager.Instance.PlayBGM(2.0f);
            }
        }

        if (time > 30.0f)
        {
            endWindow.SetActive(true);

            endTxt.text = "����";
            endTxt.color = Color.red;
            scoreTxt.text = "0";

            Time.timeScale = 0.0f;
            // BGM�� ���� ���� ���� �ѹ��� �۵��ϵ��� bgm Status �� ����
            if (bgmStatus != 2) 
            {
                bgmStatus = 2;
                AudioManager.Instance.StopBGM();
            }
            
        }
        
        if (firstCard != null && secondCard == null)
        {
            if (openLimitTime < time)
            {
                firstCard.GetComponent<card>().closeCard();
                firstCard = null;
                nameTxt.text = "����";
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
            //��Ī ������ ����
            AudioManager.Instance.PlayClip(audioMatch);
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();
            if (firstCardImage == "pic3" || secondCardImage == "pic3" || firstCardImage == "pic4" || secondCardImage == "pic4")
            {
                nameTxt.text = "�ּ���";
            }
            else if (firstCardImage == "pic1" || secondCardImage == "pic1" || firstCardImage == "pic2" || secondCardImage == "pic2")
            {
                nameTxt.text = "�赵��";
            }
            else
            {
                nameTxt.text = "�迹��";
            }

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            Debug.Log(cardsLeft);
            if (cardsLeft == 2)
            {
                endWindow.SetActive(true);

                endTxt.text = "����";
                endTxt.color = Color.yellow;
                scoreTxt.text = (100 - numsOfTrials).ToString();

                Time.timeScale = 0.0f;
            }
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            nameTxt.text = "����";
        }

        firstCard = null;
        secondCard = null;
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
