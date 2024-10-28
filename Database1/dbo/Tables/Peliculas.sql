CREATE TABLE [dbo].[Peliculas] (
    [IdPeliculas]    INT             IDENTITY (1, 1) NOT NULL,
    [Titulo]         NVARCHAR (255)  NOT NULL,
    [Genero]         NVARCHAR (100)  NOT NULL,
    [Director]       NVARCHAR (255)  NOT NULL,
    [Descripcion]    NVARCHAR (MAX)  NOT NULL,
    [Precio]         DECIMAL (10, 2) NOT NULL,
    [Disponibilidad] BIT             NOT NULL,
    CONSTRAINT [PK__Pelicula__3214EC0762A8C801] PRIMARY KEY CLUSTERED ([IdPeliculas] ASC)
);

