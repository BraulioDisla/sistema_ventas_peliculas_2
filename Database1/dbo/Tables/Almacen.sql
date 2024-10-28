CREATE TABLE [dbo].[Almacen] (
    [IdAlmacen]          INT            IDENTITY (1, 1) NOT NULL,
    [IdPeliculas]        INT            NOT NULL,
    [CantidadDisponible] INT            NOT NULL,
    [Ubicacion]          NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK__Almacen__3214EC07AA954B4D] PRIMARY KEY CLUSTERED ([IdAlmacen] ASC),
    CONSTRAINT [FK_Almacen_Peliculas] FOREIGN KEY ([IdPeliculas]) REFERENCES [dbo].[Peliculas] ([IdPeliculas])
);

