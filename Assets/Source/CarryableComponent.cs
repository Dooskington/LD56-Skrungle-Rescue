using UnityEngine;

public class CarryableComponent : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isBeingCarried;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetIsBeingCarried(bool isBeingCarried)
    {
        _isBeingCarried = isBeingCarried;

        if (_isBeingCarried)
        {
            _rigidbody.isKinematic = true;
        }
        else
        {
            _rigidbody.isKinematic = false;
        }
    }
}
