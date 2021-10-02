using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]
public class StickyNotePickable : MonoBehaviour
{

    public Vector3 rotateSpeed;
    public Vector3 rotateSpeed1;
    public bool random;
    public StickyNote stickyNote;
    public float animDuration0;
    public float animDuration1;

    // [Space]
    // public AudioClip pickedClip;

    // AudioSource audioSource;

    public delegate void OnEnterInventory(StickyNote sticky);

    void Start()
    {
        // audioSource = GetComponent<AudioSource>();

        if (random)
            rotateSpeed = new Vector3(
                Random.Range(rotateSpeed.x, rotateSpeed1.x),
                Random.Range(rotateSpeed.y, rotateSpeed1.y),
                Random.Range(rotateSpeed.z, rotateSpeed1.z)
            );

        GetComponent<Renderer>().sharedMaterial = stickyNote.material;
    }

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    public StickyNote AnimateToInventory(Vector3 targetPos, OnEnterInventory onEnter = null)
    {
        GetComponent<Collider>().enabled = false;
        var seq = DOTween.Sequence();
        var upPos = transform.position + new Vector3(0, .8f, 0);
        targetPos.Set(transform.position.x, targetPos.y, transform.position.z - 5);
        seq.Append(transform.DOMove(upPos, animDuration0));
        seq.Append(transform.DOMove(targetPos, animDuration1));
        seq.OnComplete(() =>
        {   
            if (onEnter != null)
                onEnter(stickyNote);
            Destroy(gameObject);
        });
        seq.Play();
        return stickyNote;

        // audioSource.clip = pickedClip;
        // audioSource.pitch = Random.Range(0.8f, 1.2f);
        // audioSource.Play();
    }

}
