using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform pointA;
    private Transform pointB;
    public GameObject lineRendered_GameObject;

    private LineRenderer lineRenderer;
    private Vector3[] pathCorners;

    [SerializeField] private Transform destinationAgent;

    public float startWidth = 0.3f, height = 1f;
    public float endWidth = 0.4f;
    public Material lineMaterial;
    public bool routeAssigned = false;
    public GameObject currentPackage;

    private Animator _animator;
    private Transform originalParent; // Variable to store the original parent


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        //pointA = gameObject.transform;
        //pointB = destinationAgent;

        // Get or add Line Renderer component
        lineRenderer = lineRendered_GameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            Debug.Log("line red was null");
        }
        // Set Line Renderer properties
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default")); // Use default material if no material provided


        // Generate the route between points A and B
        //GenerateRoute(pointA.position, pointB.position);
    }

    // Update is called once per frame
    void Update()
    {

        // Check if pointB position has changed
   
            if (routeAssigned)
            {
                GenerateRoute(pointA.position, pointB.position);
            }
            else
            {
                destinationAgent = null;
                lineRenderer.positionCount = 0;
            }
        



        // Check for input to trigger debug actions
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            _animator.enabled = false;

        }

        // Check for input to trigger debug actions
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {

            //_animator.SetTrigger("Stand");

        }
    }

    public void AssignRoute(Transform startPoint, Transform endPoint)
    {
        pointA = startPoint;
        pointB = endPoint;
        destinationAgent = endPoint;
        routeAssigned = true;

        // add animation here
    }

    public void SwitchPlayer(Transform startPoint)
    {
        pointA = startPoint;
    }

    // Generate the route between two points using NavMesh
    void GenerateRoute(Vector3 startPoint, Vector3 endPoint)
    {
        NavMeshPath path = new NavMeshPath();

        // Calculate the route between startPoint and endPoint
        if (NavMesh.CalculatePath(gameObject.transform.position, endPoint, NavMesh.AllAreas, path))
        {
            // Set Line Renderer positions
            pathCorners = path.corners;

            // Set Line Renderer positions
            lineRenderer.positionCount = pathCorners.Length;
            for (int i = 0; i < pathCorners.Length; i++)
            {
                // Adjust Y position to be slightly above the ground
                pathCorners[i].y = GetHeightAtPoint(pathCorners[i]) + height; // Adjust as needed

                lineRenderer.SetPosition(i, pathCorners[i]);
            }
        }
        else
        {
            Debug.LogWarning("Failed to generate route between points.");
        }
    }


    // Get the height at a specific point on the terrain
    float GetHeightAtPoint(Vector3 point)
    {
        // Cast a ray from above to find the height of the terrain or other objects
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(point.x, 1000f, point.z), Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y;
        }
        else
        {
            return 0f; // Default height if raycast fails
        }
    }

    public void AssignPackage(GameObject package)
    {
        currentPackage = package;
    }

    public void TranslatePackage(GameObject dest)
    {
        if(currentPackage != null)
        {
            // Before changing the parent, store the current parent
            if (currentPackage.transform.parent != null)
            {
                originalParent = currentPackage.transform.parent;
            }
            else
            {
                // If the currentPackage has no parent, store null
                originalParent = null;
            }

            if (currentPackage != null)
            {
                // Change the parent to the destination GameObject
                currentPackage.transform.SetParent(dest.transform);
                currentPackage.transform.localPosition = Vector3.zero;
            }
        }

    }

    public void RestorePackageParent()
    {
        if (currentPackage != null)
        {
            // If originalParent is not null, restore the parent
            if (originalParent != null)
            {
                currentPackage.transform.SetParent(originalParent);
            }
            else
            {
                // If originalParent was null, detach the currentPackage (making it a root GameObject)
                currentPackage.transform.SetParent(null);
            }

            currentPackage.transform.localPosition = Vector3.zero;         // Optionally, reset the local position if necessary
            
        }

    }


    public void SetAnimState(string statename)
    {
        if(statename == "drive")
        {
            _animator.SetBool("Driving", true);
        }
        if(statename == "detach")
        {
            _animator.SetBool("Driving", false);
        }
        if (statename == "pickup")
        {
            _animator.SetTrigger("pickup");
        }

    }
}
