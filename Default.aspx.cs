

using RestSharp.Newtonsoft;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestSharp;
using Prueba_Tecnica.Data;
using RestSharp.Newtonsoft.Json.NetCore;
using Newtonsoft.Json;
using Prueba_Tecnica.Models;

namespace Prueba_Tecnica
{
 
    public partial class Default : System.Web.UI.Page
    {
        public List<Documento> InvoiceList;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonBuscar_Click(object sender, EventArgs e)
        {
            string param = String.IsNullOrWhiteSpace(TextBoxDocumento.Text) ? "0" : TextBoxDocumento.Text;
            var client = new RestClient("http://localhost:60712/api/Invoice/"+ param + "");
            var request = new RestSharp.RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "1566cf66-995b-4636-a2d8-587b139cedae");
            request.AddHeader("cache-control", "no-cache");
            
            var response = client.Execute<List<Documento>>(request);

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var response2 = client.Execute(request);

            Documento Doc = JsonConvert.DeserializeObject<Documento>(response.Content);

            var Items = from de in Doc.Master.Invoie_Detail
                        join It in Doc.Item on de.ID_Item equals It.ID_Item
                        select new { Codigo = It.Code, Name = It.Name_Item, Quantity = de.Quantity, Price = de.Unit_Price, Total = de.Price_Total };

            DropDownListCliente.SelectedValue   = Doc.Custom.ID_Customer.ToString();
            TextBoxDocumento.Text = Doc.Master.ID.ToString();
            GridView1.DataSource = Items;
            GridView1.DataBind();
        }

        protected void ButtonAgregarItem_Click(object sender, EventArgs e)
        {
            IDDocumento IDDoc = new IDDocumento();
            //if (String.IsNullOrEmpty(TextBoxDocumento.Text))
            //{
            //    LabelInfo.Text = "Documento Obligatorio";
            //}
            if ( String.IsNullOrEmpty(TextBoxCantidad.Text) || String.IsNullOrEmpty( TextBoxPrecio.Text) )
            {
                LabelInfo.Text = "Cantidad y precio obligatorios";                    
            }
            var client = new RestClient("http://localhost:60712/API/InvoiceMaster/BuscarFactura");
            var request = new RestSharp.RestRequest(Method.POST);
            //request.AddHeader("Postman-Token", "f9c55325-a7b3-4b53-85ec-cebe8a96b453");
            //request.AddHeader("cache-control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            IDDoc.Documento = Convert.ToInt32(string.IsNullOrWhiteSpace(TextBoxDocumento.Text) ? "0": TextBoxDocumento.Text);
            request.AddJsonBody(IDDoc);
            var response = client.Execute<List<Invoice>>(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            //var response2 = client.Execute(request);
            Invoice Doc = JsonConvert.DeserializeObject<Invoice>(response.Content);

            if(Doc is null)
            {
                DocumentoFactu newDoc = new DocumentoFactu();
 
                newDoc.ID_Customer = Convert.ToInt32 (DropDownListCliente.SelectedValue);
                newDoc.SoftDelete = false;
                //foreach (GridViewRow ro in GridView1.Rows)
                //{
                //    Invoie_Detail detail = new Invoie_Detail();
                //    detail.ID_Item = Convert.ToInt32(DropDownListProductos.SelectedValue);
                //    detail.Quantity = Convert.ToInt32(TextBoxCantidad.Text);
                //    detail.Unit_Price = Convert.ToInt32(TextBoxPrecio.Text);

                //    newDoc.Invoie_Detail.Add(detail);
                //}
     
                DocumentoFactuDet detail = new DocumentoFactuDet();
                newDoc.Invoie_Detail = new List <DocumentoFactuDet>();
                detail.ID_Item = Convert.ToInt32(DropDownListProductos.SelectedValue);
                detail.Quantity = Convert.ToInt32(TextBoxCantidad.Text);
                detail.Unit_Price = Convert.ToInt32(TextBoxPrecio.Text);
                detail.SoftDelete = false;
                
                newDoc.Invoie_Detail.Add(detail);
                client = new RestClient("http://localhost:60712/API/InvoiceMaster/Create");
                request = new RestSharp.RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;              
                request.AddJsonBody(newDoc);
                response = client.Execute<List<Invoice>>(request);
                Mensaje Mens = JsonConvert.DeserializeObject<Mensaje>(response.Content);
                

                if(Mens.ErrorNo =="0")
                {
                    TextBoxDocumento.Text = Mens.doc;
                    ButtonBuscar_Click(sender, e);
                }
                else
               {
                    LabelInfo.Text = Mens.MensajeTexto;
                }
                

            }
            else
            {
                DocumentoFactu newDoc = new DocumentoFactu();

                newDoc.ID_Customer = Convert.ToInt32(DropDownListCliente.SelectedValue);
                newDoc.SoftDelete = false;

                DocumentoFactuDet detail = new DocumentoFactuDet();
                newDoc.Invoie_Detail = new List<DocumentoFactuDet>();
                detail.ID_Item = Convert.ToInt32(DropDownListProductos.SelectedValue);
                detail.Quantity = Convert.ToInt32(TextBoxCantidad.Text);
                detail.Unit_Price = Convert.ToInt32(TextBoxPrecio.Text);
                detail.SoftDelete = false;

                newDoc.Invoie_Detail.Add(detail);
                client = new RestClient("http://localhost:60712/API/InvoiceMaster/Create");
                request = new RestSharp.RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(newDoc);
                response = client.Execute<List<Invoice>>(request);

                LabelInfo.Text = response.ErrorMessage;
            }
        }
    }
}