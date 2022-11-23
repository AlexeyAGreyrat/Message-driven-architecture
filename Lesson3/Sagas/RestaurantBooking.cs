using MassTransit;

namespace Restaurant.Booking.Sagas;

public class RestaurantBooking : SagaStateMachineInstance
{    
    //остояние саги
    public int CurrentState { get; set; }

    //ID заказа
    public Guid OrderId { get; set; } 
        
    //ID Клиента
    public Guid ClientId { get; set; }

    //идентификатор для соотнесения всех сообщений друг с другом
    public Guid CorrelationId { get; set; }

    //маркировка для "композиции" событий (наш случай с кухней и забронированным столом)
    public int ReadyEventStatus { get; set; }
        
    // пометка о том, что наша заявка просрочена
    public Guid? ExpirationId { get; set; }

    public Guid? GuestIncomeId { get; set; }

    public int IncomeTime { get; set; }

    public int? TableId { get; set; }

}