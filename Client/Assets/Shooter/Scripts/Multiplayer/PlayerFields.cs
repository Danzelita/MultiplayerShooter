using Shooter.Scripts.Multiplayer.generated;

namespace Shooter.Scripts.Multiplayer
{
    public static class PlayerFields
    {
        public const string PositionX = nameof(Player.pX);
        public const string PositionY = nameof(Player.pY);
        public const string PositionZ = nameof(Player.pZ);
        
        public const string VelocityX = nameof(Player.vX);
        public const string VelocityY = nameof(Player.vY);
        public const string VelocityZ = nameof(Player.vZ);
        
        public const string RotationX = nameof(Player.rX);
        public const string RotationY = nameof(Player.rY);
        
        public const string Crouch = nameof(Player.cr);
    }
}