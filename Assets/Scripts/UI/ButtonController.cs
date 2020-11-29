using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public string audioEvent;

    bool buttonPressed;
    bool buttonHighlighted;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        if(audioEvent != null)
        {
            AkSoundEngine.PostEvent(audioEvent, gameObject);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonHighlighted = true;
    }
}