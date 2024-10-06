using UnityEngine;

public class AnimatorAudioComponent : MonoBehaviour
{
    [SerializeField] private AudioClip _footstep;
    [SerializeField] private AudioClip _climb;
    [SerializeField] private AudioClip _jump;

    private void AnimEvent_Footstep()
    {
        AudioEvent.Play3D(_footstep, transform.position);
    }

    private void AnimEvent_Climb()
    {
        AudioEvent.Play3D(_footstep, transform.position);
    }

    private void AnimEvent_Jump()
    {
        AudioEvent.Play3D(_jump, transform.position);
    }
}
