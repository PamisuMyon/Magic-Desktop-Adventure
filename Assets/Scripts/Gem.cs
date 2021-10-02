using UnityEngine;
using DG.Tweening;

public class Gem : MonoBehaviour 
{

    public Vector3 rotateSpeed;
    public Vector3 rotateSpeed1;
    public bool random;
    public float floating;
    public float floatingAnimDuration;
    public float pickedAnimDuration;

    [Space]
    public AudioClip pickedClip;

    AudioSource audioSource;
    Sequence sequence;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (random)
            rotateSpeed = new Vector3(
                Random.Range(rotateSpeed.x, rotateSpeed1.x),
                Random.Range(rotateSpeed.y, rotateSpeed1.y),
                Random.Range(rotateSpeed.z, rotateSpeed1.z)
            );
        sequence = DOTween.Sequence();
        var tweener = transform.DOMove(transform.position + new Vector3(0, floating, 0), floatingAnimDuration).SetEase(Ease.InOutSine);
        sequence.Append(tweener);
        tweener = transform.DOMove(transform.position, floatingAnimDuration).SetEase(Ease.InOutSine);
        sequence.Append(tweener).SetLoops(-1);
        sequence.Play();
    }

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    public void Picked()
    {
        GetComponent<Collider>().enabled = false;
        sequence.Kill();
        audioSource.clip = pickedClip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        transform.DOMove(transform.position + new Vector3(0, floating * 2, 0), pickedAnimDuration);
        transform.DOScale(new Vector3(0, 0, 0), pickedAnimDuration).OnComplete(() => 
        {
            LevelManager.Instance.GemCollected();
            Destroy(gameObject);
        });
    }

}