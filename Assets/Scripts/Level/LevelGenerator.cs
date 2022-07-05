using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private LevelManager levelManager;
    private BezierCurvePath bezier;

    public Level lvl;
    public GameObject tempPOST;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
        bezier = this.gameObject.AddComponent<BezierCurvePath>();
        lvl = levelManager.level;
        tempPOST = levelManager.GK;
    }

    private void Start()
    {
        generateLevel(lvl);
    }

    private void generateLevel(Level level)
    {
        GameObject map = Instantiate(level.map, transform);
        map.name = "level " + level.name;
        Vector3 mapPos = map.transform.position;
        mapPos.y -= 2;
        map.transform.position = mapPos;

        List<Vector3[]> points = getData(level);
        foreach (var i in points)
        {
            bezier.generateLevel(i, level.thickness, level.material);
        }

        bezier.CreateMesh();
        transform.position = new Vector3(-level.thickness / 2, 6, 0);


        for (int i = 0; i < points.Count; i++)
        {
            Transform spline = transform.Find("Curve" + i);
            Transform[] sides = new Transform[4];
            sides[0] = spline.Find("PlaneFront");
            sides[1] = spline.Find("PlaneBack");
            sides[2] = spline.Find("PlaneLeft");
            sides[3] = spline.Find("PlaneRight");
            foreach (Transform t in sides)
            {
                t.GetComponent<MeshRenderer>().material = level.barrierMaterial;

                if (t.name == "PlaneLeft" || t.name == "PlaneRight")
                {
                    Vector3 temp = t.localPosition;
                    temp.y += 1.5f;
                    t.localPosition = temp;

                }

                if(i == 0 && t.name == "PlaneFront")
                {
                    Vector3 temp = t.localPosition;
                    temp.y += 1.5f;
                    t.localPosition = temp;
                }

                if (i == points.Count - 1 && t.name == "PlaneBack")
                {
                    Vector3 temp = t.localPosition;
                    temp.y += 1.5f;
                    t.localPosition = temp;
                }
            }
        }

        Vector3[] forGoalPosition = points[points.Count - 1];
        Vector3 GoalPosition = (forGoalPosition[3] + forGoalPosition[2]) / 2;
        GoalPosition.y += 6;
        tempPOST = Instantiate(tempPOST);
        tempPOST.transform.position = GoalPosition;
        Vector3 look = forGoalPosition[1];
        look.y += 6;
        tempPOST.transform.LookAt(look);
        Vector3 tempGoalRotation = tempPOST.transform.rotation.eulerAngles;
        tempGoalRotation.y = Mathf.Abs(tempGoalRotation.y) - 90;
        tempPOST.transform.rotation = Quaternion.Euler(tempGoalRotation);
        GoalPosition.z -= 5;
        tempPOST.transform.position = GoalPosition;

        Vector3 PlayerPosition = points[0][0] + (Vector3.forward + Vector3.up) * 5;
        PlayerPosition.y += 6;

        if(level.blocks.Length > 0)
        {
            GameObject blocks = Instantiate(new GameObject());
            blocks.name = "Blocks";
            foreach(var block in level.blocks)
            {
                GameObject go = Instantiate(block.block);
                Vector3 blockPos = block.position;
                blockPos.y += 6;
                go.transform.position = blockPos;
                for (int p = 0; p < points.Count - 1; p++)
                {
                    if (points[p][0].z >= blockPos.z || points[p + 1][0].z <= blockPos.z)
                    {
                        blockPos.x = (points[p][0].x + points[p + 1][0].x)/2;
                        break;
                    }
                }
                go.transform.LookAt(blockPos);
                go.transform.parent = blocks.transform;
            }
        }

        levelManager.SetPlayerAndBall(PlayerPosition);


    }

    private List<Vector3[]> getData(Level lvl)
    {
        List<Vector3[]> data = new List<Vector3[]>();
        for (int i = 0; i < lvl.points.Length - 1; i++)
        {
            Vector3[] output = new Vector3[4];
            Vector3 mid = lvl.points[i + 1] - lvl.points[i];
            for (int q = 0; q < 4; q++)
            {
                if (q == 0)
                {
                    output[0] = lvl.points[i];
                    continue;
                }
                Vector3 temp = Vector3.zero;
                temp = lvl.points[i] + (mid / 4) * q;
                output[q] = temp;
            }
            data.Add(output);
        }
        return data;
    }

    
}

public class levelData
{
    public List<Vector3[]> points = new List<Vector3[]>();

    public levelData(Vector3[] point)
    {
        for (int i = 0; i < point.Length - 1; i++)
        {
            Vector3[] output = new Vector3[4];
            Vector3 mid = point[i + 1] - point[i];
            for (int q = 0; q < 4; q++)
            {
                if(q == 0)
                {
                    output[0] = point[i];
                    continue;
                }
                Vector3 temp = Vector3.zero;
                temp = point[i] + (mid / 4) * q;
                output[q] = temp;
            }
            points.Add(output);
        }
    }
}