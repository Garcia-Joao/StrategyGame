using System;

[Serializable]
public class UnitResources
{
    public ResourcePool Health = new();
    public ResourcePool Mana = new();
    public ResourcePool Movement = new();

    public event Action Died;

    public void Initialize(
        int health,
        int mana,
        int movement)
    {
        Health.Initialize(health);
        Mana.Initialize(mana);
        Movement.Initialize(movement);

        Health.Changed += CheckDeath;
    }

    private void CheckDeath()
    {
        if (Health.Current <= 0)
        {
            Died?.Invoke();
        }
    }

    public void ConsumeHealth(int amount)
    {
        Health.Consume(amount);
    }

    public void ConsumeMana(int amount)
    {
        Mana.Consume(amount);
    }

    public void TakeDamage(int amount)
    {
        Health.Consume(amount);
    }

    public void Heal(int amount)
    {
        Health.Restore(amount);
    }
}