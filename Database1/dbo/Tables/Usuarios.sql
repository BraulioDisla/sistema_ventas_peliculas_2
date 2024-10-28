CREATE TABLE [dbo].[Usuarios] (
    [IdUsuarios]        INT            IDENTITY (1, 1) NOT NULL,
    [NombreUsuario]     NVARCHAR (255) NOT NULL,
    [CorreoElectronico] NVARCHAR (255) NOT NULL,
    [Contrasena]        NVARCHAR (255) NOT NULL,
    [Rol]               NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK__Usuarios__3214EC07F3347FDA] PRIMARY KEY CLUSTERED ([IdUsuarios] ASC),
    CONSTRAINT [CK_Usuarios_Rol] CHECK ([Rol]='Invitado' OR [Rol]='Usuario' OR [Rol]='Administrador'),
    CONSTRAINT [CK_Usuarios_Rol1] CHECK ([Rol]='Invitado' OR [Rol]='Usuario' OR [Rol]='Administrador')
);

