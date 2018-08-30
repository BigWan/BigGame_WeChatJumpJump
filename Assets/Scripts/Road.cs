using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtil;

public class Road : MonoBehaviour {

    //public enum Orientation {
    //    Forward = 0,
    //    Left = 1,
    //}

    // setting //
    [Header("Setting")]

    public Block[] blockPrefabs;

    public float
        minDistance,
        maxDistance;

    public float blockSpawnHeight;

    public AnimationCurve blockFallCurve;

    [Space(30)]

    // runtime //
    [Header("Runtime Info")]

    public Block currentBlock;

    public Orientation orientation;

    public List<Block> blocksRef;

    private Vector3 m_spawnPoint;


    public void RoadReset() {
        if (blocksRef == null)
            blocksRef = new List<Block>();

        if (blocksRef.Count > 0) {
            foreach (var b in blocksRef) {
                Destroy(b.gameObject);
            }
            blocksRef.Clear();
        }

        orientation = Orientation.Forward;
        currentBlock = null;
        m_spawnPoint = Vector3.zero;
    }

    private void Awake() {
        RoadReset();
    }

    public void SpawnFirstTwo() {
        m_spawnPoint = Vector3.zero;

        Block b = Instantiate(blockPrefabs[0]) as Block;
        b.transform.SetParent(transform,false);
        b.spawnPoint = Vector3.zero;
        b.transform.localPosition = Vector3.zero;
        b.orientation = orientation;
        blocksRef.Add(b);
        SpawnNextBlock(false,false);
    }


    public void SpawnNextBlock(bool needRandom = true,bool needFall = true) {
        if(needRandom)
            orientation = Random.value > 0.5 ? Orientation.Forward : Orientation.Left;
        m_spawnPoint += Util.GetOrientationVector(orientation) * Random.Range(minDistance, maxDistance);

        blocksRef[blocksRef.Count - 1].orientation = orientation;

        Block bp = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        Block b = Instantiate(bp) as Block;
        b.transform.SetParent(transform, false);
        //b.transform.localPosition = m_spawnPoint + Vector3.up* blockSpawnHeight;
        b.spawnPoint = m_spawnPoint;
        if (needFall)
            b.StartFall(m_spawnPoint, Vector3.up * blockSpawnHeight,blockFallCurve);
        else
            b.transform.localPosition = m_spawnPoint;

        if (blocksRef.Count > 0) {
            blocksRef[blocksRef.Count - 1].next = b;
            b.last = blocksRef[blocksRef.Count - 1].next;
        }

        blocksRef.Add(b);
        currentBlock = b;
    }


    public Vector3 GetFollowPoint() {
        if (blocksRef.Count >= 2)
            return (blocksRef[blocksRef.Count - 1].spawnPoint + blocksRef[blocksRef.Count - 2].spawnPoint) / 2f;
        else
            return Vector3.zero;
    }


}
