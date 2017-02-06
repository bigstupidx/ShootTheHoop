using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesManager : MonoBehaviour {

    private ParticleSystem pSystem;

	// Use this for initialization
	void Awake () {
        pSystem = GetComponent<ParticleSystem>();
        pSystem.Stop();
	}

    public void EnableFlames()
    {
        if(pSystem.isStopped && ScoreManager.successfulShotsInARow > 1)
        {
            pSystem.Play();
        }
        else if(pSystem.isPlaying)
        {
            pSystem.Emit(30);
        }
    }

    public void DisableFlames()
    {
        if(pSystem.isPlaying)
        {
            pSystem.Stop();
        }
    }
}
