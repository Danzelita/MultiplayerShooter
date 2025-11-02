using System.Collections.Generic;

namespace Shooter.Scripts.Data
{
    [System.Serializable]
    public struct ShootData
    {
        public string pKey;
        public string GunId;
        public List<BulletData> Bullets;
    }
}