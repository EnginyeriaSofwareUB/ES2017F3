using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSuddenDeath : MonoBehaviour {

	public GameObject sudden;
	public float transitionDuration = 3.0f;

	public void SuddenDeath() {
		StartCoroutine (ShowSudden());
	}

	IEnumerator ShowSudden() {
		sudden.SetActive (true);

		AudioManager am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		am.SuddenDeath();

		Image im = sudden.GetComponent<Image> ();
		im.CrossFadeColor (Color.black, transitionDuration, false, false);
		im.CrossFadeAlpha (0.01f, transitionDuration, false);

		yield return new WaitForSeconds(transitionDuration);
		sudden.SetActive (false);
		am.source.pitch = 2.0f;
	}
}
