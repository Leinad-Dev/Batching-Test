using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombine : MonoBehaviour
{
    // This script should be put on an empty GameObject
    // Objects to be combined should be children of the empty GameObject

    [SerializeField] private List<GameObject> chunksToCombine = new List<GameObject>();
    [SerializeField] Material material;
    private bool needNewChunk = false;
    GameObject chunk;
    static int chunkCount = 0;
    private void Start()
    {
        MeshFilter[] allChildMeshFilters = GetComponentsInChildren<MeshFilter>();
        CreateNewChunk();
        int countTracker = 0;

        for (int i = 0; i < allChildMeshFilters.Length; i++)
        {
            //Debug.Log("child mesh filter count is : " + allChildMeshFilters.Length);
            if (needNewChunk == true)
            {
                CreateNewChunk();
                needNewChunk = false;
            }

            countTracker += allChildMeshFilters[i].mesh.vertexCount;

            if (countTracker < 65000)//ADD TO CHUNK
            {
                if(i>0) //this ignores our original parent
                allChildMeshFilters[i].gameObject.transform.parent = chunksToCombine[chunksToCombine.Count - 1].gameObject.transform; //chunk is now parent
            }
            else if (countTracker >= 65000)
            {
                //STOP AND CREATE NEW CHUNK
                needNewChunk = true;
                i = 0;
                allChildMeshFilters = GetComponentsInChildren<MeshFilter>();
                countTracker = 0;
            }

            for (int c = 0; c < chunksToCombine.Count; c++)
            {

                if (chunksToCombine[c].gameObject.GetComponent<MeshFilter>() == null)
                {
                    chunksToCombine[c].gameObject.AddComponent<MeshFilter>();
                }

                if (chunksToCombine[c].gameObject.GetComponent<MeshRenderer>() == null)
                {
                    chunksToCombine[c].gameObject.AddComponent<MeshRenderer>();
                    chunksToCombine[c].gameObject.GetComponent<MeshRenderer>().material = material;
                }

                CombineMeshesOfChildren(chunksToCombine[c].gameObject);

            }
        }
    }


    private void CreateNewChunk()
    {
        chunkCount += 1;
        chunk = new GameObject();
        chunksToCombine.Add(chunk);
        /*Debug.Log("chunkcount is : "+chunkCount)*/
        RenameChunk(chunksToCombine[chunksToCombine.Count - 1], chunkCount);
    }


    private void RenameChunk(GameObject objToRename, int index)
    {
        objToRename.name = "chunk_" + index;
    }

    private void CombineMeshesOfChildren(GameObject parentObject)
    {
        //set position to zero to simplify matrix math
        Vector3 position = parentObject.transform.position;
        parentObject.transform.position = Vector3.zero;

        //get all mesh filters and combine
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        parentObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        parentObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        parentObject.transform.gameObject.SetActive(true);

        //return original position
        parentObject.transform.position = position;
    }
}