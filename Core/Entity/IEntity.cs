namespace Core.Entity;

public interface IEntity<TID>
{
    public TID Id { get; set; }
}
