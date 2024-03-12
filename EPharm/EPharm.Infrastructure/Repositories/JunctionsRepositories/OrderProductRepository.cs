using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class OrderProductRepository(AppDbContext context, IProductRepository productRepository)
    : Repository<OrderProduct>(context), IOrderProductRepository
{
    public async Task<int> InsertOrderProductAsync(int orderId, int productId, int quantity)
    {
        var product = await productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new InvalidOperationException($"Product with id {productId} does not exist.");

        var orderProduct = new OrderProduct
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity
        };

        await InsertAsync(orderProduct);
        return product.Price;
    }
}
