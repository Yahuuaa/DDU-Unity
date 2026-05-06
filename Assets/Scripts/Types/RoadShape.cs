using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Library
{
    public enum RoadShape { Straight, Turn, TJunction, Cross }

    public static class RoadShapeResolver
    {
        private const float Expand = 0.15f;

        public static void UpdateShape(Road road, Dictionary<Guid, Road> allRoads)
        {
            GameObject model = road.GetModel();
            Transform t = model.transform;

            List<GameObject> neighbours = GetNeighbourModels(model);
            bool front = false, back = false, left = false, right = false;
            foreach (GameObject neighbour in neighbours)
            {
                Vector3 localDir = t.InverseTransformPoint(neighbour.transform.position);
                if (Mathf.Abs(localDir.z) >= Mathf.Abs(localDir.x))
                {
                    if (localDir.z > 0) front = true;
                    else back = true;
                }
                else
                {
                    if (localDir.x > 0) right = true;
                    else left = true;
                }
            }

            int count = (front ? 1 : 0) + (back ? 1 : 0) + (right ? 1 : 0) + (left ? 1 : 0);
            RoadShape shape;
            float yRot = 0f;

            switch (count)
            {
                case 4:
                    shape = RoadShape.Cross;
                    break;

                case 3:
                    shape = RoadShape.TJunction;
                    if      (!back)  yRot = 0f;
                    else if (!left)  yRot = 90f;
                    else if (!front) yRot = 180f;
                    else             yRot = 270f;
                    break;

                case 2:
                    if (front && back)
                    {
                        shape = RoadShape.Straight; yRot = 0f;
                    }
                    else if (left && right)
                    {
                        shape = RoadShape.Straight; yRot = 90f;
                    }
                    else
                    {
                        shape = RoadShape.Turn;
                        if      (front && right) yRot = 0f;
                        else if (right && back)  yRot = 90f;
                        else if (back && left)   yRot = 180f;
                        else                     yRot = 270f;
                    }
                    break;

                default:
                    shape = RoadShape.Straight;
                    break;
            }

            road.SetShape(shape, Quaternion.Euler(0f, yRot, 0f));
        }

        private static List<GameObject> GetNeighbourModels(GameObject model)
        {
            BoxCollider box = model.GetComponent<BoxCollider>();
            Vector3 worldSize = Vector3.Scale(box.size, model.transform.lossyScale);

            Collider[] hits = Physics.OverlapBox(
                model.transform.TransformPoint(box.center),
                (worldSize / 2f) + new Vector3(Expand, Expand, Expand),
                model.transform.rotation
            );

            List<GameObject> result = new List<GameObject>();
            foreach (Collider hit in hits)
            {
                if (hit.transform.IsChildOf(model.transform)) continue;
                if (!Variables.Object(hit.gameObject).IsDefined("type")) continue;
                if (Variables.Object(hit.gameObject).Get<string>("type") != "Road") continue;
                if (!RoadManager.SharesFullEdge(model, hit.gameObject)) continue;
                result.Add(hit.gameObject);
            }
            return result;
        }
    }
}