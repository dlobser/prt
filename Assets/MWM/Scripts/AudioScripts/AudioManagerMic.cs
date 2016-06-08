using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * AUDIO MANAGER - 
 * 
 * This class does a few things.
 * 
 * 1) Figures out at what volume the audio should be playing at, given the difference between the rotation of the user and the object. It also serves
 *    to start the audio clip when the user first orients to that rotation, within the fullVolumeRange.
 *    
 * 
 * 
 */

[RequireComponent(typeof(AudioSource))]
public class AudioManagerMic : MonoBehaviour
{
	
	private AudioSource _aud;
	public float bpm = 120f;
	
	public bool SPHERES = true;
	public int numOfSamples = 8192;
	public float spectrumRefreshRate = 30f;
	
	private float[] freqData;
	private float[] band;
	private GameObject[] g;
	
	public float volume = 0f;
	public bool isPlaying = false;

	bool setupMic = true;
	
	// Use this for initialization
	void Start()
	{

	}
	
	void Update()
	{
		if (setupMic) {
			_aud = this.GetComponent<AudioSource> ();
			_aud.clip = Microphone.Start ("Built-in Microphone", true, 999, numOfSamples);
			while (!(Microphone.GetPosition("Built-in Microphone") > 0)) {
				_aud.Play ();
			}
			//		_aud.Stop();
			
			SetUpSpectrum ();
			setupMic=false;
		}
		this.isPlaying = _aud.isPlaying;
		_aud.volume = this.volume;
	}
	
	void SetUpSpectrum()
	{
		freqData = new float[numOfSamples];
		
		int n = freqData.Length;
		
		int k = 0;
		for (int j = 0; j < freqData.Length; j++)
		{
			n = n / 2;
			if (n <= 0) break;
			k++;
		}
		
		band = new float[k + 1];
		
		if (SPHERES)
		{
			g = new GameObject[k + 1];
			
			for (int i = 0; i < band.Length; i++)
			{
				band[i] = 0;
				g[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				g[i].transform.position = this.transform.position + new Vector3(i, 0, 0);
			}
		}
		InvokeRepeating("check", 0.0f, 1.0f / spectrumRefreshRate);

	}
	
	private void check()
	{
		_aud.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);
		
		int k = 0;
		int crossover = 2;
		
		for (int i = 0; i < freqData.Length; i++)
		{
			float d = freqData[i];
			float b = band[k];
			
			// find the max as the peak value in that frequency band.
			band[k] = (d > b) ? d : b;
			
			if (i > (crossover - 3))
			{
				k++;
				crossover *= 2;   // frequency crossover point for each band.
				if (SPHERES && g != null)
				{
					Vector3 tmp = new Vector3(g[k].transform.position.x, band[k] * 32, g[k].transform.position.z);
					g[k].transform.position = tmp;
				}
				band[k] = 0;
			}
		}
	}
	
	public float GetBands(int num)
	{
		return band[num];
	}
	
	public float GetBands(int[] nums)
	{
		float sum = 0f;
		
		for (int i = 0; i < nums.Length; i++)
		{
			sum += band[nums[i]];
		}
		
		return sum;
	}
	
	public float GetBands(List<int> nums)
	{
		float sum = 0;
		for (int i = 0; i < nums.Count; i++)
		{
			sum += band[nums[i]];
		}
		return sum;
	}
	
	//AUDIO CONTROLS
	public void Play()
	{
		_aud.Play();
	}
	
	public void Pause() { _aud.Pause(); }
	public void UnPause() { _aud.UnPause(); }
	public void Stop() { _aud.Stop();  }
}
