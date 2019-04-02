﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Prueba_Tecnica.Data;
using Prueba_Tecnica.Models;

namespace Prueba_Tecnica.Controllers
{
    public class InvoiceController : ApiController
    {
        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        //Api para buscar una factura con parametro desde el Body
        [Route("API/InvoiceMaster/BuscarFactura")]
        public Invoice BuscarFactura([FromBody] IDDocumento id)
        {
            using (ProyectosDataClassesDataContext data = new ProyectosDataClassesDataContext())
            {
                Invoice InvoiceSelect = new Invoice();
              
                InvoiceSelect = data.Invoice.Where(inv => inv.ID == id.Documento).FirstOrDefault();
                var cantidad = !(InvoiceSelect is null) ?  InvoiceSelect.Invoie_Detail.Count() :0 ;
                return InvoiceSelect;
            }
        }
        //Api para buscar una factura con parametro desde url con cambios en el tipo de documento que devuelve
        // GET api/values/5
        public Documento Get(int id)
        {
            using (ProyectosDataClassesDataContext data = new ProyectosDataClassesDataContext())
            {
                Documento InvoiceSelect = new Documento();
                InvoiceSelect.Master  = data.Invoice.Where(inv => inv.ID == id).FirstOrDefault();
                InvoiceSelect.Custom =  data.Customer.Where(inv => inv.ID_Customer == InvoiceSelect.Master.ID_Customer).FirstOrDefault();

                var Items = from de in InvoiceSelect.Master.Invoie_Detail 
                        join It in data.Item on de.ID_Item equals It.ID_Item
                        select It;

                InvoiceSelect.Item = Items.ToList();
                if (InvoiceSelect.Master.Invoie_Detail.Count() > 0);
               return InvoiceSelect;
            }
        }

        // POST api/values
        [Route("API/InvoiceMaster/Create")]
        public Mensaje Post([FromBody]Invoice value)
        {
            Mensaje Respuesta = new Mensaje();
            Decimal total = 0;
            bool error = false;
            using (ProyectosDataClassesDataContext data = new ProyectosDataClassesDataContext())
            {
                try
                {

                    if (value.ID_Customer is null || data.Customer.Where(cus => cus.ID_Customer == value.ID_Customer).Count() <= 0)
                    {
                        Respuesta.ErrorNo = "3";
                        Respuesta.MensajeTexto = "Faltan Cliente";
                    }
                    else if (value.Invoie_Detail.Count == 0)
                    {
                        Respuesta.ErrorNo = "2";
                        Respuesta.MensajeTexto = "Faltan Detalles";
                    }
                    else
                    {
                        foreach (Invoie_Detail Detail in value.Invoie_Detail)
                        {
                            
                            if (Detail.ID_Item is null || data.Item.Where(cus => cus.ID_Item == Detail.ID_Item).Count() <= 0)
                            {
                                Respuesta.ErrorNo = "4";
                                Respuesta.MensajeTexto = "Faltan Productos";
                                error = true;
                            }
                            else
                            {
                                ValidarItemResult carl = data.ValidarItem(Detail.ID_Item, Detail.Quantity).FirstOrDefault();
                                if ((carl is null))
                                {
                                    Respuesta.ErrorNo = "5";
                                    Respuesta.MensajeTexto = "Producto sin existencia";
                                    error = true;
                                }
                                else
                                {
                                    //Se podria validar o enviar un error si el precio total no es igual al precio por la cantidad
                                    //En este caso se remplazara por el valor correcto
                                    Detail.Cost = carl.Cost;
                                    Detail.Price_Total = Detail.Unit_Price * Detail.Quantity;

                                    total = total + (decimal)Detail.Price_Total;
                                }

                            }                                            
                        }
                        //Se podria validar o enviar un error si el precio total del documento no es igual a los totales del detalle
                        //En este caso se remplazara por el valor correcto
                        value.Total = total;

                        if (!error)
                        {
                            data.Invoice.InsertOnSubmit(value);
                            data.SubmitChanges();
                            //Me parece importante en una aplicacion de factura llevar el invetario de alguna forma 
                            data.AgregarKardex(value.ID);                  
                            Respuesta.ErrorNo = "0";
                            Respuesta.MensajeTexto = "Factura Creada correctamente";
                            Respuesta.doc = value.ID.ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Respuesta.ErrorNo = "-1";
                    Respuesta.MensajeTexto = e.Message;
                }

            }
            return Respuesta;
        }

        // PUT api/values/5

        [Route("API/InvoiceMaster/Update")]
        public Mensaje Put(int id, [FromBody] Invoice value)
        {
            Mensaje Respuesta = new Mensaje();
            using (ProyectosDataClassesDataContext data = new ProyectosDataClassesDataContext())
            {
                try
                {
                    Invoice InvoiceUpdate = data.Invoice.Where(inv => inv.ID == id || inv.SoftDelete == false).FirstOrDefault();

                    if (InvoiceUpdate is null)
                    {
                        Respuesta.ErrorNo = "1";
                        Respuesta.MensajeTexto = "Factura No existe";
                    }
                    else
                    {
                        InvoiceUpdate = value;              
                        data.SubmitChanges();
                        Respuesta.ErrorNo = "0";
                        Respuesta.MensajeTexto = "Factura Actualizada correctamente";
                    }

                }
                catch (Exception e)
                {
                    Respuesta.ErrorNo = "-1";
                    Respuesta.ErrorNo = e.Message;
                }
            }
            return Respuesta;
        }

        // DELETE api/values/5
        [Route("API/InvoiceMaster/Delete")]
        public Mensaje Delete(int id)
        {
            Mensaje Respuesta = new Mensaje();
            using (ProyectosDataClassesDataContext data = new ProyectosDataClassesDataContext())
            {
                try
                {
                    Invoice InvoiceUpdate = data.Invoice.Where(inv => inv.ID == id).FirstOrDefault();
                    if (InvoiceUpdate is null)
                    {
                        Respuesta.ErrorNo = "1";
                        Respuesta.MensajeTexto = "Factura No existe";
                    }
                    else
                    {
                        InvoiceUpdate.SoftDelete = true;
                        data.SubmitChanges();
                        Respuesta.ErrorNo = "0";
                        Respuesta.ErrorNo = "Factura Actualizada correctamente";                      
                    }
                }
                catch (Exception e)
                {
                    Respuesta.ErrorNo = "-1";
                    Respuesta.ErrorNo = e.Message;
                }
            }
            return Respuesta;
        }
    }
}
