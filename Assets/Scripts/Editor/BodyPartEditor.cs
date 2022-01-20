using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace Prototype
{


    [CustomEditor(typeof(BodyPart))]
    public class BodyPartEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BodyPart mp = (BodyPart)target;

            if (GUILayout.Button("Setup body cells"))
            {
                Debug.Log("Setup");

                //GameObject go = new GameObject("Test");
                //go.transform.parent = mp.transform;
                ClearPrevCells(mp.transform);
                CreateNodes(mp.transform);
                ConnectNodes();
            }
        }

        void ClearPrevCells(Transform transform)
        {
            var cells = transform.GetComponentsInChildren<BodyCellNode>();

            for (int i = 0; i < cells.Length; i++)
            {
                DestroyImmediate(cells[i].gameObject);
            }
        }

        private BodyCellNode[,] Nodes;
        private void CreateNodes(Transform transform)
        {

            var savedRot = transform.rotation;
            transform.rotation = Quaternion.identity;

            var m_Renderer = transform.GetComponent<SpriteRenderer>();
            m_Renderer.ResetBounds();
            var bounds = m_Renderer.bounds;

            float size = 0.1f;
            var cellSize = new Vector3(size, size, 1);
            int horizonalCellsNumber = Mathf.CeilToInt(bounds.size.x / size);
            int verticalCellsNumber = Mathf.CeilToInt(bounds.size.y / size);

            float startOffset = size / 2;
            var minPosWithOffset = new Vector3(bounds.min.x + startOffset, bounds.min.y + startOffset, 0);
            float moveOffset = size;

            Nodes = new BodyCellNode[horizonalCellsNumber, verticalCellsNumber];
            var prefab = Resources.Load<GameObject>("BodyCell");
            for (int i = 0; i < horizonalCellsNumber; i++)
            {
                for (int j = 0; j < verticalCellsNumber; j++)
                {
                    var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;              
                    var  cell = obj.GetComponent<BodyCellNode>();
                    cell.transform.position = new Vector3(minPosWithOffset.x + moveOffset * i, minPosWithOffset.y + moveOffset * j, 0);
                    cell.transform.localScale = cellSize;
                    cell.transform.rotation = transform.rotation;
                    cell.transform.parent = transform;
                    Nodes[i, j] = cell;

                }
            }

            transform.rotation = savedRot;
            m_Renderer.enabled = false;
        }

        private void ConnectNodes()
        {
            var height = Nodes.GetLength(0);
            var width = Nodes.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    BodyCellNode right = null;
                    BodyCellNode top = null;
                    BodyCellNode bottom = null;
                    BodyCellNode left = null;


                    var topIndex = i + 1;
                    var bottomIndex = i - 1;
                    var rightIndex = j + 1;
                    var leftIndex = j - 1;

                    if (rightIndex < width)
                        top = Nodes[i, rightIndex];
                    if (leftIndex >= 0)
                        bottom = Nodes[i, leftIndex];

                    if (topIndex < height)
                        right = Nodes[topIndex, j];
                    if (bottomIndex >= 0)
                        left = Nodes[bottomIndex, j];

                    Nodes[i, j].ConnectCells(right, left, bottom, top);

                }
            }
        }

    }
}


