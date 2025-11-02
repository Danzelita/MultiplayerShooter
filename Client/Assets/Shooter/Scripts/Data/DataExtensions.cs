using UnityEngine;

namespace Shooter.Scripts.Data
{
    public static class DataExtensions
    {
        public static Vector3Data ToData(this in Vector3 vector3) => 
            new(vector3.x, vector3.y, vector3.z);
        
        public static Vector3 ToVector3(this in Vector3Data vector3) =>
            new(vector3.X, vector3.Y, vector3.Z);
    }
}