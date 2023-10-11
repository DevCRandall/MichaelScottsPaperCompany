using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelScottsPaperCompany.DATA.EF.Models //Metadata
{
    //internal class Partials
    //{
    //}

    #region Category
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }
    #endregion


    #region Item
    [ModelMetadataType(typeof(ItemMetadata))]
    public partial class Item { }
    #endregion Item


    #region Manufacturer
    [ModelMetadataType(typeof(ManufacturerMetadata))]
    public partial class Manufacturer { }
    #endregion Manufacturer


    #region OrderItem
    [ModelMetadataType(typeof(OrderItemMetadata))]
    public partial class OrderItem { }
    #endregion OrderItem


    #region Sale
    [ModelMetadataType(typeof(SaleMetadata))]
    public partial class Sale { }
    #endregion Sale


    #region UserDetail
    [ModelMetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail { }
    #endregion UserDetail
}
