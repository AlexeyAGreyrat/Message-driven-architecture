using Restaurant.Messages;

namespace Restaurant.Kitchen;

public class Chef
{
    private readonly Random _random = Random.Shared;

    public bool CheckDishReady(Guid orderId, Dish? dish)
    {
        if (dish is Dish.Salad) throw new Exception("There can not be any Salad");
        // Check ready dish
        return _random.Next(0, 50) > 15;
    }
}