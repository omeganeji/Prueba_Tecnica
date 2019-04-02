using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_Tecnica.Data;
namespace Prueba_Tecnica.Models
{
    public class Documento
    {
        public Invoice Master;
        public Customer Custom;
        public List<Item> Item;
    }
    public class IDDocumento
    {
        public int Documento;

    }
    public class DocumentoFactu
    {
        public int ID_Customer;
        public decimal  Total;
        public Boolean SoftDelete;
        public List<DocumentoFactuDet> Invoie_Detail;
    }
    public class DocumentoFactuDet
    {
        public int ID_Item;
        public decimal Quantity;
        public decimal Unit_Price;
        public decimal Price_Total;

        public Boolean SoftDelete;
    }

}