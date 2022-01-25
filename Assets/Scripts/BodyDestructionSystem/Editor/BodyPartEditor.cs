using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace Prototype
{




    [CustomEditor(typeof(BodyPart))]
    public class BodyPartEditor : Editor
    {
        private const string SETUP_BODY_CELLS = "Setup body cells";
        private const string CLEAR_BODY_CELLS = "Clear body cells";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BodyPart bodyPart = (BodyPart)target;

            if (GUILayout.Button(SETUP_BODY_CELLS))
            {
                Debug.Log(SETUP_BODY_CELLS);

                ClearPrevCells(bodyPart);
                CreateNodes(bodyPart);
                ConnectNodes(bodyPart);
            }

            if (GUILayout.Button(CLEAR_BODY_CELLS))
            {
                Debug.Log(CLEAR_BODY_CELLS);

                ClearPrevCells(bodyPart);
            }
        }

        void ClearPrevCells(BodyPart bodypart)
        {
            var cells = bodypart.GetComponentsInChildren<BodyCellNode>();

            for (int i = 0; i < cells.Length; i++)
            {
                DestroyImmediate(cells[i].gameObject);
            }
        }

        private void CreateNodes(BodyPart bodypart)
        {
            var bodyPartTransform = bodypart.transform;
            var savedRot = bodyPartTransform.rotation;
            bodyPartTransform.rotation = Quaternion.identity;

            var m_Renderer = bodypart.GetComponent<SpriteRenderer>();  
            m_Renderer.ResetBounds();
            var bounds = m_Renderer.bounds;

            float size = 0.1f;
            var cellSize = new Vector3(size, size, 1);
            int horizonalCellsNumber = Mathf.CeilToInt(bounds.size.x / size);
            int verticalCellsNumber = Mathf.CeilToInt(bounds.size.y / size);

            float startOffset = size / 2;
            var minPosWithOffset = new Vector3(bounds.min.x + startOffset, bounds.min.y + startOffset, 0);
            float moveOffset = size;

            bodypart.BodyCellLines = new BodyCellLine[verticalCellsNumber];
            var prefab = Resources.Load<GameObject>("BodyCell");
            for (int i = 0; i < verticalCellsNumber; i++)
            {
                var bodyCellLine = new BodyCellLine();
                bodyCellLine.Cells = new BodyCellNode[horizonalCellsNumber];
                for (int j = 0; j < horizonalCellsNumber; j++)
                {
                    var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;              
                    var  cell = obj.GetComponent<BodyCellNode>();
                    cell.transform.position = new Vector3(minPosWithOffset.x + moveOffset * j, minPosWithOffset.y + moveOffset * i, 0);
                    cell.transform.localScale = cellSize;
                    cell.transform.rotation = bodyPartTransform.rotation;
                    cell.transform.parent = bodyPartTransform;
                    cell.BodyPart = bodypart;
                    bodyCellLine.Cells[j] = cell;
                    cell.IndexX = j;
                    cell.IndexY = i;
                   

                }

                bodypart.BodyCellLines[i] = bodyCellLine;
            }

            bodyPartTransform.rotation = savedRot;
            m_Renderer.enabled = false;
        }

        private void ConnectNodes(BodyPart bodypart)
        {
            var height = bodypart.BodyCellLines.Length;
          

            for (int i = 0; i < height; i++)
            {
                var width = bodypart.BodyCellLines[i].Cells.Length;

                for (int j = 0; j < width; j++)
                {
                    BodyCellNode topCell = null;
                    BodyCellNode rightCell = null;
                    BodyCellNode leftCell = null;
                    BodyCellNode bottomCell = null;


                    var topIndex = i + 1;
                    var bottomIndex = i - 1;
                    var rightIndex = j + 1;
                    var leftIndex = j - 1;

                    if (rightIndex < width)
                        rightCell = bodypart.BodyCellLines[i].Cells[rightIndex];
                    if (leftIndex >= 0)
                        leftCell = bodypart.BodyCellLines[i].Cells[leftIndex];

                    if (topIndex < height)
                        topCell = bodypart.BodyCellLines[topIndex].Cells[j];
                    if (bottomIndex >= 0)
                        bottomCell = bodypart.BodyCellLines[bottomIndex].Cells[j];

                    bodypart.BodyCellLines[i].Cells[j].ConnectCells(topCell, bottomCell, leftCell, rightCell);

                }
            }
        }

    }
}


