using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SoundManager : MonoBehaviour {

	public AudioSource backgroundClip;
	private static SoundManager instance = null;

    public List<AudioSource> timeManagedSounds;
    public List<int> delayTimePerManagedSound;

    private Dictionary<int, List<AudioSource>> _timeManagedSoundsHash;
	public static SoundManager Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
        _timeManagedSoundsHash = new Dictionary<int, List<AudioSource>>();
        Awake ();
        
		if (backgroundClip != null && !backgroundClip.isPlaying)
			backgroundClip.Play();
	}
	public void DestroyMe()
	{
		backgroundClip.Pause ();
	}
	// Update is called once per frame
	void Update () {
        PlayTimeManageSounds();
    }

    private void PlayTimeManageSounds()
    {
        for (int i = 0; i < Mathf.Min(timeManagedSounds.Count, delayTimePerManagedSound.Count); i++)
        {
            if(delayTimePerManagedSound[i] == (int)LevelManager.GameTimer)
                timeManagedSounds[i].Play();
        }
    }


}
