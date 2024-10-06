using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioUIComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _buttonClickAudio;
    [SerializeField] private AudioClip _buttonHoverAudio;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioEvent.Play(_buttonClickAudio);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioEvent.Play(_buttonHoverAudio);
    }
}
