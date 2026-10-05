/****** Script de creacion de FGF-BDD (limpio) ******/
SET NOCOUNT ON;
GO
GO
USE [master]
GO
IF DB_ID(N'FGF-BDD') IS NOT NULL
BEGIN
	ALTER DATABASE [FGF-BDD] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [FGF-BDD]
END
GO
CREATE DATABASE [FGF-BDD]
GO
ALTER DATABASE [FGF-BDD] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FGF-BDD].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FGF-BDD] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FGF-BDD] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FGF-BDD] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FGF-BDD] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FGF-BDD] SET ARITHABORT OFF 
GO
ALTER DATABASE [FGF-BDD] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FGF-BDD] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FGF-BDD] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FGF-BDD] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FGF-BDD] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FGF-BDD] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FGF-BDD] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FGF-BDD] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FGF-BDD] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FGF-BDD] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FGF-BDD] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FGF-BDD] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FGF-BDD] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FGF-BDD] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FGF-BDD] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FGF-BDD] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FGF-BDD] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FGF-BDD] SET RECOVERY FULL 
GO
ALTER DATABASE [FGF-BDD] SET  MULTI_USER 
GO
ALTER DATABASE [FGF-BDD] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FGF-BDD] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FGF-BDD] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FGF-BDD] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FGF-BDD] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FGF-BDD] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [FGF-BDD] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [FGF-BDD] SET QUERY_STORE = ON
GO
ALTER DATABASE [FGF-BDD] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FGF-BDD]
GO
/****** Objeto: Table [dbo].[DigitoVerificador] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DigitoVerificador](
	[NombreTabla] [nvarchar](50) NOT NULL,
	[DigitoVertical] [nvarchar](max) NOT NULL,
	[CantidadRegistros] [int] NULL,
 CONSTRAINT [PK_DigitoVerificador] PRIMARY KEY CLUSTERED 
(
	[NombreTabla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Etiqueta] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Etiqueta](
	[IdEtiqueta] [int] IDENTITY(1,1) NOT NULL,
	[Formulario] [varchar](100) NOT NULL,
	[ControlId] [varchar](100) NOT NULL,
	[DV_Horizontal] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEtiqueta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Objeto: Table [dbo].[Familia] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia](
	[id_familia] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](200) NULL,
 CONSTRAINT [PK_Familia] PRIMARY KEY CLUSTERED 
(
	[id_familia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[FamiliaFamilia] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaFamilia](
	[id_familia_padre] [int] NOT NULL,
	[id_familia_hija] [int] NOT NULL,
 CONSTRAINT [PK_FamiliaFamilia] PRIMARY KEY CLUSTERED 
(
	[id_familia_padre] ASC,
	[id_familia_hija] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[FamiliaPermiso] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaPermiso](
	[id_familia] [int] NOT NULL,
	[id_permiso] [int] NOT NULL,
 CONSTRAINT [PK_FamiliaPermiso] PRIMARY KEY CLUSTERED 
(
	[id_familia] ASC,
	[id_permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Idioma] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idioma](
	[IdIdioma] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[DV_Horizontal] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdIdioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Mascota] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mascota](
	[id_mascota] [int] IDENTITY(1,1) NOT NULL,
	[dni] [nvarchar](8) NOT NULL,
	[nombre] [nvarchar](20) NOT NULL,
	[especie] [nvarchar](20) NOT NULL,
	[raza] [nvarchar](20) NOT NULL,
	[fechaNacimiento] [date] NOT NULL,
	[DV_Horizontal] [varchar](max) NULL,
 CONSTRAINT [PK_Mascota] PRIMARY KEY CLUSTERED 
(
	[id_mascota] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Permiso] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[id_permiso] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](200) NULL,
 CONSTRAINT [PK_Permiso] PRIMARY KEY CLUSTERED 
(
	[id_permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Rol] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[id_rol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](200) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[RolFamilia] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolFamilia](
	[id_rol] [int] NOT NULL,
	[id_familia] [int] NOT NULL,
 CONSTRAINT [PK_RolFamilia] PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC,
	[id_familia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[RolPermiso] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolPermiso](
	[id_rol] [int] NOT NULL,
	[id_permiso] [int] NOT NULL,
 CONSTRAINT [PK_RolPermiso] PRIMARY KEY CLUSTERED 
(
	[id_rol] ASC,
	[id_permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Traduccion] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Traduccion](
	[IdTraduccion] [int] IDENTITY(1,1) NOT NULL,
	[IdIdioma] [int] NOT NULL,
	[IdEtiqueta] [int] NOT NULL,
	[Texto] [nvarchar](max) NOT NULL,
	[DV_Horizontal] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTraduccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Usuario] Fecha de script: 4/10/2026 22:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[dni] [nvarchar](8) NOT NULL,
	[nombre] [nvarchar](max) NOT NULL,
	[apellido] [nvarchar](max) NOT NULL,
	[usuario] [nvarchar](max) NOT NULL,
	[contraseña] [nvarchar](max) NOT NULL,
	[intento] [int] NOT NULL,
	[bloqueado] [bit] NOT NULL,
	[mail] [nvarchar](max) NOT NULL,
	[rol_viejo] [nvarchar](50) NULL,
	[idioma] [nvarchar](10) NULL,
	[DV_Horizontal] [varchar](max) NULL,
	[rol] [int] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DigitoVertical], [CantidadRegistros]) VALUES (N'Etiqueta', N'0A521BE90B7E05216D02AAE76B804EACBC874D128B2BBBBF9FC38C7C333B154E6FD', 337)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DigitoVertical], [CantidadRegistros]) VALUES (N'Idioma', N'19D2A3FF7C91EBD6E7C8F699307D7DB64079D49A10C24556138E48CF13A9CFA11', 2)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DigitoVertical], [CantidadRegistros]) VALUES (N'Mascota', N'4B17DC762FB6F39B61CCE86D33F4F9A30D7044738E35CA2D45F131D2AC3E28AE', 1)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DigitoVertical], [CantidadRegistros]) VALUES (N'Traduccion', N'155488727FA6456CF0B54CB05F4A8C7FAFE33688EB013D67A322E0BF09D600E3F38', 674)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DigitoVertical], [CantidadRegistros]) VALUES (N'Usuario', N'122B1F35717C65A4A3C2CAF45ACD938192FE30DC03870EA06574869EDC2CD0E91', 4)
GO
SET IDENTITY_INSERT [dbo].[Etiqueta] ON 

INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (1, N'Login.aspx', N'lblTituloLogin', N'2DD93D3EFF8126B043CE625C3A491D19E898D52C6D45257E71D8580B0426D905')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (2, N'Login.aspx', N'UserName', N'37E4DD98BEBF4927711D98B8CD95D02C54EFF2BEB5826891CF0915D6FD2B8987')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (3, N'Login.aspx', N'rfvUserName', N'236BF7BB89C15C7F0DE2D8E705BFF4835BCD430357D91DD47EDAD5ECEC4F446A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (4, N'Login.aspx', N'Password', N'421B5902F57908732389EFF2A601348E93C258316F70D35A7BFE23D6701B5320')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (5, N'Login.aspx', N'rfvPassword', N'7E26BB0631F1D6DAF0D862961270C8C9B9BDA325D7DC06DACEC6D3FED062DDE1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (6, N'Login.aspx', N'chkRememberMe', N'0D067E8614E773BE711405B3BC6732DE56F7F3CFFC9889D990C6C741B2B8B8994')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (7, N'Login.aspx', N'btnLogin', N'0E0EA4F7DA0ACA735ADA052EB2A17C9330857F6C7A1BF884B35B8634274553BA5')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (8, N'Errores', N'ERR_SESION_YA_INICIADA', N'26997A962EE621F891CC5E17EB8D7F7DF95F607F31396D39FFCE483202C0B857')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (9, N'Errores', N'ERR_CREDENCIALES_INVALIDAS', N'51303472EFCC3DAAC43AD2D4ECFD1B2503C5AE13FAB654C401E4BD4ABB8EB106')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (10, N'Errores', N'ERR_USUARIO_BLOQUEADO', N'09304EFF06A1AD459F6F56B4210F333BE2D82154928B45CB3F9CFE949E4C1E4AD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (11, N'Errores', N'ERR_INESPERADO', N'0FBEB4EB6C5994219714D3F5A4BE0BAE2FDB9EF1959C84D60153C258F7C71E148')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (12, N'Site.Master', N'lnkLogin', N'08425398A9B1DAB45E0DE85196CE3C62C071FDB396E615B698B65EB34B5B6AAE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (13, N'Site.Master', N'lnkRegistro', N'4B9C8CF3401F5B7F7CC06BAC34AD3B4227E5F7DF8DD4238A3A54D8F8E5130A09')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (14, N'Site.Master', N'lnkAdmin', N'1A4E1525418C3B5B755DC5C49C78A082DDC04A8B591EAC409180C87C2E0AF718')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (15, N'Site.Master', N'lnkMascotas', N'08E510041A78DA1E5675A060CA5D943FF84953E3C87696C0947BD971AD06EEB51')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (16, N'Site.Master', N'lnkWebMaster', N'45708ECF34D368452C26C4C36ABE635DDC16EBF0E48DF0E7CC8EF0995ABECB9B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (17, N'Site.Master', N'lnkCambiarPass', N'76AA6F9345918965C8F7C70C5DA760C35CE5ED6FB2A8BA37A091A30DFE3537BA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (18, N'Site.Master', N'LogoutBtn', N'0F1575FF820A5B078859FB6E73656B4053F5EAFDA69F7137958F42672947BCF91')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (19, N'Default.aspx', N'lblBienvenida', N'5717E1644A49B8C09C47D27B1925D6140BDCAFED8A1F4751DC055177D6A619B8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (20, N'Registrarse.aspx', N'lblTituloRegistro', N'0C238210ACFC54CED73C07DC375BC9E0A041D455EFD60A7B1693FBC5E5000E5B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (21, N'Registrarse.aspx', N'lblSubtituloDatos', N'3DD829B3D7E90B2E116724D4D905910DD7960C5698A280CEF8A3C7D7E9F6545')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (22, N'Registrarse.aspx', N'DniUsuario', N'0C5F6A9A1DF35E2878EB13F511ADFA6CE389A5A892D1EC156D53F583E1EB7DE15')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (23, N'Registrarse.aspx', N'Nombre', N'0FF7C029FE5AC777E8A7A55EF9FE0170F51DADDAE6BF2FEA0250288E926276F43')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (24, N'Registrarse.aspx', N'Apellido', N'0F31689C4A9616DB56A2185A69EA7C2E3D53DAF19389C516600A88B47F3D43C11')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (25, N'Registrarse.aspx', N'Email', N'359704484F11BA759D1CEDEF0E734E8AEA3AA2EF6EBC5B32243FAC7C047215AD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (26, N'Registrarse.aspx', N'UserName', N'17F8C3132637B385BD6970914BC45EA6ECE1552AD5C0AA287A827DB4956FF22C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (27, N'Registrarse.aspx', N'Password', N'59E2D129338CB2C11139459F25D3EAA252D5CE70014A5AED3547B4DC8F3EBDB9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (28, N'Registrarse.aspx', N'ConfirmPassword', N'0950EAEF2FAD87F13DC645CBF69A78C5EC0924096BE0866A55BA0261ACDF81067')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (29, N'Registrarse.aspx', N'BtnCancelar', N'09310CA756D6CFA9CD0AA2914983A0852409700EEA5F6C13CB76E5A3DFB0DDF0E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (30, N'Registrarse.aspx', N'BtnRegistrar', N'0C2E20BE8ED9B2F24EB502CAE2E9C2B0DDBC15F59AC38BA7EB52A180D568B420B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (31, N'RegistrarMascotas.aspx', N'lblTituloMascota', N'3652D55E508871BF421365A51627E17ACAE4C9DEA61E6EF02D95838793619D12')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (32, N'RegistrarMascotas.aspx', N'lblSubtituloDatos', N'0A1D075AEE3464E5256D02AF1655E77716F19FA02D0931611C4D172277D573B96')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (33, N'RegistrarMascotas.aspx', N'lblNombre', N'76EBEE20E58BDA3A0F958BE40507BC78BE0346A17B9DEAEB9336F28F22CE5691')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (34, N'RegistrarMascotas.aspx', N'lblEspecie', N'0A1A50FEEB976883C2DB35602934595CBB076CD64030595741FF6B3D1DD0B1716')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (35, N'RegistrarMascotas.aspx', N'lblRaza', N'0E84660C07FEAEA1EF635DEC6C5567C0ADC53DD4705D425077CEC40C5339B11B6')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (36, N'RegistrarMascotas.aspx', N'lblFechaNacimiento', N'5618A88AA4D5366047D35BB4F124C8E7A3FEE1DCEF0AF702950F5FE795BE93F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (37, N'RegistrarMascotas.aspx', N'txtNombre', N'71792B2135A87FDB68BF71CFA79E9D67580ECCFAF07B7F4FAA5E0C08D4D78776')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (38, N'RegistrarMascotas.aspx', N'txtEspecie', N'0B96A7C1FF1957456F0DAEFA1F9BA2C560D5F83EEDDA5CB25DBA290E2FF78A9A0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (39, N'RegistrarMascotas.aspx', N'txtRaza', N'73812CCE232C16F7AF5535A8346B560D368AF1B643237DF94DD7A2C09DCDBCFB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (40, N'RegistrarMascotas.aspx', N'btnCancelar', N'47E110AF02794D4D7A7764EC742969DE4A9075D3C43195FA44E1A80329A9243D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (41, N'RegistrarMascotas.aspx', N'btnRegistrar', N'3DB15D14F2678D047AE8DDB9057F8DBF4DEE8A19905BC3F70214C7BF771D6576')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (42, N'Cambiar-Contraseña.aspx', N'lblTituloCambio', N'0896C22B8EEB7D78C3C7D23090361C1E13EB02062A48FABCD8654D18DEC34A4F2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (43, N'Cambiar-Contraseña.aspx', N'lblSubtituloDatos', N'09978619F4010A642F80B278D3FBCD9152A52DAFFB851B9D3EF731C8AF6EFA3C9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (44, N'Cambiar-Contraseña.aspx', N'txtContraseñaActual', N'09992160EDA4AECF2CFF848CCF10DDA9E66471AB013A76872CBD97211737EA3A4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (45, N'Cambiar-Contraseña.aspx', N'txtNuevaContraseña', N'0A74B5D17133CDDBA079978DB6D9C0424C157F996C9320C022360216653A92130')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (46, N'Cambiar-Contraseña.aspx', N'txtConfirmarContraseña', N'4C945067CF56F0D294D919B62A7386162551CBBA4279EAADD6D78E7B0ADCA0D2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (47, N'Cambiar-Contraseña.aspx', N'btnCancelar', N'154126A52E14EF4648B1DF4C4BACC187F31F250CED1C226915A7F940E2FCEC93')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (48, N'Cambiar-Contraseña.aspx', N'btnCambiar', N'7BD8D7D5E4450AE7B3F5B350FA9FE5F60B68882DF89DC07E8E16E6D479FBE634')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (49, N'Backup-Restore.aspx', N'lblTituloPanel', N'0AE331DB19C1E03D816153F9BD9D689CC81BE74CECC5B4DAA38F11C14C3E816FB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (50, N'Backup-Restore.aspx', N'lblTituloMenu', N'7CC2D29EA3F5C3AD9CB90EAA524E558F36492192EF27B23FE637263F3D6111A4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (51, N'Backup-Restore.aspx', N'lnkMenuUsuarios', N'0DE6A4C2ECEE75B720DE4A2D3454B3C77AD1D090166DE886615BEB378E75DE694')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (52, N'Backup-Restore.aspx', N'lnkMenuBitacora', N'5EDD5014659F07B7C65470CF79E063603910413EA21AF7A121B4F94254DDC8D0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (53, N'Backup-Restore.aspx', N'lblMenuPerfiles', N'08ABE761546950A6FBD0CD793B9B76B2B546BD90AB79957BA3CE7CB2E804545E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (54, N'Backup-Restore.aspx', N'lblMenuFamilias', N'0A49F0F7ECA00CD4191771490FFF29385EC43AFC6BE9FB30D25E3C367829AD524')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (55, N'Backup-Restore.aspx', N'lblMenuBackup', N'0D2B831C24EA32292153B6BEC92F0B327003165F6184763BE4709805F5F983EF7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (56, N'Backup-Restore.aspx', N'lblTituloBackup', N'36EB8C84ED07647F2E031179B993CF25845378FEF12F0616885B0DAA24F716A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (57, N'Backup-Restore.aspx', N'lblDescripcionBackup', N'0DBA25339442362AB24090CD152C79E9B07A46D03C1DD96D28FA6BE7A9DAC38EE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (58, N'Backup-Restore.aspx', N'btnBackup', N'64C7B339AB6FB04D18C58F6A5CBCE627B379EABD88A32CC91226399BE6009170')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (59, N'Backup-Restore.aspx', N'lblTituloRestore', N'0DA1ED58DB5A6894EF538319360026A52CF43711318E0D82B78EE902E30339AA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (60, N'Backup-Restore.aspx', N'lblDescripcionRestore', N'0CC7768095FE2BF5399BE01B0C3EE67A8BE8E0B48C28B4A1FA0EBCF80F6BFFC1F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (61, N'Backup-Restore.aspx', N'lblSubirBak', N'0EF52D117F79EB895293760DCC339125F0A8D8E89FC59B862728294690963AE8D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (62, N'Backup-Restore.aspx', N'btnRestore', N'0E9C54F7F2FA9095069A5AEF2EEB382051D70F349EEDF4CAFEE7AD9655467D78F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (63, N'DV_Form.aspx', N'lblTituloInconsistencia', N'34A8E04CCAEB61C67EBEFB1C817D32118BAC88E35F79F33B4F023AFCB033CDB1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (64, N'DV_Form.aspx', N'btnSalir', N'5B5CC4F72BAF53E9B45E31799586DC03B617A7D5369E2B160F358E47323C0596')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (65, N'DV_Form.aspx', N'btnInicializar', N'6D0AA1E2D83177CEA6F5607B07F296F199F2EC016A1BA1561D2FEA0790B0C5F5')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (66, N'DV_Form.aspx', N'btnRestore', N'092538CDABBFBD0985041E8C0DF5442774A8C70BFDC4E5BE273FFB5C1CE58CF2A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (67, N'DV_Form.aspx', N'lblSubirBak', N'3E8A3591363AFB66A75191D921A4CBCA996C78947B7E45B4F058D8547DDD209A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (68, N'DV_Form.aspx', N'GridInconsistencias_Header_NombreTabla', N'7C00A490BE5B21E66D62878B7DF4768976C6621F2124013D7E742C5BF5A54FB7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (69, N'DV_Form.aspx', N'GridInconsistencias_Header_IdRegistro', N'0ECD769808696C756D21FE4D9CB1E50B460ED1ED120C40728B17E3F489F779A8B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (70, N'DV_Form.aspx', N'GridInconsistencias_Header_TipoFalla', N'0BF3A8E38BCB93587BCAAA2822611E2C0132DECD55A4789A544BD500D27D2BF14')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (71, N'BitacoraEventos.aspx', N'lblTituloPanel', N'0964858D5F8CAA255F3157A8889EDE7068CBC0801388BC4D8FBA93C42DEBECB7C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (72, N'BitacoraEventos.aspx', N'lblTituloMenu', N'0C2E3579C47CABE3706FC1510BC7A4899D5C696277376948271C8B8C5C3F9AF2A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (73, N'BitacoraEventos.aspx', N'lnkMenuUsuarios', N'0963127D3E2206C28D77B128DEB3ECD9736F033BFBD54C30E27AC88DC8793107E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (74, N'BitacoraEventos.aspx', N'lblMenuBitacora', N'0D1AE8E57F17044ADBC773B131271FFD0C3192373E217C027A4062CBF652326F0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (75, N'BitacoraEventos.aspx', N'lblMenuPerfiles', N'7C5E6ACA53E56BE29CC6DD75AE7EF470583378E296CD4ABF59C9C31D349A475')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (76, N'BitacoraEventos.aspx', N'lblMenuFamilias', N'472AB38A423BF5EDF33FCCBF699B1DA0E8293867DF4AC048A7E988D84B24229E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (77, N'BitacoraEventos.aspx', N'lnkMenuBackup', N'0D794F3A5F9C9900BAC99A8C8187FB46936B1AB4F8C5CF58FFE769134EB0CF998')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (78, N'BitacoraEventos.aspx', N'lnkMenuIdiomas', N'5EB39EF1BF1E2BAC5CE8E800786D36C024B527DAF0966E8D46CFB24352A28C5D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (79, N'BitacoraEventos.aspx', N'lblSubtituloEventos', N'0E1FFE853EB70CF66675640B3F2791699FACEE14A66B46392BB1FE66E7EB1F752')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (80, N'BitacoraEventos.aspx', N'GridViewEventos_Header_Usuario', N'0A32F69BE082AEAD5B1F377EE11E0A256CAD44E93886281AB8DBDF80F8F9B1ACD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (81, N'BitacoraEventos.aspx', N'GridViewEventos_Header_FechaHora', N'3D0032C9602963B0167F45FE7D06820132479B37A641478F604C8068E165DA58')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (82, N'BitacoraEventos.aspx', N'GridViewEventos_Header_Modulo', N'0C5DBDF551C487F39642D63C9B490D46DDD84F22F6A7550586A2001A73A47F46D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (83, N'BitacoraEventos.aspx', N'GridViewEventos_Header_Evento', N'0D28566258651358EC2C514597F6F78F41F4494F0BC72341DB3FB4D15903FD63E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (84, N'BitacoraEventos.aspx', N'GridViewEventos_Header_Criticidad', N'0D322B7B84CD4813E58C68BCB4B2CCDFB90DAB15ED79F4C73D73C7884601EB917')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (85, N'BitacoraEventos.aspx', N'nombreUsuario', N'3BB0DA44607946DCA63B9DB658B10D8D135FEFCF3DFC27ACFE0F6F0F54240169')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (86, N'BitacoraEventos.aspx', N'btnAplicar', N'0E20DB1CB77853266A5EF37F4B9307D7F38A8BE9E98CBED812EF9C129C1D8D219')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (87, N'BitacoraEventos.aspx', N'btnLimpiar', N'0EA2E264E76DB4D4F15F060B22BF95325A9FA220CAF9A4A45E2FB6B3A1CC071A7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (88, N'BitacoraEventos.aspx', N'conFecha', N'0CF12EE2F6CC60513567F5196B55CEED69123BF217EDD843CBA079C5AFF436D6F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (89, N'BitacoraEventos.aspx', N'moduloFiltro_Modulo', N'0FCEF54F2F140C208886DFB0331E244EF9F69BFF4E825C20FBA28512F318CE2ED')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (90, N'BitacoraEventos.aspx', N'moduloFiltro_Usuarios', N'70633800FE2A5AB1A435895BF1093C5809AAE79965098CB5E838B309082AC0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (91, N'BitacoraEventos.aspx', N'moduloFiltro_Administrador', N'08291B0E127CE90F85BFF91B8052BBD6B56644DB9911DE21D0533D87D69B1E434')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (92, N'BitacoraEventos.aspx', N'moduloFiltro_Clientes', N'0E6C0A93B0A8DCF9A2D8B5812203C67936408F4E6AEF80C84AD899C4C3B519122')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (93, N'BitacoraEventos.aspx', N'moduloFiltro_EnDesarrollo', N'36652B315D709FBB2CA2A670F5E309FB97C3B9B900E3F5AAF4426860FF396F85')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (94, N'BitacoraEventos.aspx', N'eventoFiltro_Evento', N'2A9CD77BCADD7FEC4BEC19C48EF000586A8934B631AB157174D2CC32F68C0B32')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (95, N'BitacoraEventos.aspx', N'eventoFiltro_IniciarSesion', N'0BB535A107BBB18B8887540474795469DA73EDA4C129FF9CC949A68AF90910FC9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (96, N'BitacoraEventos.aspx', N'eventoFiltro_RegistrarUsuario', N'39A7430F54953E7B1AE82611329093BFB6EB023D99F950A3799F985E4F62F2D6')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (97, N'BitacoraEventos.aspx', N'eventoFiltro_CerrarSesion', N'5BF8CA6C0FFC23C18913089773EBCBBE8574728997176F869EC7BB258C570906')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (98, N'BitacoraEventos.aspx', N'eventoFiltro_CambiarContrasena', N'0D538897870F4348A8A9B1C354BA9B5FFEB6001E5E47F01EA938763D666A433C7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (99, N'BitacoraEventos.aspx', N'eventoFiltro_DesbloquearUsuario', N'0CF1B1FD4CAB10A4F8D88DE0C5F2510C2A03945C85C6958E4AB9058BAF008D2AC')
GO
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (100, N'BitacoraEventos.aspx', N'eventoFiltro_ModificarUsuario', N'4E0A17320A0C867D04FCA58D3BC16294E97A09E67243FBCA0C8A8B02B5F56539')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (101, N'BitacoraEventos.aspx', N'eventoFiltro_HacerBackup', N'0B30362F81B6ADFFD6C30E2069A4EEB03AC7ADC644A89638541E11562010D41AD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (102, N'BitacoraEventos.aspx', N'eventoFiltro_HacerRestore', N'0AA1F72E732615D7503B3396839305D4B364DCB524525D80EE6819826FD38B2A2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (103, N'BitacoraEventos.aspx', N'eventoFiltro_RegistrarMascota', N'4DAE9817DA627001C41B5510A8785BEBE4F52CB4E47FEE6F44CDF706AF4008AC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (104, N'BitacoraEventos.aspx', N'criticidadFiltro_Criticidad', N'67613653D3C9B7AD6B03DD538A3A6909B2CB283C261D109B5A7252A49AA5E032')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (105, N'BitacoraEventos.aspx', N'criticidadFiltro_1', N'5B57C766C90BA9929D25DADA7AE276BFF21872F0E15CB9265C00EE401FC8C987')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (106, N'BitacoraEventos.aspx', N'criticidadFiltro_2', N'384BA5564D7BA69045B6018BD14F7F5AB0E33296D1EE56D71A5EE8CBD91B3F2E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (107, N'BitacoraEventos.aspx', N'criticidadFiltro_3', N'5F14E132FECD06AF02B7AF0BDEC34DFD160E29AEDB4CD339DF74B03CA49595F4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (108, N'BitacoraEventos.aspx', N'criticidadFiltro_4', N'45F4352B531A07C0856E701EFA752297A9BA86E6F7A9ED3AB834825DB15DD7CE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (109, N'BitacoraEventos.aspx', N'criticidadFiltro_5', N'0B880BED98E13CB95911F2D2D02E706A926A249A1B44D24519B2B88D1D43B407E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (110, N'Gestion-Usuarios.aspx', N'lblTituloPanel', N'7CF97D683F03A507587CC1937E0A75366D054DB138B08C1676114A09BF40A28B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (111, N'Gestion-Usuarios.aspx', N'lblTituloMenu', N'7F45A766845D644D5C882B8F1CDC0E33F9CA7FE95664E6138E286DDD242D6650')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (112, N'Gestion-Usuarios.aspx', N'lblMenuUsuarios', N'0F2C4354D26C5E1F18961EE5D37F5E0AF7D773E01B81A64E1191FEAD97C5E8BA1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (113, N'Gestion-Usuarios.aspx', N'lnkMenuBitacora', N'0BAF1DFEC5B06B32A0C2EE9D73313CE0434F651401FD8F365098BBD3706FA4E28')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (114, N'Gestion-Usuarios.aspx', N'lblMenuPerfiles', N'3B9BC93DC585622D6C0ADA5CF2C6D3B489270A79D9B2188CF8BBF2A406161A2C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (115, N'Gestion-Usuarios.aspx', N'lblMenuFamilias', N'0EC36B152CFF66ED00E8F99C6DBC9F375698ED5C5581D81461CC353CA145507B4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (116, N'Gestion-Usuarios.aspx', N'lnkMenuBackup', N'08FBD5054B10CF688E9ABC82BB0D745B0872953389149075780555D55BF0DB9A2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (117, N'Gestion-Usuarios.aspx', N'lnkMenuIdiomas', N'3A4B219FA8B9A529CD1C0F733A8D49811DCFD56DBAD5891E5B09E27D9A932989')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (118, N'Gestion-Usuarios.aspx', N'lblSubtituloUsuarios', N'6C4F993844D24D01E161980648C7A6C3C8F8321AED53D176AEFB4D1F80C9BE74')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (119, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Dni', N'1E85694EB3FB4598B79F0265ED117AC20A5A93429BCC976CC1FE063EB6E62E56')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (120, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Nombre', N'0B3B06D3B09ADF04CB2A65AE595DD53EB205961D96245E4E598EF63D9467661E3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (121, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Apellido', N'0E179574343C08AE9EB19531665831CD46B3146B73666D011B96CD59B453A7E8D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (122, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Mail', N'63FEF0BD466D0E365431AAE4E44AACD72F16EC98D3AFCE7819C87A8C35DC6BD1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (123, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Usuario', N'10785710F5C293837F7F20B270B3AB2F93F7005FDC19D1C359F1BE70C580B97E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (124, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Header_Rol', N'11EA43B92B0CE3A6B1C91B3521747608E0F9F49B8712AC9883D20E83E2555E30')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (125, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Seleccionar', N'7956EE1C1D2D45D1C59C6E44AE5631B7F1E33D1DB44F16D45F5AB4E5B3CD4378')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (126, N'Gestion-Usuarios.aspx', N'dgvUsuarios_Vacio', N'5FA290ED6D286D7CEC59F8EC89C9F99CFC7A88BEB54D5BC6A9D72E4FED586A6C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (127, N'Gestion-Usuarios.aspx', N'rblFiltroTodosActivos_Bloqueados', N'0AF955EC9641C32B2272057ED4D05D7FD3DAD92912EF468854ECCA77823CBCC6')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (128, N'Gestion-Usuarios.aspx', N'rblFiltroTodosActivos_Todos', N'449974BDEC7BD8C4D585B6968C80ECD2BF1ADB7AF3EE1258C9F5A759CE8A733E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (129, N'Gestion-Usuarios.aspx', N'chkVerEncriptado', N'0D3964C70BAA549E0FF32095B15F1D69EE503DBDBD41843A31E3715E2C2AC6816')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (130, N'Gestion-Usuarios.aspx', N'lblModo', N'644D03B59A2DF49D06A1D858028053E1867EB34DC7CDE7FC2B99F79AD628064D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (131, N'Gestion-Usuarios.aspx', N'lblDni', N'7C0147298E0A00FD421DEC4D0A0B0B6A5B7DBC529803B80B5D3E9B7DD55C747D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (132, N'Gestion-Usuarios.aspx', N'lblNombre', N'41887015C54423B5148386103CD11386110D83BF78EE44CABF259148CF1DEB8A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (133, N'Gestion-Usuarios.aspx', N'lblApellido', N'4D1A8A57B856870B459E56E4DF6B05E820A172E63F19838AA0852F091567598E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (134, N'Gestion-Usuarios.aspx', N'lblEmail', N'4920471953C6102F5A26B5DCFF2F1AF5C56B11836330D97FA4D5D02F94FBA563')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (135, N'Gestion-Usuarios.aspx', N'lblNombreUsuario', N'0DF6E0F8C34ABAE7A11A953D203B26DDC90C6D78F0E08E2939CCD46C60D13FBBA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (136, N'Gestion-Usuarios.aspx', N'lblRol', N'2231AA33A95DA375ECCC6D914D63C391F249BEFDA65BEB44F3B530E69263DED7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (137, N'Gestion-Usuarios.aspx', N'ddlRol_Seleccionar', N'48AD71D266E62F4737426ABEBE9CCAE8E345B0FDCDBF3E4D97D4D2D721D53068')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (138, N'Gestion-Usuarios.aspx', N'btnCrear', N'0FF9B8E462D3E8138EA5C732A7474EE90C96DB06E5B5D7CF1128166BBB8F903F6')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (139, N'Gestion-Usuarios.aspx', N'btnDesbloquear', N'3435B88D79E89891EADBE1E9C20013A156CBD6FEAF0B723AEE434A0E6D9FE0D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (140, N'Gestion-Usuarios.aspx', N'btnModificar', N'7A61821BD27E4F08CA4037BC3123F7FAF9F42C8DDFFDC40D3787F24714B6524A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (141, N'Gestion-Usuarios.aspx', N'btnAplicar', N'18839645646CE8485A6F45A5728CE2DD2E1F723219CBC72125A3042A021CFDD8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (142, N'Gestion-Usuarios.aspx', N'btnCancelar', N'0E46984CC9DD38AFE980D24DF21B2EF3A7553FB813DD2732687B05E99C859BE95')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (143, N'Gestion-Idioma.aspx', N'lblTitulo', N'5C840B0CD12A2BF6ACB5C299358F83CB8A98D911811151B459EC218AB2835D87')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (144, N'Gestion-Idioma.aspx', N'lblTituloMenu', N'0E3CBD9F4280EF543C76E9642A47897DD896AFD69FDB1F347AEBD554E4803D15A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (145, N'Gestion-Idioma.aspx', N'lnkMenuUsuarios', N'3C266623A624EB3598D145AD741235BBCC4A905FDBC2608FC1AD79CA37105469')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (146, N'Gestion-Idioma.aspx', N'lnkMenuBitacora', N'0A5A4772B1433A9EDD26D02C91B7E758876110109A1B9D6E625151EFB64A8F0C5')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (147, N'Gestion-Idioma.aspx', N'lnkMenuBackup', N'08EECCA8DEE0CF93A5CB3B842644D77DFF0A9FB1BE78E90217AABA18AEB5626BC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (148, N'Gestion-Idioma.aspx', N'lblMenuIdiomas', N'0D4097E27898EDAFEEB334ECAD2C0554234DE39D69D6FAD4E67F653E965DD251A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (149, N'Gestion-Idioma.aspx', N'lblSubtituloTraducciones', N'0E1F65A9FB57DB110984EC01BEAA498010B03EB30A794CE5DBDEA348E1CD5ED13')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (150, N'Gestion-Idioma.aspx', N'lblIdiomaEditar', N'0CB18553871E9302BF9A0E842E8DC8B26BC406CB19450A5A75CC9ACC28D75A892')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (151, N'Gestion-Idioma.aspx', N'lblFormulario', N'5D53DC65A0270388A720765025617AFB1F5E18A8C05C338143F6FB48A3A2C6B2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (152, N'Gestion-Idioma.aspx', N'dgvTraducciones_Header_Control', N'147B5471AB03112989CB802C851F0070C1310551009759829DD4B142389EB822')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (153, N'Gestion-Idioma.aspx', N'dgvTraducciones_Header_TextoTraducido', N'0F557C16568A7AD33B9D0E6DDDDC3BE7898204215934F69E43E1AAD7B6BA7984A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (154, N'Gestion-Idioma.aspx', N'dgvTraducciones_Header_Acciones', N'0B1238FDAAB5705A372061DB9A17D45D83F9BF28E5FCF1C09FAE8095B5C5C8FFC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (155, N'Gestion-Idioma.aspx', N'dgvTraducciones_Vacio', N'568E5D345F9B89C5E71509B044130409BB813E3BB65578E47309240E180A1C16')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (156, N'Gestion-Idioma.aspx', N'dgvIdiomas_Header_Codigo', N'1AA8AB74FDFEF1905F986E90B6AB155BDFBFF5A4CC1A9D1792D67144E71E3675')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (157, N'Gestion-Idioma.aspx', N'dgvIdiomas_Header_Nombre', N'361BF584BB7F21575A9046EE774FA0E53EFE4996FF8332B3A93464C798B57364')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (158, N'Gestion-Idioma.aspx', N'dgvIdiomas_Header_Acciones', N'0B8A0BC1849DA792C466D87CB2D52395F4E16F68F5CC6D4B291431C630CE386B3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (159, N'Gestion-Idioma.aspx', N'dgvIdiomas_Vacio', N'083F163D0A50D0C5BB5ABD65DED993F508A5067FA49F825D60A55679FA1593C9D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (160, N'Gestion-Idioma.aspx', N'lblTituloNuevoIdioma', N'76D0A4D4DEFF4FA5456618A4882886746E97A91EED0DA5980A52DE1C19373A20')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (161, N'Gestion-Idioma.aspx', N'lblDescripcionNuevoIdioma', N'0EFC7029CAE810C8705D0E5F36DF1454922C1B16056342197A13CD49511846A2B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (162, N'Gestion-Idioma.aspx', N'lblCodigoIdioma', N'0C480B882A80296BC34699DE940D9242BCCCD3859E6A39C066E177449C485C8AA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (163, N'Gestion-Idioma.aspx', N'lblNombreIdioma', N'0C73EF2C03882854A456B942DF2980E259D32B0B2EE411B4F3093B68B00474B13')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (164, N'Gestion-Idioma.aspx', N'txtNombreIdioma', N'0F6E44CCC106D023432F1EB567A2BAF8063E06973D46508B9BCA0FA1063B7FBA0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (165, N'Gestion-Idioma.aspx', N'btnCrearIdioma', N'1279E211E88483EC258D17B4D80F9B7355663EA9F602B9E43EA418D5DC5E69CE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (166, N'Gestion-Idioma.aspx', N'lblTituloIdiomasRegistrados', N'0ACAC2EB5091C1F5EED042C6CFFFCAEA073A1030ED5DE77A695D7DEBF7C6F1FB6')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (167, N'Gestion-Idioma.aspx', N'btnGuardarTraduccion', N'0D32CFE1A75FCBE216FCA155CF995476FE0A81B750A2516CC745F0C439A8447D9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (168, N'Gestion-Idioma.aspx', N'btnEliminarIdioma', N'08BF81AF7D682CBE3F7EC7236B17785A3FB507D4CA96D1519D4DDD1E85EA77E9E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (169, N'Errores', N'ERR_SISTEMA_MANTENIMIENTO_MASCOTA', N'5E89BAE4E3CC9743F94B70E279697D4E614021DD20F58B3BC7040C64BAF88EFB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (170, N'Gestion-Idioma.aspx', N'MSG_RESUMEN_TRADUCCION', N'2293556BA84DBA2462A2A365DCE8A403EE2421183E81015069CDA070BFDCFAF3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (171, N'Errores', N'ERR_ACCESO_DENEGADO', N'0DD67E22A04B9131BDCB8EA50A6CE5AE1DE3E3F126E76AE85DD249B417A4C1C42')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (172, N'Errores', N'ERR_ACTUALIZAR_PASSWORD', N'0933F163D1E9604703746DE0DED3B842DEE6D633CCF2E8D49402E681594FC1515')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (173, N'Errores', N'ERR_ACTUALIZAR_USUARIO', N'1F99C2985DF4CAC5867D2A487D424EFE5E4DD801ADCDC15DE4AB701E75A872E7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (174, N'Errores', N'ERR_APELLIDO_INVALIDO', N'082A4AA9A06692871E6EC4DFE54E84052F9D7A3FC368D003DB8F2060A34C05299')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (175, N'Errores', N'ERR_APELLIDO_OBLIGATORIO', N'78234F58F3A57900441606AE8A67A450D98300C2A53164EEE2DA19512D548580')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (176, N'Errores', N'ERR_ARCHIVO_EXTENSION_BAK', N'26DE3E63F9BB3BEAEA9FE38786443F036E0995DA7B5F72ACEDFA2BDD1BE56618')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (177, N'Errores', N'ERR_ARCHIVO_NO_SUBIDO', N'0E0A2C16569A40004A5738DDD59544C306716FED1CD4A64BC2163DCFF94F57189')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (178, N'Errores', N'ERR_ARCHIVO_REQUERIDO', N'260F1278A8B85F23EFC13EB3E536844272381B76870CE7F133F9392616FE510D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (179, N'Errores', N'ERR_BACKUP', N'0C8E37FADD326F87DBF989E86951010C8710D62430464A55958589C029356F9EC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (180, N'Errores', N'ERR_CAMBIAR_VISUALIZACION', N'12B91AE37EE3A6B43656533D14E6F36D9FDD37342E6DF7FAD0027FFD8FD4D37E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (181, N'Errores', N'ERR_CAMPOS_OBLIGATORIOS', N'1FE158F22F6FDA58D1541BE15D957AB55EFCF6D5E2726E5D05FA98FC879814EE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (182, N'Errores', N'ERR_CARPETA_TEMPORAL_NO_EXISTE', N'0C94C41108B70E094716B5CC25CF24501A40D6E2D84DCC1FA273C9645B599F660')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (183, N'Errores', N'ERR_CODIGO_IDIOMA_DUPLICADO', N'0B9AC32759CC7659650BDE314949D93ED4FA74484AC91A95A149F10C6331DF3A4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (184, N'Errores', N'ERR_CODIGO_IDIOMA_FORMATO', N'6C3C09450CD30FC4DBC28DBAF2603F88B05CB5CE88949464EA2CD1D8D0EB70F8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (185, N'Errores', N'ERR_CODIGO_IDIOMA_OBLIGATORIO', N'6E8DB8888DF2736FB189DD38D3A8307779DFC91FBB382A647931930E90B65F4D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (186, N'Errores', N'ERR_CONFIRMACION_NO_COINCIDE', N'6D6ECFF97C9A5DBAEFD31EE793ED496DF933953BB482CCD1732198FA95056825')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (187, N'Errores', N'ERR_CONSULTAR_USUARIO', N'3403FF6B87ECB4FA0A3453BD62B4DC226974EB1B5F706C7208F2561D974EEC57')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (188, N'Errores', N'ERR_DNI_8_DIGITOS', N'0E10371FF51A01D043A8DA2AB603F2F959C66CF5B35F44AF889F3A7D4EA992058')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (189, N'Errores', N'ERR_DNI_8_DIGITOS_USUARIO', N'18918A9A4E19D02255ACDA984CA42CA743EFF47CFF9A0F647C1DE8D3EB5CECFA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (190, N'Errores', N'ERR_DNI_EN_USO', N'0EDA47B79C1EBED1A9D06D1E3D6D080C4F78619DBD9D18340E9397AD93151D74D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (191, N'Errores', N'ERR_DNI_OBLIGATORIO', N'0B9B47684B1BC438FBF2E70A989036C3665DB29EFFF93DD62C749E9161E722569')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (192, N'Errores', N'ERR_DNI_YA_REGISTRADO', N'0C1F439A3F8CEB5E857587B6B2F3328A17F795B618F0ED28307A1F550F62D6548')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (193, N'Errores', N'ERR_EMAIL_EN_USO', N'24C16E6528A03DDE1E8555A258677FAFCE08D3D2201D54655B3B63F0F2B5A0DA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (194, N'Errores', N'ERR_ETIQUETA_NO_IDENTIFICADA', N'1D6281CDE907001348BE8155EFEB689669759CE91FF14EAE791468EEDCB93D39')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (195, N'Errores', N'ERR_ETIQUETA_NO_PERTENECE', N'1ADABBB3240F2C1233AB27B8BC4F859407C2E3D8D98B886F68BC5DF808DB3C3D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (196, N'Errores', N'ERR_FECHA_NACIMIENTO', N'0871394C6E4835196534F4C407F1957D6508989E9566D0EACC0709CA909B6847F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (197, N'Errores', N'ERR_FILTRAR_LISTA', N'5A31F3813C957ECAACEAE77C790A29C2999DB8129AB78D1CCA52F1704B71CEDB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (198, N'Errores', N'ERR_FORMULARIO_OBLIGATORIO', N'40F1AB87C8542423E09C8202484FEEEFFCE3AED5D337489360F2DC58B5D9E033')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (199, N'Errores', N'ERR_IDIOMA_NO_EXISTE', N'1BE847CEE2BB8B0F72357ADB69CE7A42857F837E2D8FE7A4F54EB4F7841EA5DD')
GO
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (200, N'Errores', N'ERR_IDIOMA_NO_REGISTRADO', N'0A6A00A49ECD1CFFF2945FB1CD133A6269E4C7AF47FA1F6A2ECE5414A9134B85')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (201, N'Errores', N'ERR_IDIOMA_NO_SELECCIONADO', N'689A97C76CCF715C87CA2ACD9D053448F17DBB4D84A40F2E64F2DD8418F1EE76')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (202, N'Errores', N'ERR_IDIOMA_POR_DEFECTO', N'0D46ABF5F363DC3327A67BD32522BC7B72A320775373DCBAA95028D8AD8AE746')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (203, N'Errores', N'ERR_IDIOMA_POR_DEFECTO_FALTANTE', N'08EDBD433861F5FAF03462DF686257E5967BB481FCE3C0545F3912A368BBC1447')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (204, N'Errores', N'ERR_NOMBRE_IDIOMA_LARGO', N'08FCEC662E68E2ADA36E871ECD63F720A740CA54D146630BD8400161083F60056')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (205, N'Errores', N'ERR_NOMBRE_IDIOMA_OBLIGATORIO', N'0E3DEAC33C79E66913F0AAE60322B95C3527AFFD1F37CE6F5F59911676BEF4835')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (206, N'Errores', N'ERR_NOMBRE_INVALIDO', N'15F5915B051410117A3EBCB1AA29207C64CF04F2A1E30AE97DD1153829A2D3D2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (207, N'Errores', N'ERR_NOMBRE_OBLIGATORIO', N'13561591D8FB1408570260B25EE1B3A6DD5487A3FB0FF9615636F15620713357')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (208, N'Errores', N'ERR_PASSWORD_ACTUAL_INCORRECTA', N'39737A5385E5B7F1CB0C30906F9941B946CC0A222B2BEF7703352BB9C71191')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (209, N'Errores', N'ERR_PASSWORD_IGUAL_A_ACTUAL', N'3DED86BAD056A5C6A03A9E0F319F171D4468229F077D700D1765F47EB2E6DFA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (210, N'Errores', N'ERR_PASSWORD_INVALIDA', N'0FD3537494B89F9935D486FB40B5B94E4970D6D3F7D944C6FF9943D027039D2B1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (211, N'Errores', N'ERR_PASSWORD_NUEVA_INVALIDA', N'0D28E9DE2C0561AD97F78D7DC8522F78DEFE2BEAEE21829F808CCB48E4A0FB115')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (212, N'Errores', N'ERR_PASSWORDS_NO_COINCIDEN', N'0FDA0CD1C153C2A9A414B268CCD84E0CDD4FE06762742A9952DAD36210CB59582')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (213, N'Errores', N'ERR_PROCESO_RESTAURACION', N'0BF7904133C49E314652EDD94BFEC893E668B18978F254425848569D799431F91')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (214, N'Errores', N'ERR_REESTABLECER_DV', N'0F0547E096A23DE2C961B90FCB7A4822629C878540C1109A432E13E09FA10F28B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (215, N'Errores', N'ERR_RESTORE', N'7797600E884F533ACE26CD5114B569DC9AE065EB19EDBD0814AE1D1ECBCD92C5')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (216, N'Errores', N'ERR_RESTORE_CRITICO', N'20DF94A3286F550A8EA0C3772CA3DE6C4DADFE1955B58409AFE98DE64AF0847C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (217, N'Errores', N'ERR_SELECCIONAR_USUARIO_DESBLOQUEAR', N'0D1B28D01DCCF642D6930EA3E6E5F48C7F8AB0F150C528701ED321F7AEC465403')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (218, N'Errores', N'ERR_SELECCIONAR_USUARIO_MODIFICAR', N'2BA189FBEA877848A198358462C88E99575A842F60D43EB4013B838E462F106A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (219, N'Errores', N'ERR_SISTEMA_INCONSISTENCIA_PASSWORD', N'616A350683ABD776F5B790A633CCE8B3EB7FDCAFA8EBC118D2A4BB3F2A7B8195')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (220, N'Errores', N'ERR_SISTEMA_MANTENIMIENTO', N'665F43F3526E30D1FF002DE7C41311B3636ABA67043342349A3C1D74D3597EE7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (221, N'Errores', N'ERR_SISTEMA_MANTENIMIENTO_ACTUALIZACION', N'37D3C61E590FCEA62AB3FF62B97FE770E24B6C7F8AE5AE9DD2105AC922AE71D8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (222, N'Errores', N'ERR_SISTEMA_MANTENIMIENTO_REGISTRO', N'093CE42DE4568F85D0D4465AA0F7B23417548A8E9D20F55083852B025A679D19')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (223, N'Errores', N'ERR_USERNAME_EN_USO', N'0B968856A5B766EB40A9319B236B70288A680D2A814A92F55D03980D881A63D42')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (224, N'Errores', N'ERR_USUARIO_CAMPOS_CREAR', N'69B6C1E06F3598DFAB10DF194FFB93BF7AE44F4A0A2ABD757A68B61CE4E1F9A0')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (225, N'Errores', N'ERR_USUARIO_CAMPOS_MODIFICAR', N'0C3845236F3D6CE7C606DC2CA5AEC0029C0752848CBACBE5B2E11B631610D9F87')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (226, N'Errores', N'ERR_USUARIO_MODIFICAR_NO_EXISTE', N'6332E3CAE1554BED49A53E7B8242C3533C9B5F1464C4E2218E6D44A9E46EA645')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (227, N'Errores', N'ERR_USUARIO_NO_BLOQUEADO', N'432E926FA115AA3B6959EB26E1CA5DBCFAE6162D595E79017763BFFBEA3E1463')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (228, N'Errores', N'ERR_USUARIO_NO_EXISTE', N'0B8E4E0CF5F7ABFBAE3733212966A4225628F7CB801428FA09A8BD0592D3C6374')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (229, N'Errores', N'ERR_USUARIO_OBLIGATORIO', N'3A28BD7D96F23EE42F6D1D822FE959BBB87335EAF441FB8C3973B73C9CADD02E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (230, N'Errores', N'ERR_USUARIO_YA_EXISTENTE', N'395037CC251A98258257E6B879D4DA2F1B2E1870654CA9C944008D16B52586F3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (231, N'Backup-Restore.aspx', N'MSG_BACKUP_CREADO', N'092FDD5A8A02B302D2566152B1CF782824F3EF837E1BC2F7D49E596CD4A761FEF')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (232, N'Backup-Restore.aspx', N'MSG_BASE_RESTAURADA', N'08B001CC85D0A001FE3B446FD8ADC0574C4AB343AFE65CC8032DDC99D9BF82F38')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (233, N'DV_Form.aspx', N'MSG_BASE_RESTAURADA_INICIO', N'710C44C7460979CE33BDCDFCCAB37E2B83FCB1437669349918B4D1661A28244E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (234, N'Cambiar-Contraseña.aspx', N'MSG_CONTRASENA_CAMBIADA', N'0EB2B76FA4346F99BD9031EAE7675997AC88113BC00C0789B3048BC5A871A5B57')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (235, N'DV_Form.aspx', N'MSG_DV_RESTABLECIDOS', N'0F9FA29A73B47D453F37841CE3B0A1C62955D609D4388665B68EF20DA096F40BD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (236, N'Gestion-Idioma.aspx', N'MSG_IDIOMA_CREADO', N'0975A6543854EFEECA8F65467A50C31E03157759D0A7C54A9DC353EA4DAB16F3B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (237, N'Gestion-Idioma.aspx', N'MSG_IDIOMA_ELIMINADO', N'3A390B5836B204CF3B79656F33AB2474EC5C40CCAF85254857A47CD279D5BE81')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (238, N'RegistrarMascotas.aspx', N'MSG_MASCOTA_REGISTRADA', N'19A906221C877A85B01A4A6B41C70C1069B28D356CAE75DCF64F1CCBEAACA16')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (239, N'Gestion-Idioma.aspx', N'MSG_TRADUCCION_GUARDADA', N'0CCAC10AB610045CCA2CE6F8EBC630784CA1E79414A452EFE82F829B72E5C2060')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (240, N'Gestion-Usuarios.aspx', N'MSG_USUARIO_AGREGADO', N'0EDEFA735BF8838903421AC7369201C9E499EFCD050C1738C8EFE0F9384144BB9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (241, N'Gestion-Usuarios.aspx', N'MSG_USUARIO_DESBLOQUEADO', N'09D312992172AFD088566B056EDBB57B344FC62226DC23B499193937A33029DBB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (242, N'Gestion-Usuarios.aspx', N'MSG_USUARIO_MODIFICADO', N'0EC8029A18E684DA675F0C82537D778BC505B237D72A5C939E6C9CB495AA828F8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (243, N'DV_Form.aspx', N'DV_FALLA_ALTA_NO_REGISTRADA', N'0ADFB729E59E7F7A5B61E7FD1352F2EE1B80FAB1725882B228513D837344E42AE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (244, N'DV_Form.aspx', N'DV_FALLA_ELIMINACION_NO_REGISTRADA', N'080CEC7898B284073B246598109A4C84DA22C34E938FBDBAE199C8585E1ED45BA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (245, N'DV_Form.aspx', N'DV_FALLA_MODIFICACION_REGISTRO', N'0B240AD2F8CA976652772091FFCB59C51B76A0F316DDAF1C085762166E0D703E3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (246, N'DV_Form.aspx', N'DV_FALLA_INTEGRIDAD_ESTRUCTURAL', N'114545D52254E92D03132CC36A08AC8F2BB9448CBCCE3C4F25F2BCA833FC24AA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (373, N'Errores', N'ERR_FAMILIA_RAIZ', N'348C071B1978F4454D9D5CA928385726DFC7DBEB38EB45247A784747155C8F72')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (374, N'Errores', N'ERR_FAMILIA_SELECCIONAR', N'0F04A5C06613DABD0F5A413197D1D447F96D031DE58A8F51EE7133F5B8D49DD89')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (375, N'Errores', N'ERR_FAMILIA_SELECCIONAR_AGREGAR', N'310DC3493E92E6C682E597C5211BC31E23E5B37C77FD8E17E777FE68513F74BE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (376, N'Errores', N'ERR_FAMILIA_SELECCIONAR_BASE', N'3DCF4345A74F2DFAAB51D80D9ACF186FA12FCD3C9DB7A8F567C8A7234FEC198E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (377, N'Errores', N'ERR_FAMILIA_SELECCIONAR_BASE_FAMILIA', N'09F57043B0AE31929953078FFB9CC8679D6CD96E8645968D72FA0F1EAF218D271')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (378, N'Errores', N'ERR_FAMILIA_SELECCIONAR_COMPONENTE', N'6E83567683A6205FDE75D4AF0DCB3A12292324EC6D837B208E9A8DCABF1EA2C8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (379, N'Errores', N'ERR_FAMILIA_SELECCIONAR_NO_PERMISO', N'7CB4B788FD34C42CABFE84AF686724CF917739248B9E937E0371563C9F3D957D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (380, N'Errores', N'ERR_FAMILIA_SELECCIONAR_PERMISO', N'0CFFE8C62D790C3AF18BAF10DC8D547C128095DC1634C76C9A189BEB104ABB13C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (381, N'Errores', N'ERR_PERFIL_PERMISO_DE_FAMILIA', N'0BC63B516536DEDA6FC58AA70632C08CE7D680CA273D15896D12A3FB9193CEC0A')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (382, N'Errores', N'ERR_PERFIL_SELECCIONAR', N'3C459C5C3B6F24F663A32618E05E28053654E63AE473CFEE2C2E8CD71BB1A7FB')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (383, N'Errores', N'ERR_PERFIL_SELECCIONAR_COMPONENTE', N'0B5A5B56D5C52667337CC2891C176264C1D8AA93509B6863BBFD81248A8C5B94')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (384, N'Errores', N'ERR_PERFIL_SELECCIONAR_COMPONENTE_ALT', N'0B2559B05683C673BA73C7A9FD8C8D45377CB9E1F18CE66F4F523BF360976778')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (385, N'Errores', N'ERR_PERFIL_SELECCIONAR_FAMILIA', N'0FD1CFD414778FE1829D72DD196E55F8C31A6CB8079559997E085BDB39C49536')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (386, N'Errores', N'ERR_PERFIL_SELECCIONAR_PERMISO', N'5211082E66E89692D955E1AD52F6B60C178235C409CF1B0AB0E7C99B184945F3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (387, N'Errores', N'ERR_PERFIL_SELECCIONAR_PRINCIPAL', N'0E99A7E6EAA868B700113B402FBCC698F3AC72DF412103F7C24B467CD0FD1FF76')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (388, N'Errores', N'ERR_PERFIL_SELECCIONAR_PRINCIPAL_COMPONENTE', N'3B01A915046AC5C31BE3B3A1DD71654E9495B137414F6D14072B64990593F01D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (389, N'Errores', N'ERR_PERFIL_SELECCIONAR_PRINCIPAL_ELIMINAR', N'0E186C21D546B3F35591AE913B33E90E65C97BB3B376DAD854FA274B13F675FEE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (390, N'Gestion-Familias.aspx', N'btnAgregarFamilia', N'21A1EBE56E405490B960AC110F1B9521B0EBDDED6B071BD7AC9EB73DB1ACE337')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (391, N'Gestion-Familias.aspx', N'btnAsignarFamilia', N'2CEA6FB4BE1B1B107B516CFB32C85E7DD7DCA0981FC5BCF431D250578BE5F750')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (392, N'Gestion-Familias.aspx', N'btnAsignarPermiso', N'4FB99297E5EDD4B83A4CFFABEC0DDD5BE07B72285E8A26AC68FB57DC3BA2637C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (393, N'Gestion-Familias.aspx', N'btnEliminarComponente', N'44EC531274E7E544C5088446F3019DD90B060F211390AC096D85392C10BEBF9F')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (394, N'Gestion-Familias.aspx', N'btnEliminarFamilia', N'0831DE91B5074B2C178716C7DBA8B040AD710CC2CCDB2CBD71AFE2B1D46DC0449')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (395, N'Gestion-Familias.aspx', N'lblFamiliaAgregar', N'0C84E674B6EF304F3E93817BDFEBCC2B9F9F76486210B671D8B9CE03417AD3D95')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (396, N'Gestion-Familias.aspx', N'lblFamiliaBase', N'21F624023598E70657BC0BF7176E9387E259A9D73F5207AB4CBFEF0C73D13670')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (397, N'Gestion-Familias.aspx', N'lblMenuFamilias', N'16211C40F082C5B04A121BE693E241CD82E04F04E45BDE4C3F7B77D363A42673')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (398, N'Gestion-Familias.aspx', N'lblMenuPerfiles', N'409C974230E572631AF7780A78630966DB7AE532D71F5E38B5853398D4943445')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (399, N'Gestion-Familias.aspx', N'lblMenuUsuarios', N'0842D308FDDFF8E94A909D6292CDBEBE91030791EA9967F835E001EC3F36C45FC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (400, N'Gestion-Familias.aspx', N'lblPermisos', N'08D684F454E5D44E6711CBE183B8F417A971663E2DF3153D7B151BA9321F6028')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (401, N'Gestion-Familias.aspx', N'lblTituloFamilias', N'0F7092B8D1D223E9D5BBC96F5B74740F1A3D4FC3D45B4AC8716E2E836DA3FDA11')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (402, N'Gestion-Familias.aspx', N'lblTituloMenu', N'0C0D82D4F2A9E592DD5ACF9894A15019E89859307130FC4D786CB9FD9B0A02238')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (403, N'Gestion-Familias.aspx', N'lblTituloPanel', N'61B1EFAF1FEAD7E06A22D7794265FAD556A8D0B32E9D9769F68E14818D540CCD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (404, N'Gestion-Familias.aspx', N'lnkMenuBackup', N'4F56D43669480C29E9EE8DE02267FD2D65C00FD78CCC9944415E329CF8E330DA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (405, N'Gestion-Familias.aspx', N'lnkMenuBitacora', N'0853565FF01771F65D8C99F008214D7AF3E8D369FB95501C643788F733F416AF1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (406, N'Gestion-Familias.aspx', N'lnkMenuIdiomas', N'08222992D969EC82DDADF2D88AAD08794AD4BCC491240A4A8BB7771A40CD13DBD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (407, N'Gestion-Familias.aspx', N'MSG_FAMILIA_ASIGNADA', N'0AA9319AD785CD2341D09D2B8F39958F8B2A5F11FA493543F73CF913B0877AA01')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (408, N'Gestion-Familias.aspx', N'MSG_FAMILIA_COMPONENTE_ELIMINADO', N'0A6057D38201861FBECF5E927D51D0474DE2592498941B40A45C470C9B777BD09')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (409, N'Gestion-Familias.aspx', N'MSG_FAMILIA_CREADA', N'504D576EDC2AF5207F7789F5EE34C6F252E67210FBDAAFFE438EBBA91D115EF5')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (410, N'Gestion-Familias.aspx', N'MSG_FAMILIA_ELIMINADA', N'09E2297410164F8874F40E49B174C536BFF10D16FCEF2A4D94941EC0DDF4E5BFE')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (411, N'Gestion-Familias.aspx', N'MSG_FAMILIA_PERMISO_ASIGNADO', N'0869F2633FF6E9C6EA198345532253101A4FE1A68BA315EE86EF2DE9A8190AE58')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (412, N'Gestion-Familias.aspx', N'txtNombreFamilia', N'0A3D6E849DDD0C801C76F5A6BD554CFB33CC47738201C520A344A2B232D5DF963')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (413, N'Gestion-Perfiles.aspx', N'btnAgregarPerfil', N'6C83AE6FFCE64F339A25A03ED890E12ECC4C0C530CBB2CA23DFAA2DE9E6F0034')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (414, N'Gestion-Perfiles.aspx', N'btnAsignarFamilia', N'0B3BE1E1BE44BB49C3D074F39C0EDFF362E318F2793A95FFF3A43E2C8FCF41F69')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (415, N'Gestion-Perfiles.aspx', N'btnAsignarPermiso', N'0AF7140AFBF7354533C1221583D4DBEC7C25AC8B490535BC61D53E7FF4804165C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (416, N'Gestion-Perfiles.aspx', N'btnEliminarComponente', N'0B66ACD81183E6B2AD7E78E6E7C502CC3EA39C3FB1668F23732FDA201BC223E26')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (417, N'Gestion-Perfiles.aspx', N'btnEliminarPerfil', N'0BEE61D7FD21F79D7644D566329E654085C53F81A6272FC2E8594959D52FFAE8C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (418, N'Gestion-Perfiles.aspx', N'lblFamilias', N'643AA14D1D39B4EB4FCB7A87CEA937480A259DBBD11A66D0FE986B8FA37C44A3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (419, N'Gestion-Perfiles.aspx', N'lblMenuFamilias', N'0F21081382AFC228EB60774E24872CDAA7949331B0CD56632D485382F7BB707A1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (420, N'Gestion-Perfiles.aspx', N'lblMenuPerfiles', N'4192C9EEB2D5312D4EF5760B4E469B1CDE29FE64DD842AEBCF3AB792F2B34E68')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (421, N'Gestion-Perfiles.aspx', N'lblMenuUsuarios', N'0F88D39BDF20C65C6308017C31C4B1F528A57C8EB08935C5D80CFF519FB560B26')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (422, N'Gestion-Perfiles.aspx', N'lblPerfiles', N'297D4AE7DA97D7C23E1A3071243EB8E475A597F3B072A87350D4177AFAC576D1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (423, N'Gestion-Perfiles.aspx', N'lblPermisos', N'0B67C5C65747A3609D834651F897AF746C422CE55CFC7E576FB771F9840E87AB8')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (424, N'Gestion-Perfiles.aspx', N'lblTituloMenu', N'2409BEDB7E4BE6DD09AADCDD63CEEC2C7EEA6144A91E674A629D7A740D77FECD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (425, N'Gestion-Perfiles.aspx', N'lblTituloPanel', N'0B7C2702032917C5FCB20A1092AEDD3E862086213A0A40925A1A02D666ADACC75')
GO
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (426, N'Gestion-Perfiles.aspx', N'lblTituloPerfiles', N'728AA5EE8CE61C536234565FFC023E7C3E514DCFF704EE6E0F1C007A80729181')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (427, N'Gestion-Perfiles.aspx', N'lnkMenuBackup', N'0C112F5D3FF010958A40D06A4F8B4A7C31D5E863B193FF9B0342BC0AAA67610C3')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (428, N'Gestion-Perfiles.aspx', N'lnkMenuBitacora', N'6DA1652B1D6E6E829D181827864E8676A2D075E9DDCB5D263B98BC4C1E0967B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (429, N'Gestion-Perfiles.aspx', N'lnkMenuIdiomas', N'1367F49B04BB9E837B02045FC221BE0C487C8B6B81163F78429CECEDA1B579F4')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (430, N'Gestion-Perfiles.aspx', N'MSG_PERFIL_COMPONENTE_ELIMINADO', N'0AB08B15214600945BBEF1B94AC5B792131BFBD83D3ECED1F104643B56AD3B929')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (431, N'Gestion-Perfiles.aspx', N'MSG_PERFIL_CREADO', N'08780637EB47F1FD6D9E4B72BA6A3309E107E102669A3F681A9FFA87E365AC7D2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (432, N'Gestion-Perfiles.aspx', N'MSG_PERFIL_ELIMINADO', N'3CFE9A5AA5ECB5DC5C5A25A6991DA2AE9C0C76F95C188431E9E196CAC8E63827')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (433, N'Gestion-Perfiles.aspx', N'MSG_PERFIL_FAMILIA_ASIGNADA', N'0960EDB25A6879D0D1F2F89C018F700A5DC4F4E9C174065FA06C15BDB8250405E')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (434, N'Gestion-Perfiles.aspx', N'MSG_PERFIL_PERMISO_ASIGNADO', N'1E77360E68D20775697142662B7D485A509CF0EE97CD97C07497E72809F97C34')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (435, N'Gestion-Perfiles.aspx', N'txtNombrePerfil', N'0C40DD22601C726CD8317E4937A7DEBB7FDC8883567E2717E775811FB9A9704E9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (486, N'Errores', N'ERR_FAMILIA_AGREGAR_NO_EXISTE', N'08E44B4E27C5900CE19E2C89938FA2344726DA91196863CD91136B3780B5DFB94')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (487, N'Errores', N'ERR_FAMILIA_AUTOASIGNAR', N'09E6DEB8B90A458C424E9BADFB4580C1EE3AC7D83F908EE0AF8243FD776750A37')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (488, N'Errores', N'ERR_FAMILIA_BASE_NO_EXISTE', N'090E2E9D8266C8F6A108DA4E1D40F2ED9BE365A478D963E26C24A5A54270DCF24')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (489, N'Errores', N'ERR_FAMILIA_CICLO', N'29EDDF5C10EDAA0CC7B1D6028D0BA385335CB37EB2B9CE2E26BB8DB5C1C1EDDD')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (490, N'Errores', N'ERR_FAMILIA_COMPONENTE_INVALIDO', N'7EB1E214888929E18DD961C52094998C9F61ED5A82DA17A690843673F6384CB1')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (491, N'Errores', N'ERR_FAMILIA_INVALIDA', N'6253BECADCCA3063025335A2798A3337DB2946CB0B9AD6B2B8D1241E0117B268')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (492, N'Errores', N'ERR_FAMILIA_NO_EXISTE', N'48B10A4A7C5654F942952B42A8538AC8C071A9E46858BA6A921E426D59C4E5EA')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (493, N'Errores', N'ERR_FAMILIA_NOMBRE_DUPLICADO', N'0BD686E168224CB7DFB51203A800E5CC4CD51C03B3070872260A3A3E9F1E11041')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (494, N'Errores', N'ERR_FAMILIA_NOMBRE_OBLIGATORIO', N'685ED9A9E7847146EFB707C7D7EE6DC10C9E8DB27D5A4CBFD49D2A8C5F3F7CB9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (495, N'Errores', N'ERR_FAMILIA_PERMISO_EN_ANCESTRO', N'0BCA72624596454F63D59333B47E13151C766430EF4D81EFB8D6C07D6320D1F9C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (496, N'Errores', N'ERR_FAMILIA_PERMISO_REPETIDO', N'3932BF8AFEB86634C217526E224FC4BC66508698414DBEFA826284D06F8FC2D9')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (497, N'Errores', N'ERR_FAMILIA_PERMISO_REPETIDO_ENTRE', N'3A9BDB394185A8F7136D29B6A7E3748ECD8653BAADCD31F8DC3395A161C38A8C')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (498, N'Errores', N'ERR_FAMILIA_RELACION_INVALIDA', N'196C87F0FB2B9AE80A1257C0C4856B835E1079ADA2750FD3A778722B40344D18')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (499, N'Errores', N'ERR_FAMILIA_SELECCIONAR_AMBAS', N'7C0C5ED44DB2FB9223FA922730D4956606AB34F7D60125A948A8C32E6045D436')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (500, N'Errores', N'ERR_FAMILIA_YA_PERTENECE', N'19B795AA8F1ED27AD7C60B2DED42E3A485BA68FCCCDA1F98F2B3634CCEE53E43')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (501, N'Errores', N'ERR_PERFIL_COMPONENTE_INVALIDO', N'0BD622B98D5EC39F8669BDB238F18524140F6898DBEFF9C15BC5672513C7D1510')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (502, N'Errores', N'ERR_PERFIL_ELIMINAR_ASIGNADO', N'17DE02F12042CAE9099D1DFD14F49F6BA3A096D799AAE04DB6F4F25138034D54')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (503, N'Errores', N'ERR_PERFIL_FAMILIA_ASIGNADA', N'0E09BF31151E0C3C90B91E2B4CA706F241A67CB54B8AB3E721058519EDD067442')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (504, N'Errores', N'ERR_PERFIL_FAMILIA_NO_EXISTE', N'446DAF6555FAC060E90CB8120A74F8984E2508EFE476F087C897E4D56399ED5D')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (505, N'Errores', N'ERR_PERFIL_FAMILIA_PERMISO_REPETIDO', N'242CBCB742EB5E76CAC13558413C084C501CC85AF787B99B11278B35FFE3E760')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (506, N'Errores', N'ERR_PERFIL_INVALIDO', N'2274D1EB9B08E4D44CA8F3F91CF358BB5EFD73FEE62255E9F064D48BC93ABAB7')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (507, N'Errores', N'ERR_PERFIL_NO_EXISTE', N'0EC9980B81ED3EBB71DCA934DE88810310F22363B54F35840C0E5970354BC37AC')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (508, N'Errores', N'ERR_PERFIL_NOMBRE_DUPLICADO', N'3742DA835D5521C0DB7E3B7ABE1C131E342E289062E59A232DBD040B4ADFFE9B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (509, N'Errores', N'ERR_PERFIL_NOMBRE_OBLIGATORIO', N'0CFDE04CDD1F09B634AE5480A96CC288C576D335F9A39779807A1A277AB7D656B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (510, N'Errores', N'ERR_PERFIL_PERMISO_REPETIDO', N'2F27860691F5F37355379F2E0CB545D85B4250D3E7B46C43A03F8ADF33838EE2')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (514, N'Backup-Restore.aspx', N'lnkMenuIdiomas', N'6DB8058AF5725687ED9D5ED7B0E640150A590CA515AF10396B1C57A0064C9935')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (515, N'Gestion-Idioma.aspx', N'lblMenuFamilias', N'7E8A0EB8DB9ADA920F8F48A938279353C866D05E0E88828894E0908DDF80031B')
INSERT [dbo].[Etiqueta] ([IdEtiqueta], [Formulario], [ControlId], [DV_Horizontal]) VALUES (516, N'Gestion-Idioma.aspx', N'lblMenuPerfiles', N'0BBA2C440001E9018BFD0C49D3F33B5B3156585FC0E113B3A0A591324D30E5393')
SET IDENTITY_INSERT [dbo].[Etiqueta] OFF
GO
SET IDENTITY_INSERT [dbo].[Familia] ON 

INSERT [dbo].[Familia] ([id_familia], [nombre], [descripcion]) VALUES (1, N'Admin', NULL)
INSERT [dbo].[Familia] ([id_familia], [nombre], [descripcion]) VALUES (2, N'Usuario', NULL)
INSERT [dbo].[Familia] ([id_familia], [nombre], [descripcion]) VALUES (3, N'Web Master', NULL)
INSERT [dbo].[Familia] ([id_familia], [nombre], [descripcion]) VALUES (4, N'Cliente', NULL)
SET IDENTITY_INSERT [dbo].[Familia] OFF
GO
INSERT [dbo].[FamiliaFamilia] ([id_familia_padre], [id_familia_hija]) VALUES (1, 2)
INSERT [dbo].[FamiliaFamilia] ([id_familia_padre], [id_familia_hija]) VALUES (3, 2)
INSERT [dbo].[FamiliaFamilia] ([id_familia_padre], [id_familia_hija]) VALUES (4, 2)
GO
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (1, 1)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (1, 2)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (1, 3)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (1, 5)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (1, 9)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (2, 7)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (3, 4)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (3, 8)
INSERT [dbo].[FamiliaPermiso] ([id_familia], [id_permiso]) VALUES (4, 6)
GO
SET IDENTITY_INSERT [dbo].[Idioma] ON 

INSERT [dbo].[Idioma] ([IdIdioma], [Codigo], [Nombre], [DV_Horizontal]) VALUES (1, N'es-AR', N'Español', N'0B9CB46F5F750616BABC3F12131597476FBE0BF14DEAFFF751BC644ED8067BCE6')
INSERT [dbo].[Idioma] ([IdIdioma], [Codigo], [Nombre], [DV_Horizontal]) VALUES (2, N'en-US', N'English', N'702F2AD8F4BB4F1324A5DE7E7A5EC0F250A3275C4FAEA53E1DE077E683A6F278')
SET IDENTITY_INSERT [dbo].[Idioma] OFF
GO
SET IDENTITY_INSERT [dbo].[Mascota] ON 

INSERT [dbo].[Mascota] ([id_mascota], [dni], [nombre], [especie], [raza], [fechaNacimiento], [DV_Horizontal]) VALUES (5, N'46208842', N'Tobias', N'Perro', N'Scottish Terrier', CAST(N'2015-03-14' AS Date), N'0911521C43F2A2F16C09F41C6F9ED3D8811BF63A0B7C731C1C8A1D789B45EA937')
SET IDENTITY_INSERT [dbo].[Mascota] OFF
GO
SET IDENTITY_INSERT [dbo].[Permiso] ON 

INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (1, N'UsuarioGestionar', N'Gestionar usuarios')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (2, N'FamiliaGestionar', N'Gestionar familias')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (3, N'PerfilGestionar', N'Gestionar perfiles (roles)')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (4, N'BackUpRestore', N'Realizar backup y restore')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (5, N'BitacoraVer', N'Ver la bitacora de eventos')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (6, N'MascotaRegistrar', N'Registrar mascotas')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (7, N'ContrasenaCambiar', N'Cambiar contrasena')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (8, N'DV', N'Gestionar el digito verificador')
INSERT [dbo].[Permiso] ([id_permiso], [nombre], [descripcion]) VALUES (9, N'IdiomaGestionar', N'Gestionar etiquetas y traducciones')
SET IDENTITY_INSERT [dbo].[Permiso] OFF
GO
SET IDENTITY_INSERT [dbo].[Rol] ON 

INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion]) VALUES (1, N'Admin', N'Acceso total al sistema')
INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion]) VALUES (2, N'Cliente', N'Usuario cliente registrado')
INSERT [dbo].[Rol] ([id_rol], [nombre], [descripcion]) VALUES (3, N'Web Master', N'Administracion tecnica')
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO
INSERT [dbo].[RolFamilia] ([id_rol], [id_familia]) VALUES (1, 1)
INSERT [dbo].[RolFamilia] ([id_rol], [id_familia]) VALUES (2, 4)
INSERT [dbo].[RolFamilia] ([id_rol], [id_familia]) VALUES (3, 3)
GO
INSERT [dbo].[RolPermiso] ([id_rol], [id_permiso]) VALUES (3, 9)
GO
SET IDENTITY_INSERT [dbo].[Traduccion] ON 

INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1, 1, 7, N'Iniciar sesión', N'098CA927DB18503F668174CCFA6D0C4DA0AD2DE5B7B811AE819E4A99685269120')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2, 1, 6, N'Recordar cuenta', N'6B23D82E02A0F2B598A2D041BE8DAAB8FCD588A7E558E5DA79DEB669E8BD3C78')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3, 1, 1, N'¡Bienvenido!', N'09493735794A8E32243EE188AD640C55CDB558CBDB59C25FD07E547117264B73C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (4, 1, 4, N'Contraseña', N'0F0087AAC3CB2F456651D359565781163602838953D329DFECB1FE20B8A2CF67')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (5, 1, 5, N'El campo de contraseña es obligatorio.', N'5DB376F65D3EFF9F8B8066F48077C153144586D8C397896650DA2CFF6FA5B328')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (6, 1, 3, N'El campo de nombre de usuario es obligatorio.', N'0AB1B8F3B60FC1E1CEF4C4755CC9E70F98DD72373873B57BE96DF15D3420E953B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (7, 1, 2, N'Usuario', N'08F512E9F635C721A4E723905B3EC9DF490469DF531BD9E015FB5FFEBC350D668')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (8, 2, 7, N'Sign in', N'56CD11F16419BD900491D6BC7427D38F08792852829CF3EAA04906F6BB533E59')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (9, 2, 6, N'Remember me', N'0E093CADCAD93AE7F905301520F34242C51093D3122F4E8591C6C46D1A109B6FC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (10, 2, 1, N'Welcome!', N'1F028966E2DE08530B812E25B98AD85D8D4C75D1EB4F169E102DF6088D47D5F8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (11, 2, 4, N'Password', N'6DF203D69288D55D44C56B4E35C9FD6D35595D6CAD64195358E8B26FA247299F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (12, 2, 5, N'Password field is required.', N'1EC56508BBC4C5904591E084AD8768DD006A518F902A6FE7254229AE8DFB77E3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (13, 2, 3, N'Username field is required.', N'09F9C6BBCDC177DACA0B4A22C706A6A875509589244B09E387AE54A81476E6264')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (14, 2, 2, N'Username', N'0D97695FAECCA7C02D811732C28C06C7E0C1DB1729A6ABF5D7605D3EFBF1564B3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (15, 1, 9, N'Credenciales incorrectas.', N'0F6274D10483A6DF5E297F7E0805D7C5A25839E5BA007E3164F8DA7C4FF999098')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (16, 1, 11, N'Ocurrió un error inesperado en el sistema.', N'086BEDED3E294BD93A5C208A05811D8484C067A6A89A085FFB9C7DF1A0FA0BFCE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (17, 1, 8, N'Ya ha iniciado sesión en el sistema.', N'126EBFC7DF002A8537BC312307F44C50FF2D93473B6293FB134E24995F174208')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (18, 1, 10, N'El usuario se encuentra bloqueado.', N'0B69CE3253FD8050BA2451ECD073E3F7A233DE725EC117693D2440AAD032852DB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (19, 2, 9, N'Invalid credentials.', N'42E2C58D8A71046071FE4ABDCEB97716D1273A14364DF90F503B16010640D1B7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (20, 2, 11, N'An unexpected error occurred.', N'0838C0932AB25DA3869965AC1E4C355A0189CC332E1C9737507BC9A6EF717951E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (21, 2, 8, N'User is already logged in.', N'2F0FB434CA961C9A55FE690497B5CF8562F2AC648D21B39C8ED66690D7339E6F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (22, 2, 10, N'User account is locked.', N'69C8BE045852D2F2005B5FE0CCF4F90D286A7A69412F77625D2DEB4119C0619B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (23, 1, 14, N'Administrador', N'70F0A8E7174FE7700F5A40951494E77664B53539B1A868481C79F0AC6EEEA7B3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (24, 1, 17, N'Cambiar Contraseña', N'0E7C35BF6E4A6AFEBE8F78B8A8941F159AB106FEF35F25F134B99DAA0C97AECCF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (25, 1, 12, N'Iniciar sesión', N'25DB12E613E561D7C899D90FEABC3C6427E5EE8D2F91DA339DD724E2C4056E2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (26, 1, 15, N'Mis mascotas', N'3E44A7C5A2B9271FC1E96774062F547116D1DBAE0411F31D8A25442F5A65722E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (27, 1, 13, N'Registrarse', N'7E4CA5CA15B468743276F1CDFB152846DF70A7FE94AEBBAE4607F903E684EE5A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (28, 1, 16, N'Web Master', N'090CA405F67B7AF7025D3F4C229CBA82E3BA899F4042B0879C42E29DDB2F386CD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (29, 1, 18, N'Cerrar Sesión', N'7FAE1AC027988358537C5903FDED5E857C2DA38272BB615D4D571D1E31165479')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (30, 2, 14, N'Administrator', N'513F5D367CBAAF9104720118D2C8E9C15AB50CE112CC8924DA8B680207DD7ABB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (31, 2, 17, N'Change Password', N'126526C4A3DCD2366B0115E014182663B30A8864E6D9A312DEC52A2A844DEB79')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (32, 2, 12, N'Sign In', N'4E6C0FA32C912F468057EF29FAF1966B3D9FA2E051FC4FF7524B919CD0F3D5B8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (33, 2, 15, N'My Pets', N'1AE2AB0C234DACF03194B27A068938AC062B66BB49CB80B614EAC2B46F342CA6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (34, 2, 13, N'Sign Up', N'11894915B1BC5E451E41647878007894D3D6195393B74B3FEA31AEE3F375EA22')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (35, 2, 16, N'Web Master', N'4F887A44E22F8EBF9AA45372618B3BCDE4F7764749FDCBABEA72723889693D71')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (36, 2, 18, N'Log Out', N'67658FD6CE32A133FF1F704B28F27D1E20230647E44DC2E98FEDBBFD70AE3B30')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (55, 1, 58, N'Bajar backup', N'0D79DDBA134180F2E4AC8F6F47C622373E40FF586CBE1712743370DF523EDB652')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (56, 1, 62, N'Restaurar', N'6162788F0B856F6D1351C0F731FE1E86536C00D1A058937A7B9A888EA22A1A45')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (57, 1, 57, N'Pulse el botón para generar un backup de la base de datos y descargarlo en su computadora', N'0FFCA2F9C36ECB28834B1209256734A05F58CCE5B916E5FA1D7C9616BB8CD46E5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (58, 1, 60, N'Suba un archivo ".bak" y pulse el botón para restaurar la base de datos a partir de ese backup.', N'0DBB1974887F203D46ECA49B30251F88A96117702F43A807ED7F7424EF816B738')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (59, 1, 55, N'Backup/Restore', N'14A413C37E5A065F98255EBF8B88AFAF244470288515C944EB741696D7E82006')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (60, 1, 54, N'Gestión Familias', N'089D34C0938566F09FC65449DAA5F8CB7EAB3A93AA416AA24A05E59938356A25E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (61, 1, 53, N'Gestión Perfiles', N'0BA1FEF4CA8AD88B0BF30C47285601444141A8D935C05857E8584FF4C835B52F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (62, 1, 61, N'Subir .BAK', N'08911CA19BE129A4C5FCEC7B2D7A83EA2F9BFA03BD71CAF6B0D9895016302A1D0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (63, 1, 56, N'Backup', N'7CE7C2D3C2FCB809FD71FB97092D1D2A57FBC6D089B791981BAAB935D04FCD50')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (64, 1, 50, N'Menú', N'743BC828886F6C297E50B17057C2BF2CF933062CDFD1EE9B0ED4B8C359E7593')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (65, 1, 49, N'Panel de Administrador', N'69A5A3C829603F97F6DA70385554481FEC738B3B267B9EC4AEDE93085444DDBD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (66, 1, 59, N'Restore', N'157F68C6FEAB0B936FE59226E347858EA306F9BBEEF8BAE26A9920FB41465EA3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (67, 1, 52, N'Bitácora eventos', N'68A5B87DF0A224709C89C01C394C4E812DBBB097AAA0C702E2AB2094835820')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (68, 1, 51, N'Gestión Usuarios', N'7359C3CB31776A2A3B6AF9B4A27DE44F022FE452A4651A5DD4B319C42E7EB54')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (69, 1, 86, N'Filtrar', N'08FC73CB82B283551AF3405456BFA94B6FCF947261149239B88DEF2B092AA81E0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (70, 1, 87, N'Limpiar', N'0F1ED83F9AE60D2B13B0A4C7B9844B21B236898778AF5BCDEB8F19C30ADC5E19')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (71, 1, 88, N'Filtrar con fecha', N'099D6214989C0E812C2BE51C5BDAD2CB3801D0795EFFD72827CC50C1B7124F209')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (72, 1, 105, N'1 (Crítica)', N'0EE370DAACA6A218BCD2C5A84884CF1EC2720D5A3E3B557A22184AECDE4CD88B8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (73, 1, 106, N'2 (Importante)', N'6AB4361AFF1327DB2081E4A3F4B8A15B4FD182DFA49973A54137CAC31AE8654A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (74, 1, 107, N'3 (Media)', N'6565F017AF58300AAD201EC660A83B4BF507461B97BCEFA2B0DA5F2A25D1A809')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (75, 1, 108, N'4 (Baja)', N'09196680C9D635076A0D38AFCBB1CF99B574F479795EB3507533B36F9E74D0A4B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (76, 1, 109, N'5 (Mínima)', N'0E17D24A41EF5FF9B4E87F35313A6045DB03AF80BBEC2A6A09C6A036A3FABB25A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (77, 1, 104, N'Criticidad', N'0A669D5A29814877C0871D9F88D30D4EFF0CCA440C3BEF011532AD8E0DB0ED5F5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (78, 1, 98, N'Cambiar Contraseña', N'0CD0B5A700E0717CF9530BFAF1D2DA952E04073D37DD364030F9DBC224F0094C4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (79, 1, 97, N'Cerrar Sesión', N'0EC5D1ED2E5F67807B2133FA85778D9D4632309039C3D78E729641500D7C45167')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (80, 1, 99, N'Desbloquear Usuario', N'0C92636E132045B656729106C9AACB0452A34BB00D01C7CF313C28018DCE73F4D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (81, 1, 94, N'Evento', N'0AE0D836026AF58E909926758F23BD1F83A1C582675824C430C6B7DD3A6A1E7FF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (82, 1, 101, N'Hacer Backup', N'0902015432DA0C835946E1D83EF6033CB6C3A82CF8BBF6440E519C12038EECFEF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (83, 1, 102, N'Hacer Restore', N'4DE2BFCC58F4079F9E68DECCE8C936A73EA47C590901A83F016A2BE2CA9C6279')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (84, 1, 95, N'Iniciar Sesión', N'58B4B7EDC113F4A1537427CA82EDC7846E091B431EC58AF9659789F35BA34198')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (85, 1, 100, N'Modificar Usuario', N'487720AD9AD40BB910377C43C0C0AF74A73422F7CD93A2A6BD97A011C0802A28')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (86, 1, 103, N'Registrar Mascota', N'0DF51E4DDCA08A4A68D07D713C8043997A93806C54F57C669EBF478C043E44A90')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (87, 1, 96, N'Registrar Usuario', N'71F58C52F8C7C55D529C56DFDD05C4289EE6503841CB1AED07A81CF85B936AEE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (88, 1, 84, N'Criticidad', N'5BD4FC27BDBFED940838494D91FE77C63C1B6896F866D243AC6E66F8AABFD669')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (89, 1, 83, N'Evento', N'4C960EBA7C0D23D4DE26C703B571E12A84FC10DF24E1EA56605337AA001306FA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (90, 1, 81, N'Fecha y Hora', N'402E0DBA099AC29A9E9AA79D689553978693F307A2DBC0BB548CC2D92F7F138')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (91, 1, 82, N'Módulo', N'08773B15B9A8B90D10EA0E6FB92ED80C7E8073302CC07FE30215A215430745D0C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (92, 1, 80, N'Usuario', N'7ECB96C06A1258EF0B93AF4DDAC96F57CB0EF78F1ED1791FDB23FAE6F477978B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (93, 1, 74, N'Bitácora eventos', N'78A0A722F7C3AA25404BF884E25234CD91398F5446B99CF00F2FA2EB3DBF10DB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (94, 1, 76, N'Gestión Familias', N'0E40391A63E4BAAE064A3A0F44CFA980027967BC7D686227DDCE518311F48E07A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (95, 1, 75, N'Gestión Perfiles', N'7E9BD50442421C02AACA6A7E7350765B60F541FAF7CE8CBF366280C123716436')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (96, 1, 79, N'Bitácora de Eventos', N'183264D62F23FD4A0925C665A4CF83C3A46B3CB74D2D45453E22B0A1BB96C416')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (97, 1, 72, N'Menú', N'7FED06B9A1E8339616B13BCADBD815A054A0AAF6636D3600F2E8951CA3CDE77A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (98, 1, 71, N'Panel de Administrador', N'0F4AE8DC86CAA9BF65A63CC670A28E3D86688F5DEA7B1521C1361D8617D48EEB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (99, 1, 77, N'Backup/Restore', N'13B5E52DB8CF9BC2D44FD5E4C004CB5CE31644AB462FBA20BA25F6C5424D089A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (100, 1, 78, N'Gestión Idiomas', N'756B2528748ECEB6319E347C6EF11611C4480C5E0192A4585E41D6E25D4DD53')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (101, 1, 73, N'Gestión Usuarios', N'09F05A6079E8CE2A3855E82D4CBA325F651C2557B3B050CFBD4ABFF251F2D2C22')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (102, 1, 91, N'Administrador', N'084DEE8E368BD49B04F65E41D668EE1202F143A4BCD7ACC606F3142F7CFD39260')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (103, 1, 92, N'Clientes', N'0C3DBC2F24807CD1F3ACF1591FBCDAB0E40A6128DB9408FB88DFCF835B839A8C8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (104, 1, 93, N'En desarollo ...', N'08781FE71FE8326C621AB755697BBCEA8263EFB18D6E3E51CD4DF6AE0E5EE69A9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (105, 1, 89, N'Módulo', N'0B1D9882DE5A4D2C0B5A30A7E0356866157661B55FFF339B691D4A34684014511')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (106, 1, 90, N'Usuarios', N'0B5425942BBF545DD8E3410E29CE3CC2984E497E3E6497C45D4FD6D6F797966F0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (107, 1, 85, N'Nombre usuario', N'0DA493223665B45AFE805DE627BBB898BABDD094AB9EF36E96C2758FABE1D957')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (108, 1, 48, N'Cambiar contraseña', N'27DD1DE95264C88C596FA1822E0F1840A629AC21457DC503ED700336F1100634')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (109, 1, 47, N'Cancelar', N'1A413B66F79441993CB203F421BEAC0F896F3E3D7210EF109A0993EA6016DAD2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (110, 1, 43, N'Complete los datos:', N'73DC6BE3D406B78E7826AF272B690E26A2D188136B109B33DCC11D77269684D3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (111, 1, 42, N'Cambiar contraseña', N'35562541012F016EC7A3FE7CABDD2EEF17C79EA6F5E900298861B76CE0212F76')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (112, 1, 46, N'Confirmar nueva contraseña', N'25FFAE333FC03632D3C69751374A56B03A0741010F0917B759327C379C10C311')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (113, 1, 44, N'Contraseña actual', N'0EF39B933B7FA43656B8908018E229E61A654372B3F086D1953F9AD7E9EC879F9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (114, 1, 45, N'Nueva contraseña', N'0D3CA08DF46B92C57D1F263BA2C6D9CD27E0AEF09BE23162CDF88C064DC943299')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (115, 1, 19, N'¡Bienvenido!', N'1FFFE57502AC65337D6C80FBFE1832C76263B45892516F6F991C03215838A854')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (116, 1, 65, N'Recalcular', N'08163517C6A96C2E71DBEDFBCFA10273D6FECB9A42813BFA9F31844BFD10A568E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (117, 1, 66, N'Restaurar BD', N'5AADB6ACDFD038B051CE176B96D64D247EC3A7B3F14C1A1730E5B029EF3BD9A9')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (118, 1, 64, N'Salir', N'09D7A3CFA67A8D9FECDEAEB3F595C6768D05B92FBCF49F76D2B2A53D9F85BE309')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (119, 1, 69, N'ID Registro', N'0F1C3F914CDA6092271F22F5F1E6FE1B94281EAC7E320FD3B4701E1AE14098F50')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (120, 1, 68, N'Tabla Afectada', N'2BD3B42C464A0A835DFE65B6753AB6F187B2B3D8141592C7DA07FB7709F2EDF7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (121, 1, 70, N'Diagnóstico de la Inconsistencia', N'0E8FBBF7981554241E97F3A08ECAA522F99C9DE2641EE0BC304E61626E0DB7FE5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (122, 1, 67, N'Subir .BAK', N'09ABF41F9366C88B9E5DE79471A2C3EAD065F8C96F43970C8082C6275A94EB1B4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (123, 1, 63, N'¡Inconsistencia de Datos!', N'68E325751F39CCD92F3F4031C25F29C9B779CF11BC5BD0CD7699771BE4D83CC7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (124, 1, 165, N'Crear idioma', N'35B596544055578B361A52D75233638D07D2A3355942EBBD192EB1C1E300366F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (125, 1, 168, N'Eliminar', N'64BFBBDCFABC0B9229A7756A7ABEF292A26A9AE44E3019D22E1F750D03ECA7DD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (126, 1, 167, N'Guardar', N'2495BB57774299F094325AD6BD1C9865E2B36550037BEBEFA0FDCAE03AB5BB47')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (127, 1, 158, N'Acciones', N'090C4CF9F528D26E830E75F7808AD385920BF285A2C59456E60D67442F3CB4CED')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (128, 1, 156, N'Código', N'2D87E1D88535C0F49D9979BA6B2914B0EADC9CABF1D4CC035B02F4F052275E7F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (129, 1, 157, N'Nombre', N'0E4C0814BC374185E53D1E12731DD1CD340FFC0854E60EF590546F98F674711E4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (130, 1, 159, N'No hay idiomas registrados.', N'2A92391FC0549111F078C39AEF4EE0C88ECE01B1E7F3CB0BBFBC6867B694D7CE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (131, 1, 154, N'Acciones', N'0900FA36A5265CFA555B511003A7B45EA0C3752AA710A14F713CFFB59518D0F2A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (132, 1, 152, N'Control', N'1EE8BCE5DACE497EB352FD25F8437976BB8643E32A4421D2068B5B0F67503E65')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (133, 1, 153, N'Texto traducido', N'3A846A6AE68C24D4DA4F766C53BA3303D0BEEAF46C9CDB45A46F242B16B908E5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (134, 1, 155, N'No hay etiquetas para mostrar.', N'5180A01B2C3523EDE2BE3690549A40BCAD71555E4AF52CD1E2DE6A42346A851D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (135, 1, 162, N'Código:', N'1BB93FC1270DEAB3218D6A4805F134EA992DAFF449384FBA8A84A106B17D6F7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (136, 1, 161, N'El idioma se crea copiando los textos en español por defecto (es-AR), que luego se editan de forma independiente.', N'0F8F651FA70A1FDEB29D60CEC347C30249735FBC67E787E2051F1310CDEE4CB25')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (137, 1, 151, N'Formulario:', N'14C76584D8F14430434F567F6141800A2B8C91A4FBED7AB9E317E4A2822D850C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (138, 1, 150, N'Idioma a editar:', N'0FBCDC86542B111CAD3E80B0C2C12C92AE602859554075C802DB95593C79993FF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (139, 1, 148, N'Gestión Idiomas', N'0A919A0B5D654DD9310B14575AD375692ED88CA051FB3066CA180374A4C7D15B8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (140, 1, 163, N'Nombre:', N'0F7D7179FD21466DD5449949A302DC82379018587C18D15BEC9D0F54207EA6498')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (141, 1, 149, N'Traducciones del sistema', N'0B52B9D3EA7A07399E79715F8A1C5F4FCB0F4675EAEBB232FDC703AA56B9A7E5C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (142, 1, 143, N'Gestión de Idiomas', N'0FF0E927A278DB6DD6A035E5780EB4B05CD1948EE45D4E2EA52EBC06DC55F164')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (143, 1, 166, N'Idiomas registrados', N'0A9DA26C52B09DEA8FCBE5937CD295B3BF4FB6681B4C1518AD12DD3FF63D585A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (144, 1, 144, N'Menú', N'6497C8DE1D4D03F5AFBDFC09785DE07E86B92FD1C69DDF9E17C29EA80F122B98')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (145, 1, 160, N'Nuevo idioma', N'0856A33A99FA34E948352AACBBB277583F8DF6CE6DA6D140944814D8817CEC49A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (146, 1, 147, N'Backup/Restore', N'52F2605F759023DA71B57EF1BD392B31F32390E8392BC765BF43978E0836CF21')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (147, 1, 146, N'Bitácora eventos', N'0B68E070BA06B6A1033B881C9072CBE98A5D70728BD9D001C8CD7B8F3CB458866')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (148, 1, 145, N'Gestión Usuarios', N'43F58D92D0A4A247E46CCC8C27BF91A47E080BF1328E36DD9A40E04B503F93CA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (149, 1, 164, N'Portugués', N'0C85D78507BF5F6154BA7C06BBB1EEFF39A97D4298A331ECA6BF93D0A32FBA637')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (150, 1, 141, N'Aplicar', N'6B33701DCFD514EFD26961626F14F95806EE3453CAA5680DCDD539377068199B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (151, 1, 142, N'Cancelar', N'6817F7958CEEC4209A33C5F8D9B900D2A1544BC3E1FDD1B3D0878FE4288D6235')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (152, 1, 138, N'Crear', N'489547F57EF134CC3CD939BBC6B4D200DD3D6835963B08E27DBECD0C6C07420E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (153, 1, 139, N'Desbloquear', N'5B0D623CE8FD5710B8491900962BB5D8029A31BEB18368BCEAA36F35BB635940')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (154, 1, 140, N'Modificar', N'2273912987D73F4967D62A06C6A04DB26289FAA446FA3A0972A95256D8AC52DC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (155, 1, 129, N'Ver datos encriptados', N'0BFE9DB8FE09F2F8717755010BF4DB653926A4DCCDE76157638749FA0A90FE6B2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (156, 1, 137, N'-- Seleccionar Rol --', N'11A5C8E641E0E86937427D3F6C364604EBFA3190C71E834B031A3D8C0DC02F7D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (157, 1, 121, N'Apellido', N'0852BA5401A0FDA7A7C57D06F8AF0F7C0F95FB4FE92266384A8BA0EFE9DB9DBFA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (158, 1, 119, N'DNI', N'708546C52CC9A2100E942F59F5F031B3F3DA1C26200D91BB2E9CDCC4282CA330')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (159, 1, 122, N'Email', N'1DBBD17006593550289EB9A79FAAC4EA120DE3CD5CEB43A0D6EBEDABD35F571B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (160, 1, 120, N'Nombre', N'63686173308333F1E3046BB9A1EADD41E42CF63C23E9228DC3F3389FB7D333DA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (161, 1, 124, N'Rol', N'7CC2CC83D2BC72CD4AB2533CB5E5D48ACD4964ADB9E087DC67B7C36AAC0E58E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (162, 1, 123, N'Usuario', N'2DA800EA09B6D852C9987DB72FADDFC43B33AC0735A7B44B9EA329A83AE80CBC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (163, 1, 125, N'Seleccionar', N'4DDC7B83D05EF7357EF73B444E66D442657C3586644BAC7306E4E1741B9CD3AF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (164, 1, 126, N'No hay usuarios para mostrar.', N'7B79E8EA7A4331EED288B0B9652428DAB783DE1A9650FA681F6532EB8A21D899')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (165, 1, 133, N'Apellido:', N'09F17E670E3D5B9AE0A01F93EB978FEC04EAA6725DCDCDA5DD08DD781F7CC1772')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (166, 1, 131, N'DNI:', N'7DF48F6595725777CDA3FF2A307B39994C382F539717A7DE9495629DE08B13D3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (167, 1, 134, N'Email:', N'0A8FDD26F9EAABEF05A7D169C3CE929920FD6E71F1F29BA1CAC141BB03B96A04B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (168, 1, 115, N'Gestión Familias', N'2CE331DE9115EB34AB7915F1F35F2E2D9CEE19444B390F5CC8C0881B327EE6C5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (169, 1, 114, N'Gestión Perfiles', N'0F468CFEF4914B536857AB6D837B6F0FADD86D82EE7A16D7A40637EEC5D0705CE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (170, 1, 112, N'Gestión Usuarios', N'39B3E5CCC6F0F0C9BAB47A046101DDBF970D314C8F57EEBB6DBFBA22AA3DBB9A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (171, 1, 130, N'Modo:', N'6EE8EE8A7046C2FE99FB5FBEF48AE0C0BBAD0CBD521502DF7C5698CCE09FB97F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (172, 1, 132, N'Nombre:', N'39D928A9B538838E731CB0220366506BA876F17B05D457D5C7E0582B497E0C25')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (173, 1, 135, N'Nombre usuario:', N'0E0561FAF4EAC9FCB3AB37E915035C9404AE9B71B2F2B51F05D7211A4C2DAEBE9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (174, 1, 136, N'Rol:', N'7C284657FFEA7F3D66C34CA7269526E5337C6A287B5421AE8B08531C91E680A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (175, 1, 118, N'Gestión de usuarios', N'0FE8284DD44BE2024F0C59F7EF959E3C0E5B7CE2AA33312812554B61EE4EBB188')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (176, 1, 111, N'Menú', N'0DA2C393713E0E4B9210557AFB2D95BC17F86AE2F280EC4BF63061A83EE6D3A29')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (177, 1, 110, N'Panel de Administrador', N'419AB59300EBF585B3E3D6B11D8A072BBF8A89CB18B8B2EB0DFD55E6DD63F9E6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (178, 1, 116, N'Backup/Restore', N'0E0ECA83C15F5F6BE8834A1EFBEA3718C882367137198E212EAE771CE0C685ACF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (179, 1, 113, N'Bitácora eventos', N'10C9816BBB2036341109E6137EAA5A23B9E5EA707EBAD988CA0DB7590C9587FD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (180, 1, 117, N'Gestión Idiomas', N'3596F45471153D8949D83A238CE68010002D666B1CA081820D265A1FB9E6C62D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (181, 1, 127, N'Bloqueados', N'0AC64F7823702B84B954E798B5D2C2C4598E0075E67703BF529B9384231324E98')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (182, 1, 128, N'Todos', N'0B3CD7B5A7F578DB2EC5ED1CA773A8ADDF013BBF054B5496186FFFDEEAF7EE5B7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (183, 1, 40, N'Cancelar', N'0EB732B1AD88C4571C44E627C8779EE9ED2B6C485A556C9A5118B23E29A08E74C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (184, 1, 41, N'Registrar', N'272DDBFFFEC097FE244F2320B2E504BF7189DAD8FDA9D690EF2FD90A044F4DF9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (185, 1, 34, N'Especie:', N'199E8242B9A3E3B89B0D3E7B0D2C0B50003DD28C34CEF58C10B52964FC39147B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (186, 1, 36, N'Fecha de nacimiento:', N'6235F98CF9AFB85BD0B1E387731E6B466E057DB1A2CC4CEE9596E3E9F61ACFCB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (187, 1, 33, N'Nombre:', N'603CDEA52752C1BCCF1CD174B089E96BB3500896D7354CCCF8ABBD837ED1C0CD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (188, 1, 35, N'Raza:', N'0C958CCAE2A690B67DF31028B467CF14ED496E96C14286F173E024FC386E7C112')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (189, 1, 32, N'DATOS DE LA MASCOTA', N'13CE1C28B6B44D936BB13E176893516DA35429BD4266BB73087F7C4B8EFB94C2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (190, 1, 31, N'Registrar Mascota', N'42B5D9B597658AB04511FFEC9BA69CE446EEF38859EBACB44376396AE9E6422E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (191, 1, 38, N'Especie', N'0E0B8FCAE48B75512354BF968E2F974EECF5B95B5EF81072B8A368A7B8B1BCE89')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (192, 1, 37, N'Nombre', N'6FB0FBC251E030048D73FED99AA7A0263CB87231301B6E22612B6210F42CA349')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (193, 1, 39, N'Raza', N'0E11094BD6892B87F45A82187E40EFA0BE13B337C53905E1552616F489E7F3C34')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (194, 1, 24, N'Apellido', N'0C8059BCF7B201880CAF72E6E7307D3CAF14BB42362D4AFB92D2CCD1D748B399C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (195, 1, 29, N'Cancelar', N'0ED725AA8C97B720864F5F7E4E68CE6152B02FE3423074AE7EE7402263CB5B49E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (196, 1, 30, N'Registrarse', N'316B37F145757E61623638AADC4C046D571CDDA6D6AD5AF5729EA19E6146380C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (197, 1, 28, N'Confirmar contraseña', N'12BB1F768ABF6B05D655705A5339FEDE05E884DC5B060372CAB21FB5131CFAF7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (198, 1, 22, N'DNI', N'09EDEC515AD5C400D6D351CE2C2EBCA1C196073E31130B7D3A3408C8065A808DF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (199, 1, 25, N'Email', N'0833E7378BFD9DAA15F4E12E428B5FE8D22A0C74A9DAFED76DE62798322FCF0CA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (200, 1, 21, N'INGRESE SUS DATOS', N'09A614C204B789CF7DB86D7399F3499ACCA7BDB3BB897477269375A111E1DD7E6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (201, 1, 20, N'Registrarse', N'0CAC510FBA97B0A173D38EA8B1A173A7642FB8BCBCF238CBD056096077F69BB09')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (202, 1, 23, N'Nombre', N'74100400C4A61AE97AD0A8DEB81B8C5199C0F578719A3B5A1C921B644377CD07')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (203, 1, 27, N'Contraseña', N'24DEAEC3BF6353562A31CFA51B78263040DE7205607BF27E910355B0705E65F7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (204, 1, 26, N'Nombre usuario', N'0AA787C64DFA13A690AF3039C8CAA1539EDD4A7A8B00CC1820C68A89FCDA20A7F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (205, 2, 19, N'Welcome!', N'78AFAEFF41B1237BAB7E79E0118A85E6EDCD30ADC1B01B5F3214F0D4A656374F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (206, 2, 20, N'Sign Up', N'4710534AE99081EB459D812F5C497C8BC8B36F45402725A6855A4A08DD3A4211')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (207, 2, 21, N'ENTER YOUR DATA', N'087F84449D16429F234DDBA6620003767BDE8334B5BEA1D950A2B4AD1B1147074')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (208, 2, 22, N'ID Number', N'6787EE9C2DC484485D10A610BA6A434C07E54E1E396C91EAF343E3B286E60F87')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (209, 2, 23, N'First Name', N'15FC527A55236F3A9014318A747F9BEECD02CE073891BE00B69F5DB30A3B8DC9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (210, 2, 24, N'Last Name', N'0AA62E4C855E26F7634CC4668992E1CFC39D725E749FCABA49BEEFF3360B4AE54')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (211, 2, 25, N'Email', N'57229ACE42094FBD61902A536B3D6D4A793BE2F55A2E3528C858F2597C7E5CB4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (212, 2, 26, N'Username', N'5F8FA02C442A495CE6C7E1B784F9A2A7DC04076FF70AF5171E1A7EBD8EFF48ED')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (213, 2, 27, N'Password', N'0A4F0C5017495B4B916B696A79248C11EF39FE3F2273AA054A0D2FA66C5A158F2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (214, 2, 28, N'Confirm Password', N'0CF2344BD3A3910E94D3DFE22715C1EFF64928ABB70210C5B38AA138450BC2254')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (215, 2, 29, N'Cancel', N'40E53C3ED0AE31F9607F68D9100D7B2AC211D8955E8AD8EDA64C5BD52B63D0C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (216, 2, 30, N'Sign Up', N'0AB7F1ABE610F758762CBEEF8AED81235420F42DC39BB01581E1E86B76199AA7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (217, 2, 31, N'Register Pet', N'0CF5760D84387185986DC76BC6CDB0C2CCDF1B76C0BE3FF2A776A7623916972E9')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (218, 2, 32, N'PET DATA', N'50AA1B91C0BCA11025B3BFF427CFE063018BF6B07896586474CBF04C846BAC44')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (219, 2, 33, N'Name:', N'1384B3089BE35C965C1A46901E00881656203815371950488D67CFE5FBC2767E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (220, 2, 34, N'Species:', N'43C54AEFC24B8484A296A1897A1DE4A3E6E7256C9BDB2A9957C72FAFF4519026')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (221, 2, 35, N'Breed:', N'0C8F33AB4E93663501A80228BF8DEAA33F4E90D502F1C7A42BCE82902EF99FA50')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (222, 2, 36, N'Date of birth:', N'4738738793BB20CF32A23395256FFBE68CDF2276CE742CB1257B7ED7E3CEE269')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (223, 2, 37, N'Name', N'09D092BF1F1B0D8486B548816CA8A430FB6E9EA814CBFCC2F156FA265D0B97184')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (224, 2, 38, N'Species', N'0E9FEF6E27EAB14126B8E2202B6FD80F5CE52392A893EA249AA6534501C5245B8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (225, 2, 39, N'Breed', N'0D150B2FF5E6E49A032C9CC7C0EC4ED61863F9540A659FCE049626BCFAD996DAE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (226, 2, 40, N'Cancel', N'0FB0785547B25095DA59C12741593CDE97600C9A99D28BA0762B1C0D6C9ACC9E0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (227, 2, 41, N'Register', N'0F00073A921CD9153FDAAE52AA214E008673E12B42842234762E251158855F8BD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (228, 2, 42, N'Change password', N'0C8876D459BDAB13F9B7312EED4FCC735C6353F7FE78BB81B2D39ECB995120562')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (229, 2, 43, N'Fill in the data:', N'092EC63F6A13BBBCED80531B848B97A372B18A19163E49B70F30842414E52172')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (230, 2, 44, N'Current password', N'0CAA0D745E418E2F4B0A081DF36B7697B631B0A80B878FEB95549F24E2F50D8E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (231, 2, 45, N'New password', N'0A1B684A735028E043DD89DAC82B61084F8F59A1D0FBB7BA5553CB8A359E6E45C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (232, 2, 46, N'Confirm new password', N'1208DE0F38B34553BF89C0B2EA21315E4327A162C3ABAA5A8066A7E0DEF89BA5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (233, 2, 47, N'Cancel', N'0F4E5D39EB704E93CF24C8688C76702511A649F801B16EFE978DB9E01522E853C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (234, 2, 48, N'Change password', N'514B31D3557F3708B1D55BB1145F3BD2B75274BD63D329FB688AAA4A64843245')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (235, 2, 49, N'Administrator Panel', N'084183C3B71318F699DE35804F970E0833143652D7953FA46CCC3A2016601D2E6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (236, 2, 50, N'Menu', N'0CA8483A12DA718141AA770098D4D540A229407AAC38047A7A698FC89487BD674')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (237, 2, 51, N'User Management', N'51084DF778C89ACEAEB2102EAE24469660AC4DB136D63022CE201CD1760E18D8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (238, 2, 52, N'Event Log', N'0B37AC3B0121C2B2A159A12DC90DB07E34490C0787B1546C2EACBB5A91099E8FA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (239, 2, 53, N'Profile Management', N'2D4E69DA7E028ACD3F3D2A87954CE25C7BD4CF212B70CBFE2962789D9681DB73')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (240, 2, 54, N'Family Management', N'1A4CB646F3DA98B6210351EE6C4C94DCDD76B58C5BE356F0769147E59D7439E9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (241, 2, 55, N'Backup/Restore', N'7384B7FC92634A77325CAEE93A553B75548468CA0DC7F8F45BBBB8B3CDC42191')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (242, 2, 56, N'Backup', N'0B7E7FD40BFC1B9F40BB4C87D614EFC3EE7ED9ED74E0CB64980F06681A2B14287')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (243, 2, 57, N'Click the button to generate a database backup and download it to your computer', N'0822964AD18545E791374CC02D97639DEBB500DF6FCB944B62FD3B1BA19000753')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (244, 2, 58, N'Download backup', N'0FC93C35564680BDECF1C2C6E871A3E83D7A6740C26BB0ABAA8B435F55F74C03C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (245, 2, 59, N'Restore', N'69FC790A2D346D055E5AEFD333AEDBF31FFA3A03B2D9F9CADB9515B4C50948DB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (246, 2, 60, N'Upload a ".bak" file and click the button to restore the database from that backup.', N'0F2FA93E4863956BB3BCFFBAE450C2753E3EB0B4AA522922982C7148292699965')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (247, 2, 61, N'Upload .BAK', N'4EA5D339B6E27B9CCD280756956A49E92C3DFAD3305E11558A5AB84CD677DD2A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (248, 2, 62, N'Restore', N'0D6A95ECC658270901F59FCB0EBA0F403392497F38413FD6EE2C23DDC35E7D718')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (249, 2, 63, N'Data Inconsistency!', N'5E26FCE5BFA8B758CD4AD155459FAAD3A26E97677BD2229B607FC0807CF2F9B4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (250, 2, 64, N'Exit', N'4A66E111AE74441C2FC7FF7BC1A7C7BAC382DCD47F08B730A2B514EB5919D31A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (251, 2, 65, N'Recalculate', N'33BC5DE4FDEBAEAC42FF43CC29A5C9355E7A768A63EEEA3675DD9A3F9ED8E6BB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (252, 2, 66, N'Restore DB', N'3F3639B07EA48128D142251502C4BF03AFB508A3CA735B5CC1DE4B346B338D19')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (253, 2, 67, N'Upload .BAK', N'0999634B9CEC7520C5A549D0E2455327FA194ADEF162D48EC317F6C41538F080A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (254, 2, 68, N'Affected Table', N'78B6C4683C995C905EDFE4BC65BE299F76F76496B6FC7CC5F1A51DD1E49D8D47')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (255, 2, 69, N'Record ID', N'1D52DABC6E21B498FCB499C396C2B50993C4628DBA1FA7149D61B16D7566D494')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (256, 2, 70, N'Inconsistency Diagnosis', N'0A81AB707DD9B2BA86883FE41B233B7B8267A3D3D1F1EDDA1BE49A19BF13B7FA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (257, 2, 71, N'Administrator Panel', N'0AC9487C468817B96437882D0FABAF0CE8D1C10C79D2D1C0256931F824B69B856')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (258, 2, 72, N'Menu', N'377F0D6EFFA089E665F5452C80855D657864AD53CDF4B23A157A5D61EDEA5A62')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (259, 2, 73, N'User Management', N'14EB33E75A69BBC65AD48579F475ABB6FB93BE1603FD7068CB9DD99A33BCD95A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (260, 2, 74, N'Event Log', N'20A365E24339720699B8B9A4E6AC0BC0D6E1AEC26833341771E0FC7D1C07FA4E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (261, 2, 75, N'Profile Management', N'44A91409BE2EE4D68BC69C5255D268A5B1AADB1D8FCBEB150162511B35F3CAAE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (262, 2, 76, N'Family Management', N'4D2654BE19BC8BC1103450B78458A7D92BAA869D9BD9DD34FDB65F84AB6630C2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (263, 2, 77, N'Backup/Restore', N'5B1E0330757EEABAB4E06D1F04D052A7AD5C7A1D2F758231D4953B323C7F31DC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (264, 2, 78, N'Language Management', N'08E65ACF0C9F4F260494AEA1C19C8381D49C6CEFF25E8D2A7F08384B4349E4BBA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (265, 2, 79, N'Event Log', N'0ABFD121BFD64893E2C3A4E1FB198478AF94609547986AA98B04771D011349785')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (266, 2, 80, N'User', N'5A8E02F2FB838F89FA04C1127B3D5718C432FC7F2804BB2A2CDFB067EFAF78CE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (267, 2, 81, N'Date and Time', N'0EA7C83BEDD614D0FC7FA80E82892AF563D218F7C7623E3B534EDE2F9C45DCA7F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (268, 2, 82, N'Module', N'595CF47312C25A991343D02258F12A2891C570289D76EB51AC021BB2D3D16A95')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (269, 2, 83, N'Event', N'0D5A1ED6888BB2FA3A978696A69B8B04DA7DC99EEA37C2E1235830C5AD4495EF1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (270, 2, 84, N'Criticality', N'7BE4E11E31C215E41D39070D192BF5610C1F63FCDC6EA982AF9C22E404956675')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (271, 2, 85, N'Username', N'1EAA00C338DDF33EBCD03F3401F9CDCB6045F1E48420B1DF62B5CDCC0B75C8B8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (272, 2, 86, N'Filter', N'58E2CF90BD554BEF67AD445CB016FA6AD27BFE56A30855D9A5F25EBF328BB0D1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (273, 2, 87, N'Clear', N'7A968618842C0654EA05A447D8A7EB9F613CA2292B326A17E725E9BDFC18C835')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (274, 2, 88, N'Filter by date', N'77B1EA0AB93710A7985F5E9B5477C932A444CADD5B64AC8983FF6EDEF2A56FDF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (275, 2, 89, N'Module', N'7C07ADC92B4C8C2177717F150DE33BAB61C5FE7BFB7EA1C3129922FDA88E2351')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (276, 2, 90, N'Users', N'0D899AFE7F067867B37F1FFED86B2530AE02138EE94F43ADC052467D4B75A3955')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (277, 2, 91, N'Administrator', N'0A07D8FB39ABD70BDCB09E70ED576BB46C0CDB1AA889BD5ACF803F803C1873B35')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (278, 2, 92, N'Clients', N'0B0BB60E7F509E6DDBE7DC209DC1B2538571BB7F6A904F18C414A67B11D19138')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (279, 2, 93, N'Under development ...', N'0AFEA7B56C4B0E197993963AC64AD0214AD5C7A2B7D8370AF1A85752EF7F17BBF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (280, 2, 94, N'Event', N'0B650CEA87B8355981E056537D79921896607A961B97EE9CC74AA3E8976793A68')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (281, 2, 95, N'Sign In', N'0CFB3CCADFD125D83D91B41E5EAC803DBEE6F63BF79575803B4DD4BF31B12BFEB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (282, 2, 96, N'Register User', N'737D37027AAA97E22E35E83A4816A47D00425A34809DDD81900700D6D768124F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (283, 2, 97, N'Log Out', N'0A732815A46F19F3706E6E49F470E9C4DB565DD9BF507622EAA88DF08D8A79AF4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (284, 2, 98, N'Change Password', N'488F15A2F8BF5766A23FBD0F507A1A557CB6D210A40655B8C445D5F0B9F0E632')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (285, 2, 99, N'Unlock User', N'57A566E87A5E619E9C4739DD6E07285D9A07EB5578537C3499D79C8D014A4BD1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (286, 2, 100, N'Edit User', N'7422802E3CF32665D3E5838D5D662A54103A162D09CDCC15E17B8A991DB4A405')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (287, 2, 101, N'Make Backup', N'108D220122E622C48538CFDE9D7A2CDC714BE83A1CBF15B69DAEF55E74DAD09A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (288, 2, 102, N'Make Restore', N'0C257631752FC607F126E39A6BB5800A05DFDDC819935BB5D164B728B1CA6F68D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (289, 2, 103, N'Register Pet', N'442E01471CF747621B1F667F24237ED93CEFFD9D0EAB6F3E4A7857519A4954A4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (290, 2, 104, N'Criticality', N'688A218A40798EC27903ED483B6670946F35A3903E61ED6E486B56A33B1266CA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (291, 2, 105, N'1 (Critical)', N'0FD627F84EB03BF4D8D469C6929D4F33CFA126D67B9097BC9B23B626D0643623E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (292, 2, 106, N'2 (Important)', N'08B8A848EAFC39A9D729C79129255DA550C61E5271F6CAA2478F19FE9B23773FB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (293, 2, 107, N'3 (Medium)', N'09B9570386D65F0790EC00F3F3BF730D1D96850BB9D630AA740238428C1BDF3DD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (294, 2, 108, N'4 (Low)', N'09413B0B0D54890E784B7D4FD3C5D4E31A1AE31BEA590AB4DCE4A05E8A13B4184')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (295, 2, 109, N'5 (Minimal)', N'09531B036DE1D2C2BDAE65D36B5E45673D7DC904DE6521BA33143D8CDFA1191B3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (296, 2, 110, N'Administrator Panel', N'64EFCF57A397C3674BF9A2F699E64FE05E2AD87CD7418D56C2954B63C0BBC58')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (297, 2, 111, N'Menu', N'0CA8255E4EA5B91A6E2FED09D2BACFC0171B4E3328CFEACD44826D20F43912072')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (298, 2, 112, N'User Management', N'10434FCF88F1286517C9ACC038D4DEC1933124B7B06F3B6A2E3EA543B787308C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (299, 2, 113, N'Event Log', N'5BBB84E84306AF77EECF1A9D6800150D96A83038E9AC271B74276FB5DF5AE42D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (300, 2, 114, N'Profile Management', N'09437FDADC773614619AC6E5D9EBFA6160E4617D5FCEBE88763A4E2BB56FAD631')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (301, 2, 115, N'Family Management', N'4ACD96B58FF6FFD29A4F2778ED24976348702A1760E632FA1B77D0ED0B5A8D6E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (302, 2, 116, N'Backup/Restore', N'0AB332F55891FD7506DE0AC704051FB0A263D401147CA28A54C50BC4AC0915087')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (303, 2, 117, N'Language Management', N'692386C5A5119162DABBFECEDE8AA5F4798420794299DC0197724223735837A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (304, 2, 118, N'User Management', N'0BB3D558645B06EEFBE30515A8AA521C30EC72AC8E3D7B0CD96E4DB86F16DE010')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (305, 2, 119, N'ID', N'6C55D8053783B54E408BCF48E468B27A55580687E6D2C2B319323ADFD670D34A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (306, 2, 120, N'First Name', N'0F92114771936FD89EC2B3023044241BC790372C67F3E0E7C06D909F7220EAAD0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (307, 2, 121, N'Last Name', N'432FE6CC89C8B7277083CC9C2E8652BC558F17E330D62A931DE8B1493B27CFC7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (308, 2, 122, N'Email', N'344EDD74EBADD4D4B278B47F380A091A9A76AAC4DBFD81FD50B9E4D5B29A8F9E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (309, 2, 123, N'Username', N'39960BCD076704C4EF50103448ABF76379CD69779E62F6C7EFEF925E0C5906E5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (310, 2, 124, N'Role', N'0DD0F63C28D06953AE86E7D6F17BCC21B4B8C82364D2BE9D30AD51BE65BF61CD5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (311, 2, 125, N'Select', N'0F81F5DA11027F147505694C41925A43C63979C6DB6CCC5C2DD101F021F8F6825')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (312, 2, 126, N'No users to display.', N'0BBB32CAE28E0418DDE028526F7C67CBB10A55739A6FE3AE3D6BD04319622D2A6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (313, 2, 127, N'Locked', N'0F742B299062AB959CF52C68B043DFD571A31984F4988F26890E3023F72085A5D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (314, 2, 128, N'All', N'0D803ADDD4CBE0ECC7A1C3A36CC46E5BA3462E1E1AC829BE131D2911E38AB2B03')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (315, 2, 129, N'View encrypted data', N'71F482AB477CE455ABBCDC17B562A7D38D67CD4ACCE28EB94EFC9457E3889F81')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (316, 2, 130, N'Mode:', N'0D4AC5CA7A622FDB82A5C75B56B2CF54BEB9F7F12BC90D9E43577EFA021D633D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (317, 2, 131, N'ID:', N'75DEF53DCD78D1E445644E9D48FA199F802FF11A60544B7D1499C4DA354BCEEB')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (318, 2, 132, N'First Name:', N'23E34825BE02FF9FDE3E686DC25BA004545288B9E2E7A78C1E4B73DF85848AFB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (319, 2, 133, N'Last Name:', N'0C83C142812303F548C1E1D78C82E7E7B9A96C9DE47B6DD6CAF6A97E035149457')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (320, 2, 134, N'Email:', N'47F52A5B53B1475082472C4A3AF87D28CEBD99E20AC0A3E39F8DE901FB6D82FE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (321, 2, 135, N'Username:', N'0A6893EF9B0AE85293F80C80D49EB7BD3DA84293CD1EF5E72335BE51182A3F8DA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (322, 2, 136, N'Role:', N'0AA9B5D1296DEEB78FFA2C402C68549D835E649F34DE7B1C575656AAFAE9A4CAE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (323, 2, 137, N'-- Select Role --', N'4777D6672E8B5FCD51AA392B092E8D682C0AC00631B4EAFF3C3B9D243D58F51A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (324, 2, 138, N'Create', N'09F508159DC77DF3F7474867721FF83E5E60DB4DFBE434028B365B4B6C387D75B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (325, 2, 139, N'Unlock', N'6EFFE10B83CED3F0D2159994E3EA765927CB34EA8962EC8B1A7EC71ABB55C072')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (326, 2, 140, N'Edit', N'09CC2F4A66E09AF8A510EE53C8A97C8FDC2A29819457D27464BC2F9D59CA7A00E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (327, 2, 141, N'Apply', N'5F248CFD14241A734D7554FF7FAFF971A2D5F40F22EAC8EFBE830D5ECCF37721')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (328, 2, 142, N'Cancel', N'0B2822D1358BB5D92C19FB21BBD07699DE8F827B7CDDC3ADE98AF6235449C0DCC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (329, 2, 143, N'Language Management', N'0DB03F23441E140A7AC9B183E5237F5213C01C69081AF990D201F65DB6B0F1E26')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (330, 2, 144, N'Menu', N'78FB69E8FACA711F6D84B2ECE876BDD1C6E1DC38456732E241A7019BCD45E8CC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (331, 2, 145, N'User Management', N'0AE85C2B9AAA96B0086377966B58295C5268110C18B88789F2C33ADADF168AE52')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (332, 2, 146, N'Event Log', N'34EEDBFE34554654932686FC02C90F4065D551F273555BD0D3AC784B30DAD91D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (333, 2, 147, N'Backup/Restore', N'136FB80EBF279231534ACB41480B4EE1181B109E93D1965B46502FB1E473A9EC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (334, 2, 148, N'Language Management', N'0E1886765314B1407EF529E63BC1647B4C918BB255BB9CA0CE183F40013547463')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (335, 2, 149, N'System Translations', N'2031F1BE6C45F443C54CD08CC40698DACCFB739F09453712CD52A45147E3C1CD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (336, 2, 150, N'Language to edit:', N'211B7475604B05B88244346DF621325B69D8B9C11DD05A4A77A88FEBEECB9290')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (337, 2, 151, N'Form:', N'088391C11998C3C39108802D6A1EB00602DE5D5FE83947479EC95E3A81BED4820')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (338, 2, 152, N'Control', N'09486325DF1B95D672C75FB5E639E1F93F1D656D6777970A108EAFECE6AF1CE15')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (339, 2, 153, N'Translated text', N'0C31B0BAEFFAEA6B503400E71CF63B609F957B0CB2C761A1CB1392F04902E997A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (340, 2, 154, N'Actions', N'0E26D8B5553D06C5ECBAB4F958C9A1ED79C781903423C8186B4C6A5DAF12AF1C4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (341, 2, 155, N'No labels to display.', N'0BAB10C8C7E7C4FE508434F26200E3E5CA6EED125FBDFE69F0BD072C1BF5E8599')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (342, 2, 156, N'Code', N'11824E9017FC70105A1897905F1FED42D7DC1504A277CE3762933558F57F3E54')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (343, 2, 157, N'Name', N'4B7F8B79AE2250FC485A925AEEBE4B9BA39316FA0E84D1C5EF8BB844E9E50940')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (344, 2, 158, N'Actions', N'0E16F5A8434F925117F33EC390C55AF6C66D5B93DED32CA6F8D2E98443E8E3B3F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (345, 2, 159, N'No languages registered.', N'0A2143FC5B525D4B5FF69B857C35FAEBED245A5A6FEDA641A2BCCACFE23D01126')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (346, 2, 160, N'New language', N'0AF5C55B5B956C9C55D51C9DFB1475F031C039CC340FB9FA3127743FE037B3E2E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (347, 2, 161, N'The language is created by copying the default Spanish texts (es-AR), which are then edited independently.', N'09CD6D3A77AC5FDCEC21CC1498C2400FA38483DE1549AB76A605C20FDFF06E568')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (348, 2, 162, N'Code:', N'2DF028B1818C8A51A3A9C210A7CF7150264506AA208A79B5672C13A34FFBA3D6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (349, 2, 163, N'Name:', N'0F116771759EA5CEF7ACAD286663B6C54BE7290053FC61193FB8DF4F2F667943A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (350, 2, 164, N'Portuguese', N'09C16D1A54CC152AEA79B55C50BE1BFB5AE377E4BB6829185BEB55BF1BBE346C9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (351, 2, 165, N'Create language', N'0E35DEAEC0092C336F29DBB410D8D1254837FF6C9F3BA2DD12C120AD505788C35')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (352, 2, 166, N'Registered languages', N'0E276CE9F6252941067B4F03C28BAD1B198D03592E34330291F01CD231CCC3ABB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (353, 2, 167, N'Save', N'631C97AA8FF146BEB08FA854EEBAF26FAB74E5C37A90E2CE847B92670151D12D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (354, 2, 168, N'Delete', N'0C15A33BE79442DE665E7EACD745100CA8EFB7547AD74386F61CDF16A43EBA571')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1849, 1, 169, N'No se pueden registrar mascotas. El sistema se encuentra en mantenimiento.', N'0B0B8970E698A11A9D13EAB2575AA17B7291DFBC82FFEC4D5389B91C689E966F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1850, 1, 170, N'Editando {0} ({1}) en {2} — {3} etiqueta(s), {4} sin traducir.', N'7A30453DDB12789EAF90FFE7BBA49430DA7A45F093863F667258CBA86D270FBF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1851, 2, 169, N'Pets cannot be registered. The system is under maintenance.', N'0E95775D466B808061A4F398571694D542841269BE2B8D00FC7F6B3C5EAC9F8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1852, 2, 170, N'Editing {0} ({1}) in {2} — {3} label(s), {4} untranslated.', N'0F29D1AA877FB47BD412E6A0287DA2D47D2413DE527A2D6F93A24BCDF525D235')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1853, 1, 171, N'Acceso denegado: No tienes los permisos necesarios para ingresar a esta sección.', N'419535FA6084A7D6CDDAF371D804E79678996ED94B443B24F4323C4C39CA3313')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1854, 1, 172, N'Error al actualizar la contraseña.', N'0B0A4E0D02C7A39062E6E3119F3310214E5D2B60F5B2AA79C13F3F17B6EB1DACE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1855, 1, 173, N'Error al actualizar el usuario.', N'67DC67790E7904E4D0B83741AF28A4816331EFA2B00991177B0F78E35EA8A5A2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1856, 1, 174, N'Ingrese su/s apellido/s correctamente.', N'0BECAF5E25509853C3BF77C7B151FACBCC8E3E4349D03B2FE512A461766496C06')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1857, 1, 175, N'El campo de Apellido es obligatorio.', N'3DD3368617D84431287AFCE7755FF97F8DB6E70F2BB1D3CE9E2C394D7D05EEE7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1858, 1, 176, N'Error: El archivo seleccionado debe tener la extensión .bak', N'455B4745B0504323C5420001D99E73F49C2CEA89AB7AB6759DFE6C3CA775E03C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1859, 1, 177, N'El archivo no se subió correctamente al servidor.', N'0B713D6EFE4DDFFCBC001C57EA0F92956D89E38E0034E732FCC7B696C8DA81F3D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1860, 1, 178, N'Debe seleccionar un archivo .bak en su computadora primero.', N'08559307FF1EDA3B184085723A4C951FB4803E46990ED4448484A6CEDE97FB18B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1861, 1, 179, N'ERROR: {0}', N'0F899A0A29F9056E529E7AE27469F274D20369B0D81E7A131BD28EC323FE11EC6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1862, 1, 180, N'Error al cambiar visualización: {0}', N'0F30B6214A85BA220A1B076AEDF3C9B27096D8AEF1503B2E2787ADD3501CBEEC4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1863, 1, 181, N'Debe completar todos los campos.', N'0CADF46337AA1598A99BFD68D54043E5AB434271B14AA88BA653593D75C61BC45')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1864, 1, 182, N'No existe la carpeta temporal en el servidor.', N'3A23342456FD9DFDBE644A5EC4E6B8B6409333B5F2E747880ED5E46BFBF25E71')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1865, 1, 183, N'El código de idioma ''{0}'' ya se encuentra registrado.', N'0A33E92FE180EBC87E51F46AB19D9D3AA61A40BCB3C65325590F6923080883E29')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1866, 1, 184, N'El código debe tener el formato ''es-AR'' o ''en-US'' (por ejemplo: pt-BR).', N'188CD2FE8E47CAF51DC6D692A3D8F01E88A45C49E1E2752511F571F4FDADC5BA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1867, 1, 185, N'El código del idioma es obligatorio.', N'0B347BDDC88BE643CE2B1128E94CC72E75CCB38D61BB49396F6700DD4CE1838B1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1868, 1, 186, N'La contraseña nueva no coincide con la contraseña de confirmación.', N'72B525D05E7A5173CE2BF3C8C85AC500BC26273D831B53F0FBD0812C9DA584D5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1869, 1, 187, N'Error al consultar el usuario.', N'0B535AA780969A21343400BC8AF23E324DF39A33E9D9E1D5F6FDFD7F455647F4D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1870, 1, 188, N'El DNI debe contener 8 (ocho) dígitos.', N'0D22842B6E475517EF37E28D42F6742E9D53BDA737391A32329A46C7C1529B09F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1871, 1, 189, N'El DNI debe tener 8 dígitos.', N'5B6375981A0C807AEFE59458617FA159F535CB45346CB98596B0D8E053DDBB7C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1872, 1, 190, N'El DNI ingresado ya se encuentra en uso.', N'0FD9F196B541DB768DD5681E3D18BE09B7738E751D98719B5362E6585A00045DC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1873, 1, 191, N'El campo de DNI es obligatorio.', N'0A5951BC62508514052D1F31F6EC6CB067F3AF6608533166E139F782E23C6969D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1874, 1, 192, N'DNI ya registrado.', N'0B6BE211839E3AF2F73DB27882AFB11BA190BF9ACA0CDE9C4B32D5492EAA65F17')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1875, 1, 193, N'El Email ingresado ya se encuentra en uso.', N'084373F5956B04490F7B7AA54302B002653196109171CC2072F6609BFF088AD74')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1876, 1, 194, N'No se pudo identificar la etiqueta a actualizar.', N'09F4EC08BF2A010FB57CED018331F9014E24778915C44A11372983E56A9EF56C8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1877, 1, 195, N'La etiqueta seleccionada no pertenece al formulario indicado.', N'0DB56A647720A0DBB4F854F0D47AF61AE37859EE9346CDCA78D365F117B204946')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1878, 1, 196, N'Fecha de nacimiento inválida.', N'0C913D9CDB1537B3D3F9AB81837D22CC8B2C2A28F021D079DFFF4B89597A06EAC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1879, 1, 197, N'Error al filtrar la lista: {0}', N'26FF9F0658256D6A91890928C130B6FD7EADBBC64401E1CE2A3F37AEB0939C78')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1880, 1, 198, N'Debe seleccionar un formulario.', N'0E0296832B8990158170F7B84377CC23E9C6FB6CE897C22651CAFD3C1E44A53CB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1881, 1, 199, N'El idioma que intenta eliminar no existe.', N'0A78F15FA620248695D82C69626978EB0C2BF8D4F687BF7F921134DDE342F305D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1882, 1, 200, N'El idioma ''{0}'' no se encuentra registrado.', N'0B6B61D07525E19B7324555C01DE446697D8ABDF53D2736DB67232A7A8212108E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1883, 1, 201, N'Debe seleccionar un idioma.', N'0FD3BDB578DE07F0EE84EE5397A20127325E93364A09F4B83BBC3B30859D797F4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1884, 1, 202, N'No se puede eliminar el idioma por defecto ''{0}''.', N'09492C560AFF7354F5442C240E0F35F72DC2FF44E664B8F9206A05172835DDEA7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1885, 1, 203, N'No se encontró el idioma por defecto ''{0}'' necesario para copiar las traducciones.', N'08EDFC263DF59772BCABD803F2FBF9611F7B04FF4F55E42705DD5E6A53C40F47D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1886, 1, 204, N'El nombre del idioma no puede superar los 50 caracteres.', N'09C63E322DB176E101645395BF76999F42B884051A15FFB8CB712D18E71AA34FB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1887, 1, 205, N'El nombre del idioma es obligatorio.', N'0D8B6E7F2F547D8A9F27CA748F17D45FEEF4CE4B3E212C900CB99ABF89296DD02')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1888, 1, 206, N'Ingrese su/s nombre/s correctamente.', N'0866299EE267780EF6BF0C074993782C1C99929FA010E703C30BD007E1063F90B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1889, 1, 207, N'El campo de Nombre es obligatorio.', N'2DA2B6DD00C4DB13489D2606F9C79D46A5F2372553CB513B321C1231300846EF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1890, 1, 208, N'La contraseña actual es incorrecta.', N'414FEC3E7E3A9520BC3B33FEB7F2F53043A204B247B9D5161D3C4B20A093C5BA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1891, 1, 209, N'La nueva contraseña no puede ser igual a la actual.', N'0FF79995A0EAC8681905BC60BC8816BDC5891E23B5DA436CE5A87E681461D1722')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1892, 1, 210, N'La contraseña debe tener entre 8 y 20 caracteres, e incluir al menos una mayúscula, una minúscula, un número y un carácter especial (@*_/#$%).', N'09675AD381EF834EFD3C40026F27EAA80F7302EED4C68224B3106974F751BAB61')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1893, 1, 211, N'La contraseña nueva debe tener entre 8 y 20 caracteres, e incluir al menos una mayúscula, una minúscula, un número y un carácter especial (@*_/#$%).', N'0C64D0BEB4746DFF83AA7ECFE7D2CE1DA926A1A67E7F8038A55CDFBC66EBF9779')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1894, 1, 212, N'La contraseña y la contraseña de confirmación no coinciden.', N'6644E39756504895FE523132864C876FB3869D4BC3E9F2AAA9E63CBD46A7D470')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1895, 1, 213, N'Error en el proceso de restauración: {0}', N'0E2DDCBD32011FFFA9EB3500FA9F51B1362EDEC627E71628949319E411D6F1631')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1896, 1, 214, N'Error al reestablecer dígitos: {0}', N'0C143DB9F9B567B2A4EF75FFA8C21811784669DCFC9E7645B2189FC8EAACA2B4C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1897, 1, 215, N'ERROR al restaurar: {0}', N'0FB2AF5B6D3B23874453BC55BB7763351D2CED9FC13BAF5FEA0550FE3C4EAF36E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1898, 1, 216, N'ERROR crítico en el proceso de restauración: {0}', N'7B05DC548AB0C99D5AFEEF84F1BD2DEDF1D1F8958889FCCCCEDA946CCBD2FC58')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1899, 1, 217, N'Debe seleccionar un usuario de la lista para poder desbloquearlo.', N'64DD72018007CA9E46DE1860E5298EF1E8CE425F74E9CFAE0693FAD10BCBFFE5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1900, 1, 218, N'Debe seleccionar un usuario en la lista para poder modificarlo.', N'22F77AB25ED8640E02BCCD17CED98F60DBDFA16BEFB82D04B11EAF3CBFF0536D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1901, 1, 219, N'No se puede cambiar la contraseña. El sistema se encuentra en estado de inconsistencia.', N'0EDE13ECDF940ED4C2C665DEB4AB2D52F4E29C1B36D19EDC5BFADDB1AB719B6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1902, 1, 220, N'El sistema se encuentra en mantenimiento.', N'08F22D7ACD8D9FD5B19DAA2E5E605104872B1103DF7FF3969B3011E7747C68117')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1903, 1, 221, N'No se pueden actualizar datos. El sistema se encuentra en mantenimiento.', N'0E8C1D9883483F9568FF989948409F0CAE402294DA93BF77AA8D80200815ECD19')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1904, 1, 222, N'No se pueden registrar usuarios. El sistema se encuentra en mantenimiento.', N'1B9EC34666339898C9397331830F5C3CF98417A4C8EBCF28AF25767EA94CA75')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1905, 1, 223, N'El nombre de usuario ingresado ya se encuentra en uso.', N'09E67A93D1E7D0BAE9B49B64F60E848FD71CAFA93179651D535FD2D3F7E6B8BB1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1906, 1, 224, N'Todos los campos son obligatorios para crear el usuario.', N'0E881F834769D9E54E4BF34EDF259C291B256F299A1BBDC421570EB277D1B3812')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1907, 1, 225, N'Todos los campos son obligatorios para modificar el usuario.', N'5C07ACDAAA4625D5DDC9DF6C8263EE17BB47ADD18AAF01FB30875B884B5E51ED')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1908, 1, 226, N'El usuario que intenta modificar no existe.', N'0D459A820226E98B934E0AC154F9E17ECED6ADA006DA6C3C633302BEC8450D944')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1909, 1, 227, N'El usuario no está bloqueado.', N'0EE909AF321342778B7E8AFC272A07BB43004EEADF177E7BB588EE1175D563C1A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1910, 1, 228, N'El usuario no existe.', N'71CB752291B78431849402E0C43CA284DBABAF1020297932E48A1B8F3AF1E73F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1911, 1, 229, N'El campo de Usuario es obligatorio.', N'09737770FB362C6F3D218695B15A9E79FCAFF8590E22C8CEA6A8C6EB6C888119E')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1912, 1, 230, N'Usuario ya existente.', N'0F893D04C06653FF5F1BAD986FDC6904378148106929C78567F253795A5623CE4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1913, 1, 231, N'Backup creado con éxito.', N'786C7ED2939AD7146A79E3A9842E10FA087084E577AF992EDE1086411AF9F151')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1914, 1, 232, N'Base de datos restaurada con éxito.', N'0AF8575D85E630F108B4E576DB7E6AEB9F07AD5327674DC51EC77370011133DB8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1915, 1, 233, N'Base de datos restaurada con éxito. El sistema volverá al inicio.', N'0C4B2BF6B4FA810E4913B78DDCB4B6CFB71CF17B4D369CBCA7A26AEA41BFE4933')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1916, 1, 234, N'La contraseña fue cambiada exitosamente.', N'35538BDEFCBA09DF751980670FEBD69D797EB05020F456C4B19F3E8B9F54E862')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1917, 1, 235, N'Se han reestablecido los dígitos verificadores con éxito.', N'4D348AE07E71836808E30DECFCC3ECDF06BDB6E6AF40A408ADE31BB48E1D1C9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1918, 1, 236, N'Idioma ''{0}'' creado con las traducciones por defecto.', N'0FD6285687EECCC6B75108E4E67314AEE5098DA8DD217603896CF450BE5B86C34')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1919, 1, 237, N'Idioma ''{0}'' eliminado correctamente.', N'09CE5BB818358C4D182AA877EDD75D66E56B57A76C39F9F0C402C5FAF25D25F2D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1920, 1, 238, N'Mascota registrada correctamente.', N'0E0812DC047E2EED4B3291965C7A64FE13BDE09CAAC047AE3ECAD096AD7059B15')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1921, 1, 239, N'Traducción de ''{0}'' guardada para ''{1}''.', N'154871064029F32B5F026C36B82ECA8549BDC8050B333C34B360FC7D983A1418')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1922, 1, 240, N'Usuario añadido correctamente.', N'685C18A6133BA639E9D41259C2F63EB966D96BE2A1829DBA2A2EC7B3FF2B3ED8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1923, 1, 241, N'El usuario ha sido desbloqueado correctamente.', N'53C42A930EBCF793BB308891460ECFE279DE09901711B71C6EBD12D9F021C881')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1924, 1, 242, N'Usuario modificado correctamente.', N'0CD09F83ABC8306D31A2E2F2EB6B4587DB6905ECA77123CB5F3B0DA32A14EE572')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1925, 2, 171, N'Access denied: You do not have the required permissions to enter this section.', N'0F77C935C914066637835D471434A9B8C71933DCF8E435A812BF0649880316892')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1926, 2, 172, N'Error updating the password.', N'5DCE4B632E62A3778883628F87FB35D1F7B9B1C15D96BFEFE60C778C46C94A48')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1927, 2, 173, N'Error updating the user.', N'78DBA99DB98DCCBFD69B9D6FADD552501A40498C24E57FC52405CC29A84989C3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1928, 2, 174, N'Enter your last name(s) correctly.', N'715739E719A22A45ACC27975C2EDCF93AF0E73302C325BFBA7E18C9B92A5797C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1929, 2, 175, N'The Last Name field is required.', N'7590EF26548F3365E9D31758D6FE7B5157C5DB04C385A1D9A5C8B8D5613D652F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1930, 2, 176, N'Error: The selected file must have the .bak extension', N'0D9ECDD97C3A4CB6C008A099362FBE8D6801CDF056EAB8E3A067EB087DE55F312')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1931, 2, 177, N'The file was not uploaded correctly to the server.', N'0FF3BFD8323CF5DE15A1CDB0AC353A9ABAAC46428E5FC310DFE64721F9E890881')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1932, 2, 178, N'You must select a .bak file on your computer first.', N'49090F01E369CAFF2950E792075001AD9F68FDB033B73E105F0F626FA9609B98')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1933, 2, 179, N'ERROR: {0}', N'0E0AA008B68D48534FFF01A2F9A3C55184A5817956A8A52830C517F5CE7D16F30')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1934, 2, 180, N'Error changing the view: {0}', N'0EFD77EF6384E3653BF42E973BD398A35CFA18A43CAEB8EB40A64703BD5CF52E6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1935, 2, 181, N'You must complete all fields.', N'0F9D9FC6A45BCBC5B8E20FC4BCA32FB8059CE65C054EC41C5CB6C9244F90F1688')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1936, 2, 182, N'The temporary folder does not exist on the server.', N'0DF0A7B73FA4D22B4D7DB580555E68EB65B394DAF39E856063D053849153158D6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1937, 2, 183, N'The language code ''{0}'' is already registered.', N'23C54912EA284F38362FA335FD81BDA3405FF455BA9CC164776924569F9DF50F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1938, 2, 184, N'The code must have the format ''es-AR'' or ''en-US'' (for example: pt-BR).', N'28D02326E50C0B4F3FAA7D4F4C9C88DE6E8EC76E4C591A2A0C977650F10B45A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1939, 2, 185, N'The language code is required.', N'0A516631416B6E2E2B4F4819105004E43DD88B4435D0B78011075838E3E939AC7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1940, 2, 186, N'The new password does not match the confirmation password.', N'0ABB2714D33A6CBEFBC5C747B589327FF393F7941C0BB6D1322E443C265890103')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1941, 2, 187, N'Error querying the user.', N'0BA1E23DD2D498A2E8530BDF18C946A2A8EBE99FBD7E22FAFCF98CEB4335B2EE7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1942, 2, 188, N'The DNI must contain 8 (eight) digits.', N'30457461487E178EBC7EC906712589EEE3CFAC7CEA6022C06D7401439EE6F909')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1943, 2, 189, N'The DNI must have 8 digits.', N'607E1B3A8DF1F4A00E0F5C8E1B724740F33D28EA8B52F621C713D0FD1589A84C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1944, 2, 190, N'The entered DNI is already in use.', N'19FFA4F681F764AA3B36E6333AF281D23601BF9FE4F61D4F103C8E09871923C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1945, 2, 191, N'The DNI field is required.', N'0E7CD749DE7B1F9E2FAFFCD6084DE71B42699CDFC8FD60260B70A823E9EA7E8AE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1946, 2, 192, N'DNI already registered.', N'737CAAD2CEF26D8C1A6B4286844F3EC65DF141C81801384DCDD51C6AF7057B0E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1947, 2, 193, N'The entered email is already in use.', N'4C57C6FE7F4874E2199A112D60B773DE6914C629DD5DA4A738D8165616DDCF25')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1948, 2, 194, N'The label to update could not be identified.', N'6F16FB21E33A9B95BD63F4591E228651E60AF1CFC8168574BE53C8E746A5754B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1949, 2, 195, N'The selected label does not belong to the specified form.', N'0CC1905AB27C828BE8F41DE847E132627C7118FEA5CA23C629AF124A0F00E2890')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1950, 2, 196, N'Invalid date of birth.', N'2C569E38F4952AFC3B2C72D2859B750487840EB2BAE6D74959CF2F56D0472815')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1951, 2, 197, N'Error filtering the list: {0}', N'0E4C8B5261D60A67357DCAEBADBE089225AFD91B8444F237FDE524A9B5BDD7FF3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1952, 2, 198, N'You must select a form.', N'0E28258B6944DA9039C2687077ABDB03947A021AE7649B05D6CB3D98BB16306E2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1953, 2, 199, N'The language you are trying to delete does not exist.', N'262D4DB05BC1B70EFD449E6BE7854C44F6BDF01E04061F37824FB582DD2A034A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1954, 2, 200, N'The language ''{0}'' is not registered.', N'0BD84A31376C09F145A507535D3047563FD1A0E6EA0D51AFE48541FCDF3F03B82')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1955, 2, 201, N'You must select a language.', N'0E11BBC56DA415EBB1B913BF2E75686FA24BB3D1DA9BEC188CF58F217818992E4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1956, 2, 202, N'The default language ''{0}'' cannot be deleted.', N'0C719D16C44D9D1FBC952F5792FEA8B756C82BF560D6EDA4E9CA79DF9536DE8DF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1957, 2, 203, N'The default language ''{0}'' required to copy the translations was not found.', N'3F9D8FFD05ABC37E25EC4DDDFBC9BC79EF699D9F8B12CB6CC5E18B59A183AC6F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1958, 2, 204, N'The language name cannot exceed 50 characters.', N'08917C81D376F7BC33B402721262F4255B89F73D4D1E19C892D83BB4991FF4585')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1959, 2, 205, N'The language name is required.', N'66819B00BAE5EDE7FB8916DCDC40429CE57788E385C16F89DA133D3A575E29F4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1960, 2, 206, N'Enter your first name(s) correctly.', N'0B4B9252F80E22B575B6A61F52B5434AA8360D9976261AA656FAC50DA1A90E81C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1961, 2, 207, N'The First Name field is required.', N'0E53B98E595D0D5159EDA82B1FA43B7C48566E7162609A55DAE6CDE9AA8EF7EE8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1962, 2, 208, N'The current password is incorrect.', N'0BD4CD2C9D88E7698B46C5B0B651A7243326328666208CA6EB7AD699B4B9F23B3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1963, 2, 209, N'The new password cannot be the same as the current one.', N'0ECCE04952795113351AE574F57DFE057429EB78A60DB3A986011ADAA5E6F8A9E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1964, 2, 210, N'The password must be between 8 and 20 characters long and include at least one uppercase letter, one lowercase letter, one number and one special character (@*_/#$%).', N'36CD4D468B8C9085E4D11540FADA3F8E645C5E82B894A233DD02A63F3C5E345C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1965, 2, 211, N'The new password must be between 8 and 20 characters long and include at least one uppercase letter, one lowercase letter, one number and one special character (@*_/#$%).', N'0831AD21D4E44ED30A52D39C893B44B79A165675DF8182FF014A498E0457B70A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1966, 2, 212, N'The password and the confirmation password do not match.', N'0DAD4A0C1C2A3DBA2E7648738B1C61A9381CB7C945FEFB7D5A66892D0B8599FC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1967, 2, 213, N'Error during the restore process: {0}', N'0F5804C8A5585793786E57B16118B14DF84EEE26E8BDD8BBAD30F3AF966BB6F22')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1968, 2, 214, N'Error resetting digits: {0}', N'0A972D1911339AD65DE6D9559FAEEC91047B7A3E7403AFE59BF46B95D427CBBE6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1969, 2, 215, N'ERROR restoring: {0}', N'0E74E84BE4E11C243EA254AF464B91281E5E35B41713D9CD4777A1AAA31C13F35')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1970, 2, 216, N'CRITICAL ERROR during the restore process: {0}', N'7672E1BB3BC0CBE0D31EFA34E5DC7980852505A800F6AB23F4895F7FA86F15E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1971, 2, 217, N'You must select a user from the list to unblock.', N'0A203EDE6791A7F8D66B6508B96D9A471931CF2342B06ECAD6F8B5291D090FA7F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1972, 2, 218, N'You must select a user from the list to modify.', N'084779E54D3D9E99FEEDDBABFBC60555B1BCC231F24BAC89114ADFD1ED8396CB1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1973, 2, 219, N'The password cannot be changed. The system is in an inconsistent state.', N'40A50B0ECB79DAE82302D6B833282006E28981408581704CF66E62706911CDEF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1974, 2, 220, N'The system is under maintenance.', N'57FB211C9B20A5FFC3801EDEFD29DE9AFEAAD385D11076DFA4BEF004FC00F98')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1975, 2, 221, N'Data cannot be updated. The system is under maintenance.', N'478E8CA3968C0EEA19A1CC3378BFA084EB390CA908CA4A2641CD67633D2F23AE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1976, 2, 222, N'Users cannot be registered. The system is under maintenance.', N'5934126A0C78A2631C79209845FA2F4927E8A507AD1CB62CFC3AD0246FF4A16')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1977, 2, 223, N'The entered username is already in use.', N'09CF122AC4443ACE428720D096D209D7DAF34BEFB8FB6396BBEB34B47A97DE431')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1978, 2, 224, N'All fields are required to create the user.', N'0802223589E2D01B7C81595ABA23F0F00E36794EF8760017E3967F1102E17CF62')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1979, 2, 225, N'All fields are required to modify the user.', N'0E13EA05A75B2FC4A718995A62A668FE0C728D1261DBCE9CA7C7DD7B8EB3E5341')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1980, 2, 226, N'The user you are trying to modify does not exist.', N'0FAF1F9CB8F9EAAE9489597F6C0E97137A8647D1163A29EA997561E3F7648F5BB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1981, 2, 227, N'The user is not locked.', N'09948DADECA91CBF716276D01FD7339354A7504B1CCD97B3F4C9EA569975C1404')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1982, 2, 228, N'The user does not exist.', N'4B42CF05944524672365A771A01EAEBA3BD4B2F98286320247424650201181ED')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1983, 2, 229, N'The User field is required.', N'6795182231353842FA6CFC37E12139290B592A8328E50D1C3E32BC1A38BFAB7C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1984, 2, 230, N'User already exists.', N'1B3A3C45916104AA2669EE92538AA738A50F72924B5E506CF2C55F88CA678B95')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1985, 2, 231, N'Backup created successfully.', N'7411B83BDD3BC1611B572927AD4BFF49ED62CF7F77AEC23EF2551604D601F5FB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1986, 2, 232, N'Database restored successfully.', N'0BEC1FE3FC22711E93B901A1D3BFA89DA5C216E57D60CA03E554B9542F65DB4A1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1987, 2, 233, N'Database restored successfully. The system will return to the home page.', N'080F4D047AF71F5D3FB0BE2A3B5A4EF50060EECF7AAD17FFAA4D599263E77429C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1988, 2, 234, N'The password was changed successfully.', N'290347E33E45BEA8AE5D6F5C66278349ACA69A30BE56CAC1F86576A776B415E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1989, 2, 235, N'The check digits have been successfully reset.', N'3CAF5FE0C64ADF0C832F37A18E6CF8D74DFE550CBD7CDE261E6CF1299F6CCE7B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1990, 2, 236, N'Language ''{0}'' created with the default translations.', N'50D4517F8C83C5ED47243D3F5791A062F6C0F4BD0811D262C4B8127E7F7A2A70')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1991, 2, 237, N'Language ''{0}'' deleted successfully.', N'0FAAFB79891635160CD71D70AE0E53EF0B0F9B7DC662B6CE693BE2DF234C3D2DD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1992, 2, 238, N'Pet registered successfully.', N'0A86901916C93AE23881E8805781525C164A04599209522B7B43CD0F434E82317')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1993, 2, 239, N'Translation of ''{0}'' saved for ''{1}''.', N'6FECEA0C8592AC62AC94E9A2763D4C96121BDE9B8BF38280B904417BFD8A2AB8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1994, 2, 240, N'User added successfully.', N'3C4FF22B670469F24B4FBBB96D51F71EDB3E35618F6151DA43ADEFB386693CF1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1995, 2, 241, N'The user has been successfully unblocked.', N'08514A52AFD2DD7FE17465719C20935AFAA051FBCEE102A072F568FBA2A888859')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (1996, 2, 242, N'User modified successfully.', N'0DCE8B33BAAE29EFC6FDD0E43B16E0C1C5C38D341F46C72E7DFA984742C53C710')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2239, 1, 243, N'Hubo un alta no registrada.', N'0FDE6D0E6A10F86DBBFB6CB8A889F56E8CADAE37A0B5FFEAED5E13FFAF7332EAE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2240, 1, 244, N'Hubo una eliminación no registrada.', N'0A093976D8EEA31AFC3B144BE6A11A29A8709F781E4DB5CD909E3C233F2DEF25B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2241, 1, 245, N'Hubo una modificación en el registro.', N'0DA9A9F01C36DC0926516021542E48B3D26CA5BC01D0E7B1B67E7A8E8B114781')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2242, 1, 246, N'Falla de integridad estructural.', N'08E194C0F4B2338F4DB7F5129C88BCA7134FFB5EA4396444F4CF8207C3B495D32')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2243, 2, 243, N'There was an unregistered insertion.', N'0E9051763CA5C59BD54C43EB4FF1CCFF01FA7A3A4DC2C5D94A8D0B63625B0A6BA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2244, 2, 244, N'There was an unregistered deletion.', N'0A041EBF956F107EB847A9442D7D41DD92D3881DCFDFC038EC9BEA005E66B422B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2245, 2, 245, N'There was a modification in the record.', N'7CFADC912322C03B3ACA45922A781EDF16BD70BF24AEA314FB585290866BE1F5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2246, 2, 246, N'Structural integrity failure.', N'185CA7AB2180D6A9AFF179EFEEA279D3789EB24B68DA269BF3501C681347875C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2745, 1, 403, N'Panel de Administrador', N'0AF0B6CF8D19F327ED2FFA10404642DCDD20E5D861FA03CCD2175DB9DD223EEFE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2746, 1, 402, N'Menú', N'0BE62220B812D16FCD29663865C3080086DD99AFC8DD43FE81BFEB3D0AE192B68')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2747, 1, 399, N'Gestión Usuarios', N'0FB213E78AEC15B17C1E14E3C7850DD2666D6216281DD8DCA09A70C041ACC0BBC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2748, 1, 405, N'Bitácora eventos', N'0F475502734DF2052ECEFCB7C7B6609EA96DC3DB2447343C4B3DB6263C81BF718')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2749, 1, 398, N'Gestión Perfiles', N'0C34124FFE8661A39ABE0E83125D48470C0561DB0DF7E63574FE5D1E381BADDF9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2750, 1, 397, N'Gestión Familias', N'0C96B3929BF27D142073C70BE0B463D636116C729318458240F43E99AB9E48CF4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2751, 1, 404, N'Backup/Restore', N'54D9E02CBE270BDD1C8D7C3508F5B062E896D7F3618F668BCD8936EFCB5D9142')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2752, 1, 406, N'Gestión Idiomas', N'4BBDD148EC5C2E9B46E501827CD833898CFDDAB63C3D993660B46673956641F0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2753, 1, 401, N'Gestión de Familias', N'1C3E66FFC4E3D87E62AA49F2AB6D479EA6FBDA9918F04C8C3E36F96F9FF40AA0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2754, 1, 396, N'Familia base', N'4A5BE908269279A570BC29692D723F1213A9C3BD9020FFAE1B852D54296F43D8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2755, 1, 395, N'Familia a agregar', N'09D518FA724C45F9BDD38A5D5995BA0A5C98B814E7CA75F8801A361B6C198E7D5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2756, 1, 400, N'Permisos', N'0FE0CA692036A1A6824D4186958A030DC53CC38DD457AAA68409ED73D6924700E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2757, 1, 392, N'Asignar permiso', N'0FF3FA48E9D4B258B7C7B8EBA0540A1A35EC9F2FE43D4BE73BF77F21C7B03593F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2758, 1, 391, N'Asignar familia', N'08327542E574CCD034D0F35B9CD966DA3D403D23A50BB8D42474518B8E99ECD4B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2759, 1, 394, N'Eliminar familia', N'09734821E575115D6F4C6D89E8160F20835AF6F413E0F419CB3B0093B91C9E1D2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2760, 1, 393, N'Eliminar componente', N'78875B2FBABA8540AFDAB815A892C6744A938D52781085F19C7C600DA15E052F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2761, 1, 390, N'Agregar familia', N'0B5B1D09B7244754CBAA6AF176F3426BF15221568621D2B1AB2CD7DFCEF7272AF')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2762, 1, 412, N'Nombre de familia', N'0D7B38804CBA556CCCF480250858935740A27B41D3392039FA98230D99A1336C4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2763, 1, 411, N'Permiso asignado correctamente.', N'5A2F786CFC8237EFC86C425DD3F1AC8B8F9165CECA076494F4BDBA71EFB8165')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2764, 1, 407, N'Familia asignada correctamente.', N'6839F8BB8FDF289A078D3A5B4E8A4DF66AD2165E9AEEB6E7DAF04EF1236BBC99')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2765, 1, 410, N'Familia eliminada correctamente.', N'2069E97E0DE92744B1B8318ACA5FA839F8A0BF1ACB4F245134A5307DBC91D8E8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2766, 1, 408, N'Componente eliminado correctamente.', N'0C936D107C2E381695F45C3837755896EC3D758FCE52EA6381721E2523D7CF54D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2767, 1, 409, N'Familia creada correctamente.', N'0E0061DF993126A578407C93D67015364C5F896214BDF0680D38E83718908C2DC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2768, 1, 425, N'Panel de Administrador', N'7C9457B130A0E2AB5F0B6E42BC8F5940A9AA5D4FFE5643730E55A843EFCA58AA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2769, 1, 424, N'Menú', N'09774A4EC197EFA8EB55670C058BD10CC05D8FB32FFFCA0359DE14DAF338CE342')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2770, 1, 421, N'Gestión Usuarios', N'608C9DA87B9A50CEE883470AA9BC9FD1A8A1A894F6D908AD01CCACDA3E0A1C5F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2771, 1, 428, N'Bitácora eventos', N'09AB37CAD63125B7CA697952E54E7CFD391F2A75E495EBF47184885CD0E6E3FCA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2772, 1, 420, N'Gestión Perfiles', N'10749686C9C20CD23BB9060D0FDB383D9871D56425EB2E722846280BEE4980F6')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2773, 1, 419, N'Gestión Familias', N'0CBA87C45417684E32BDE510031AEC152974FE1E10C7245747DCFD7D9A9F4E86')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2774, 1, 427, N'Backup/Restore', N'170667A12748AADA50B6D4E9DF385DADC054A32CB4CA45D0C6142340F3BA3A7A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2775, 1, 429, N'Gestión Idiomas', N'5FFFF81919D7F157859C7F0C9320D093C56D815664353FC438DA408B543FADE8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2776, 1, 426, N'Gestión de Perfiles', N'0B2AC1B19E9D7F1423B862A5438CA70BB1937AC0A70E4543AEA1F116BA204A3DC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2777, 1, 422, N'Perfiles', N'09867C8D25BDEF790AFD22D6EE19EA50046F797F69332CFD3AD7EC57E34930652')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2778, 1, 418, N'Familias', N'382279241F32017CE580EB76E9A19E4F642E10B032497678990EEDD977AB968A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2779, 1, 423, N'Permisos', N'0F071231B7A8A72B7AB57624571C42EF0B168257388EE7EAC2517697602F51626')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2780, 1, 415, N'Asignar permiso', N'6C941CE58DD1234AA04B09288E7794D76EA5B5B6C569CB10AB2087B8F7AFC574')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2781, 1, 414, N'Asignar familia', N'0EB18752AD6D96526D0D09E000779789C93DF6C6BEEDBC5B45E4AAE1773D35053')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2782, 1, 417, N'Eliminar perfil', N'08F0DA254289F19996304F342953FA480A3D1F437943DB08EC7873A90A091522E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2783, 1, 416, N'Eliminar componente', N'0B5098EBFA84E370CB9C1E6EB3F6FFAEE12DFA00966F2A4DBAAC4BB59710D1725')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2784, 1, 413, N'Agregar perfil', N'4C589B44EB775803155029189672390B4E344B4726E6B5D81C81189C602CCB54')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2785, 1, 435, N'Nombre de perfil', N'7B7665B4F02BD24D2C27EE8E0600C410803664CA23F568F8D3F28E93EA3FA25D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2786, 1, 434, N'Permiso asignado correctamente.', N'7F8522ECF6CEC895D062E47B6F45E7BDAA84BE655E1B3FC2B65C9F0256F1C218')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2787, 1, 433, N'Familia asignada correctamente.', N'58078D2E7BB61FB28A5BB8C1FF62164641028F3AEB1E1ECFC0FE42D2864717DA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2788, 1, 432, N'Perfil eliminado correctamente.', N'0EB4724A8C2617FD3AFF998ECD62B798AAC17CF907CCC2124E3A1AD36B565DEEC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2789, 1, 430, N'Componente eliminado correctamente.', N'093FDAEDCC60B3AA9C3EBB4EB3E09B9FEB9B31E9B7CE8C2CB78CF8C1E5F53479A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2790, 1, 431, N'Perfil agregado correctamente.', N'3397D2A2D6C20E978E5C38C64E812AEFC728D5511301FA5AA9840A6EF24D0281')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2791, 1, 376, N'Debe seleccionar una familia base.', N'08DD3F25B066C963FB50514AAD58DAB5785654AF927FD36279F52156FC9ADC2F0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2792, 1, 380, N'Debe seleccionar un permiso.', N'2EA8164BC2AFA6AB4839C73402AC5BEAA9548249C6DAFA994F343E3FA146D2CC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2793, 1, 379, N'Debe seleccionar una familia, no un permiso.', N'0831A80CB6F9AF0BA7E6E6D9AC6ACC5D763E8397E3EBCD2A540732A0E39AF3B46')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2794, 1, 375, N'Debe seleccionar la familia que desea agregar.', N'662578C1058975E9F55034805CE2B7E66096B3946AC41982D5BA078E51EA6F8B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2795, 1, 377, N'Debe seleccionar una familia como familia base.', N'4DC3044ECBD51280D8474634A721B5C77BB0AA05B40BF58E1D6D16FC7DC79523')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2796, 1, 374, N'Debe seleccionar una familia.', N'0DB3B7A1A79A2AB4D461306B917AA2A079BA937AA28766116F88E1D454E54FB3A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2797, 1, 378, N'Debe seleccionar un componente.', N'55027B011BEF8EDA5A0706FF419C1FF98893915020F90918213A1C2FEE53D8BA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2798, 1, 373, N'La familia seleccionada es una familia raíz. Utilice ''Eliminar familia'' si desea eliminarla completamente.', N'19AE4E8D00D01D26D3C0BEC1E046270C3A9C569D834521F7EF28ECB08A245E90')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2799, 1, 382, N'Debe seleccionar un perfil.', N'0A7583195CFA89B2C3DB79390A5A457E0B0F023B672281C86C1235B333DD4D581')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2800, 1, 386, N'Debe seleccionar un permiso.', N'4E4021774BCF2E5F5D0EB0D07F1F51D40E81DC56A9E5C8BD6853C521E423ACF7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2801, 1, 388, N'Debe seleccionar el perfil principal, no uno de sus componentes.', N'0C520C598C827A9FDB0AAE0B5ADB9C69E3CB1E94667728703F142188ABCD82610')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2802, 1, 385, N'Debe seleccionar una familia.', N'0CC467502FAAB157177813944ED2B027E888A43C2CE58D076BCB56A89EA69E163')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2803, 1, 387, N'Debe seleccionar el perfil principal.', N'30CE31760109A8EE514EF24553165FF459F0A57169E55932523AB5850995D4FA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2804, 1, 389, N'Debe seleccionar el perfil principal que desea eliminar.', N'0AEC6314D1C1D56D744245226A8FB37C6B59AD45C03B595A35F3B9D71A2591722')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2805, 1, 383, N'Debe seleccionar un componente.', N'0ACF625FABE9723EDFE34F667BE2CB68E16144F066FEBF01B6DED7BB9499737C4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2806, 1, 381, N'Ese permiso pertenece a una familia. Debe modificarlo desde Gestión de Familias.', N'0B2E8AB9EC26CDFCFDC1D3A515AB9CA14DE2FAB04FE409B4C2D30A3E67EA18357')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2807, 1, 384, N'Seleccione un permiso o una familia del perfil.', N'6045BE88873C57B0F9BD10CA1198C12825896408F0E8B11991CBBD95F100B9F7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2808, 2, 403, N'Administrator Panel', N'55EAD15DD627841D793C91DA1B40095D584F13796DABFCDD87221B3D7A562D34')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2809, 2, 402, N'Menu', N'5FB245B4CFAB5813E85F5DEA702AC7BD8A917BD7D733345511A82F8E928F139C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2810, 2, 399, N'User Management', N'23254159D277FA7CEAB607D9F0F95DDB8C8F8DA69306B8D21835D535D1B5DC89')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2811, 2, 405, N'Event Log', N'3194AA0C4E9F7ECFC65B6E578583BE9A467DE421F84085EA88B264585AF02E4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2812, 2, 398, N'Profile Management', N'1D308B76BD054C23B5AC7D24DF6325F14D9A64DF9516D0D5707A26DEF7E4A84C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2813, 2, 397, N'Family Management', N'5D1A98DCA90DF8901FC0214EE141394463BED71B80F8120A3FD00FCD41711F72')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2814, 2, 404, N'Backup/Restore', N'15E3B9CE490BA64CCE62B8DE357F309F42772F08DE646C891AE9C2D533D9910')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2815, 2, 406, N'Language Management', N'099531696D8DFFBFDCB4ADA2F7B17D89BC0100C99AC2C7786FA2C79C36F542163')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2816, 2, 401, N'Family Management', N'082370BE0F9B142EB9BF0297DB205D6F01F812A13A1B391363BE838CE276BB594')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2817, 2, 396, N'Base family', N'7A3FE471D44BAACA55473183B9D2C9BCE18D73BB6BEB60D6BAA2041D27F87E48')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2818, 2, 395, N'Family to add', N'22DBB162752C14A9B0F414C87D7BF82C32B0A7FB32ABB9713EC39B964356C350')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2819, 2, 400, N'Permissions', N'28747CF4892E3E433F720F41F27DCB127C858BA26755EA5F9FFCDD460C696951')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2820, 2, 392, N'Assign permission', N'0CC3A46716C3236B5BE7D14F6511A9B8D0C11D0C611261AFBC159808535D203D9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2821, 2, 391, N'Assign family', N'6E3561886620E0D66E024CA26CC16901A72D3760805B8064C1FC395FD1583F10')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2822, 2, 394, N'Delete family', N'0BFD2F1AD787FE1FC461B629EA300B8CC47FD5E731916E6B32F56673AFDA771F3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2823, 2, 393, N'Delete component', N'1B6B3BCC7B80C152187586D5DE81699D912ED771E8484CB3593AA6A1857FC50B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2824, 2, 390, N'Add family', N'09B08A458CF9EC70C01BF4BF07CD18450D9D9D2BABBC26A890EFDECB3265048DA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2825, 2, 412, N'Family name', N'0B093C3FEF25B0E5A4639A11B866FB6AD04D15205ED2AF33211D4D9F7BD83F42A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2826, 2, 411, N'Permission assigned successfully.', N'676D4C9FFBAF8CE3CB7DBCF46FD76A631E019048D91DC60B50490086D692D1DA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2827, 2, 407, N'Family assigned successfully.', N'49415C3A3AE11844A6E1CF8F510EB16920B521601837CBD4D382F8B8884A94CB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2828, 2, 410, N'Family deleted successfully.', N'0DE7019F29F9433AC2EB6FB656F9E8620FCFE64A554D6E27431178CBB63DE86F3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2829, 2, 408, N'Component deleted successfully.', N'40BCE542D7FD19E696A0DFD4F4B0A3BA56A68ABC726DC447654BDD4CE76EFAE0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2830, 2, 409, N'Family created successfully.', N'674A151244DDFDA3937762370D6161BBEDF7593DB4D52369752E45CD22BCDA4E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2831, 2, 425, N'Administrator Panel', N'0DAE621230B9B78B839784911ADC9F4428035D2D5AF26E30FC903088059A81B20')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2832, 2, 424, N'Menu', N'0FA44E5C9A2406B4901153C5A5BBC36EAE492838CCC1EF69435983C97C6964B12')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2833, 2, 421, N'User Management', N'0D99F5D805B73A1B93497CCF3F66F7CEEBF066A80D6B4FF2FAED865021F16A2EB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2834, 2, 428, N'Event Log', N'3EEC508987CEDB4E60A24304DAEC8A1F2FD4E37007626B1D09A9FE7520FD379E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2835, 2, 420, N'Profile Management', N'1BDB3BE039B7F0BEE4688231F10A2ADE46C7FBCA64C529C37AD86ECDCBBF08A5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2836, 2, 419, N'Family Management', N'0D8EC12DA11868DF5BD0977DADAD1805FB86E49EEE6786991F4A8E4FE072AEE20')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2837, 2, 427, N'Backup/Restore', N'0EAD21C2082CCF77183018FF8CD3C9DB6CC83A9EDAD92DF1CDCB681540CA674D1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2838, 2, 429, N'Language Management', N'60F3CFF0A93A1230892111CD379F58553AB4FB41FB946BF4CB39A47994ACCEEC')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2839, 2, 426, N'Profile Management', N'0FE1DCA2F889C037CBC75F440B84F9FB55A1F2E965654DF9266A9BBC409BC4B71')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2840, 2, 422, N'Profiles', N'08F517C7EA960A8979870A83E4F0C60AAE73232124CE644CA1A128E6DA93F183A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2841, 2, 418, N'Families', N'350A12BDEAF2E93A9A6343A10259B37B3EB454992EC94A8F1139DE13A9471A8C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2842, 2, 423, N'Permissions', N'0DCB788536F92C194302A08F8CB4832F0A314AC674CA8C865A80C7B896FAB06F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2843, 2, 415, N'Assign permission', N'7BFC81904F39D37A99F971CC7FC39AC261F96731BCF88DE97F6AB42FAF772F8A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2844, 2, 414, N'Assign family', N'22889E678935F825011876F4644F9B6870750CF82538952CC33B15A3BB9A92AB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2845, 2, 417, N'Delete profile', N'0F16829B1D370600ED4B32D3C3175820EDC9C112747DDB7ECB99FEEAB5F9F2041')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2846, 2, 416, N'Delete component', N'08F51F9194318EEC0FA865DADD2F84AFD670B091D7F1927DCF537CF67B97FDB6A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2847, 2, 413, N'Add profile', N'1B573EF82BA9C06D2E4DBDF5C0A73FB612A48E8BC32AF74D2D0CF02BB4CED582')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2848, 2, 435, N'Profile name', N'1AD7B64C4401204C4141A6F7F632639357340ACDF021B7919D84B662A2081B84')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2849, 2, 434, N'Permission assigned successfully.', N'6FCA7BDC88DDE4A170FFBF22760571824D5D4CB7B661F83F05CD9105049FDE63')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2850, 2, 433, N'Family assigned successfully.', N'1B002092C8436A5AF15262978617A8D07D8F0F98CBD6736F860262BA506030AD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2851, 2, 432, N'Profile deleted successfully.', N'0852DB7B6267560C58006F2817B6E2CE25D8C890D37BB319576C146F3D275D8D8')
GO
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2852, 2, 430, N'Component deleted successfully.', N'088D4AF3209798587BFD369B377C838D2960CE189F506B63F52E0C95E8B1E3E1D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2853, 2, 431, N'Profile added successfully.', N'43DC96906AD5A9E8AAB9E40074F8C06339E648861C9CCFE084E0F7AD6C6E6A3D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2854, 2, 376, N'You must select a base family.', N'2AA0601D4DFCAC324D5F96A63A54A3803824950D5D2A11F0439B042E4BD2BD67')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2855, 2, 380, N'You must select a permission.', N'2FFDEAF5706B37580122838296C4CA95F9F06BCA887CA44FA4A956E94F6F134A')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2856, 2, 379, N'You must select a family, not a permission.', N'0A8012AD6E6CAB9CC28A1788CE8295DA56979DE7EAAC7D8758AE840005D789C10')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2857, 2, 375, N'You must select the family to add.', N'464630B99C767043FB7F3F7E2DB5FE2B558093736100054999BAE08A66BDF818')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2858, 2, 377, N'You must select a family as the base family.', N'0A4C3B886FBADDD3CDFA37B654C3583CC7C5C3EDE11D025AC618688BD0268A4AE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2859, 2, 374, N'You must select a family.', N'09E1ABACB600091C7B288C3F2B0931C3E97372075D6808A2179520E37C9930D5B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2860, 2, 378, N'You must select a component.', N'12BFB5886F393BC90FD6E5F6AA4053F8981D75432D17887AC436B21E38F444A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2861, 2, 373, N'The selected family is a root family. Use ''Delete family'' if you want to remove it entirely.', N'13CC9B46712DC3931E929A274158D02AA6938A43B188F567DCC9B5737CF88B00')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2862, 2, 382, N'You must select a profile.', N'0C9936B86274557D8A02BD83105352865DD7BE1062F6861715A7EC3AF2F351858')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2863, 2, 386, N'You must select a permission.', N'0B5DC15A0C676E368F4AC96CBAF58433854DA1000279A857F944B60C152375552')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2864, 2, 388, N'You must select the main profile, not one of its components.', N'0CAF076E82C7E2F254F4D0B38E20A009D91705C01AA9C0B467DFE7BD62C7A1CDE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2865, 2, 385, N'You must select a family.', N'0D1D5410E1F228582C12FA8225F6A5E400D3F38B7D3C93F0D9A87B54E817F2232')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2866, 2, 387, N'You must select the main profile.', N'085DBEC1E546A1690A95DC735FAF83752025B2C279543E7349EF93A7E0EEFF1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2867, 2, 389, N'You must select the main profile to delete.', N'1EAE03C6E8F23D390F05B52C05A135179DB46057B877298761FCEE96D7B31962')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2868, 2, 383, N'You must select a component.', N'0FE07D88300AC76B1D5373519AE69A67C9223AF37569A5A765658BDAE016706FE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2869, 2, 381, N'That permission belongs to a family. You must change it from Family Management.', N'0D893093FE5AC72BA8158A47F92EDD0EA7BC09780575F6786F7EE9E43654F5171')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2870, 2, 384, N'Select a permission or a family of the profile.', N'0E7681DC8FF8839EC10A6990E7F619CC4EE837C152F51095FD63014CDD44441D9')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2971, 1, 506, N'El perfil no es válido.', N'0D42F9070A69EEBF5E72502C1B821F2250B1801F8062381928D42D04F19F0D3DB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2972, 1, 509, N'Debe ingresar un nombre para el perfil.', N'22512F5404FD7C64BDD4297C9AB9CF46172923EE548EF591BCE9E4911D8DF1')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2973, 1, 508, N'Ya existe un perfil con ese nombre.', N'0A602C38CE955FFA5C6FEDAA91E290331C23F07EA6F9A4D347AF147F286E680D0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2974, 1, 507, N'El perfil seleccionado no existe.', N'08AADBE7BB1EC603FAD71E36E27DC83CB1B838DD1413A7F467D31EA4AED1D18E5')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2975, 1, 510, N'No se puede asignar el permiso ''{0}'' porque el perfil ''{1}'' ya lo posee directamente o mediante una familia.', N'0C56A08000661445228EF281C9FBD158563437C34FD90753C6D581A3DC37F6AD2')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2976, 1, 504, N'La familia seleccionada no existe.', N'21EF721EBF1F9545F75EF8D190F8D2C4203FA6EBBE7B78995D047C9A1C0947AB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2977, 1, 503, N'La familia ''{0}'' ya está asignada al perfil ''{1}''.', N'0F5D89BE66E59B3183DF4838C6B090081B3709050FD7B8B896594B5633412B824')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2978, 1, 505, N'No se puede asignar la familia ''{0}'' al perfil ''{1}'' porque ambos contienen el permiso ''{2}''.', N'475018DFBCA85FEA6FD40F90208BE903A8A1FED05FB2EF764E660B87933EAC3F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2979, 1, 501, N'El componente seleccionado no es válido.', N'0B2EC11C1662D9B0163CCE3480AF038866336646674CA14C04738099F6860EB8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2980, 1, 502, N'No se puede eliminar el perfil ''{0}'' porque está asignado a uno o más usuarios.', N'46B21E3E74E561F8764F32FC8B11821A306811AF83F37DABA2DC057B3E302CD0')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2981, 1, 491, N'La familia no es válida.', N'0C3EAD7B1A148ED4C475C8F4059DD89A67B6D53D39EE175B80C88CFC860238F63')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2982, 1, 494, N'Debe ingresar un nombre para la familia.', N'08696737D779AC5BA9B3B7F32B0BF5B7DFF0CC0F38B07DC4A2B45C283B6CD9C4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2983, 1, 493, N'Ya existe una familia con ese nombre.', N'0CF117A9C9E039AD9EDA03445C27FD27E63A54BDED064C5B28D9B93990890412B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2984, 1, 492, N'La familia seleccionada no existe.', N'0CBB2F58D71046420A547CBD5A492228C623D8C05D754FC69A2B60D456F883BA4')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2985, 1, 496, N'No se puede asignar el permiso ''{0}'' porque ya existe dentro de la familia ''{1}'' o en alguna de sus familias hijas.', N'11381FBC3EC3927A36C4CBA1116EDE885FB880E80E5293A3C3D49DCA205D2E90')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2986, 1, 495, N'No se puede asignar el permiso ''{0}'' a la familia ''{1}'' porque ya existe en una familia superior: ''{2}''.', N'0D2DA4023AB483F6C5AE0F2CFD6E2FFAB4C1ED58DBDF047D5954D582E4CB725FA')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2987, 1, 490, N'El componente seleccionado no es válido.', N'0C0A74D1E5F453B1FB5B7CF5D76DC5922271C034A08C8E8F6FA0487126D00194B')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2988, 1, 499, N'Debe seleccionar ambas familias.', N'0B9B0D0F51E382747AE03B568004B82FA5A7F5DAC6DBA8B4168B2B631BC475AC7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2989, 1, 488, N'La familia base no existe.', N'0E9DCCBF2E8B87A7BADB0F27F9A1C67A20C2D1D0C909B9BDA654F5A55D966EC3')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2990, 1, 486, N'La familia a agregar no existe.', N'0FCA8B95141DE6C85C7DC79B4860F440DEF68BF7A3BCBC78C075856422D40213C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2991, 1, 487, N'Una familia no puede agregarse a sí misma.', N'0BFF3573DB0C29DDA897BE5596C3A2450D346F3B14A3DBD3AF7D52F4271B76A19')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2992, 1, 500, N'La familia ''{0}'' ya pertenece a la familia ''{1}''.', N'2F2427E3B4AEFFB591162F4A789D2009DF2ECCD338B4420CC05980F525EE0176')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2993, 1, 489, N'No se puede agregar la familia ''{0}'' dentro de ''{1}'' porque se generaría un ciclo.', N'0D7843FE41035DE67B83F3DBA3882872A85D9D1DCCA17F2CDFEC157BF12D60444')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2994, 1, 497, N'No se puede agregar la familia ''{0}'' dentro de ''{1}'' porque ambas contienen el permiso ''{2}''.', N'0999CFCC3D2A0A9770958AFE3CB3D657B220CF53436ACB41601984D362EE41FCE')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2995, 1, 498, N'La relación seleccionada no es válida.', N'092B35F680A15C64E56901729B8EDF9E769AFEDBD64E35236E5D43D1686A20433')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2996, 2, 506, N'The profile is not valid.', N'0F8EB25BEC06DE65E54095BAE297B3DD5844FAA85AC91934BBE0C12C7CAFB613F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2997, 2, 509, N'You must enter a name for the profile.', N'0B8FADB25566AE9AAED27F9F24C57D5BD6C4F8E58053912B513C6E36F13B0539')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2998, 2, 508, N'A profile with that name already exists.', N'5BF09BBFEFFB33D50E9E7C3003E572D70FFF33DCA7339F8052D2087464B14EDD')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (2999, 2, 507, N'The selected profile does not exist.', N'0E81A07CF0B24EBBBD76F608DDA9B0DEDDCE07B74FD79CD71E5E019BB60ED5393')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3000, 2, 510, N'The permission ''{0}'' cannot be assigned because the profile ''{1}'' already has it directly or through a family.', N'0BC3DA0045049A834482F926B276BBA3FBEEAC3D89E73030F09EAA07547707DC8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3001, 2, 504, N'The selected family does not exist.', N'52B4FD942713F2670D8E872C87ACB278690207BCD1103CE40A079622751FFC97')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3002, 2, 503, N'The family ''{0}'' is already assigned to the profile ''{1}''.', N'19D7F62CFF42CDA707741C8253561F557B14FC3767272CF1DB482A208D70247C')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3003, 2, 505, N'The family ''{0}'' cannot be assigned to the profile ''{1}'' because both contain the permission ''{2}''.', N'0E8F29715B72F63CA2D3959515693A18BC2ABC5F6C0F3359DD8C0A26C7CBB8C3E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3004, 2, 501, N'The selected component is not valid.', N'0C47DA1BAEC2A2E36A360957BCC64757617EAA0467D4E6D753C340A6CDC752320')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3005, 2, 502, N'The profile ''{0}'' cannot be deleted because it is assigned to one or more users.', N'0AD2B7C3F5008868879026A904E4B123AD125C8DD1691D9F68C02FF28A2449EE8')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3006, 2, 491, N'The family is not valid.', N'0A890D3DE4D3118F4FA4444BDA24D2FDF093F665BAE0D52114799244AD0167903')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3007, 2, 494, N'You must enter a name for the family.', N'1987862880E543A3A030CB6D9F0B037E4E694D08DFD11146DAE076F09EF0B959')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3008, 2, 493, N'A family with that name already exists.', N'418C241F4BBCA6F0A864AF3C9F3CC8B6D31C75EEC2BE2FFB2336B0FB9F573866')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3009, 2, 492, N'The selected family does not exist.', N'0F26D9832CC6761DE25B8BA56600A8FAB06492832C43052AF6AB8042B9C0FA948')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3010, 2, 496, N'The permission ''{0}'' cannot be assigned because it already exists within the family ''{1}'' or one of its child families.', N'0F3A6633DB14140FEBEE9C2F45242735CD6BECCDF20A098E974447BF9B6118506')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3011, 2, 495, N'The permission ''{0}'' cannot be assigned to the family ''{1}'' because it already exists in a parent family: ''{2}''.', N'0D289D738FE632F103A884D19F166866737E3B3C451BB4DEBAD8BA0B1244FA026')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3012, 2, 490, N'The selected component is not valid.', N'0DEB88DF22F41833AB8A34CD7ADBBAFFA7A1AB1154D0E98E531A8CE378D27E23')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3013, 2, 499, N'You must select both families.', N'6B4E96966F7D41E4E2EA1F4C0C71B2D45AE01D0314B82629BFDCBB3BC93BBA04')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3014, 2, 488, N'The base family does not exist.', N'0C9B25E02B6EA0498D644FFBDC803BE3736836B5B88D0258ECCC711CF6B751630')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3015, 2, 486, N'The family to add does not exist.', N'392D6CD35C965B8DC2AA438D28F8A64C1D38BCBCB366AB1592B6763F61476C37')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3016, 2, 487, N'A family cannot be added to itself.', N'2E50D962AADDD9FABCF8B21A8A534AE1E61BEBDED632485A2081B17F0B483697')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3017, 2, 500, N'The family ''{0}'' already belongs to the family ''{1}''.', N'0ECA97D5FDBDB42D1BF6BF7F0B7BB157A5FB91C7EB451B68CA3E616AF2D1E4911')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3018, 2, 489, N'The family ''{0}'' cannot be added inside ''{1}'' because it would create a cycle.', N'0D5D73CA4A8D29DDF9B5AD8EE00572F92EB3D51CB78D47C8B688FCC62478D97EB')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3019, 2, 497, N'The family ''{0}'' cannot be added inside ''{1}'' because both contain the permission ''{2}''.', N'0D882FEFC08C0A1B98C7AAAE713CB89DFCC4E6E3B698FC3B85003C636440D263E')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3020, 2, 498, N'The selected relationship is not valid.', N'09B4210411EB5F119694EDF2068C7CCBCD68BBF34B2555A9380592CB115B6D724')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3027, 1, 514, N'Gestión Idiomas', N'0E86A609506E88BAD91A408959D074958B185A90760561BA249E6961B44C1444D')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3028, 1, 516, N'Gestión Perfiles', N'3F388C869FA5FB86DFC32BE2EA5F58A9CD55393816ECF35800556222FBA4221F')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3029, 1, 515, N'Gestión Familias', N'0EE499E0F33A8CBE8D11A39F871FC840009EDEDD4EA9AFD67D52ED35DC6D084A7')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3030, 2, 514, N'Language Management', N'09E31F4649124572B1D2274CB6ACB295D1DC4CC9ACAA589AC9EBD78C10A4C4909')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3031, 2, 516, N'Profile Management', N'72E0C345A2D2B5C7FEC518FF0098813E3DACB8E655621093E07503567636AE83')
INSERT [dbo].[Traduccion] ([IdTraduccion], [IdIdioma], [IdEtiqueta], [Texto], [DV_Horizontal]) VALUES (3032, 2, 515, N'Family Management', N'0FE933B3DEF83845168C0A5DC2385BEAC7ECC1A1A0E8F39688CB5702D91371567')
SET IDENTITY_INSERT [dbo].[Traduccion] OFF
GO
INSERT [dbo].[Usuario] ([dni], [nombre], [apellido], [usuario], [contraseña], [intento], [bloqueado], [mail], [rol_viejo], [idioma], [DV_Horizontal], [rol]) VALUES (N'12345678', N'J7rXjkPaH9E0P51XCFcqJg==', N'CLcn0skFWVvmDz7kfzVJFw==', N'J7rXjkPaH9E0P51XCFcqJg==', N'59adce8f418147ce4a0006eae33755e2229f522de79d4d372df4512a792b6b32', 0, 0, N'gscpyuW/hmCLuuHHwHiIljmIM0u5jlkE81jfmQHqjO8=', N'Cliente', N'es-AR', N'0C4E932038C458C3DED834B6019E805C94B7046D17D94B19D0B3D1A0C71339EF0', 2)
INSERT [dbo].[Usuario] ([dni], [nombre], [apellido], [usuario], [contraseña], [intento], [bloqueado], [mail], [rol_viejo], [idioma], [DV_Horizontal], [rol]) VALUES (N'45823327', N'f9bRhwRJjrQFaseJ5/cn9w==', N'uox8FY5Ig4ydj+vh0KnKZA==', N'oNBZjp19NEZIBBeQ1Z1vog==', N'91c214f1a754f35b3195a85a89db40ad9b7123efc6843c6fbe49ea412c8b8cbd', 0, 0, N'gW5OA9w7A59LTOugGH4zGRkrb6ArUghGEkjmL4z5qVI=', N'Web Master', N'en-US', N'781213AA263608F5903AFF2752E89F77DADD361403706C2C4A43461D8760DDB2', 3)
INSERT [dbo].[Usuario] ([dni], [nombre], [apellido], [usuario], [contraseña], [intento], [bloqueado], [mail], [rol_viejo], [idioma], [DV_Horizontal], [rol]) VALUES (N'46030516', N'hyQxewN5YVD/aMhdhxKAzQ==', N'8D1gdOjb1nlc02BcDw98Lg==', N'2pLWMqGcuFCeG93WyQppNQ==', N'b678d4758e2e820ea88498aa7a2d45bbbd11f5d2a1f05bde9c44fc88de8f7652', 0, 0, N'V4MTOzhvs8mnNSuZbNFxO9AtLHCmZctvMK20+FM5jbg=', N'Admin', N'en-US', N'0E8D220A714A677022777D9E887B6200BAFF94CB39D4134D49CF99A23D893FD8A', 1)
INSERT [dbo].[Usuario] ([dni], [nombre], [apellido], [usuario], [contraseña], [intento], [bloqueado], [mail], [rol_viejo], [idioma], [DV_Horizontal], [rol]) VALUES (N'46208842', N'RZg6QJvLzYQc2qPqQK4+jQ==', N'Hk61rFx2yT40Sj+sWA8YzA==', N'6VR4hrDKEJUdn2T2NUqPZA==', N'1626173001ce87be578a1694838f96363ab1529ec0774eee7a092e93ec05532f', 0, 0, N'AKKOl4pVtmKkVeqKlB88XR3L9s0mdwCwSHX1zvy7byc=', N'Cliente', N'es-AR', N'0CEDE535E77980C3D0B1BF541477202700A3EF69DDC085233F929BD815635574B', 2)
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ_Formulario_Control] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Etiqueta] ADD  CONSTRAINT [UQ_Formulario_Control] UNIQUE NONCLUSTERED 
(
	[Formulario] ASC,
	[ControlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ_Familia_nombre] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Familia] ADD  CONSTRAINT [UQ_Familia_nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ__Idioma__06370DAC13DF4E25] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Idioma] ADD UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ_Permiso_nombre] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Permiso] ADD  CONSTRAINT [UQ_Permiso_nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ_Rol_nombre] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [UQ_Rol_nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [UQ_Idioma_Etiqueta] Fecha de script: 4/10/2026 22:56:28 ******/
ALTER TABLE [dbo].[Traduccion] ADD  CONSTRAINT [UQ_Idioma_Etiqueta] UNIQUE NONCLUSTERED 
(
	[IdIdioma] ASC,
	[IdEtiqueta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DFUsuariointento4AB81AF0]  DEFAULT ((0)) FOR [intento]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DFUsuariobloquea4BAC3F29]  DEFAULT ((0)) FOR [bloqueado]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_idioma]  DEFAULT ('es-AR') FOR [idioma]
GO
ALTER TABLE [dbo].[FamiliaFamilia]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaFamilia_Hija] FOREIGN KEY([id_familia_hija])
REFERENCES [dbo].[Familia] ([id_familia])
GO
ALTER TABLE [dbo].[FamiliaFamilia] CHECK CONSTRAINT [FK_FamiliaFamilia_Hija]
GO
ALTER TABLE [dbo].[FamiliaFamilia]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaFamilia_Padre] FOREIGN KEY([id_familia_padre])
REFERENCES [dbo].[Familia] ([id_familia])
GO
ALTER TABLE [dbo].[FamiliaFamilia] CHECK CONSTRAINT [FK_FamiliaFamilia_Padre]
GO
ALTER TABLE [dbo].[FamiliaPermiso]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaPermiso_Familia] FOREIGN KEY([id_familia])
REFERENCES [dbo].[Familia] ([id_familia])
GO
ALTER TABLE [dbo].[FamiliaPermiso] CHECK CONSTRAINT [FK_FamiliaPermiso_Familia]
GO
ALTER TABLE [dbo].[FamiliaPermiso]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaPermiso_Permiso] FOREIGN KEY([id_permiso])
REFERENCES [dbo].[Permiso] ([id_permiso])
GO
ALTER TABLE [dbo].[FamiliaPermiso] CHECK CONSTRAINT [FK_FamiliaPermiso_Permiso]
GO
ALTER TABLE [dbo].[Mascota]  WITH CHECK ADD  CONSTRAINT [FK_Mascota_Usuario] FOREIGN KEY([dni])
REFERENCES [dbo].[Usuario] ([dni])
GO
ALTER TABLE [dbo].[Mascota] CHECK CONSTRAINT [FK_Mascota_Usuario]
GO
ALTER TABLE [dbo].[RolFamilia]  WITH CHECK ADD  CONSTRAINT [FK_RolFamilia_Familia] FOREIGN KEY([id_familia])
REFERENCES [dbo].[Familia] ([id_familia])
GO
ALTER TABLE [dbo].[RolFamilia] CHECK CONSTRAINT [FK_RolFamilia_Familia]
GO
ALTER TABLE [dbo].[RolFamilia]  WITH CHECK ADD  CONSTRAINT [FK_RolFamilia_Rol] FOREIGN KEY([id_rol])
REFERENCES [dbo].[Rol] ([id_rol])
GO
ALTER TABLE [dbo].[RolFamilia] CHECK CONSTRAINT [FK_RolFamilia_Rol]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Permiso] FOREIGN KEY([id_permiso])
REFERENCES [dbo].[Permiso] ([id_permiso])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Permiso]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Rol] FOREIGN KEY([id_rol])
REFERENCES [dbo].[Rol] ([id_rol])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Rol]
GO
ALTER TABLE [dbo].[Traduccion]  WITH CHECK ADD FOREIGN KEY([IdEtiqueta])
REFERENCES [dbo].[Etiqueta] ([IdEtiqueta])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Traduccion]  WITH CHECK ADD FOREIGN KEY([IdIdioma])
REFERENCES [dbo].[Idioma] ([IdIdioma])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([rol])
REFERENCES [dbo].[Rol] ([id_rol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
USE [master]
GO
ALTER DATABASE [FGF-BDD] SET  READ_WRITE 
