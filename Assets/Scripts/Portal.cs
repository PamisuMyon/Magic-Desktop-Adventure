using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour 
{

    public bool isOn = false;

    public AudioClip openClip;
    public AudioClip transportClip;

    ParticleSystem[] particles;
    Collider col;
    AudioSource audioSource;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        particles = GetComponentsInChildren<ParticleSystem>();    
        col = GetComponent<Collider>();
        if (!isOn)
            Hide();
    }    

    public void Show()
    {
        col.enabled = true;
        foreach (var item in particles)
        {
            item.Play();
        }
        audioSource.clip = openClip;
        audioSource.Play();
    }

    public void Hide()
    {
        col.enabled = false;
        foreach (var item in particles)
        {
            item.Stop();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.isTrigger && other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().Teleported();
            audioSource.clip = transportClip;
            audioSource.Play();
            StartCoroutine(DelayNextLevel());
        }
    }

    IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.NextLevel();
    }

}