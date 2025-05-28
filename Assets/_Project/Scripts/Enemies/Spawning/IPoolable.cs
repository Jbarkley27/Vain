
// enemies will need to reset themselves when pulled from the pool, and clean up when returned.
public interface IPoolable
{
    void OnSpawned();    // Called when taken from pool
    void OnDespawned();  // Called when returned to pool
}
