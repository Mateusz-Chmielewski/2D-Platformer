using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    [SerializeField]
	Transform rotationCenter;

	[SerializeField]
	float rotationRadius = 2f, angularSpeed = 2f;

	float posX = 0f, posY = 0f, angle = 0f;

    static int PLATFORMS_NUM = 8;
    GameObject[] platforms;
    Vector3[] positions;
    //private float time = 0f;
    // Start is called before the first frame update

    void Awake()
    {

        platforms = new GameObject[PLATFORMS_NUM];
        for (int i = 0; i < PLATFORMS_NUM-2; i++)
        {
            platforms[i] = Instantiate(platformPrefab, new Vector3(40 + 6 * Mathf.Sin((2 * Mathf.PI / PLATFORMS_NUM) * i),10*i-5), Quaternion.identity);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PLATFORMS_NUM-2; i++)
        {
            posX = rotationCenter.position.x + Mathf.Cos(angle + (i * 360 / PLATFORMS_NUM)) * rotationRadius;
            posY = rotationCenter.position.y + Mathf.Sin(angle + (i * 360 / PLATFORMS_NUM)) * rotationRadius;
            platforms[i].transform.position = new Vector2(posX, posY);
        }
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360f)
            angle = 0f;
    }
}
