using MShop.Application.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.ListCategorys
{
    public class ListCategory : BaseUseCase, IListCategory
    {
        private readonly ICategoryRepository _categoryRepositiry;
        private readonly List<CategoryModelOutPut> _listCategory;
        public ListCategory(ICategoryRepository categoryRepositiry, INotification notification) : base(notification)
        {
            _categoryRepositiry = categoryRepositiry;
            _listCategory = new List<CategoryModelOutPut>();
        }

        public async Task<List<CategoryModelOutPut>> Handler()
        {
            var categorys = await _categoryRepositiry.GetValuesList();
            foreach(var item in categorys)
            {
                _listCategory.Add(new CategoryModelOutPut(item.Id, item.Name, item.IsActive));
            }

            return _listCategory;  
        }
    }
}
