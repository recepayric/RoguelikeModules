namespace Runtime.Interfaces
{
    public interface IDamageable
    {
        public void DealDamage();
        public void AddFreeze();
        public void RemoveFreeze();
        public void SetRarity(bool isRare, bool isMagic);
    }
}