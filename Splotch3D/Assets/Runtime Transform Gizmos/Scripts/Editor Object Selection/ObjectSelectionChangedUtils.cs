using UnityEngine;
using System.Collections.Generic;

namespace RTEditor
{
    // Meant to be used with selection event capturing to adjust selection
    public static class ObjectSelectionChangedUtils
    {
        public static void SelectEntireHierarchy(GameObject gameObject)
        {
            GameObject rootObject = gameObject.transform.root.gameObject;
            List<GameObject> allObjectsInHierarchy = rootObject.GetAllChildrenIncludingSelf();

            foreach(var gameObj in allObjectsInHierarchy)
                EditorObjectSelection.Instance.AddObjectToSelection(gameObj, false);
        }
    }
}