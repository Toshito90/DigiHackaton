using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField] float interactionRange = 2f;
    [SerializeField] float faceViewThreshold = -0.8f;
    [SerializeField] GameObject floatingTextUI;
    [SerializeField] bool destroyAfterInteraction = true;
    [SerializeField] UnityEvent events;

    bool canSpawnFloatingTextInteract = true;
    GameObject floatingInteractUI = null;

    bool canInteract = false;

    [SerializeField] Player currentPlayer = null;

    public event Action<Transform> onInteract;

	// Start is called before the first frame update
	void Start()
    {
        SphereCollider sphereColl = gameObject.AddComponent<SphereCollider>();
        sphereColl.isTrigger = true;
        sphereColl.radius = interactionRange;

        currentPlayer = FindObjectOfType<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        currentPlayer = other.gameObject.GetComponent<Player>();
        if (currentPlayer != null)
        {
            Vector3 dir = (currentPlayer.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(dir, currentPlayer.transform.forward);
            if (dotProduct < faceViewThreshold)
            {
                Mover mover = currentPlayer.GetComponent<Mover>();
                if (floatingInteractUI == null && canSpawnFloatingTextInteract && !mover.IsPaused())
                {
                    floatingInteractUI = Instantiate(floatingTextUI, transform.position, transform.rotation);
                }
                canInteract = true;

                if (onInteract != null) onInteract(currentPlayer.transform);
            }
            else
            {
                if (floatingInteractUI != null)
                {
                    Destroy(floatingInteractUI);
                    floatingInteractUI = null;
                }
                canInteract = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentPlayer = other.gameObject.GetComponent<Player>();
        if (currentPlayer != null)
        {
            if (floatingInteractUI != null)
            {
                Destroy(floatingInteractUI);
                floatingInteractUI = null;
            }
            canInteract = false;

            currentPlayer = null;
        }
    }

    private void Update()
    {
        if (canInteract)
        {
            // Just temporary code
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (events != null)
                {
                    events.Invoke();
                }

                if (destroyAfterInteraction)
                {
                    DestroyInteractText();
                }
            }
        }
    }

    public void DestroyInteractText()
    {
        if (floatingInteractUI != null)
        {
            if (currentPlayer != null) currentPlayer = null;

            Destroy(floatingInteractUI);
            floatingInteractUI = null;
        }
    }

    public void TeleportToLocation(Transform positionReference)
    {
        if (currentPlayer != null)
        {
            currentPlayer.transform.position = positionReference.position;
            Camera cam = Camera.main;
            cam.transform.position = currentPlayer.transform.position;
        }
    }

    public void SetSpawnFloatingText(bool canSpawnFloatingTextInteract)
	{
        this.canSpawnFloatingTextInteract = canSpawnFloatingTextInteract;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
