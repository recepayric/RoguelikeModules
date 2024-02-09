using Runtime.Enums;

namespace Runtime.Interfaces
{
    public interface ICursable
    {
        void AddCurse(AllStats stat, float amount);
        void RemoveCurse(AllStats stat, float amount);
    }
}