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
    public GameObject overlayBackground;

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
    private int[] pics; 

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

        if(LevelData.Instance.GameLevel == 2)
        {
            pics = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
            numsOfCards = 16;
        }
        else if(LevelData.Instance.GameLevel == 3)
        {
            pics = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9};
            numsOfCards = 20;
        }
        else
        {
            pics = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5};
            numsOfCards = 12;
        }

        pics = pics.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        if(LevelData.Instance.GameLevel == 1)
        {
            for (int i = 0; i < numsOfCards; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("cards").transform;

                float x = (i / 4) * 1.4f - 1.4f;
                float y = (i % 4) * 1.4f - 2.8f;
                newCard.transform.position = new Vector3(x, y, 0);

                string picName = "pic" + pics[i].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(picName);
            }
        }

        if(LevelData.Instance.GameLevel == 2)
        {
            for (int i = 0; i < numsOfCards; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("cards").transform;

                float y = (i / 4) * 1.4f - 2.8f;
                float x = (i % 4) * 1.4f - 2.1f;
                newCard.transform.position = new Vector3(x, y, 0);

                string picName = "pic" + pics[i].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(picName);
            }
        }

        if(LevelData.Instance.GameLevel == 3)
        {
            for (int i = 0; i < numsOfCards; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("cards").transform;

                float y = (i / 4) * 1.4f - 4.1f;
                float x = (i % 4) * 1.4f - 2.1f;
                newCard.transform.position = new Vector3(x, y, 0);

                string picName = "pic" + pics[i].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(picName);
            }
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
            overlayBackground.transform.SetAsLastSibling();
            overlayBackground.SetActive(true);
            endWindow.transform.SetAsLastSibling();
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
        else
        {
            CalculateScore(); // ���� �� (30�� �̸����� ���� ���� ��) ���� ��� �Լ�
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
                overlayBackground.transform.SetAsLastSibling();
                overlayBackground.SetActive(true);
                endWindow.transform.SetAsLastSibling();
                endWindow.SetActive(true);

                endTxt.text = "����";
                endTxt.color = Color.yellow;
                scoreTxt.text = (100 - numsOfTrials).ToString();

                Time.timeScale = 0.0f;
                AudioManager.Instance.StopBGM();
            }
            DecreaseTimeOnMatchSuccess(); // ��ġ ���� �� �ð� 1�� ���� �Լ� ȣ��
        }
        else
        {
            IncreaseTimeOnMatchFail(); // ��ġ ���� �� �ð� 2�� ���� �Լ� ȣ��

            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            nameTxt.text = "����";
        }

        firstCard = null;
        secondCard = null;
    }




    public void IncreaseTimeOnMatchFail() // ��ġ ���� �� �ð� 2�� ����
    {
        time += 2.0f; // �ð� 2�� ����
        timeTxt.text = time.ToString("N2"); // UI�� �ð� ������Ʈ

        // ���� ó��
        if (time > 30.0f) // �ð��� 30�� �̻��̸�
        {
            overlayBackground.transform.SetAsLastSibling();
            overlayBackground.SetActive(true);
            endWindow.transform.SetAsLastSibling();
            endWindow.SetActive(true);

            AudioManager.Instance.StopBGM();

            endTxt.text = "����"; 
            endTxt.color = Color.red; 
            scoreTxt.text = "0"; 
            Time.timeScale = 0.0f; 
        }
    }

    private void DecreaseTimeOnMatchSuccess() // ��ġ ���� �� �ð� 1�� ����
    {
        if (time >= 1.0f) 
        {
            time -= 1.0f; // �ð� 1�� ����
            timeTxt.text = time.ToString("N2"); // UI�� �ð� ������Ʈ
        }
    }

    void CalculateScore()     // �ð��� ���� ������ ���

    {
        int baseScore = 100; // �⺻����
        int timeBonus = 0;

        if (time >= 25.0f)
        {
            timeBonus = -15; // ���� �ð��� 25�� �̻��� ��� 15�� ����
        }
        else if (time >= 20.0f)
        {
            timeBonus = -10; // 20�� �̻� 10�� ����
        }
        else if (time >= 15.0f)
        {
            timeBonus = -5; // 15�� �̻� 5�� ����
        }
        // 15�� �̸� ���� 0

        int finalScore = Mathf.Max(0, baseScore + timeBonus - numsOfTrials); // ���� ����
        scoreTxt.text = finalScore.ToString(); // ���� UI�� ����
    }





    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene("IntroScene");
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
