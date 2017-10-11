using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
	public Toggle fullscreenToggle;
	public Dropdown resolutionDropdown;
	public Dropdown textureQualityDropdown;
	public Dropdown antialiasingDropdown;
	public Dropdown vSyncDropdown;
	public Slider musicVolumeSlider;

	public Resolution[] resolutions;
	public GameSettings gameSettings;

	void OnEnable()
	{
		gameSettings = new GameSettings ();
		resolutions = Screen.resolutions;
	}

	public void OnFullscreenToggle()
	{
		
	}

	public void OnResolutionChange()
	{

	}

	public void OnTextureQualityChange()
	{
		
	}

	public void OnAntialiasingChange()
	{

	}

	public void OnVSyncChange()
	{

	}

	public void OnMusicVolumeChange()
	{

	}
}
