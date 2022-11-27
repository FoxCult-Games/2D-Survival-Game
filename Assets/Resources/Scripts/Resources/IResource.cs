namespace Resources.Scripts.Resources
{
    public interface IResource
    {
        ResourceType _type { get; }
        int _amount { get; }
        
        int ChangeAmount(int amount);
        bool IsEmpty();
    }
}