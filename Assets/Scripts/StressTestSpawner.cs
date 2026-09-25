using UnityEngine;

public class StressTestSpawner : MonoBehaviour
{
    public int count = 200;
    public float areaSize = 8f;
    public float speed = 2f;

    GameObject[] cubes;
    Vector3[] directions;

    void Start()
    {
        cubes = new GameObject[count];
        directions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.position = new Vector3(
                Random.Range(-areaSize, areaSize),
                Random.Range(-areaSize, areaSize),
                Random.Range(-areaSize, areaSize)
            );

            cube.transform.localScale = Vector3.one * 0.3f;

            cubes[i] = cube;
            directions[i] = Random.onUnitSphere;
        }
    }

    void Update()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].transform.position += directions[i] * speed * Time.deltaTime;

            Vector3 p = cubes[i].transform.position;

            if (Mathf.Abs(p.x) > areaSize) directions[i].x *= -1;
            if (Mathf.Abs(p.y) > areaSize) directions[i].y *= -1;
            if (Mathf.Abs(p.z) > areaSize) directions[i].z *= -1;
        }
    }
}