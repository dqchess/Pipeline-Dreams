﻿using UnityEngine;


public class Util {

    public static readonly Quaternion TurnUp = Quaternion.Euler(-90, 0, 0);
    public static readonly Quaternion TurnDown = Quaternion.Euler(90, 0, 0);
    public static readonly Quaternion TurnRight = Quaternion.Euler(0, 90, 0);
    public static readonly Quaternion TurnLeft = Quaternion.Euler(0, -90, 0);
    public static Vector3Int FaceToLHVector(int f) {
        return new Vector3Int(f >> 1 == 0 ? 1 : 0, f >> 1 == 1 ? 1 : 0, f >> 1 == 2 ? 1 : 0) * (-((f & 1) << 1) + 1);
    }

    public static int LHUnitVectorToFace(Vector3Int v) {

        return (1 - v.x) / 2 + Mathf.Abs(v.y) * (5 - v.y) / 2 + Mathf.Abs(v.z) * (9 - v.z) / 2;
    }
    /// <summary>
    /// North = (0,0,1) Up = (0,1,0)
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public static int LHQToFace(Quaternion q) {
        var v = Vector3Int.RoundToInt(q * Vector3.forward);
        return (1 - v.x) / 2 + Mathf.Abs(v.y) * (5 - v.y) / 2 + Mathf.Abs(v.z) * (9 - v.z) / 2;
    }

    public static Quaternion FaceToLHQ(int f) {
        return Quaternion.LookRotation(new Vector3Int(f >> 1 == 0 ? 1 : 0, f >> 1 == 1 ? 1 : 0, f >> 1 == 2 ? 1 : 0) * (-((f & 1) << 1) + 1), Vector3.up);
    }
    public static Vector3Int LHQToLHUnitVector(Quaternion q) {
        return Vector3Int.RoundToInt(q * Vector3.forward);
    }
    public static bool IsBasis(Vector3Int v) {
        return v.x * v.y == 0 & v.y * v.z == 0 & v.z * v.x == 0;
    }
    public static Vector3Int Normalize(Vector3Int v) {
        v.Clamp(Vector3Int.one * -1, Vector3Int.one);
        return v;
    }
    public static Quaternion RotateToFace(int f, Quaternion q0) {
        if (LHQToFace(q0) == f) return Quaternion.identity;
        if (LHQToFace(q0 * TurnUp) == f) return TurnUp;
        if (LHQToFace(q0 * TurnDown) == f) return TurnDown;
        if (LHQToFace(q0 * TurnLeft) == f) return TurnLeft;
        if (LHQToFace(q0 * TurnRight) == f) return TurnRight;
        return TurnUp;
    }
    public static bool CompareTiles(Tile t1, Tile t2) {
        return (uint)(t1 ^ t2) >> 8 == 0;
    }
    public static bool CompareBlocks(Block t1, Block t2) {
        return (uint)(t1 ^ t2) >> 8 == 0;
    }
}
