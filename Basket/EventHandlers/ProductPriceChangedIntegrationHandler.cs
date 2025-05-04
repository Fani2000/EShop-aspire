using MassTransit;
using ServiceDefault.Messaging.Events;

namespace Basket.EventHandlers;

public class ProductPriceChangedIntegrationHandler(BasketService service) : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        await service.UpdateBasketItemProductPrice(context.Message.ProductId, context.Message.Price);
    }
}
