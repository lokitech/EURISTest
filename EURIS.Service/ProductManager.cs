using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EURIS.Entities;
using EURIS.Model.Exceptions;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using EURIS.Service.Interfaces;

namespace EURIS.Service
{
    public class ProductManager : IProductManager, IDisposable
    {
        readonly LocalDbEntities _context = new LocalDbEntities();
        IMapper _mapper;

        public ProductManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ListViewModel<ProductViewModel> GetProducts(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page must be greater than zero.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater than zero.");
            }

            var baseQuery = _context.Product.AsNoTracking();

            var totalCount = baseQuery.Count();
            var pageCount = (int)Math.Ceiling(totalCount / (decimal)pageSize);

            if (pageCount != 0 && page > pageCount)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, $"Page must be lower than the total page count ({pageCount}).");
            }

            var query = ApplySort(baseQuery);
            var products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var productListViewModel = new ListViewModel<ProductViewModel>();
            productListViewModel.Items = products.Select(x => _mapper.Map<ProductViewModel>(x)).ToList();
            productListViewModel.CurrentPage = page;
            productListViewModel.PageCount = pageCount;

            return productListViewModel;
        }

        public IEnumerable<SelectListItem> GetSelectListProducts()
        {
            var products = _context.Product.AsNoTracking().ToList();
            var selectListItems = products.Select(x => _mapper.Map<SelectListItem>(x));
            var selectList = new SelectList(selectListItems, "Value", "Text");
            return selectList;
        }

        public Product GetProduct(int prodId)
        {
            var product = _context.Product.AsNoTracking().SingleOrDefault(x => x.Id == prodId);
            return product;
        }

        public Product GetProduct(string prodCode)
        {
            var product = _context.Product.AsNoTracking().SingleOrDefault(x => x.Code == prodCode);
            return product;
        }

        public int InsertProduct(ProductModel product)
        {
            var original = _context.Product.SingleOrDefault(x => x.Id == product.Id);
            if (original == null)
            {
                if (GetProduct(product.Code) != null)
                {
                    throw new UserFriendlyException("Another product with that code already exists.");
                }
                var newProduct = _mapper.Map<Product>(product);
                _context.Product.Add(newProduct);
                _context.SaveChanges();
                return product.Id;
            }
            else
            {
                throw new UserFriendlyException("Product already exists.");
            }
        }

        public int UpdateProduct(ProductModel product)
        {
            var original = _context.Product.SingleOrDefault(x => x.Id == product.Id);
            if (original != null)
            {
                if (product.Code != original.Code && GetProduct(product.Code) != null)
                {
                    throw new UserFriendlyException("Another product with that code already exists.");
                }
                _mapper.Map(product, original);
                DeleteProductReferences(original.Id);
                _context.SaveChanges();
                return original.Id;
            }
            else
            {
                throw new UserFriendlyException("Product with the specified id does not exist, cannot edit non-existing product.");
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Product.SingleOrDefault(x => x.Id == productId);
            if(product == null)
            {
                throw new UserFriendlyException($"Product with ID: {productId} does not exist.");
            }
            DeleteProductReferences(product.Id);
            _context.Product.Remove(product);
            _context.SaveChanges();
        }

        private void DeleteProductReferences(int prodId)
        {
            var productCatalogs = _context.CatalogProduct.Where(x => x.ProductId == prodId);
            foreach (var productCata in productCatalogs)
            {
                _context.CatalogProduct.Remove(productCata);
            }
        }

        private IOrderedQueryable<Product> ApplySort(IQueryable<Product> baseQuery)
        {
            return baseQuery.OrderBy(x => x.Code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}
