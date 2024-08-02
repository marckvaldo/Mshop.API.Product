using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.DeleteImage
{
    public class DeleteImageInPut : IRequest<Result<ImageOutPut>>
    {
        public DeleteImageInPut(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
