using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.List
{
    public record ListProductsQuery() : IRequest<List<ProductRecordDto>>;

}
