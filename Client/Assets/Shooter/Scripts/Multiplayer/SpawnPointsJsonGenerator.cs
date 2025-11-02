using System.Collections.Generic;
using Shooter.Scripts.Data;
using UnityEngine;

namespace Shooter.Scripts.Multiplayer
{
    [CreateAssetMenu(fileName = "SpawnPointsJsonGenerator", menuName = "SpawnPointsJsonGenerator")]
    public class SpawnPointsJsonGenerator : ScriptableObject
    {
        public string Tag;
        
        public string Json;
        
        [ContextMenu(nameof(Generate))]
        public void Generate() => GenerateJson();
        private void GenerateJson()
        {
            PointMarker[] points = FindObjectsOfType<PointMarker>();
            List<PointMarker> resultPoints = new List<PointMarker>();

            foreach (PointMarker point in points)
                if (point.Tag == Tag) 
                    resultPoints.Add(point);
            
            PointsData pointsData = new PointsData();
            pointsData.Points = new Vector3Data[resultPoints.Count];
            for (int i = 0; i < resultPoints.Count; i++) 
                pointsData.Points[i] = resultPoints[i].transform.position.ToData();
            
            string json = JsonUtility.ToJson(pointsData);
            Json = json;
        }
    }
}