using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class OrderProductRepository(AppDbContext context, IProductRepository productRepository)
    : Repository<OrderProduct>(context), IOrderProductRepository
{
    public async Task<Product> InsertOrderProductAsync(OrderProduct orderProduct) 
    {
        var product = await productRepository.GetByIdAsync(orderProduct.ProductId);

        if (product is null)
            throw new InvalidOperationException($"Product with id {orderProduct.Id} does not exist.");

        await InsertAsync(orderProduct);
        return product;
    }
}
