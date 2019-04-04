
--Nombre BD : [Proyecto]


CREATE TABLE Invoice  (
    ID int IDENTITY(1,1) not null PRIMARY KEY,
	ID_Customer int null, 
	Total decimal(25,2) null,
	SoftDelete bit null
);

CREATE TABLE Invoie_Detail  (
    ID int IDENTITY(1,1) not null PRIMARY KEY,
	ID_Invoice int null, 
	ID_Item int null, 
	Quantity decimal(25,2) null,
	Cost decimal(25,2) null,
	Unit_Price decimal(25,2) null,
	Price_Total decimal(25,2) null,
	SoftDelete bit null,
	FOREIGN KEY (ID_Invoice) REFERENCES Invoice(ID) 
);

CREATE TABLE Item  (
    ID_Item int IDENTITY(1,1) not null PRIMARY KEY,
	Code varchar(50) null,
	Name_Item varchar(100) null,
	Inventory_Quantity decimal(25,2) null,
	Cost decimal(25,2) null,
	Unit_Price decimal(25,2) null,
	SoftDelete bit null
	
);

CREATE TABLE Customer  (
    ID_Customer int IDENTITY(1,1) not null PRIMARY KEY,
	Code varchar(50) null,
	Name_Customer varchar(100) null,
	SoftDelete bit null
		
);

CREATE TABLE Kardex_Inventory  (
    ID_Kardex_Inventory int IDENTITY(1,1) not null PRIMARY KEY,
	ID_Item int null, 
	Quantity decimal(25,2) null,
	Type_Inventory varchar(50) null,
);

Go

Create PROCEDURE [dbo].[AgregarKardex]
( @id int)
as
begin

insert into Kardex_Inventory (ID_Item,Quantity,Type_Inventory)
select Id_Item,Quantity,'-' from Invoie_Detail where ID_Invoice = @id

end

go

Create PROCEDURE [dbo].[ValidarItem]
( @id_Item int,@Inventory_Quantity decimal(25,2))
as
begin


 select Cost from Item where ID_Item=@id_Item and Inventory_Quantity >= @Inventory_Quantity

end

go

Create PROCEDURE [dbo].[Buscar_Factura]
( @id int)
as
begin

select inv.ID_Customer ,inv.ID ID_Documento ,de.ID ID_Detalle,It.Code, It.Name_Item, de.Quantity, de.Unit_Price, de.Price_Total from Invoice inv
inner join Customer cus on inv.ID_Customer = cus.ID_Customer 
inner join Invoie_Detail de on de.ID_Invoice = inv.ID 
inner join Item it on it.ID_Item = de.ID_Item where inv.ID =@id and de.SoftDelete =0 and inv.SoftDelete =0  
order by de.id 

end
go

INSERT INTO [dbo].[Customer]
           ([Code]
           ,[Name_Customer]
           ,[SoftDelete])
     VALUES
           ('2'
           ,'Cliente 2'
           ,0)
GO


INSERT INTO [dbo].[Customer]
           ([Code]
           ,[Name_Customer]
           ,[SoftDelete])
     VALUES
           ('1'
           ,'Cliente 1'
           ,0)
GO


INSERT INTO [dbo].[Item]
           ([Code]
           ,[Name_Item]
           ,[Inventory_Quantity]
           ,[Cost]
           ,[Unit_Price]
           ,[SoftDelete])
     VALUES
           ('1'
           ,'Item 1'
           ,100.0
           ,100.0
           ,200.0
           ,0)
GO


INSERT INTO [dbo].[Item]
           ([Code]
           ,[Name_Item]
           ,[Inventory_Quantity]
           ,[Cost]
           ,[Unit_Price]
           ,[SoftDelete])
     VALUES
           ('2'
           ,'Item 2'
           ,100.0
           ,100.0
           ,200.0
           ,0)
GO


