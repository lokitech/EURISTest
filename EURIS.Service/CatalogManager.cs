using AutoMapper;
using EURIS.Entities;
using EURIS.Model.Exceptions;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using EURIS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EURIS.Service
{
    public sealed class CatalogManager : ICatalogManager, IDisposable
    {
        readonly LocalDbEntities _context = new LocalDbEntities();
        private IMapper _mapper;

        public CatalogManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ListViewModel<CatalogViewModel> GetCatalogs(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page must be greater than zero.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater than zero.");
            }

            var baseQuery = _context.Catalog.AsNoTracking();

            var totalCount = baseQuery.Count();
            var pageCount = (int)Math.Ceiling(totalCount / (decimal)pageSize);

            if (pageCount != 0 && page > pageCount)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, $"Page must be lower than the total page count ({pageCount}).");
            }

            var query = ApplySort(baseQuery);
            var catalogs = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var catalogListViewModel = new ListViewModel<CatalogViewModel>();
            catalogListViewModel.Items = catalogs.Select(x => _mapper.Map<CatalogViewModel>(x)).ToList();
            catalogListViewModel.CurrentPage = page;
            catalogListViewModel.PageCount = pageCount;

            return catalogListViewModel;
        }

        public Catalog GetCatalog(int catalogId)
        {
            var catalog = _context.Catalog.AsNoTracking().SingleOrDefault(x => x.Id == catalogId);
            return catalog;
        }

        public Catalog GetCatalog(string catalogCode)
        {
            var catalog = _context.Catalog.AsNoTracking().SingleOrDefault(x => x.Code == catalogCode);
            return catalog;
        }

        public IEnumerable<SelectListItem> GetSelectListCatalogs()
        {
            var catalogs = _context.Catalog.AsNoTracking().ToList();
            var selectListItems = catalogs.Select(x => _mapper.Map<SelectListItem>(x));
            var selectList = new SelectList(selectListItems, "Value", "Text");
            return selectList;
        }

        public IEnumerable<ProductViewModel> GetProductsFromCatalog(int catalogId)
        {
            var products = _context.CatalogProduct.Where(x => x.CatalogId == catalogId).Select(x => x.Product).ToList();
            var productViewModel = products.Select(x => _mapper.Map<ProductViewModel>(x));
            return productViewModel;
        }

        public int InsertCatalog(CatalogModel catalog)
        {
            var original = _context.Catalog.SingleOrDefault(x => x.Id == catalog.Id);
            if (original == null)
            {
                if (GetCatalog(catalog.Code) != null)
                {
                    throw new UserFriendlyException("Another catalog with that code already exists.");
                }
                var newCatalog = _mapper.Map<Catalog>(catalog);
                _context.Catalog.Add(newCatalog);
                _context.SaveChanges();
                return catalog.Id;
            }
            else
            {
                throw new UserFriendlyException("Catalog already exists.");
            }
        }

        public int UpdateCatalog(CatalogModel catalog)
        {
            var original = _context.Catalog.SingleOrDefault(x => x.Id == catalog.Id);
            if (original != null)
            {
                if (catalog.Code != original.Code && GetCatalog(catalog.Code) != null)
                {
                    throw new UserFriendlyException("Another catalog with that code already exists.");
                }
                _mapper.Map(catalog, original);
                DeleteCatalogReferences(original.Id);
                _context.SaveChanges();
                return original.Id;
            }
            else
            {
                throw new UserFriendlyException("Catalog with the specified id does not exist, cannot edit non-existing catalog.");
            }
        }

        public void DeleteCatalog(int catalogId)
        {
            var catalog = _context.Catalog.SingleOrDefault(x => x.Id == catalogId);
            if (catalog == null)
            {
                throw new UserFriendlyException($"Catalog with ID: {catalogId} does not exist.");
            }
            DeleteCatalogReferences(catalog.Id);
            _context.Catalog.Remove(catalog);
            _context.SaveChanges();
        }

        private void DeleteCatalogReferences(int catalogId)
        {
            var catalogCatalogs = _context.CatalogProduct.Where(x => x.CatalogId == catalogId);
            foreach (var catalogCata in catalogCatalogs)
            {
                _context.CatalogProduct.Remove(catalogCata);
            }
        }

        private IOrderedQueryable<Catalog> ApplySort(IQueryable<Catalog> baseQuery)
        {
            return baseQuery.OrderBy(x => x.Code);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
