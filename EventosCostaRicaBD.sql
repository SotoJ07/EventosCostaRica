-- Create the database
CREATE DATABASE EventosCostaRicaDb;
GO

USE EventosCostaRicaDb;
GO

-- Create Roles table
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

-- Create Usuarios table
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
	Apellidos NVARCHAR(100) NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    ContraseñaHash NVARCHAR(255) NOT NULL,
    RolId INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RolId) REFERENCES Roles(Id)
);

ALTER TABLE Usuarios
ADD Apellidos NVARCHAR(100) NULL;

-- Seed roles for Movie Theater
INSERT INTO Roles (Nombre) VALUES ('Admin');
INSERT INTO Roles (Nombre) VALUES ('Guest');

--Creación de la tabla eventos si no se desea migrar desde Visual
CREATE TABLE [dbo].[Eventos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](300) NOT NULL,
	[FechaInicio] [datetime2](7) NOT NULL,
	[FechaFin] [datetime2](7) NOT NULL,
	[Lugar] [nvarchar](100) NOT NULL,
	[Precio] [decimal](18, 2) NOT NULL,
	[ImagenUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_Eventos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO