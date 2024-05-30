using Domain.Entities.Products.Especifications;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.List
{
    internal sealed class ListProductQueryHandler: IRequestHandler<ListProductsQuery, List<ProductRecordDto>>
    {
        private readonly IProductRepository _productRepository;

        public ListProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductRecordDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var specification = new ListProductSpec();
            var listProducts = await _productRepository.GetAll().ToListAsync();

            var listProductsDto = listProducts.Select(p => new ProductRecordDto(p.Id, p.Name, p.Description)).ToList();
            return listProductsDto;
        }
    }
}
