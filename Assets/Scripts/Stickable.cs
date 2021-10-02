using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stickable : MonoBehaviour
{
    public GameObject sticked;
    public StickyNote stickyNote;
    public float levitationForce;
    public float animateDuration;
    public Vector3 scalingOffset;
    public float stickyPostionRandom;
    public float stickyRotationRandom;

    [Space]
    public AudioClip stickOnClip;
    public AudioClip stickOffClip;
    
    GameObject hint;
    Text hintText;
    AudioSource audioSource;

    Rigidbody rb;
    Vector3 currentForce;

    int currentEffect = -1;

    public bool CanStick { get { return !sticked.activeInHierarchy; } }

    void Start()
    {
        hint = transform.Find("Canvas").gameObject;
        hintText = transform.Find("Canvas/Text").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();

        if (GameManager.Instance.isTouchInput)
        {
            transform.Find("Canvas/ImageF").gameObject.SetActive(false);
            transform.Find("Canvas/ImageSticky").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Canvas/ImageF").gameObject.SetActive(true);
            transform.Find("Canvas/ImageSticky").gameObject.SetActive(false);            
        }

        if (stickyNote)
        {
            StickOn(stickyNote);
        }
        else
        {
            sticked.SetActive(false);
            ChangePhysics(transform.localScale.x);
        }
        HideHint();
    }

    private void FixedUpdate() 
    {
         if (currentForce != Vector3.zero)
         {
            rb.AddForce(currentForce, ForceMode.Acceleration);
         }
    }

    void UpdateText()
    {
        if (CanStick)
            hintText.text = "STICK";
        else
            hintText.text = "REMOVE";
    }

    public void ShowHint()
    {
        UpdateText();
        hint.SetActive(true);
    }
    
    public void HideHint()
    {
        hint.SetActive(false);
    }

    public void StickOn(StickyNote sticky)
    {
        stickyNote = sticky;
        sticked.GetComponent<Renderer>().sharedMaterial = sticky.material;
        sticked.SetActive(true);
        var xRandom = Random.Range(-stickyPostionRandom, stickyPostionRandom) * transform.localScale.x;
        var yRandom = Random.Range(-stickyPostionRandom, stickyPostionRandom) * transform.localScale.y;
        var rotZRandom = Random.Range(-stickyRotationRandom, stickyRotationRandom);
        sticked.transform.localPosition += new Vector3(xRandom, yRandom, 0);
        sticked.transform.Rotate(new Vector3(0, 0, rotZRandom), Space.Self);
        UpdateText();
        ApplyEffect();

        audioSource.clip = stickOnClip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void Remove()
    {
        stickyNote = null;
        sticked.SetActive(false);
        UpdateText();
        RewindEffect();

        audioSource.clip = stickOffClip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void ApplyEffect()
    {
        if (currentEffect != stickyNote.number)
        {
            currentEffect = stickyNote.number;
            if (currentEffect == 0)
            {
                rb.useGravity = false;
                // rb.AddForce(new Vector3(0, levitationForce, 0), ForceMode.Acceleration);
                currentForce = new Vector3(0, levitationForce, 0);
            }
            else if (currentEffect == 1)
            {
                var scale = transform.localScale;
                scale *= 2;
                transform.DOScale(scale, animateDuration);
                if (scale.x >= 1)
                {
                    // transform.position -= new Vector3(0, scalingOffset.y, 0);
                    transform.DOMove(transform.position - new Vector3(0, scalingOffset.y, scalingOffset.z), animateDuration);
                }
                ChangePhysics(scale.x);
            }
            else if (currentEffect == 2)
            {
                var scale = transform.localScale;
                scale /= 2;
                transform.DOScale(scale, animateDuration);
                if (scale.x >= 1)
                    transform.DOMove(transform.position + new Vector3(0, 0, scalingOffset.z), animateDuration);
                ChangePhysics(scale.x);
            }
        }
    }

    public void RewindEffect()
    {
        if (currentEffect == 0)
        {
            rb.useGravity = true;
            currentForce = Vector3.zero;
        }
        else if (currentEffect == 1)
        {
            var scale = transform.localScale;
            scale /= 2;
            transform.DOScale(scale, animateDuration);
            if (scale.x >= 1)
                transform.DOMove(transform.position + new Vector3(0, 0, scalingOffset.z), animateDuration);
            ChangePhysics(scale.x);
        }
        else if (currentEffect == 2)
        {
            var scale = transform.localScale;
            scale *= 2;
            transform.DOScale(scale, animateDuration);
            if (scale.x >= 1)
            {
                // transform.position -= new Vector3(0, scalingOffset.y, 0);
                transform.DOMove(transform.position - new Vector3(0, scalingOffset.y, scalingOffset.z), animateDuration);
            }
            ChangePhysics(scale.x);
        }
        currentEffect = -1;
    }

    void ChangePhysics(float scaleX)
    {
        if (scaleX > 1)
        {
            rb.mass = 200;
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        else
        {
            rb.mass = 8;
            gameObject.layer = LayerMask.NameToLayer("Standable");
        }
    }

}
