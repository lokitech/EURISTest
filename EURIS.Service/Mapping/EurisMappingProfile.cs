using AutoMapper;
using EURIS.Entities;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EURIS.Service.Mapping
{
    public class EurisMappingProfile : Profile
    {
        public EurisMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.Catalogs, m => m.MapFrom(y => y.CatalogProduct.Select(z => new CatalogViewModel { Code = z.Catalog.Code, Description = z.Catalog.Description, Id = z.Catalog.Id })));

            CreateMap<Product, ProductModel>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.CatalogIds, m => m.MapFrom(y => y.CatalogProduct.Select(z => z.CatalogId)));

            CreateMap<Product, SelectListItem>()
                .ForMember(x => x.Value, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Text, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Selected, m => m.Ignore())
                .ForMember(x => x.Disabled, m => m.Ignore())
                .ForMember(x => x.Group, m => m.Ignore());

            CreateMap<ProductModel, Product>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.CatalogProduct, m => m.MapFrom(y => y.CatalogIds.Select(z => new CatalogProduct { CatalogId = z, ProductId = y.Id })));


            CreateMap<Catalog, CatalogViewModel>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.Products, m => m.MapFrom(y => y.CatalogProduct.Select(z => new ProductViewModel { Code = z.Product.Code, Description = z.Product.Description, Id = z.Product.Id })));

            CreateMap<Catalog, CatalogModel>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.ProductIds, m => m.MapFrom(y => y.CatalogProduct.Select(z => z.ProductId)));

            CreateMap<Catalog, SelectListItem>()
                .ForMember(x => x.Value, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Text, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Selected, m => m.Ignore())
                .ForMember(x => x.Disabled, m => m.Ignore())
                .ForMember(x => x.Group, m => m.Ignore());

            CreateMap<CatalogModel, Catalog>()
                .ForMember(x => x.Id, m => m.MapFrom(y => y.Id))
                .ForMember(x => x.Code, m => m.MapFrom(y => y.Code))
                .ForMember(x => x.Description, m => m.MapFrom(y => y.Description))
                .ForMember(x => x.CatalogProduct, m => m.MapFrom(y => y.ProductIds.Select(z => new CatalogProduct { ProductId = z, CatalogId = y.Id })));
        }
    }
}
