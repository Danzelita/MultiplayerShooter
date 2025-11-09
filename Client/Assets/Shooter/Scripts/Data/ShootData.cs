using System;

namespace Shooter.Scripts.Data
{
    [System.Serializable]
    public struct ShootData
    {
        public string pKey;
        public string GunId;
        public Vector3Data Pos;
        public Vector3Data Dir;
        public int Seed;
    }
}