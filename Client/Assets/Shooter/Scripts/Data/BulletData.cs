namespace Shooter.Scripts.Data
{
    [System.Serializable]
    public class BulletData
    {
        public Vector3Data Pos;
        public Vector3Data Vel;

        public BulletData(Vector3Data pos, Vector3Data vel)
        {
            Pos = pos;
            Vel = vel;
        }
    }
}