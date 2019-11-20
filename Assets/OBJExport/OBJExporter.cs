using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;

/*=============================================================================
 |	    Project:  Unity3D Scene OBJ Exporter
 |
 |		  Notes: Only works with meshes + meshRenderers. No terrain yet
 |
 |       Author:  aaro4130
 |
 |     DO NOT USE PARTS OF THIS CODE, OR THIS CODE AS A WHOLE AND CLAIM IT
 |     AS YOUR OWN WORK. USE OF CODE IS ALLOWED IF I (aaro4130) AM CREDITED
 |     FOR THE USED PARTS OF THE CODE.
 |
 *===========================================================================*/

public class OBJExporter : MonoBehaviour
{
    public bool onlySelectedObjects = false;
    public bool applyPosition = true;
    public bool applyRotation = true;
    public bool applyScale = true;
    public bool generateMaterials = true;
    public bool exportTextures = true;
    public bool splitObjects = true;
    public bool autoMarkTexReadable = false;
    public bool objNameAddIdNum = false;

    public MeshFilter[] sceneMeshes;

    //public bool materialsUseTextureName = false;

    private string versionString = "v2.0";
    private string lastExportFolder;

    Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }
    Vector3 MultiplyVec3s(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            string exportPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Export(exportPath);
        }
    }


    void Export(string exportPath)
        {
            //init stuff
            Dictionary<string, bool> materialCache = new Dictionary<string, bool>();
            var exportFileInfo = new System.IO.FileInfo(exportPath);
            lastExportFolder = exportFileInfo.Directory.FullName;
            string baseFileName = System.IO.Path.GetFileNameWithoutExtension(exportPath);


            //work on export
            StringBuilder sb = new StringBuilder();
            StringBuilder sbMaterials = new StringBuilder();

            if (generateMaterials)
            {
                sb.AppendLine("mtllib " + baseFileName + ".mtl");
            }
            float maxExportProgress = (float)(sceneMeshes.Length + 1);
            int lastIndex = 0;
            for(int i = 0; i < sceneMeshes.Length; i++)
            {
                string meshName = sceneMeshes[i].gameObject.name;
                float progress = (float)(i + 1) / maxExportProgress;
                MeshFilter mf = sceneMeshes[i];
                MeshRenderer mr = sceneMeshes[i].gameObject.GetComponent<MeshRenderer>();

                if (splitObjects)
                {
                    string exportName = meshName;
                    if (objNameAddIdNum)
                    {
                        exportName += "_" + i;
                    }
                    sb.AppendLine("g " + exportName);
                }

                //export the meshhh :3
                Mesh msh = mf.sharedMesh;
                int faceOrder = (int)Mathf.Clamp((mf.gameObject.transform.lossyScale.x * mf.gameObject.transform.lossyScale.z), -1, 1);
            
                //export vector data (FUN :D)!
                foreach (Vector3 vx in msh.vertices)
                {
                    Vector3 v = vx;
                    if (applyScale)
                    {
                        v = MultiplyVec3s(v, mf.gameObject.transform.lossyScale);
                    }
                
                    if (applyRotation)
                    {
  
                        v = RotateAroundPoint(v, Vector3.zero, mf.gameObject.transform.rotation);
                    }

                    if (applyPosition)
                    {
                    v += mf.gameObject.transform.position;
                }
                v.x *= -1;
                sb.AppendLine("v " + v.x + " " + v.y + " " + v.z);
            }
            foreach (Vector3 vx in msh.normals)
            {
                Vector3 v = vx;
                
                if (applyScale)
                {
                    v = MultiplyVec3s(v, mf.gameObject.transform.lossyScale.normalized);
                }
                if (applyRotation)
                {
                    v = RotateAroundPoint(v, Vector3.zero, mf.gameObject.transform.rotation);
                }
                v.x *= -1;
                sb.AppendLine("vn " + v.x + " " + v.y + " " + v.z);

            }
            foreach (Vector2 v in msh.uv)
            {
                sb.AppendLine("vt " + v.x + " " + v.y);
            }

            for (int j=0; j < msh.subMeshCount; j++)
            {
                if(mr != null && j < mr.sharedMaterials.Length)
                {
                    string matName = mr.sharedMaterials[j].name;
                    sb.AppendLine("usemtl " + matName);
                }
                else
                {
                    sb.AppendLine("usemtl " + meshName + "_sm" + j);
                }

                int[] tris = msh.GetTriangles(j);
                for(int t = 0; t < tris.Length; t+= 3)
                {
                    int idx2 = tris[t] + 1 + lastIndex;
                    int idx1 = tris[t + 1] + 1 + lastIndex;
                    int idx0 = tris[t + 2] + 1 + lastIndex;
                    if(faceOrder < 0)
                    {
                        sb.AppendLine("f " + ConstructOBJString(idx2) + " " + ConstructOBJString(idx1) + " " + ConstructOBJString(idx0));
                    }
                    else
                    {
                        sb.AppendLine("f " + ConstructOBJString(idx0) + " " + ConstructOBJString(idx1) + " " + ConstructOBJString(idx2));
                    }
                    
                }
            }

            lastIndex += msh.vertices.Length;
        }

        //write to disk
        //System.IO.File.WriteAllText(exportPath, sb.ToString());

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        System.IO.File.WriteAllText(desktopPath + "\\" + "test.obj", sb.ToString());



        if (generateMaterials)
        {
            System.IO.File.WriteAllText(exportFileInfo.Directory.FullName + "\\" + baseFileName + ".mtl", sbMaterials.ToString());
        }

    }


    private string ConstructOBJString(int index)
    {
        string idxString = index.ToString();
        return idxString + "/" + idxString + "/" + idxString;
    }
}