using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    public GameObject wheelPrefab;
    GameObject[] wheelPrefabs = new GameObject[4];

    Vector3[] wheels = new Vector3[4];
    public Vector2 wheelDistance = new Vector2(2, 2);
    float[] oldDist = new float[4];

    float maxSuspentionLength = 3f;
    float suspentionMultiplier = 120f;
    float dampSensitivity = 500f;
    float maxDamp = 40f;

    Rigidbody rb;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            oldDist[i] = maxSuspentionLength;
            wheelPrefabs[i] = Instantiate(wheelPrefab, wheels[i], Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        wheels[0] = transform.right * wheelDistance.x + transform.forward * wheelDistance.y; //front right
        wheels[1] = transform.right * -wheelDistance.x + transform.forward * wheelDistance.y; //front left
        wheels[2] = transform.right * wheelDistance.x + transform.forward * -wheelDistance.y; //back right
        wheels[3] = transform.right * -wheelDistance.x + transform.forward * -wheelDistance.y; //back left

        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + wheels[i], -transform.up, out hit, maxSuspentionLength);
            if (hit.collider != null)
            {
                rb.AddForceAtPosition((Mathf.Clamp(maxSuspentionLength - hit.distance, 0, 3) * suspentionMultiplier * transform.up + transform.up *
                    Mathf.Clamp((oldDist[i] - hit.distance) * dampSensitivity, 0, maxDamp)) * Time.deltaTime, transform.position + wheels[i]);
                wheelPrefabs[i].transform.position = hit.point + transform.up * 0.5f;
                wheelPrefabs[i].transform.rotation = transform.rotation;
            }
            else
            {
                wheelPrefabs[i].transform.position = transform.position + wheels[i] - transform.up * (maxSuspentionLength - 0.5f);
                wheelPrefabs[i].transform.rotation = transform.rotation;
            }
            oldDist[i] = hit.distance;
        }
    }
}
