using UnityEngine.UI;
using UnityEngine;


public class SettingPanel : MonoBehaviour
{

	public Slider bgmVolume;
	public Slider sfxVolume;
	public Toggle bgmMute;
	public Toggle sfxMute;

	public void ApplySettings()
	{
		AudioManager.Instance.SetVolume(bgmMute.isOn,sfxMute.isOn,bgmVolume.value,sfxVolume.value);
	}
}
