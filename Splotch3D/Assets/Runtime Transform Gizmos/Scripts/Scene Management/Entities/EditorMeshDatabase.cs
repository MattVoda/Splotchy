using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RTEditor
{
    public class EditorMeshDatabase : MonoSingletonBase<EditorMeshDatabase>
    {
        public delegate void MeshTreeBuildFinishedHandler(EditorMesh editorMesh, bool completedOnSeparateThread);
        public event MeshTreeBuildFinishedHandler MeshTreeBuildFinished;

        #region Private Variables
        private Dictionary<Mesh, EditorMesh> _meshes = new Dictionary<Mesh, EditorMesh>();
        #endregion

        #region Public Methods
        public void BuildAllMeshesInScene()
        {
            var allMeshFilters = GameObject.FindObjectsOfType<MeshFilter>();
            foreach(var meshFilter in allMeshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                EditorMesh editorMesh = CreateEditorMesh(mesh);
                if (editorMesh != null) editorMesh.Build();
            }
        }

        public EditorMesh CreateEditorMesh(Mesh mesh)
        {
            if (RuntimeEditorApplication.Instance.UseUnityColliders) return null;
            if (!IsMeshValid(mesh)) return null;

            if (_meshes.ContainsKey(mesh)) return null;
            else
            {
                EditorMesh editorMesh = new EditorMesh(mesh);
                _meshes.Add(mesh, editorMesh);

                return editorMesh;
            }
        }

        public List<EditorMesh> CreateEditorMeshes(List<Mesh> meshes)
        {
            if (RuntimeEditorApplication.Instance.UseUnityColliders) return new List<EditorMesh>();
            if (meshes == null || meshes.Count == 0) return new List<EditorMesh>();

            var editorMeshes = new List<EditorMesh>(meshes.Count);
            foreach(var mesh in meshes)
            {
                EditorMesh editorMesh = CreateEditorMesh(mesh);
                if (editorMesh == null) continue;

                editorMeshes.Add(editorMesh);
            }

            return editorMeshes;
        }

        public EditorMesh GetEditorMesh(Mesh mesh)
        {
            if (RuntimeEditorApplication.Instance.UseUnityColliders) return null;
            if (!IsMeshValid(mesh)) return null;

            if (Contains(mesh)) return _meshes[mesh];
            else return CreateEditorMesh(mesh);
        }

        public bool Contains(Mesh mesh)
        {
            return mesh != null && _meshes.ContainsKey(mesh);
        }

        public bool IsMeshValid(Mesh mesh)
        {
            return mesh != null && mesh.isReadable;
        }

        // DO NOT USE THIS IN CODE!!!
        public void OnMeshTreeBuildFinished(EditorMesh editorMesh, bool completedOnSeparateThread)
        {
            if (editorMesh != null && MeshTreeBuildFinished != null) MeshTreeBuildFinished(editorMesh, completedOnSeparateThread);
        }
        #endregion

        #region Private Methods
        private void RemoveNullMeshEntries()
        {
            var newMeshDictionary = new Dictionary<Mesh, EditorMesh>();
            foreach (KeyValuePair<Mesh, EditorMesh> pair in _meshes)
            {
                if (pair.Key != null) newMeshDictionary.Add(pair.Key, pair.Value);
            }
    
            _meshes = newMeshDictionary;
        }
        #endregion
    }
}