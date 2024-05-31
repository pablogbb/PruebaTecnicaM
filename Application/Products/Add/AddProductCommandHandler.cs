using Domain.Entities.Products;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Products.Add
{
    internal sealed class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public AddProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product { Id = request.Id, Name = request.Name, Description = request.Description};
            await _productRepository.Create(product);
        }
    }
}
