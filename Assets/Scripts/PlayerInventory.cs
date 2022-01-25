namespace Prototype
{

    public enum GunType { Pistol, Laser, RocketLauncher, Cannon }
    public class PlayerInventory
    {
        public GunType CurrentGun = GunType.Pistol;
    }
}
