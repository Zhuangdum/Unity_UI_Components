using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public RectTransform contentRect;
    public List<Mark> marks;

    private void Start()
    {
        UpdateDirection();
    }

    private void Update()
    {
        UpdateDirection();
    }
    
    private float p0 = 0;
    private float p1 = Mathf.Cos(90 / 4 * 3 * Mathf.Deg2Rad);
    private float p2 = Mathf.Cos(90 / 4 * 2 * Mathf.Deg2Rad);
    private float p3 = Mathf.Cos(90 / 4 * Mathf.Deg2Rad);
    private float p4 = 1;
    private float intervalangle = 90 / 2;
    
    public void UpdateDirection()
    {
        Vector2 direction = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;

        
        if (direction.y > 0 && direction.x >= -p1 && direction.x <= p1)
        {
            Vector2 leftDir = new Vector2(-p1, p3);
            UpdateComponent(DirType.North, direction, leftDir);
        }
        else if (direction.y > 0 && direction.x >= p1 && direction.x <= p3)
        {
            Vector2 leftDir = new Vector2(p1, p3);
            UpdateComponent(DirType.NorthEast, direction, leftDir);
        }
        else if (direction.x > p3)
        {
            Vector2 leftDir = new Vector2(p3, p1);
            UpdateComponent(DirType.East, direction, leftDir);
        }
        else if (direction.y < 0 && direction.x >= p1 && direction.x <= p3)
        {
            Vector2 leftDir = new Vector2(p3, -p1);
            UpdateComponent(DirType.SouthEast, direction, leftDir);
        }
        else if (direction.y < -p3)
        {
            Vector2 leftDir = new Vector2(p1, -p3);
            UpdateComponent(DirType.South, direction, leftDir);
        }
        else if (direction.y < 0 && direction.x <= -p1 && direction.x >= -p3)
        {
            Vector2 leftDir = new Vector2(-p1, -p3);
            UpdateComponent(DirType.SouthWest, direction, leftDir);
        }
        else if (direction.x < -p3)
        {
            Vector2 leftDir = new Vector2(-p3, -p1);
            UpdateComponent(DirType.West, direction, leftDir);
        }
        else if (direction.y > 0 && direction.x <= -p1 && direction.x >= -p3)
        {
            Vector2 leftDir = new Vector2(-p3, p1);
            UpdateComponent(DirType.NorthWest, direction, leftDir);
        }
    }

    private void UpdateComponent(DirType type, Vector2 direction, Vector3 leftDir)
    {

        Mark mark = marks.Find(s => s.selfType == type);
        RectTransform rect;
        for (int i = 0; i < marks.Count; i++)
        {
            rect = marks[i].rect;
            if (marks[i].selfType == type)
            {
                float anglePercent = Vector2.Angle(leftDir, direction) / intervalangle;
                mark.rect.localPosition = new Vector2(mark.rect.rect.width / 2, -5) - new Vector2(anglePercent * mark.rect.rect.width, 0);
                mark.rect.gameObject.SetActive(true);
                
                RectTransform preRect = marks.Find(s=>s.selfType == mark.preType).rect;
                preRect.localPosition = mark.rect.localPosition - new Vector3(mark.rect.rect.width, 0, -5);
                preRect.gameObject.SetActive(true);
                
                RectTransform nextRect = marks.Find(s=>s.selfType == mark.nextType).rect;
                nextRect.localPosition = mark.rect.localPosition + new Vector3(mark.rect.rect.width, 0, -5);
                nextRect.gameObject.SetActive(true);
                continue;
            }
            if(marks[i].selfType == mark.preType)
            {
                continue;
            }
            if (marks[i].selfType == mark.nextType)
            {
                continue;
            }
            rect.gameObject.SetActive(false);
        }
    }
}

public enum DirType
{
    North = 0,
    NorthEast = 1,
    East = 2,
    SouthEast = 3,
    South = 4,
    SouthWest = 5,
    West = 6,
    NorthWest = 7
}

[Serializable]
public class Mark
{
    public RectTransform rect;
    public DirType selfType;

    public DirType nextType
    {
        get
        {
            if (selfType != DirType.NorthWest)
                return selfType + 1;
            return DirType.North;
        }
    }

    public DirType preType
    {
        get
        {
            if (selfType != DirType.North)
                return selfType - 1;
            return DirType.NorthWest;
        }
    }
}