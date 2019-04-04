

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
            string Url = Request.Url.Authority;

            string param = String.IsNullOrWhiteSpace(TextBoxDocumento.Text) ? "0" : TextBoxDocumento.Text;
            var client = new RestClient("http://"+ Url + "/api/Invoice/"+ param + "");
            var request = new RestSharp.RestRequest(Method.GET);
       
          
            var response = client.Execute<List<Buscar_FacturaResult>>(request);

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
         

            List<Buscar_FacturaResult> Doc = JsonConvert.DeserializeObject<List<Buscar_FacturaResult>>(response.Content);
            if (!(Doc is null) && (Doc.Count()>0 ))
            {
                var Items = from de in Doc
                 select new { Identificador= de.ID_Detalle ,Codigo = de.Code, Nombre = de.Name_Item, Cantidad = de.Quantity, Precio = de.Unit_Price, Total = de.Price_Total };

            DropDownListCliente.SelectedValue   = Doc.FirstOrDefault().ID_Customer.ToString();
            TextBoxDocumento.Text = Doc.FirstOrDefault().ID_Documento.ToString();
            GridView1.DataSource = Items;
            GridView1.DataBind();
            }
            else
            {
                LabelInfo.Text = "No se encontro el documento";
            }
        }

        protected void ButtonAgregarItem_Click(object sender, EventArgs e)
        {
            IDDocumento IDDoc = new IDDocumento();

            if ( String.IsNullOrEmpty(TextBoxCantidad.Text) || String.IsNullOrEmpty( TextBoxPrecio.Text) )
            {
                LabelInfo.Text = "Cantidad y precio obligatorios";                    
            }
            string Url = Request.Url.Authority;
            var client = new RestClient("http://"+ Url + "/API/InvoiceMaster/BuscarFactura");
            var request = new RestSharp.RestRequest(Method.POST);
    
            request.RequestFormat = DataFormat.Json;
            IDDoc.Documento = Convert.ToInt32(string.IsNullOrWhiteSpace(TextBoxDocumento.Text) ? "0": TextBoxDocumento.Text);
            request.AddJsonBody(IDDoc);
            var response = client.Execute<List<Invoice>>(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();

            Invoice Doc = JsonConvert.DeserializeObject<Invoice>(response.Content);

            if(Doc is null)
            {
                DocumentoFactu newDoc = new DocumentoFactu();
 
                newDoc.ID_Customer = Convert.ToInt32 (DropDownListCliente.SelectedValue);
                newDoc.SoftDelete = false;
     
                DocumentoFactuDet detail = new DocumentoFactuDet();
                newDoc.Invoie_Detail = new List <DocumentoFactuDet>();
                detail.ID_Item = Convert.ToInt32(DropDownListProductos.SelectedValue);
                detail.Quantity = Convert.ToInt32(TextBoxCantidad.Text);
                detail.Unit_Price = Convert.ToInt32(TextBoxPrecio.Text);
                detail.SoftDelete = false;
                
                newDoc.Invoie_Detail.Add(detail);
                
                client = new RestClient("http://"+ Url + "/API/InvoiceMaster/Create");
                request = new RestSharp.RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;              
                request.AddJsonBody(newDoc);
                response = client.Execute<List<Invoice>>(request);
                Mensaje Mens = JsonConvert.DeserializeObject<Mensaje>(response.Content);
                
                if(Mens.ErrorNo =="0")
                {
                    LabelInfo.Text = Mens.MensajeTexto;
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
                DocumentoFactuMaster newDoc = new DocumentoFactuMaster();
                newDoc.ID_Customer = Convert.ToInt32(DropDownListCliente.SelectedValue);
                newDoc.SoftDelete = false;
                DocumentoFactuDet detail = new DocumentoFactuDet();
       
                ParamUpdate Update = new ParamUpdate();
                Update.Documento = Doc.ID;
                Update.Master = newDoc;
                

                detail.ID_Item = Convert.ToInt32(DropDownListProductos.SelectedValue);
                detail.Quantity = Convert.ToInt32(TextBoxCantidad.Text);
                detail.Unit_Price = Convert.ToInt32(TextBoxPrecio.Text);
                detail.SoftDelete = false;
                Update.Detalle = Update.Concertir_Detalle(detail);

                client = new RestClient("http://"+ Url + "/API/InvoiceMaster/Update");
                request = new RestSharp.RestRequest(Method.POST);         
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(Update);
                response = client.Execute<List<Invoice>>(request);
                Mensaje Mens = JsonConvert.DeserializeObject<Mensaje>(response.Content);


                if (Mens.ErrorNo == "0")
                {
                    LabelInfo.Text = Mens.MensajeTexto;
                    TextBoxDocumento.Text = Mens.doc;
                    ButtonBuscar_Click(sender, e);
                }
            }
        }

        protected void ButtonEliminar_Click(object sender, EventArgs e)
        {
            string Url = Request.Url.Authority;

            string param = String.IsNullOrWhiteSpace(TextBoxDocumento.Text) ? "0" : TextBoxDocumento.Text;
            var client = new RestClient("http://" + Url + "/API/InvoiceMaster/Delete?id=" + param + "");
            var request = new RestSharp.RestRequest(Method.DELETE);

    
            var response = client.Execute<Mensaje>(request);

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();


            Mensaje Doc = JsonConvert.DeserializeObject<Mensaje>(response.Content);
            if (!(Doc is null) )
            {
                TextBoxDocumento.Text = "";
                GridView1.DataSource = null;
                LabelInfo.Text = Doc.MensajeTexto;
                GridView1.DataBind();
            }
            else
            {
                LabelInfo.Text = Doc.MensajeTexto;
            }
        }
    }
}