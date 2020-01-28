using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class TriangleMapFromLatLong : MapFromLatLong {
    override public String ChooseName () {
        return "dyna_grid_TSUVW_LatLong_small.txt" ;
    }
    override public int [] ComputeIndicesSlice1 (int nbX, int nbY) {
        int [] indicesTris = new int [(nbX - 1) * (nbY -1) * 6];
        int tris = 0 ;
        for (int iy = 1 ; iy < nbY ; iy++) {
            for (int ix = 1 ; ix < nbX ; ix++) {
                int indice = (iy - 1) * nbX + ix - 1 ;
                indicesTris [tris++] = indice ;
                indice = iy * nbX + ix - 1 ;
                indicesTris [tris++] = indice ;
                indice = iy * nbX + ix ;
                indicesTris [tris++] = indice ;
                indice = (iy - 1) * nbX + ix - 1 ;
                indicesTris [tris++] = indice ;
                indice = iy * nbX + ix ;
                indicesTris [tris++] = indice ;
                indice = (iy - 1) * nbX + ix ;
                indicesTris [tris++]= indice ;
            }
        }
        return indicesTris ;
    }

    override public int[] ComputeIndices(int nbX, int nbY)
    {
        int[] indicesTris = new int[(nbX - 1) * (nbY - 1) * 18];
        int tris = 0;
        for (int iy = 1; iy < nbY; iy++)
        {
            for (int ix = 1; ix < nbX; ix++)
            {
                int indice = (iy - 1) * nbX + ix - 1;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix - 1;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix;
                indicesTris[tris++] = indice;
                indice = (iy - 1) * nbX + ix - 1;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix;
                indicesTris[tris++] = indice;
                indice = (iy - 1) * nbX + ix;
                indicesTris[tris++] = indice;

                indice = iy * nbX + ix - 1 + nbX * nbY;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix - 1;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix - 1 + nbX * nbY;
                indicesTris[tris++] = indice;
                indice = iy* nbX +ix;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix + nbX * nbY;
                indicesTris[tris++] = indice;

                indice = iy * nbX + ix + nbX * nbY;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix;
                indicesTris[tris++] = indice;
                indice = (iy - 1) * nbX + ix;
                indicesTris[tris++] = indice;
                indice = iy * nbX + ix + nbX * nbY;
                indicesTris[tris++] = indice;
                indice = (iy - 1) * nbX + ix;
                indicesTris[tris++] = indice;
                indice = (iy - 1) * nbX + ix + nbX * nbY;
                indicesTris[tris++] = indice;
            }
        }
        return indicesTris;
    }

    override public Shader ChooseShader () {
        return Shader.Find ("Unlit/TriangleShaderForClipping") ;
    }

    //override public Shader ChooseShader()
    //{
    //    MeshRenderer m = GetComponentInChildren<MeshRenderer>();
    //    return m.materials[0].shader;
    //}

    override public MeshTopology ChooseTopology () {
        return MeshTopology.Triangles ;
    }

    override public Vector3 ManageNullValue (Vector3 value) => value ;

    /* override public void CreateMesh () {
        base.CreateMesh () ;
        print ("dans CreateMesh redéfini") ;
        for (int t = 0 ; t < nbTime ; t++) {
            for (int d = 0 ; d < nbDepth ; d++) {
                for (int i = 0 ; i < pointMaps.Length ; i++) {
                    pointMaps [i].pointGroups [t][d].AddComponent<MeshCollider>() ;
                    pointMaps [i].pointGroups [t][d].GetComponent<MeshCollider>().sharedMesh = pointMaps [i].pointGroups [t][d].GetComponent<MeshFilter>().mesh ;
                }
            }
        }

    } */
}

