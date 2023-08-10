using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSenario : MonoBehaviour
{
	public void GameStart()
	{
		SceneManager.LoadScene("GameScene");
	}
}
