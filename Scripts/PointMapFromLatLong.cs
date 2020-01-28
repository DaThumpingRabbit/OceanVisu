using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class PointMapFromLatLong : MapFromLatLong {

    override public String ChooseName () {
        return "dyna_grid_TSUVW_LatLong_huge.txt" ;
    }

    override public int[] ComputeIndicesSlice1(int nbX, int nbY)
    {
        int[] indices = new int[nbX * nbY];
        for (int i = 1; i < nbX * nbY; i++)
        {
            indices[i] = i;
        }
        return indices;
    }

    override public int [] ComputeIndices (int nbX, int nbY) {
        int [] indices = new int [nbX * nbY];
        for (int i = 1 ; i < nbX * nbY ; i++) {
            indices [i] = i ;
        }
        return indices ;
    }

    override public Shader ChooseShader () {
        return Shader.Find ("Unlit/PointShader") ;
    }

    override public MeshTopology ChooseTopology () {
        return MeshTopology.Points ;
    }

    Vector3 antipodes = new Vector3 (10000.0f, 10000.0f, 10000.0f) ;
    override public Vector3 ManageNullValue(Vector3 value) => antipodes ;

}

