CREATE TABLE [dbo].[ReportesVentas] (
    [Id]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaReporte]         DATE            NOT NULL,
    [VentasTotales]        DECIMAL (10, 2) NOT NULL,
    [PeliculasMasVendidas] NVARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

