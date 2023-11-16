using Runtime.Enums;

public interface IPoolObject
{
    PoolKeys PoolKeys { get; set; }
    void OnReturn();
    void OnGet();
}