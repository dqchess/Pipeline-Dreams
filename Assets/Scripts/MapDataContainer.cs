﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PipelineDreams {
    /// <summary>
    /// Blocks generates space.
    /// </summary>

   
    /// <summary>
    /// Tiles are generated by blocks.
    /// </summary>
    [Flags]
    public enum TileAttribute {
        Nothing = 0,
        /// <summary>
        /// Normal entities cannot move through this tile.
        /// </summary>
        BlockEntity = 0b1,
        /// <summary>
        /// Blocks line of sight and ray weapons
        /// </summary>
        BlockLight = 0b10,
        /// <summary>
        /// Blocks projectiles
        /// </summary>
        BlockProj = 0b100,

        /// <summary>
        /// Entity could stay in this tile.
        /// </summary>
        EntityStay = 0b1000,
        EmptyTile = BlockEntity,

    }
    [CreateAssetMenu(fileName = "MapDataContainer", menuName = "ScriptableObjects/Manager/MapDataContainer")]
    public class MapDataContainer : ScriptableObject {

        public readonly TileDataset Dataset;
        protected List<Tile> Tiles;
       

        

        public bool IsLineOfSight(Vector3Int v1, Vector3Int v2) {
            var v = v1 - v2;


            if (v.x * v.y != 0 || v.y * v.z != 0 || v.z * v.x != 0)
                return false;
            var m = v.magnitude;
            v.Clamp(Vector3Int.one * (-1), Vector3Int.one);
            var f = Util.LHUnitVectorToFace(v);
            for (int i = 0; i < m; i++)
                if ((GetTile(v2 + v * i, f).Attribute & TileAttribute.BlockLight) !=0)
                    return false;
            return true;

        }
        public bool IsLineVisible(Vector3Int observer, Vector3Int line1, Vector3Int line2) {
            var v = line1 - line2;


            if (v.x * v.y != 0 || v.y * v.z != 0 || v.z * v.x != 0)
                return false;
            var m = v.magnitude;
            v.Clamp(Vector3Int.one * (-1), Vector3Int.one);
            for (int i = 0; i < m; i++)
                if (IsLineOfSight(observer, line2 + v * i))
                    return true;
            return false;

        }
        public Tile GetRandomAccessiblePoint() {
            List<Tile> AccessibleTiles = Tiles.FindAll((x) => (x.Attribute | TileAttribute.EntityStay) != 0);
            if (AccessibleTiles.Count == 0) throw new ArgumentOutOfRangeException();
            return AccessibleTiles[UnityEngine.Random.Range(0, AccessibleTiles.Count)];
        }

        /// <summary>
        /// TODO: Recode player spawn.
        /// </summary>
        /// <returns></returns>
        public Vector3Int GetPlayerSpawnPoint() {
            return GetRandomAccessiblePoint().Position;
        }

        public Tile GetTile(Vector3Int v, int f) {
            return GetTile(v.x, v.y, v.z, f);
        }
        public Tile GetTile(int i, int j, int k, int f) {
            return Tiles.Find((x) => x.Position == new Vector3Int(i, j, k) && x.Face == f);
        }


        public Tile GetTileRelative(Vector3Int v, int f, Entity e) {
            v += e.IdealPosition;
            return GetTileRelative(v.x,v.y,v.z, f, e);
        }
        public Tile GetTileRelative(int i, int j, int k, int f, Entity e) {
            return GetTileRelative(new Vector3Int(i, j, k), f, e);
        }
        public Tile GetTileRelative(Vector3Int v, int f, Vector3Int EntityPosition) {
            return GetTileRelative(v.x, v.y, v.z, f, EntityPosition);
        }
        public Tile GetTileRelative(int i, int j, int k, int f, Vector3Int EntityPosition) {
            i += EntityPosition.x;
            j += EntityPosition.y;
            k += EntityPosition.z;
            return GetTile(i, j, k, f);
            
        }
        

    }
}