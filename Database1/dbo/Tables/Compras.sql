CREATE TABLE [dbo].[Compras] (
    [IdCompras]        INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId]        INT            NOT NULL,
    [IdPeliculas]      INT            NOT NULL,
    [FechaCompra]      DATE           CONSTRAINT [DF__Compras__FechaCo__2B3F6F97] DEFAULT (getdate()) NOT NULL,
    [EstadoPago]       NVARCHAR (100) NOT NULL,
    [CantidadComprada] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Compras__3214EC074D837636] PRIMARY KEY CLUSTERED ([IdCompras] ASC),
    CONSTRAINT [FK__Compras__Usuario__29572725] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([IdUsuarios]),
    CONSTRAINT [FK_Compras_Peliculas] FOREIGN KEY ([IdPeliculas]) REFERENCES [dbo].[Peliculas] ([IdPeliculas])
);

