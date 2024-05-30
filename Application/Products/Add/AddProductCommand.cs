using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Add
{
    public record AddProductCommand(int Id, string Name, string Description) : IRequest;
}
