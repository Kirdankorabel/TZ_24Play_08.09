using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walls", menuName = "Walls", order = 1)]
public class Walls : ScriptableObject
{
    [SerializeField] private List<Wall> _walls;
    public List<Wall> GetWallInfo => _walls;
}

[Serializable]
public struct Wall
{
    public List<WallElement> elements;
}
[Serializable]
public struct WallElement
{
    public Vector3 position;
    public int height;
}