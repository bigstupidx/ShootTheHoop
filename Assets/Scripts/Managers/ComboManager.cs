using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboManager : MonoBehaviour {

    private TextMesh comboText;
    private Transform comboTextTransform;
    private Animator anim;

	// Use this for initialization
	void Awake () {
        comboTextTransform = GetComponent<Transform>();
        comboText = GetComponent<TextMesh>();
        anim = GetComponent<Animator>();
	}

    public void showText()
    {
        if (ScoreManager.successfulShotsInARow > 1) {
            comboText.text = ScoreManager.successfulShotsInARow + "x Combo";
            StartCoroutine(Delay(1f));
            comboTextTransform.LookAt(Camera.main.transform);
            comboTextTransform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
            anim.SetTrigger("playAnim");
        }
    }

    IEnumerator Delay(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
