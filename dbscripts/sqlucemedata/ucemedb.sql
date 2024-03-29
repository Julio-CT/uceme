CREATE DATABASE [UCEMEDb];
GO
USE [UCEMEDb]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[idBlog] [int] IDENTITY(1,1) NOT NULL,
	[idUsuario] [int] NOT NULL,
	[titulo] [nvarchar](500) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[foto] [nvarchar](500) NULL,
	[texto] [text] NOT NULL,
	[profesional] [bit] NULL,
 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[idBlog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cita]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[idCita] [int] IDENTITY(1,1) NOT NULL,
	[dia] [int] NOT NULL,
	[hora] [decimal](5, 2) NOT NULL,
	[nombre] [nvarchar](250) NOT NULL,
	[email] [nvarchar](250) NULL,
	[telefono] [nvarchar](9) NOT NULL,
	[idTurno] [int] NOT NULL,
 CONSTRAINT [PrimaryKey_b9aa8940-27cf-4d73-bd18-67d3aa456439] PRIMARY KEY CLUSTERED 
(
	[idCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companias]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companias](
	[idCompanias] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[foto] [nvarchar](500) NOT NULL,
	[link] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Companias] PRIMARY KEY CLUSTERED 
(
	[idCompanias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Curriculum]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Curriculum](
	[idCurriculum] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [nvarchar](350) NOT NULL,
	[Text] [text] NULL,
 CONSTRAINT [PK_Curriculum] PRIMARY KEY CLUSTERED 
(
	[idCurriculum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatosContacto]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatosContacto](
	[idDatosContacto] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](150) NOT NULL,
	[telefono] [nvarchar](15) NULL,
	[direccion] [nvarchar](500) NULL,
 CONSTRAINT [PK_DatosContacto] PRIMARY KEY CLUSTERED 
(
	[idDatosContacto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatosProfesionales]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatosProfesionales](
	[idDatosPro] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](250) NOT NULL,
	[telefono] [nvarchar](15) NOT NULL,
	[email] [nvarchar](250) NOT NULL,
	[direccion] [nvarchar](500) NOT NULL,
	[texto] [text] NULL,
	[foto] [nvarchar](500) NULL,
	[activo] [bit] NULL,
 CONSTRAINT [PK_DatosProfesionales] PRIMARY KEY CLUSTERED 
(
	[idDatosPro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documento]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documento](
	[idDocumento] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[link] [nvarchar](500) NOT NULL,
	[idUsuario] [int] NOT NULL,
 CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED 
(
	[idDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faq]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faq](
	[titulo] [nvarchar](350) NOT NULL,
	[texto] [text] NOT NULL,
	[idFaq] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PrimaryKey_bdf2be95-da1b-48d5-a78b-9780adcbf9ce] PRIMARY KEY CLUSTERED 
(
	[idFaq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fotos]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fotos](
	[idFoto] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](500) NOT NULL,
	[texto] [ntext] NULL,
	[destacada] [bit] NULL,
	[posicion] [int] NULL,
 CONSTRAINT [PrimaryKey_07f9ccf4-5bec-4db8-bf08-c2398fc2b52e] PRIMARY KEY CLUSTERED 
(
	[idFoto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCurriculum]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCurriculum](
	[idItem] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [nvarchar](350) NOT NULL,
	[Fechas] [nvarchar](250) NULL,
	[Texto] [text] NOT NULL,
	[idCurriculum] [int] NOT NULL,
 CONSTRAINT [PK_ItemCurriculum] PRIMARY KEY CLUSTERED 
(
	[idItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[idMenu] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[link] [nvarchar](150) NOT NULL,
	[posicion] [int] NOT NULL,
	[visible] [bit] NOT NULL,
 CONSTRAINT [PrimaryKey_599c50d5-b62f-4d09-98f4-4434f2ba10fa] PRIMARY KEY CLUSTERED 
(
	[idMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaginaAmiga]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaginaAmiga](
	[idPagina] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[descripcion] [nvarchar](500) NOT NULL,
	[link] [nvarchar](500) NOT NULL,
	[icono] [nvarchar](500) NULL,
 CONSTRAINT [PrimaryKey_8ee7bbb1-d88f-4d62-86a2-3c486bf7c1e3] PRIMARY KEY CLUSTERED 
(
	[idPagina] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfesionalesCompanias]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfesionalesCompanias](
	[idDatosPro] [int] NOT NULL,
	[idCompanias] [int] NOT NULL,
 CONSTRAINT [PK_ProfesionalesCompanias] PRIMARY KEY CLUSTERED 
(
	[idDatosPro] ASC,
	[idCompanias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfesionalUsuario]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfesionalUsuario](
	[idUsuario] [int] NOT NULL,
	[idDatosPro] [int] NOT NULL,
 CONSTRAINT [PK_ProfesionalUsuario] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC,
	[idDatosPro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[idRol] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicio]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicio](
	[idServicio] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](150) NOT NULL,
	[text] [text] NOT NULL,
	[foto] [nvarchar](500) NULL,
	[cabecera] [nchar](500) NULL,
 CONSTRAINT [PrimaryKey_b7f2f0af-c0cc-4b7a-8dca-c81375d0563e] PRIMARY KEY CLUSTERED 
(
	[idServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tecnica]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tecnica](
	[idTecnica] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [nvarchar](500) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[foto] [nvarchar](500) NOT NULL,
	[texto] [text] NOT NULL,
 CONSTRAINT [PrimaryKey_4d23a9bc-0fa1-4a27-9f86-8d10fc993a0a] PRIMARY KEY CLUSTERED 
(
	[idTecnica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Termino]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Termino](
	[nombre] [nvarchar](150) NOT NULL,
	[texto] [text] NOT NULL,
	[foto] [nvarchar](500) NULL,
	[link] [nvarchar](500) NULL,
	[idTermino] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Termino] PRIMARY KEY CLUSTERED 
(
	[idTermino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turno]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turno](
	[idTurno] [int] IDENTITY(1,1) NOT NULL,
	[dia] [int] NOT NULL,
	[inicio] [decimal](5, 2) NOT NULL,
	[fin] [decimal](5, 2) NOT NULL,
	[paralelas] [int] NOT NULL,
	[porhora] [int] NOT NULL,
	[idHospital] [int] NOT NULL,
 CONSTRAINT [PrimaryKey_8f9fd58b-721a-4372-8de1-4664343f95f8] PRIMARY KEY CLUSTERED 
(
	[idTurno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[apellidos] [nvarchar](150) NOT NULL,
	[nick] [nvarchar](50) NOT NULL,
	[login] [nvarchar](250) NOT NULL,
	[foto] [nvarchar](500) NOT NULL,
	[ultimoupdate] [datetime] NULL,
	[idRol] [int] NOT NULL,
	[idCurriculum] [int] NOT NULL,
	[idDatosContacto] [int] NOT NULL,
	[password] [nvarchar](150) NOT NULL,
	[newsletter] [bit] NULL,
	[linkedin] [nvarchar](max) NULL,
	[display_order] [int] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Video]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Video](
	[titulo] [nvarchar](150) NOT NULL,
	[descripcion] [text] NULL,
	[link] [text] NOT NULL,
	[idVideo] [int] IDENTITY(1,1) NOT NULL,
	[posicion] [int] NULL,
 CONSTRAINT [PrimaryKey_a88b2d85-e02c-4637-89de-24cf393a50a3] PRIMARY KEY CLUSTERED 
(
	[idVideo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 15/05/2020 17:54:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Blog] ON 

INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (34, 4, N'Cáncer de Tiroides', CAST(N'2013-07-01T17:36:04.000' AS DateTime), N'~/uploads/fotos/Blog34.png', N'<p><span style="font-size: medium;">C&aacute;ncer de Tiroides</span></p>
<p><span style="font-size: medium;">El c&aacute;ncer de la gl&aacute;ndula tiroidea es un tumor maligno de buen pron&oacute;stico en general.</span><br /><span style="font-size: medium;">Existen varios tipos de c&aacute;ncer tiroideo que se pueden dividir en diferenciados y no diferenciados</span><br /><br /><span style="font-size: medium;">-------<em> <span style="text-decoration: underline;">C&aacute;nceres tiroideos diferenciados</span></em></span><br /><br /><span style="font-size: medium;">La mayor&iacute;a de los c&aacute;nceres de tiroides son diferenciados. En estos c&aacute;nceres, las c&eacute;lulas se parecen mucho al tejido normal de la tiroides cuando se observa en un microscopio.&nbsp;</span><br /><br /><span style="font-size: medium;">- Carcinoma papilar: Aproximadamente ocho de cada 10 c&aacute;nceres de tiroides son carcinomas papilares. Los carcinomas papilares suelen crecer muy lentamente, y por lo general se originan en un solo l&oacute;bulo de la gl&aacute;ndula tiroides. Crecen lentamente por lo que su tratamiento es bastante efectivo y su pron&oacute;stico a largo plazo es muy bueno</span><br /><br /><span style="font-size: medium;">- Carcinoma folicular: Representa alrededor de uno de cada 10 c&aacute;nceres de tiroides. &Eacute;ste es m&aacute;s com&uacute;n en los pa&iacute;ses donde las personas no reciben suficiente yodo en la alimentaci&oacute;n. Probablemente, el pron&oacute;stico para el carcinoma folicular no es tan favorable como el del carcinoma papilar, aunque sigue siendo muy favorable en la mayor&iacute;a de los casos.</span><br /><br /><span style="font-size: medium;">- Carcinoma de c&eacute;lulas de H&uuml;rthle, tambi&eacute;n conocido como carcinoma de c&eacute;lulas ox&iacute;filas,&nbsp; se piensa que es en realidad una variante de carcinoma folicular. Conforma aproximadamente 3% de los casos de c&aacute;ncer de tiroides.&nbsp;</span><br /><br /><span style="font-size: medium;">----------<span style="text-decoration: underline;"><em> Otros tipos de c&aacute;ncer de tiroides</em></span></span></p>
<p><span style="font-size: medium;"></span><br /><span style="font-size: medium;">- <span style="text-decoration: underline;">Carcinoma medular tiroideo</span>: &nbsp;Representa aproximadamente un 4% de los c&aacute;nceres de tiroides.&nbsp;</span><br /><span style="font-size: medium;">A menudo, el c&aacute;ncer medular segrega demasiada calcitonina y una prote&iacute;na llamada ant&iacute;geno carcinoembrionario (carcinoembryonic antigen, CEA) en la sangre. Estas sustancias se pueden detectar con an&aacute;lisis de sangre.</span><br /><span style="font-size: medium;">Existen dos tipos de carcinoma medular de tiroides:</span><br /><span style="font-size: medium;">*El carcinoma medular de tiroides espor&aacute;dico, el cual representa aproximadamente ocho de cada 10 casos, no es hereditario.&nbsp;</span><br /><span style="font-size: medium;">*El carcinoma medular de tiroides familiar se hereda y puede presentarse en cada generaci&oacute;n de una familia. A menudo, estos c&aacute;nceres se desarrollan durante la ni&ntilde;ez o en la adultez temprana y se puede propagar temprano. El carcinoma medular de tiroides familiar a menudo est&aacute; asociado con un riesgo aumentado de otros tipos de tumores.&nbsp;</span><br /><span style="font-size: medium;">- <em><span style="text-decoration: underline;">Carcinoma anapl&aacute;sico</span></em>: el carcinoma anapl&aacute;sico es una forma poco com&uacute;n de c&aacute;ncer de tiroides, representando alrededor de 2% de todos los c&aacute;nceres de tiroides. Las c&eacute;lulas cancerosas no se parecen mucho a las c&eacute;lulas normales de la tiroides cuando son observadas con el microscopio. Tiene peor pron&oacute;stico que los anteriores.</span><br /><span style="font-size: medium;">- <em><span style="text-decoration: underline;">Linfoma tiroideo y sarcoma tiroideo</span></em> son entidades muy infrecuentes y agresivas.</span></p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (1013, 4, N'Paginas amigas', CAST(N'2013-07-18T16:13:39.000' AS DateTime), N'~/uploads/fotos/Blog1013.jpeg', N'<p>quieres aparecer en paginas amigas? Envianos un email de contacto</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (1014, 4, N'Cita Online', CAST(N'2013-07-18T16:18:59.000' AS DateTime), N'~/uploads/fotos/Blog1014.png', N'<p>Pruebe nuestro nuevo sistema de cita online!</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2010, 5, N'LA ANSIEDAD ANTE LA OPERACIÓN - ¿CÓMO DISMINUIRLA?', CAST(N'2013-07-28T23:56:28.000' AS DateTime), N'~/uploads/fotos/Blog2010.JPG', N'<p><span style="font-size: medium;">Seguro que si ha tenido la experiencia de &ldquo;pasar por quir&oacute;fano&rdquo; sabr&aacute; de qu&eacute; estamos hablando. Son muchos los pacientes que experimentan una enorme ansiedad cuando se les comunica que deben someterse a una intervenci&oacute;n quir&uacute;rgica. &ldquo;No es para menos&rdquo; pensar&aacute;n algunos&hellip;.Efectivamente, es completamente normal sertir miedo y ansiedad ante una problema de salud que requiere visitar un quir&oacute;fano. Gran parte de este sentimiento se debe al desconocimiento: Tendemos a imaginar la situaci&oacute;n en base a lo que hemos visto en pel&iacute;culas o lo que nos ha contado un amigo&hellip;.pero no basamos nuestra anticipaci&oacute;n de los acontecimientos en HECHOS OBJETIVOS.</span></p>
<p><span style="font-size: medium;">En UCEME pensamos que la labor m&aacute;s importante se desarrolla antes del ingreso en el Hospital, en la consulta. La consulta es un espacio amigable, en el que dos personas, m&eacute;dico y paciente, debe hablar de todos los aspectos relacionados con el proceso de diagn&oacute;stico y tratamiento. Es en este momento cuando hay que emplear el tiempo suficiente para dar TODA la informaci&oacute;n necesaria para que el paciente salga de la habitaci&oacute;n con todas las dudas resueltas. El conocimiento preciso de todos y cada uno de los pasos que van a suceder disminuye la ansiedad de forma muy significativa</span></p>
<p><span style="font-size: medium;">Vamos a hablar un poco de lo que consiste un episodio de ingreso habitual de una patolog&iacute;a muy com&uacute;n en Cirug&iacute;a Endocrina &ndash; Una operaci&oacute;n de Tiroides por patolog&iacute;a nodular benigna. Lo que m&eacute;dicamente llamar&iacute;amos Tiroidectom&iacute;a Total por bocio multinodular bilateral.</span></p>
<p><span style="font-size: medium;">En este campo habr&iacute;a que trasmitir, inicialmente, varias ideas: (1) La operaci&oacute;n es por causa BENIGNA, (2) La cirug&iacute;a es poco dolorosa y el postoperatorio se tolera estupendamente, de hecho casi todos los pacientes ingieren l&iacute;quidos a las 4-6 de la cirug&iacute;a. (3) Explicar las posibles complicaciones, los medios que ponemos para evitarlas, la frecuencia con que las tenemos y c&oacute;mo se solventan.</span></p>
<p><span style="font-size: medium;">(1)Aunque la indicaci&oacute;n de cirug&iacute;a es por causa benigna, siempre mandamos a analizar el tiroides por si hubiese alguna degeneraci&oacute;n en alguno de los n&oacute;dulos (hecho por otro lado, poco frecuente).</span></p>
<p><span style="font-size: medium;">(2)La cirug&iacute;a, efectivamente, es poco dolorosa, y la mayor parte de los pacientes pasan la noche de la cirug&iacute;a en el hospital m&aacute;s por un tema de precauci&oacute;n que por necesidad real de atenci&oacute;n hospitalaria. Suele iniciarse tolerancia a l&iacute;quidos a las pocas horas de la intervenci&oacute;n por lo que se duerme sin sueros. Casi todos los pacientes con 1gr de paracetamol cada 8h obtienen un nivel de analgesia correcto. Tras evaluar el cuello y las posibles molestias locales procede al alta hospitalaria con seguridad.</span></p>
<p><span style="font-size: medium;">(3)Respecto a las complicaciones, la m&aacute;s temida, y por la que se interesan m&aacute;s los pacientes, es si podr&aacute;n hablar tras la cirug&iacute;a. Claro que s&iacute;!. Es esencial en la cirug&iacute;a endocrina del cuello el conocimiento anat&oacute;mico detallado junto con la delicadeza y minuciosidad al intervenir. Los nervios de la laringe se sit&uacute;an detr&aacute;s del tiroides. Hay que localizarlos y separarlos con cuidado para evitar lesiones. En ocasiones los m&uacute;ltiples procesos inflamatorios del tiroides pueden dificultar esta disecci&oacute;n y hace que los nervios se inflamen, pudiendo ocasionar cierta afon&iacute;a que se recupera en pocas semanas. Habitualmente empleamos unas gafas de aumento, gafas lupa, que, al magnificar el campo quir&uacute;rgico, hacen m&aacute;s segura la cirug&iacute;a. En otras ocasiones, tambi&eacute;n podemos ayudarnos de dispositivos de neuromonitorizaci&oacute;n (ver apartado t&eacute;cnicas exclusivas).</span></p>
<p><span style="font-size: medium;">&nbsp;</span></p>
<p><span style="font-size: medium;">En conclusi&oacute;n, <em>en UCEME, creemos que la informaci&oacute;n preoperatoria, completa y de calidad, sobre el procedimiento quir&uacute;rgico al que v&aacute; a ser sometido un paciente es un requisito esencial</em>. De esta forma evitaremos que nuestros pacientes &ldquo;imaginen&rdquo; situaciones irreales y tendr&aacute;n una imagen adecuada de su cirug&iacute;a, reduciendo as&iacute; el miedo y la ansiedad.</span></p>
<p>&nbsp;</p>
<p>&nbsp;</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2012, 4, N'Mecanismos resolución diabetes con cirugía bariátrica', CAST(N'2014-03-13T11:12:56.000' AS DateTime), N'~/uploads/fotos/Articulo2012.JPG', N'<p><span style="font-size: large;"><strong>Mecanism of Metabolic Advantages After Bariatric Surgery</strong></span></p>
<p><span style="font-size: medium;">It&acute;s all gastrointestinal factros versus it&acute;s all food restriction</span></p>
<p><strong>Filip K. Knop, Roy Taylor</strong></p>
<p><strong>Diabetes Care, volume 36, Supplement 2, August 2013</strong></p>
<p>Interesante art&iacute;culo en el que se discuten los mecanismos implicados en la resoluci&oacute;n de la diabetes tipo II tras cirug&iacute;a bari&aacute;trica</p>
<p>publicado en agosto de 2013, en Diabetes Care, sobre los mecanismos que llevan a la resoluci&oacute;n de la diabetes tipo II tras la cirug&iacute;a bari&aacute;trica</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2013, 5, N'La importancia de la primera cirugía en el cáncer de tiroides.', CAST(N'2014-03-13T18:54:36.000' AS DateTime), N'~/uploads/fotos/Articulo2013.png', N'<p style="text-align: center;"><strong><span style="font-size: large;">Evaluating the morbidity and efficacy&nbsp;of reoperative surgery in the central compartment for persistent/recurrent papillary thyroid carcinoma.</span></strong></p>
<p><span style="font-family: arial, helvetica, sans-serif; font-size: medium;">Lang, B. H., G. C. Lee, et al. (2013).</span></p>
<p><span style="font-family: arial, helvetica, sans-serif; font-size: medium;">World J Surg 37(12): 2853-2859</span></p>
<p><span style="font-family: arial, helvetica, sans-serif; font-size: medium;">La primer cirug&iacute;a, en el tratamiento del c&aacute;ncer de tiroides, determina, en ocasiones el pron&oacute;stico del paciente.</span></p>
<p><span style="font-family: arial, helvetica, sans-serif; font-size: medium;">Es esencial un adecuado diagn&oacute;stico de extensi&oacute;n antes de la intervenci&oacute;n, as&iacute; como el conocimiento de las t&eacute;cnicas oncol&oacute;gicas para poder planificar de forma adecuada la estrategia quir&uacute;rgica.</span></p>
<p><span style="font-family: arial, helvetica, sans-serif; font-size: medium;">En este art&iacute;culo se revisa la eficacia y morbilidad de las reintervenciones sobre el compartimento central.</span></p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2016, 4, N'Congreso de la Sociedad Europea de Cirugía Endocrina', CAST(N'2014-06-03T04:30:51.640' AS DateTime), N'~/uploads/fotos/Blog2016.jpeg', N'El pasado mayo (del 15 al 17) acudimos al congreso de la sociedad europea de cirugía endocrina (ESES) que se celebró en Cardiff, Gales.
La reunión bianual que recibe a los principales cirujanos endocrinos de nuestro continente ha sido, como en otras ocasiones, un foro de encuentro y debate muy interesante. En él se ha hecho una puesta al día de los temas más interesantes de nuestra especialidad, y hemos tenido la oportunidad de saludar a nuestros colegas conocidos en previas ediciones.
Además hemos sido recibidos como nuevos integrantes de dicha sociedad en un acto presidido por el actual presidente de la misma, el Dr. Niederle, que cedió su puesto al que va a ser el presidente de la sociedad los dos próximos años, el Profesor Sitges Serra, del Hospital del Mar de Barcelona

Uceme ha hecho un importante esfuerzo por acudir a esta nueva reunión, presentando dos comunicaciones científicas en el mismo. Esperemos que dentro de dos años, nuestra aportación sea aún mayor. 
', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2017, 4, N'Cirugía de la diabetes (Tipo II)', CAST(N'2014-06-03T04:38:49.000' AS DateTime), N'~/uploads/fotos/Blog2017.jpeg', N'Cada vez son más las voces que apuntan que la Diabetes Mellitus Tipo II podría mejorar (e incluso curarse) con cirugía.
Esta afirmación tan vertiginosa parece, cada vez más, estar basada en hechos científicos constatables.
Aunque el camino aún es largo, podemos empezar a afirmar que la cirugía será una de las herramientas para combatir esta enfermedad crónica tan extendida en nuestra sociedad.', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2018, 4, N'Surgical Versus Medical Treatment For obese patients with Diabetes', CAST(N'2014-06-03T04:47:26.000' AS DateTime), N'~/uploads/fotos/Articulo2018.jpeg', N'Muy interesante artículo que presenta las primeras evidencias del resultado de la cirugía en la resolución de la diabetes a medio/largo plazo.
Hasta ahora muchos trabajos han destacado la gran efectividad de la cirugía bariátrica en la resolución/mejora de la diabetes (y otras comorbilidades). Se publican ahora los resultados del trabajo que comenzó hace tres años, y que compara tratamiento médico con bypass gástrico y gastrectomía tubular.
¿Estaremos ante la posibilidad de la cura de la diabetes (tipo II), una de las más complejas patologías a las que se enfrenta la endocrinología?Por ahora esto parece demasiado optimista, pero la mejoría/resolución experimentada en muchos pacientes tras cirugía, mantenida ahora a medio plazo, nos hace seguir investigando y manteniendo esta opción, cada vez más real', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2019, 5, N'INFORMAR A LOS PACIENTES: QUIÉN Y CÓMO', CAST(N'2014-10-01T16:03:23.917' AS DateTime), N'~/uploads/fotos/Blog2019.jpg', N'Informar adecuadamente a un paciente de un diagnóstico oncológico no es nada sencillo. Por mucho que tratemos de suavizarlo, en la cabeza de nuestros pacientes lo primero que resonará será: "tengo cáncer". 
En ese momento se presentarán un mar de miedos, angustias e inseguridades......que en muchas ocasiones bloquean al paciente para asimilar más información. 
En Cirugía Endocrina, la mayor parte de los diagnósticos corresponden al cáncer diferenciado de tiroides que, afortunadamente, y de forma global, presenta un buen pronóstico y unas buenas cifras de supervivencia.
En nuestro criterio, es absolutamente fundamental ofrecer una información completa y comprensible durante todo el proceso de diagnóstico de extensión,  tratamiento y seguimiento. Sólo de esta forma conseguiremos crear un relación médico - paciente sólida y de confianza.
En el momento del diagnóstico es esencial tranquilizar al paciente y, con una información detallada sobre la extensión de su enfermedad, poder darle un pronóstico adecuado. Creemos que es importante contarle, de forma abierta, qué va a suceder desde que salga de la consulta......explicarle que es el preoperatorio, la consulta preanestésica y la técnica. Explicarle cómo es el ingreso, cuánto se estima que durará la intervención,  cuántos días estará hospitalizado y las posibles complicaciones a las que tendremos que hacer frente si se presentan. 
Es importante dosificar la información, no podemos hablar en la primera visita de situaciones tales como cuál será la actitud si es tumor recidiva. 
Por ello, los pacientes han de tener la puerta abierta, han de saber dónde encontrarnos. Al salir de la consulta y llegar a casa, comienza un proceso de maduración del diagnóstico y de la información recibida, durante el cual surgirán nuevas cuestiones. 
Asimismo es también importante el hecho de que el paciente sepa que, habitualmente,  el tratamiento se apoya en varios pilares ( radioiodo, tratamiento hormonal supresor, quimioterápicos....) y que en cada fase,  estará asesorado y apoyado por un miembro del equipo ( endocrinólogo,  especialista en medicina nuclear, oncólogo...)
Sería lógico pensar que, si procedemos de esta manera, crearemos una buena relación y habremos conseguido tener un paciente tranquilo y correctamente informado.
Pero.....¿ qué sucede realmente cuanto cuando el paciente sale de la consulta?...Lo más habitual será que teclee en un buscador de Internet las palabras cáncer de tiroides y a partir de ahì.....empiece a leer todo tipo de información, filtrada o no. También puede consultar  con un amigo o la vecina, que le puede contar algún caso más o menos conocido.......En algunas ocasiones esta información cala hondo en los pacientes y, si lo añadimos a la ansiedad causada por la propio diagnóstico, las consecuencias pueden ser fatales........y es que, asumámoslo,  los médicos jugamos un papel importante, pero no exclusivo. La enfermedad tiene un componente emocional, muy importante....y en ese terreno, los médicos nos quedamos cortos la mayoría de las veces. No podemos abarcalo todo en una consulta de media hora. 
¿Quién cubre este, y otos, aspectos?.....las Asociaciones de pacientes. ¿ Quién vá a entender mejor a un paciente que otro que ha vívido lo mismo?, y sobre todo...¿Quién mejor que un paciente, en el marco de una asociación, vá a ofrecer información detallada y filtrada sobre profesionales, alternativas terapéuticas, vivencia y convivencia con el diagnóstico?.....los pacientes son el mejor ejemplo para otros pacientes de que se puede vivir con normalidad después de tratarse, de que pueden curarse, de cómo vivir con la secuelas que se pueden producir por el tratamiento.
En nuestro país, quizá, no se promueve consultar o facilitar información sobre Asociaciones todo lo que se debiera........No sé si será por las prisas de la presión asistencial....pero lo cierto es que tenemos muy buenas asociaciones. En nuestro campo la Asociación  Española contra el Cáncer de Tiroides  (AECAT), es un fiel ejemplo. Es una asociación inquieta, con constantes proyectos por y para los pacientes con cáncer de tiroides ( Dona tu voz, Voces en el Camino, por poner dos recientes ejemplos). Ha buscado el respaldo  de Sociedades científicas relevantes ( SEEN, AEC, SEMNim, SEOM) en aras a ofrecer información contrastada....y todo ello sin perder la cercanía con la persona que está "pasando" por un cáncer. 
Desde UCEME animamos a los pacientes a que consulten con ellos, tanto  los que tratamos nosotros  como el que pueda estar leyendo este post....sólo  trabajando en equipo conseguiremos un tratamiento integral y de calidad. 

', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (2020, 5, N'Prophylactic central neck disection in papillary thyroid cancer: a consensus report of the European Society of Endocrine Surgeons (ESES).', CAST(N'2014-12-18T15:30:00.000' AS DateTime), N'~/uploads/fotos/Articulo2020.gif', N'Langenbecks Arch Surg. 2014 Feb;399(2):155-63. 

Sancho JJ1, Lennard TW, Paunovic I, Triponez F, Sitges-Serra A.


Mucho se ha debatido durante los últimos años sobre el papel de la linfadenectomia central profiláctica en el cancer diferenciado de tiroides.
Inicialmente no se consideraba realizar este procedimiento....eran actitudes antiguas y poco intervencionistas.
Posteriormente se paso a un periodo en el que este gesto era casi la norma, con el consiguiete aumento de las tasas de parálisis recurrencial e hipoparatiroidismo, en manos inexpertas.
La Sociedad Europea de cirugía Endocrina, nuestro representante mas potente en Europa, publicó este año un interesante consenso sobre las indicaciones de linfadenectomia profilactica central. 
Se trata de un exhautivo trabajo de revisión de la literatura del que han salido una selección de indicaciones sopesadas y con evidencia científica.
Su primer firmante es un cirujano español y el último firmante, también español, es el actual presidente de la sociedad.', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3027, 4, N'Surgical Management of adrenocortical carcinoma ', CAST(N'2016-10-20T11:24:43.157' AS DateTime), N'~/uploads/fotos/Articulo3027.png', N'<p>Surg Oncol Clin N Am 25 (2016) 153-170</p>
<p>Revisi&oacute;n muy interesante de las Cl&iacute;nicas de Norteam&eacute;rica sobre el tratamiento del c&aacute;ncer suprarrenal.</p>
<p>Se trata de un c&aacute;ncer muy infrecuente, pero con un comportamiento muy agresivo, por lo que es de especial importancia el estudio preoperatorio y el plantemiento de la primera cirug&iacute;a (A menudo la &uacute;nica opci&oacute;n de curaci&oacute;n en estos pacientes)</p>
<p>Se discute la idoneidad del abordaje laparosc&oacute;pico y la importancia de la sospecha prequir&uacute;rgica en base a las carcater&iacute;stics morfol&oacute;gicas y anal&iacute;ticas de cada&nbsp;paciente</p>
<p>Propone por &uacute;ltimo un algoritmo de tratamiento tanto quir&uacute;rgico como quimiorradioter&aacute;pico en funci&oacute;n de las caracter&iacute;sticas de cada caso y del resultado tras la cirug&iacute;a (M&aacute;rgenes quir&uacute;rgicos, linfadenectom&iacute;a, afectaci&oacute;n de &oacute;rganos de vecindad)</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3028, 4, N'Bilateral areolar approach endoscopic thyroidectomy for low risk papillary Thyroid Carcinoma', CAST(N'2017-07-12T23:49:03.807' AS DateTime), N'~/uploads/fotos/Articulo3028.jpg', N'<p></p>
<p>Surg Laparosc Endosc Percutan Tech 2015;25:19&ndash;22</p>
<p><span>&nbsp;</span></p>
<p><span>La tiroidectom&iacute;a sin cicatriz en el cuello es una t&eacute;cnica muy desarrollada en Asia, y est&aacute; teniendo en occidente una evoluci&oacute;n muy importante, con grupos de amplia experiencia en EEUU y Sudam&eacute;rica, y cada vez con mayor aceptaci&oacute;n en Europa.</span></p>
<p><span>En este trabajo se presentan cerca de 140 c&aacute;nceres de tiroides intervenidos desde abordaje biareolar, demostrando iguales resultados oncol&oacute;gicos que la cirug&iacute;a abierta, pero con mucho mejores resultados est&eacute;ticos. Las tasas de complciaciones son similares a la t&eacute;cnica convencional, pero el paciente no presenta cicatriz alguna en el cuello.&nbsp;</span></p>
<p><span>Supone una "revoluci&oacute;n" en la cirug&iacute;a endocrina y la cirug&iacute;a general, al traspasar definitivamente la barrera de la cirug&iacute;a endosc&oacute;pica en el cuello</span></p>
<p><span>Sin duda asistiremos a su implantaci&oacute;n en nuestro medio a corto-medio plazo</span></p>
<p><span>&nbsp;</span></p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3029, 4, N'Tiroidectomía sin incisión cervical por abordaje endoscópico biaxilo-biareolar', CAST(N'2019-04-17T17:01:55.233' AS DateTime), N'~/uploads/fotos/Articulo3029.png', N'<p>Hemos publicado los resultados obtenidos en nuestras primeras 15 tiroidectom&iacute;as por abordaje extracervical (Biaxilo-Biareolar) en la revista Cirug&iacute;a Espa&ntilde;ola, revista oficial de la Asociaci&oacute;n Espa&ntilde;ola de Cirujanos. Hemos comenzado un camino realmente apasionante en el campo de la cirug&iacute;a endocrina, pero como algo novedoso, proponemos en nuestro trabajo que debe realizarse sobre s&oacute;lidos pilares con control estricto de resultados y morbilidad. Existe un verdadero "Boom" en relaci&oacute;n a todos los abordajes extracervicales y debemos ser cautos y serios. Hemos presentado nuestros primeros quince casos con excelentes resultados; el futuro habr&aacute; de confirmar esta tendencia</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3030, 4, N'Afrontar las Navidades es posible (en 3 etapas): ', CAST(N'2019-12-24T10:32:18.373' AS DateTime), N'~/uploads/fotos/Articulo3030.png', N'<p>Un a&ntilde;o m&aacute;s, las Navidades est&aacute;n aqu&iacute; y, un a&ntilde;o m&aacute;s, quieres cuidarte. &iquest;Sabes c&oacute;mo hacerlo? No se trata de una cuesti&oacute;n est&eacute;tica, sino principalmente de salud. Por eso, desde UCEME queremos regalarte por Navidad estos consejos de la mano de Loredana Arhip, dietista-nutricionista. &iexcl;A&uacute;n est&aacute;s a tiempo de afrontar saludablemente estas fechas!</p>
<p>&nbsp;</p>
<p><strong>Antes de las celebraciones:</strong></p>
<ol>
<li>Es importante realizar una dieta variada incluyendo en su dieta alimentos de todos los grupos, como por ejemplo verduras y hortalizas, frutas, cereales integrales, legumbres, frutos secos&hellip;</li>
<li>Preparar una lista de la compra y ce&ntilde;irse a ella. As&iacute;, nunca faltar&aacute;n de la nevera los alimentos frescos y todos los ingredientes que se necesitan.</li>
<li>Antes de comprar, leer atentamente el etiquetado de los alimentos.</li>
<li>Realizar entre 4-5 comidas al d&iacute;a, poco abundantes.</li>
<li>Aumentar el consumo de verduras y hortalizas, incluyendo al menos 2 raciones al d&iacute;a en las comidas principales. Es importante que una de las raciones sea cruda en ensalada.</li>
<li>Tomar al menos 3 raciones de fruta fresca al d&iacute;a. Elegir las variedades de temporada (kiwi, mandarina, naranja, manzana, pera, pl&aacute;tano) y consumirlas enteras (evitando los zumos) y si es posible con la piel.</li>
<li>Elegir las variedades integrales de pan y cereales. Los alimentos integrales aportan un tipo de fibra beneficiosa para el organismo.</li>
<li>Favorecer alimentos como fruta, yogur natural o frutos secos en los snacks de media ma&ntilde;ana y merienda.</li>
<li>Es fundamental mantener la hidrataci&oacute;n bebiendo agua e infusiones sin az&uacute;car.</li>
<li>Mantener una vida activa y realizar al menos 30 minutos de ejercicio al d&iacute;a.</li>
</ol>
<p>&nbsp;</p>
<p><strong>Durante las celebraciones (se consideran d&iacute;as festivos el 24, 25 y 31 de diciembre y el 6 de enero):</strong></p>
<ol>
<li>Estos son los d&iacute;as adecuados para disfrutar de los platos tradicionales junto a la familia.</li>
<li>Es importante respetar el horario de las otras comidas del d&iacute;a e intentar que sean ligeras, si la cena ser&aacute; muy abundante o viceversa.</li>
<li>Evitar los excesos, intentando no repetir. Esto es v&aacute;lido tanto para la comida como para el postre.</li>
<li>No beber las calor&iacute;as. Consumir el alcohol con moderaci&oacute;n ya que tambi&eacute;n aporta calor&iacute;as.</li>
<li>Antes de tomar el postre, reducir a la mitad la cantidad que se va a consumir e intercambiarlo por una fruta.</li>
</ol>
<p><strong>&nbsp;</strong></p>
<p><strong>Despu&eacute;s de las celebraciones: </strong></p>
<ol>
<li>Esta es la mejor &eacute;poca para volver cuanto antes a la rutina, tomando una dieta variada y equilibrada explicada anteriormente.</li>
<li>Intentar que ya no haya sobras de preparaciones de d&iacute;as previos ni tampoco dulces como turrones, polvorones o rosc&oacute;n.</li>
<li>Utilizar t&eacute;cnicas de cocci&oacute;n sencillas (al horno, al vapor&hellip;).</li>
<li>Planificar una rutina de ejercicio f&iacute;sico y ce&ntilde;irse a ella (todo el a&ntilde;o).</li>
<li>Llevar un control del peso corporal, con una frecuencia semanal o cada 15 d&iacute;as.</li>
</ol>
<p>&nbsp;</p>
<p>&iquest;Tienes alguna consulta que hacer? Ponte en contacto con nosotros en <span><a href="about:blank">http://www.endocrinologia-madrid.com/</a></span> y estaremos encantados de ayudarte.</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3031, 4, N'Propósitos de año nuevo que SÍ vas a cumplir: objetivos S.M.A.R.T.', CAST(N'2020-01-06T12:09:50.857' AS DateTime), N'~/uploads/fotos/Articulo3031.jpg', N'<p><strong>Con la llegada de un nuevo a&ntilde;o, normalmente nos planteamos unos cuantos p</strong><strong>rop&oacute;sitos: </strong></p>
<ol>
<li>Comer m&aacute;s sano</li>
<li>Perder peso</li>
<li>Aprender a cocinar</li>
<li>Apuntarme al gimnasio</li>
<li>Tomar m&aacute;s agua</li>
<li>Beber menos alcohol</li>
<li>Dejar de fumar</li>
<li>Ahorrar</li>
<li>Viajar por el mundo</li>
<li>Estudiar idiomas</li>
<li>Leer m&aacute;s libros</li>
</ol>
<p></p>
<p>Estos son solo algunos de los prop&oacute;sitos de A&ntilde;o Nuevo que apuntas con tanto optimismo cada a&ntilde;o. Pero <strong>&iquest;te has preguntado alguna vez por qu&eacute; nunca se cumplen?</strong></p>
<p>En primer lugar, una de las razones es porque te abrumas con demasiadas opciones. La lista de prop&oacute;sitos no tiene que ser eterna, solo tiene que incluir <strong>algunos</strong> <strong>objetivos que ya puedes poner en pr&aacute;ctica desde el primer d&iacute;a del nuevo a&ntilde;o</strong>. No te preocupes por los dem&aacute;s prop&oacute;sitos. No es que los borres y te olvides de ellos, sino que los ir&aacute;s a&ntilde;adiendo uno a uno a medida que vas cumpliendo los dem&aacute;s.</p>
<p>Segundo, <strong>los objetivos que te propones no son suficientemente detallados</strong> como para llevarlos a cabo. Imag&iacute;nate cada prop&oacute;sito como el t&iacute;tulo de un gran proyecto y como cualquier gran proyecto, necesita un plan de ataque muy detallado para poder llevarlo a cabo. Una forma de hacerlo es estableciendo tus prop&oacute;sitos seg&uacute;n los objetivos <strong>S.M.A.R.T.* del ingl&eacute;s <em>Specific, Measurable, Attainable, Relevant, Time bound. </em></strong>Cada &iacute;tem se puede traducir y definir como:</p>
<p><strong>Espec&iacute;fico:</strong> Define de una forma detallada tu objetivo.</p>
<p><strong>Medible:</strong> &iquest;C&oacute;mo medir&aacute;s la puesta en pr&aacute;ctica de tu objetivo?</p>
<p><strong>Alcanzable:</strong> &iquest;Puedes llevar tu objetivo a la pr&aacute;ctica? &iquest;C&oacute;mo?</p>
<p><strong>Relevante: </strong>&iquest;Es un objetivo realista que tiene relevancia para ti?</p>
<p><strong>Tiempo:</strong> &iquest;Durante cu&aacute;nto tiempo vas a llevar acabo tu objetivo?</p>
<p>&nbsp;</p>
<p>Nosotros hemos elegido 3 prop&oacute;sitos esenciales para cuidar tu alimentaci&oacute;n y que siguen los objetivos S.M.A.R.T.</p>
<p></p>
<p><strong>Prop&oacute;sitos de A&ntilde;o Nuevo para cuidar tu alimentaci&oacute;n (que s&iacute; vas a cumplir):</strong></p>
<p><strong>&nbsp;</strong></p>
<ol>
<li><strong>Comer m&aacute;s sano:</strong></li>
</ol>
<p>&nbsp;</p>
<p><strong>Espec&iacute;fico:</strong> aumentar el consumo de frutas, verduras y hortalizas.</p>
<p><strong>Medible:</strong> al menos 2 raciones de verduras y hortalizas y 3 de frutas.</p>
<p><strong>Alcanzable:</strong> apuntar en la lista de la compra: frutas, verduras y hortalizas para que nunca falten de la nevera.</p>
<p><strong>Relevante:</strong> en este punto recuerda esto: "no existen objetivos perfectos". No lo des todo por perdido solo porque al cenar fuera de casa no hayas podido comer verdura. Al d&iacute;a siguiente tienes otra oportunidad. La consistencia es la clave del &eacute;xito.</p>
<p><strong>Tiempo:</strong> el consumo de estos alimentos es al d&iacute;a, por lo que este objetivo se tiene que intentar cumplir a diario.</p>
<p>&nbsp;</p>
<ol start="2">
<li><strong>Aprender a cocinar</strong>:</li>
</ol>
<p>&nbsp;</p>
<p><strong>Espec&iacute;fico:</strong> aprender a cocinar paella.</p>
<p><strong>Medible:</strong> establecer un domingo al mes &ldquo;el d&iacute;a de paella&rdquo;.</p>
<p><strong>Alcanzable:</strong> opci&oacute;n A: ir a cursos de cocina. Opci&oacute;n B: visitar a la abuela o abuelo cocinero.</p>
<p><strong>Relevante:</strong> no esperes grandes elogios desde la primera preparaci&oacute;n.</p>
<p><strong>Tiempo: </strong>cuanto m&aacute;s tiempo le dediques a aprender, mejor te saldr&aacute;.</p>
<p><strong>&nbsp;</strong></p>
<ol start="3">
<li><strong> </strong><strong>Perder peso.</strong></li>
</ol>
<p>&nbsp;</p>
<p><strong>Espec&iacute;fico:</strong> perder 6 kg de peso.</p>
<p><strong>Medible:</strong> perder entre <strong>0,5 - 1 kg</strong> de peso a la semana.</p>
<p><strong>Alcanzable:</strong> para conseguir este objetivo pon en pr&aacute;ctica los objetivos anteriormente mencionados, pide cita con tu dietista-nutricionista, pide consejos para ejercicios f&iacute;sicos, encuentra lo que te anima y s&iacute;guelo.</p>
<p><strong>Relevante:</strong> no te pongas objetivos como perder 10 kg en 10 d&iacute;as. NO SON REALISTAS.</p>
<p><strong>Tiempo: </strong>para establecer el tiempo hacemos un peque&ntilde;o c&aacute;lculo: si quieres perder 8 kg y pierdes una media de 1 kg a la semana, necesitar&aacute;s <strong>al menos</strong> <strong>2 meses</strong> para cumplir tu objetivo. Dado que no somos m&aacute;quinas perfectas, siempre date un margen de tiempo. &nbsp;</p>
<p>&nbsp;</p>
<p>&iquest;Cu&aacute;les son vuestros prop&oacute;sitos para este 2020? &iquest;C&oacute;mo se encuadran en los objetivos S.M.A.R.T.?</p>
<p></p>
<p><em>Art&iacute;culo elaborado por Loredana Arhip, dietista-nutricionista de UCEME Endocrinolog&iacute;a Madrid.&nbsp;</em></p>
<p></p>
<p>*Doran, G.T. (1981) There''s a S.M.A.R.T. way to write management''s goals and objectives. Management Review (AMA FORUM) 70 (11): 35&ndash;36.</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3032, 4, N'Top 5 de preguntas/comentarios en 2019 después de poner una dieta', CAST(N'2020-01-14T12:24:10.253' AS DateTime), N'~/uploads/fotos/Articulo3032.jpg', N'<p><span style="font-weight: 400;">Un a&ntilde;o da para muchas preguntas y comentarios curiosos, especialmente despu&eacute;s de que una persona acuda a consulta por una dieta. De entre todos los que me han hecho en 2019, destacar&iacute;a los siguientes:</span></p>
<ol>
<li><span style="font-weight: 400;"> &ldquo;&iquest;Puedo tomar fruta despu&eacute;s de la comida?&rdquo;</span></li>
<li><span style="font-weight: 400;"> &ldquo;&iquest;En serio, puedo comer pan?&rdquo;</span></li>
<li><span style="font-weight: 400;"> &ldquo;Todas las dietas que he hecho me prohib&iacute;an comer legumbres&rdquo;</span></li>
<li><span style="font-weight: 400;"> &ldquo;La mayor alegr&iacute;a que me llevo hoy es que me digas que puedo tomar frutos secos&rdquo;</span></li>
<li><span style="font-weight: 400;"> &ldquo;Ahora que lo pienso, esta &lsquo;dieta&rsquo; que me has puesto no es tan dif&iacute;cil de hacer&rdquo;</span></li>
</ol>
<p><span style="font-weight: 400;">En todos los casos, mi respuesta ha sido S&Iacute;. S&iacute; puedes tomar fruta despu&eacute;s de la comida; s&iacute; puedes tomar frutos secos, legumbres y pan. Esto es porque se ha comenzado con un nuevo patr&oacute;n alimentario en el que se puede comer de todo en su justa medida.&nbsp;</span></p>
<p><span style="font-weight: 400;">&iquest;Ten&eacute;is alguna otra pregunta/comentario para a&ntilde;adir a esta lista? Si es as&iacute; &iquest;qu&eacute; os han contestado? Escr&iacute;benos en nuestras redes sociales.</span><span style="font-weight: 400;"></span></p>
<p><span style="font-weight: 400;"></span></p>
<p><em><span style="font-weight: 400;">Escrito por Loredana Arhip, dietista-nutricionista de UCEME.</span></em></p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3033, 4, N'¿Qué hacemos con el azúcar?', CAST(N'2020-02-06T08:46:18.433' AS DateTime), N'~/uploads/fotos/Articulo3033.jpg', N'<p>&ldquo;Me he quitado el az&uacute;car de la dieta&rdquo;. Loredana Arhip, dietista-nutricionista de UCEME, dice que ha o&iacute;do esta frase en numerosas personas que llegan a su consulta. Pero, &iquest;es esto sano? &iquest;Es necesario? Aqu&iacute; ten&eacute;is informaci&oacute;n sobre este dilema nutricional.</p>
<p><strong>&iquest;Qu&eacute; son los az&uacute;cares a&ntilde;adidos?</strong></p>
<p>Los az&uacute;cares son carbohidratos que se encuentran naturalmente en la mayor&iacute;a de los alimentos. Su papel fundamental es el de proporcionar energ&iacute;a.</p>
<p>Los az&uacute;cares a&ntilde;adidos son, tal y como su nombre indica, aquellos a&ntilde;adidos a los alimentos por los fabricantes, los cocineros o los consumidores, as&iacute; como los az&uacute;cares presentes de forma natural en la miel, los jarabes, los jugos de fruta y los concentrados de jugo de fruta.</p>
<p>Los az&uacute;cares a&ntilde;adidos se diferencian de los az&uacute;cares intr&iacute;nsecos que se encuentran naturalmente en la leche, las frutas y las verduras.<strong>&nbsp;</strong></p>
<p><strong>Recomendaciones para limitar la ingesta de az&uacute;cares a&ntilde;adidos </strong></p>
<p>Los az&uacute;cares a&ntilde;adidos no deben representar m&aacute;s del 10% de la energ&iacute;a (ingesta de calor&iacute;as) que se obtiene de los alimentos y bebidas todos los d&iacute;as. Esto viene a ser aproximadamente 50 gramos de az&uacute;car al d&iacute;a o 200 kcal en una dieta de 2000 kcal. Para poder medirlo de una forma pr&aacute;ctica, saber que una cucharada de caf&eacute; colmada o una cucharada de postre rasa pueden contener 4 gramos de az&uacute;car y una cucharada de postre colmada 8 gramos de az&uacute;car.</p>
<p><strong>&iquest;Por qu&eacute; es necesario limitar el consumo de az&uacute;cares a&ntilde;adidos?</strong></p>
<p>Seg&uacute;n la OMS, los adultos que ingieren menos az&uacute;cares a&ntilde;adidos tienen un peso corporal m&aacute;s bajo. Limitar la ingesta de estos az&uacute;cares permite realizar un patr&oacute;n de alimentaci&oacute;n saludable. Adem&aacute;s, los az&uacute;cares a&ntilde;adidos aportan calor&iacute;as, pero no nutrientes esenciales.</p>
<p><strong>El ingrediente oculto con muchos nombres diferentes</strong></p>
<p>Es muy importante leer las etiquetas nutricionales y la lista de ingredientes de los alimentos ya que hay muchas maneras en las que <strong>el az&uacute;car puede aparecer en la lista de ingredientes:</strong> az&uacute;car de ma&iacute;z, dextrosa, sacarosa, glucosa, isoglucosa, fructosa, levulosa, maltosa, melaza, almid&oacute;n hidrolizado, az&uacute;car invertido, jarabe de ma&iacute;z, jarabe de agave, miel, miel de maple, etc.</p>
<p>La lista de ingredientes siempre comienza con el ingrediente a&ntilde;adido en mayor cantidad. Esto significa que si el az&uacute;car aparece en la parte superior de la lista, es probable que ese alimento sea rico en az&uacute;cares libres.</p>
<p>Utilizar las etiquetas nutricionales y la lista de ingredientes para <strong>comparar productos</strong> y elegir aquellos con las cantidades m&aacute;s bajas de az&uacute;cares a&ntilde;adidos.</p>
<p>&nbsp;</p>
<p><strong>Por lo tanto, &iquest;es necesario o recomendable eliminar todo el az&uacute;car de la dieta? &iquest;Es posible?</strong></p>
<p>Es pr&aacute;cticamente imposible eliminar todo el az&uacute;car de la dieta ya que &eacute;ste seguir&aacute; formando parte de alimentos b&aacute;sicos que comemos a diario: leche, frutas y verduras. Por otro lado, lo que s&iacute; podemos hacer es limitar la ingesta de alimentos con az&uacute;cares a&ntilde;adidos (bebidas con az&uacute;car, pasteles y galletas, dulces, postres l&aacute;cteos, etc.).</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3034, 4, N'¿Cómo reducir los azúcares añadidos de la dieta?', CAST(N'2020-02-10T15:37:52.397' AS DateTime), N'~/uploads/fotos/Articulo3034.jpg', N'<p>Hace unos d&iacute;as Loredana Arhip, dietista-nutricionista de UCEME, explicaba el concepto de az&uacute;cares a&ntilde;adidos. Hoy os deja unas recomendaciones para quienes quieran disminuir la ingesta de az&uacute;cares a&ntilde;adidos:</p>
<ul>
<li><strong>En el desayuno:</strong>
<ul>
<li>Elegir cereales sin az&uacute;cares a&ntilde;adidos, avena o pan integral a expensas de otras opciones.</li>
<li>Reducir poco a poco la cantidad habitual de az&uacute;car de mesa (blanco y moreno), fructosa, jarabes, miel o melaza que utiliza para endulzar el caf&eacute;, t&eacute;, etc. Alternativamente, cambiar a un edulcorante no cal&oacute;rico.</li>
<li>Disminuir la ingesta de mermelada, miel o chocolate.</li>
</ul>
</li>
</ul>
<p><strong>&nbsp;</strong></p>
<ul>
<li><strong>En los snacks:</strong>
<ul>
<li>Consumir las frutas enteras (evitando los zumos) y si es posible con la piel.</li>
<li>Evitar las frutas enlatadas en alm&iacute;bar, especialmente el jarabe. Escurrir y enjuagar en un colador para eliminar el exceso de jarabe o jugo.</li>
<li>Las opciones m&aacute;s saludables son aquellas que incluyen fruta, frutos secos, yogur natural, pan integral.</li>
</ul>
</li>
</ul>
<p>&nbsp;</p>
<ul>
<li><strong>En las comidas principales:</strong>
<ul>
<li>Limitar la ingesta de comidas preparadas, aderezos y salsas.</li>
<li>Para el postre, preparar productos de reposter&iacute;a en casa con menos az&uacute;cares a&ntilde;adidos.</li>
<li>Elegir agua, leche o bebidas sin az&uacute;car, diet&eacute;ticas o sin az&uacute;car a&ntilde;adido.</li>
</ul>
</li>
</ul>
<p>&nbsp;</p>
<p>En resumen, si deseas disminuir la ingesta de az&uacute;cares a&ntilde;adidos de su dieta, acost&uacute;mbrate a <strong>leer las etiquetas de </strong>los alimentos, <strong>comparar</strong> productos y <strong>elegir</strong> versiones con menos az&uacute;car o sin az&uacute;car.</p>
<p>&nbsp;</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3035, 4, N'Doctor, tengo un nódulo tiroideo…. ¿es cáncer?', CAST(N'2020-02-23T21:13:25.167' AS DateTime), N'~/uploads/fotos/Articulo3035.jpg', N'<p>Esta es una pregunta que, con mucha frecuencia, los pacientes hacen en la consulta y afortunadamente la respuesta a la mayor&iacute;a de los casos es, no.</p>
<p>El n&oacute;dulo tiroideo es un diagn&oacute;stico muy frecuente en nuestra especialidad y de hecho cada vez m&aacute;s, debido al empleo masivo de pruebas de imagen diagn&oacute;sticas como TC y ecograf&iacute;a. Esta situaci&oacute;n genera un importante volumen de diagn&oacute;stico incidental de n&oacute;dulos tiroideos asintom&aacute;ticos, conocidos como &ldquo;incidentalomas tiroideos&rdquo;.</p>
<p><strong>Ante la aparici&oacute;n de un n&oacute;dulo tiroideo no hay que alarmarse</strong>. Los estudios iniciales son sencillos, r&aacute;pidos e indoloros. Estas pruebas diagn&oacute;sticas perseguir&aacute;n conocer si el n&oacute;dulo es sintom&aacute;tico, genera exceso de producci&oacute;n de hormonas tiroideas o si es &ldquo;sospechoso&rdquo; de albergar un c&aacute;ncer de tiroides.</p>
<p>Los especialistas implicados en el manejo de la patolog&iacute;a tiroidea realizar&aacute;n una exploraci&oacute;n de la zona anterior del cuello (la gl&aacute;ndula es bastante superficial y accesible a la palpaci&oacute;n), asimismo solicitar&aacute;n como primeras pruebas complementarias un perfil tiroideo y una ecograf&iacute;a del tiroides. El perfil tiroideo incluye determinaci&oacute;n de TSH, hormonas tiroideas perif&eacute;ricas y, en ocasiones, estudio de autoinmunidad tiroidea. Con estas determinaciones podremos hacernos una idea de si el tiroides funciona bien, poco o tiene exceso de producci&oacute;n.</p>
<p><strong>La prueba de imagen por excelencia en el estudio de la patolog&iacute;a tiroidea es la ecograf&iacute;a cervical de alta resoluci&oacute;n</strong>, realizada, a ser posible, por un radi&oacute;logo con experiencia en ecograf&iacute;a tiroidea. Los n&oacute;dulos tiroideos tienen diferentes caracter&iacute;sticas que hay que recopilar durante la exploraci&oacute;n ecogr&aacute;fica: tama&ntilde;o, n&uacute;mero, localizaci&oacute;n, estructura ecogr&aacute;fica (s&oacute;lido, qu&iacute;stico, mixto), semejanza con el resto del par&eacute;nquima de la gl&aacute;ndula (iso, hipo o hiperecog&eacute;nico), presencia de halo, microcalcificaciones, tipo de flujo vascular, localizaci&oacute;n y extensi&oacute;n intra o extratiroidea, etc. Con todos estos datos el radi&oacute;logo emitir&aacute; una categor&iacute;a de riesgo, habitualmente en unos t&eacute;rminos que puedan ser comprendidos por cualquier especialista en patolog&iacute;a tiroidea. En la actualidad la m&aacute;s empleada es la clasificaci&oacute;n radiol&oacute;gica del Colegio Americano de Radiolog&iacute;a, que es una modificaci&oacute;n de la versi&oacute;n original dise&ntilde;ada y publicada por la Dra. Eleonora Horvath y se denomina ACR-TIRAS (American Collegue Radiology - Thyroid Imaging, Reporting and Data System). Este sistema delimita 5 categor&iacute;as de riesgo de malignidad por imagen, desde TIRADS 2: Benigno hasta TIRADS 5: altamente sospechoso. En base a estas categor&iacute;as y al tama&ntilde;o del n&oacute;dulo indicaremos realizar o no una peque&ntilde;a biopsia mediante una punci&oacute;n con una aguja fina del n&oacute;dulo, conocida como PAAF. Este procedimiento se realiza guiado por ecograf&iacute;a de forma ambulatoria, y en manos expertas, tiene m&iacute;nimas complicaciones y genera pocas molestias al paciente.</p>
<p>El material celular obtenido mediante la PAAF es analizado por un pat&oacute;logo especialista en citolog&iacute;a tiroidea y su diagn&oacute;stico expresa el riesgo de existencia de un c&aacute;ncer subyacente. Las categor&iacute;as benignas tendr&aacute;n un riesgo m&iacute;nimo (entorno a un 2%), las intermedias entorno a un 5-30% y las m&aacute;s altas alrededor de un 85-99%.</p>
<p>Como observamos, son muchos los factores que buscamos, analizamos y registramos a la hora de enfrentarnos a un n&oacute;dulo tiroideo y todo ello se realiza para ofrecer la actitud m&aacute;s acertada y no incurrir en exceso de tratamientos agresivos o en pautas de observaci&oacute;n no indicadas&hellip; <strong>La realidad es que la mayor parte de los n&oacute;dulos que diagnostiquemos ser&aacute;n benignos y s&oacute;lo un 5%, de forma global, albergar&aacute;n un c&aacute;ncer de tiroides</strong>.</p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (3036, 4, N'¿Cuándo se debe operar un nódulo tiroideo?', CAST(N'2020-03-01T11:33:14.553' AS DateTime), N'~/uploads/fotos/Articulo3036.png', N'<p>De forma general, los especialistas en patolog&iacute;a tiroidea recomendaremos operar un n&oacute;dulo tiroideo en las siguientes situaciones:</p>
<ul>
<li>N&oacute;dulos con sospecha intermedia o alta de c&aacute;ncer de tiroides.</li>
<li>N&oacute;dulos &uacute;nicos o m&uacute;ltiples cuyo tama&ntilde;o supere los 3-4 cm, a&uacute;n en presencia de citolog&iacute;a benigna, y siempre evaluando de forma individualizada cada caso por un experto.</li>
<li>Algunos n&oacute;dulos &uacute;nicos o m&uacute;ltiples que originen exceso de producci&oacute;n de hormonas tiroideas (algunos de estos pueden tratarse con nuevas t&eacute;cnicas como la radiofrecuencia tiroidea que es menos invasiva que la cirug&iacute;a).</li>
</ul>
<p>Estas indicaciones generales deben ser precisadas y explicadas por un equipo m&eacute;dico-quir&uacute;rgico con experiencia (en caso de encontrarse en una de estas situaciones, le recomendamos consulte con un especialista).</p>
<p>Una vez sentada la indicaci&oacute;n de cirug&iacute;a ser&aacute; necesario completar el estudio de planificaci&oacute;n quir&uacute;rgica, enfocado a ofrecer al paciente un <strong>tratamiento personalizado</strong>. La ex&eacute;resis de tiroides o tiroidectom&iacute;a puede ser de toda la gl&aacute;ndula o de s&oacute;lo una parte y puede asociar la resecci&oacute;n de ganglios adyacentes o regionales si es preciso. Actualmente ya no se realiza la ex&eacute;resis del n&oacute;dulo solamente por los malos resultados observados en el pasado.</p>
<p>Esta intervenci&oacute;n se realiza con anestesia general y <strong>su tiempo de hospitalizaci&oacute;n suele ser inferior a 24 horas</strong>. Las complicaciones de esta intervenci&oacute;n est&aacute;n en relaci&oacute;n con la posibilidad de sangrado postoperatorio, lesi&oacute;n de los nervios de las cuerdas vocales (nervios lar&iacute;ngeo recurrente y lar&iacute;ngeo superior) y de las gl&aacute;ndulas paratiroides, (encargadas de gestionar los niveles de calcio en el organismo). En grupos quir&uacute;rgicos con alto nivel de especializaci&oacute;n son de muy baja frecuencia y la mayor parte, con car&aacute;cter transitorio.</p>
<p><strong>La &uacute;nica secuela permanente es la cicatriz</strong> en la cara anterior de la base del cuello, que se emplea para acceder a la gl&aacute;ndula, y cuya extensi&oacute;n suele ser en torno a los 6-8 cm. Recientemente, en nuestro pa&iacute;s, se est&aacute;n introduciendo y asentando abordajes quir&uacute;rgicos encaminados a eliminar la cicatriz cervical ubicando las incisiones de acceso en lugares escondidos y empleando t&eacute;cnicas endosc&oacute;picas. Estos abordajes se desarrollaron y extendieron en pa&iacute;ses asi&aacute;ticos, con series de m&aacute;s de 500-1000 pacientes, y, en la actualidad, se est&aacute;n introduciendo en pa&iacute;ses occidentales.</p>
<p>Nuestro grupo, UCEME Endocrinolog&iacute;a Madrid, introdujo la <strong>tiroidectom&iacute;a endosc&oacute;pica por abordaje biaxilo-biareolar</strong> con excelentes resultados hasta el momento (Mercader Cidoncha E, Amunategui Prats I, Escat Cort&eacute;s JL, Grao Torrente I, Suh H. Cir Esp. 2019 Feb;97(2):81-88. doi: 10.1016/j.ciresp.2018.11.006. Epub 2019 Jan 26. Scarless neck thyroidectomy using bilateral axillo-breast approach: Initial impressions after introduction in a specialized unit and a review of the literature).</p>
<p>A modo de resumen, podemos concluir que <strong>la mayor parte de los n&oacute;dulos tiroideos de diagn&oacute;stico incidental ser&aacute;n benignos y no requerir&aacute;n cirug&iacute;a</strong>. Es importante ser evaluado por un equipo m&eacute;dico-quir&uacute;rgico con amplia experiencia que ofrezca un tratamiento preciso y ajustado a lo que el paciente y la patolog&iacute;a requieren. As&iacute;, reduciremos molestias al paciente y minimizaremos la morbilidad de los tratamientos. No dude en buscar consejo m&eacute;dico y aclarar todas las dudas.</p>
<p><span>&nbsp;</span></p>', 1)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4030, 16, N'Impacto ambiental de nuestra dieta', CAST(N'2020-03-12T11:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4030.jpg', N'<p>&iquest;Alguna vez te has planteado que tu dieta tiene un impacto ambiental? Aqu&iacute; tienes una amplia explicaci&oacute;n al respecto:</p>
<p></p>
<p><strong>Concepto de dieta sostenible</strong></p>
<p>Seg&uacute;n la FAO, las dietas sostenibles son aquellas dietas con <strong>bajo impacto ambiental</strong> que contribuyen a la seguridad alimentaria y nutricional y a una vida sana para las generaciones presentes y futuras. Las dietas sostenibles <strong>son protectoras y respetuosas con la biodiversidad y los ecosistemas</strong>, culturalmente aceptables, <strong>accesibles, econ&oacute;micamente justas y asequibles</strong><strong>; nutricionalmente adecuadas, seguras y saludables</strong> y, al mismo tiempo,<strong> optimizan los recursos naturales y humanos. </strong></p>
<p><strong>&nbsp;</strong></p>
<p><span><strong>M&eacute;todos de evaluaci&oacute;n del impacto ambiental </strong></span></p>
<p><span>El impacto ambiental de una dieta se eval&uacute;a </span>en base al m&eacute;todo de Evaluaci&oacute;n del Ciclo de Vida (o Life Cycle Assessment (LCA), en ingl&eacute;s). Es decir, esta evaluaci&oacute;n incluye el an&aacute;lisis de toda la cadena de suministro, incluido el cultivo y procesamiento de materias primas, fabricaci&oacute;n, embalaje, transporte, distribuci&oacute;n, uso, reutilizaci&oacute;n, reciclaje y disposici&oacute;n final. Podr&iacute;amos decir que esta evaluaci&oacute;n incluye el an&aacute;lisis de los pasos incluidos en la frase: &ldquo;desde campo a la mesa&rdquo;.</p>
<p>&nbsp;</p>
<p><strong>Los indicadores de sostenibilidad m&aacute;s utilizados son: la huella de carbono, </strong>que mide las emisiones de gases de efecto invernadero. Es el indicador m&aacute;s utilizado y referenciado en la literatura. Su unidad de medida son los equivalentes de CO2. Tambi&eacute;n se utiliza la<strong> huella h&iacute;drica</strong>, que es la cantidad de agua requerida para producir un recurso. Su unidad de medida suele ser los litros o hm<sup>3</sup>. Por &uacute;ltimo, tenemos la <strong>huella ecol&oacute;gica</strong>, que mide el &aacute;rea productiva que se requiere para producir los recursos. Unidad de medida: m2 o hect&aacute;reas globales.</p>
<p>&nbsp;</p>
<p><strong>Pero &iquest;qu&eacute; relaci&oacute;n tiene lo que comemos, con el impacto ambiental?</strong></p>
<p>En primer lugar, el 50% de la superficie habitable se dedica a la agricultura, que a su vez se utiliza para la cr&iacute;a del ganado y alimentos de origen vegetal. Adem&aacute;s de la superficie en s&iacute;, la agricultura es tambi&eacute;n uno de los sectores con mayor producci&oacute;n de gases con efecto invernadero. Es decir, la agricultura tiene una alta huella de carbono.</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4035, 16, N'13 consejos para comer bien durante la cuarentena', CAST(N'2020-03-26T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4035.jpg', N'<p>Loredana Arhip, dietista-nutricionista de UCEME, te ofrece estos consejos para comer mejor en estos tiempos de confinamiento:</p>
<ol>
<li>Mantener, en la medida de lo posible, el mismo horario de trabajo y comidas.</li>
<li>Es recomendable realizar 4-5 comidas al d&iacute;a en peque&ntilde;as cantidades.</li>
<li>Realizar una compra efectiva, evitando comprar alimentos en exceso, sobre todo de productos perecederos. Seleccionar alimentos de origen vegetal como frutas, verduras y hortalizas, legumbres, pan y cereales integrales.</li>
<li>Evitar comprar alimentos muy energ&eacute;ticos y poco saludables: dulces, patatas fritas, cacahuetes, etc. Es muy importante no tenerlos en casa durante estos d&iacute;as.</li>
<li>Tomar 2 raciones al d&iacute;a de verduras y hortalizas, de las cuales una debe ser cruda en ensalada.</li>
<li>Para los snacks toma, siempre con moderaci&oacute;n, fruta fresca, macedonia de fruta, yogures y frutos secos.</li>
<li>Guardar toda la comida en la cocina (nevera, despensa) y entrar en la cocina solo para preparar la comida/comer.</li>
<li>Evitar tener disponible comida por toda la casa. Si quieres picar algo, tienes que levantarte e ir hasta la cocina.</li>
<li>Retirar los snacks de la zona de trabajo (habitaci&oacute;n, mesa, etc.). Tampoco guardar snacks en los cajones de la mesa. Si quieres picar algo, tienes que levantarte e ir hasta la cocina.</li>
<li>Es importante mantenerse hidratado por lo que guarda siempre al lado una botella/vaso de agua.</li>
<li>Apuntar todo lo que se come a lo largo del d&iacute;a y si es posible todos los d&iacute;as.</li>
<li>Mantener un registro de peso semanal.</li>
<li>Por &uacute;ltimo, &iexcl;MU&Eacute;VETE! Actualmente hay disponibles un mont&oacute;n de rutinas de ejercicio para hacer en casa.</li>
</ol>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4040, 16, N'Consulta dietética online', CAST(N'2020-03-30T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4040.jpg', N'<p>&iquest;Necesitas atenci&oacute;n diet&eacute;tica personalizada? Para estos d&iacute;as de confinamiento, UCEME pone a tu disposici&oacute;n una <strong>consulta diet&eacute;tica online, atendida por Loredana Arhip.</strong>&nbsp;</p>
<p><strong>El horario de consulta es:</strong> todos los martes (16h-19h), a trav&eacute;s de Skype.</p>
<p>Para solicitar consulta, <strong>env&iacute;a un E-mail pidiendo cita a:</strong> <span><a href="mailto:info@someprivate.email">info@someprivate.email</a></span>&nbsp;</p>
<p>En el e-mail hay que proporcionar nombre completo, tel&eacute;fono y la direcci&oacute;n de Skype.</p>
<p>Y recuerda... &iexcl;Qu&eacute;date en casa!</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4044, 16, N'Patrón alimentario sostenible', CAST(N'2020-04-16T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4044.png', N'<p>Un patr&oacute;n alimentario sostenible incluye un <strong>aumento de alimentos de origen vegetal</strong> (frutas, verduras y hortalizas, cereales, frutos secos y legumbres) y una<strong> disminuci&oacute;n de alimentos de origen animal</strong>. Tambi&eacute;n se recomienda<strong> limitar la ingesta de az&uacute;cares y dulces, y utilizar alimentos de temporada</strong>.</p>
<p><strong>Para llevar a cabo un patr&oacute;n alimenticio sostenible, Loredana Arhip, dietista-nutricionista de UCEME, te recomienda seguir los siguientes pasos:</strong></p>
<p><strong></strong></p>
<p><strong>Organiza la compra:</strong></p>
<ul>
<li>Antes de ir a la compra, haz una foto de la nevera para no comprar alimentos que todav&iacute;a no se han gastado.</li>
<li>Haz una lista de la compra.</li>
<li>No olvides las bolsas reutilizables. Para evitar el uso de las bolsas de pl&aacute;stico, se pueden utilizar bolsas de tela para frutas y verduras.</li>
</ul>
<p>&nbsp;</p>
<p><strong>Organiza la nevera:</strong></p>
<ul>
<li>Es conveniente mantener un balance entre los alimentos que se compran y los que se consumen. Si sistem&aacute;ticamente sobran alimentos en la nevera (que muchas veces se tiran), procura disminuir la cantidad que se compra.</li>
<li>Para conservar los alimentos cortados en la nevera existen tapas de silicona adaptables y reutilizables. Tambi&eacute;n hay tapas de silicona para platos y tazones.&nbsp;</li>
</ul>
<ul>
<li>Para guardar los alimentos tambi&eacute;n se pueden utilizar bolsas herm&eacute;ticas reutilizables de silicona.&nbsp;</li>
</ul>
<ul>
<li>Para mantener la seguridad y la frescura de los alimentos guardados en la nevera, se recomienda distribuirlos de la siguiente manera:</li>
<ul>
<li><strong>Parte superior:</strong> yogures, latas, platos cocinados.</li>
<li><strong>Medio:</strong> <strong>huevos frescos</strong> (sin sacar de su caja de cart&oacute;n original), <strong>leche,</strong> carnes y pescados frescos/descongelados (guardar en bolsas de silicona para evitar goteos).</li>
<li><strong>Cajones inferiores cerrados:</strong> frutas y verduras.</li>
<li><strong>En la puerta:</strong> Salsas, mermeladas, mantequilla, margarina. <strong>Evita guardar productos perecederos en la puerta de la nevera ya que la temperatura no es constante.</strong></li>
</ul>
<li>Para evitar malos olores en la nevera, guarda.r en un recipiente bicarbonato de sodio.</li>
<li>Limpia la nevera peri&oacute;dicamente.</li>
</ul>
<p>&nbsp;<strong>&nbsp;</strong></p>
<p><strong>Organiza la mesa:</strong></p>
<ul>
<li>Utiliza manteles de tela en vez de manteles de papel.&nbsp;</li>
</ul>
<ul>
<li>Para preparar infusiones, utiliza infusores de t&eacute; de acero inoxidable.&nbsp;</li>
</ul>
<p><strong></strong></p>
<p><strong>Para llevar:</strong></p>
<ul>
<li>Cambia las pajitas de pl&aacute;stico por una de acero inoxidable.<strong>&nbsp;</strong></li>
</ul>
<ul>
<li>Cambia las botellas de pl&aacute;stico por botellas reutilizables.</li>
</ul>
<ul>
<li>Para envolver los alimentos, puedes utilizar envoltorios ecol&oacute;gicos reusables a base de cera de abeja (por ejemplo, los de&nbsp;<span><a href="https://beecool.es/"><em>https://beecool.es/</em></a></span>&nbsp;)</li>
</ul>
<p><strong>&nbsp;</strong></p>
<p><strong>Con estos sencillos pasos, adem&aacute;s de cuidar y ordenar tu alimentaci&oacute;n, contribuyes a reducir el impacto medioambiental de nuestra dieta. &iquest;A qu&eacute; est&aacute;s esperando para llevarlos a cabo?</strong></p>
<p><span>&nbsp;</span></p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4045, 16, N'Impacto ambiental de los alimentos', CAST(N'2020-04-19T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4045.png', N'<p><strong><span>Huella de carbono </span></strong></p>
<p><span>A peque&ntilde;a escala, el impacto ambiental de los alimentos tambi&eacute;n se puede medir. En t&eacute;rminos de huella de carbono, seg&uacute;n su producci&oacute;n de gases de efecto invernadero, los alimentos se pueden representar en un esquema con forma de pir&aacute;mide invertida. Con algunas excepciones, <strong>los alimentos de origen animal suelen tener mayor huella de carbono que los alimentos de origen vegetal (Figura 1). </strong></span></p>
<p><strong><span><img src="http://www.uceme.es/uploads/fotos/img1026.jpg" alt="" /><img src="http://www.endocrinologia-madrid.com/uploads/fotos/img1026.jpg" alt="Huella de carbono" width="729" height="391" /></span></strong></p>
<p><strong><span><img src="http://www.uceme.es/uploads/fotos/img1026.jpg" alt="" /></span></strong><strong><span></span></strong></p>
<p><strong><span>Raciones de alimentos </span></strong></p>
<p><span>Puesto que a lo que comemos nos referimos habitualmente como <strong>raciones</strong>, vamos a ver un ejemplo. El filete de carne de la cena de anoche habr&aacute; producido hasta llegar en el plato unos 330 g de equivalentes de CO2. Eso es como conducir un coche 5 km aproximadamenre. Si en vez de carne, se eligi&oacute; pescado esta cifra habr&aacute; disminuido a 40 g de equivalentes de CO2. En el caso de la prote&iacute;na vegetal la cifra es pr&aacute;cticamente nula.</span><span></span><span><img src="http://www.endocrinologia-madrid.com/uploads/fotos/img1027.jpg" alt="Alimentos proteicos" width="741" height="417" /></span></p>
<p><strong><span>&iquest;Por qu&eacute; ocurre esto?</span></strong></p>
<p><span>Esto sucede porque los rumiantes como vacas y ovejas son los mayores productores de gas metano, un potente gas con efecto invernadero. Es por ello por lo que la cr&iacute;a de los rumiantes para producci&oacute;n de los alimentos proteicos produce grandes cantidades de equivalentes de CO2.</span></p>
<p><strong><span>&nbsp;</span></strong></p>
<p><strong><span>Huella h&iacute;drica </span></strong></p>
<p><span>Los alimentos tambi&eacute;n se pueden representar en una forma de pir&aacute;mide al estudiar su huella de agua. En este caso, tambi&eacute;n con peque&ntilde;as excepciones<u>, <strong>los alimentos de origen animal tienen m&aacute;s huella h&iacute;drica que los alimentos de origen vegetal.</strong></u></span></p>
<p><span><u><strong></strong></u></span></p>
<p><img src="http://www.endocrinologia-madrid.com/uploads/fotos/img1028.jpg" alt="Huella h&iacute;drica de alimentos (FAO)" width="727" height="347" /></p>
<p><strong><span>&nbsp;</span></strong></p>
<p></p>
<p><strong><span>Huella ecol&oacute;gica</span></strong></p>
<p><span>Por &uacute;ltimo, tenemos la representaci&oacute;n de los alimentos seg&uacute;n su huella ecol&oacute;gica, e igual que en las situaciones anteriores, <strong><u>los alimentos de origen animal tienen m&aacute;s huella ecol&oacute;gica que los alimentos de origen vegetal (Figura 3). </u></strong></span></p>
<p><img src="http://www.endocrinologia-madrid.com/uploads/fotos/img1029.jpg" alt="Huella ecol&oacute;gica alimentos" width="716" height="497" /></p>
<p><span>&nbsp;</span></p>
<p><strong>Para resumir&hellip;</strong></p>
<p><strong>Huella de carbono de los alimentos: </strong>Principalmente, los alimentos de origen animal tienen mayor huella de carbono que los alimentos de origen vegetal.&nbsp;</p>
<p><strong>Huella h&iacute;drica de los alimentos: </strong>Principalmente, los alimentos de origen animal tienen mayor huella h&iacute;drica que los alimentos de origen vegetal.&nbsp;</p>
<p><strong>Huella ecol&oacute;gica de los alimentos: </strong>Los alimentos de origen animal tienen mayor huella ecol&oacute;gica que los alimentos de origen vegetal.<strong>&nbsp;</strong></p>
<p><strong>&nbsp;</strong></p>
<p><strong>DIETAS O PATRONES ALIMENTARIOS</strong></p>
<p>Puesto que los alimentos se consumen de una forma combinada, es importante <strong><u>valorar si el conjunto de la dieta o patr&oacute;n de alimentaci&oacute;n es sostenible.</u></strong></p>
<p>Seg&uacute;n los estudios actuales, <strong><span>las dietas que han demostrado tener un bajo impacto ambiental son: dieta vegana, dieta vegetariana y la dieta mediterr&aacute;nea.</span></strong> Esta &uacute;ltima, se presenta no solo como un modelo cultural, sino adem&aacute;s como un modelo saludable y respetuoso con el medio ambiente, cuya adhesi&oacute;n en Espa&ntilde;a contribuir&iacute;a significativamente a una mayor sostenibilidad de la producci&oacute;n y el consumo de alimentos, adem&aacute;s de los reconocidos beneficios para la salud de la poblaci&oacute;n.</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4047, 16, N'Libros sobre Nutrición', CAST(N'2020-04-23T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4047.png', N'<p>Para celebrar el D&iacute;a del Libro, Loredana Arhip, dietista-nutricionista de UCEME, os recomienda algunas lecturas sobre nutrici&oacute;n:</p>
<p>- <strong>"Los relojes de tu vida: Descubre cu&aacute;l es el ritmo biol&oacute;gico y c&oacute;mo mejorar tu bienestar".</strong> Marta Garaulet.</p>
<p>- <strong>"M&aacute;s vegetales, menos animales: Una alimentaci&oacute;n m&aacute;s saludables y sostenible"</strong>. Julio Basulto y Juanjo C&aacute;ceres.&nbsp;</p>
<p>- <strong>"The Man Who Fed the World"</strong>. Leon Hesser.&nbsp;</p>
<p>- <strong>"The Psychobiotic Revolution: Mood, Food, and the New Science of the Gut-Brain Connection"</strong>. Scott C. Anderson, John F. Cryan, Ted Dinan.&nbsp;</p>
<p>- <strong>"Eat Move Sleep: How Small Choices Lead to Big Changes"</strong>. Tom Rath.</p>
<p>- <strong>"When Food Is Comfort: Nurture Yourself Mindfully, Rewire Your Brain, and End Emotional Eating"</strong>. Julie M. Simon.</p>
<p>Esperamos que os resulten &uacute;tiles e interesantes.</p>
<p>&iexcl;Feliz D&iacute;a del Libro!</p>
<p></p>
<p><img src="http://www.endocrinologia-madrid.com/uploads/fotos/img1030.png" alt="Libros nutrici&oacute;n" width="626" height="313" style="display: block; margin-left: auto; margin-right: auto;" /></p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4051, 16, N'Cirugía Tiroidea, ¿cuándo es necesaria?', CAST(N'2020-05-07T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4051.png', N'<p>La gl&aacute;ndula tiroides es un &oacute;rgano impar cuya principal funci&oacute;n es producir las hormonas tiroideas, encargadas de regular nuestro metabolismo.</p>
<p>La gl&aacute;ndula tiroides tiene forma de mariposa y est&aacute; dividida en dos peque&ntilde;os l&oacute;bulos conectados por un istmo (los l&oacute;bulos ser&iacute;an las alas y el istmo ser&iacute;a el cuerpo de la mariposa).</p>
<p>Se encuentra situada en la parte anterior del cuello, justo por delante de la tr&aacute;quea (que es la v&iacute;a respiratoria principal).</p>
<p><span><b>1.1&nbsp;PROCESOS BENIGNOS QUE AFECTAN AL TIROIDES</b></span></p>
<p>El tiroides puede verse afectado por varias patolog&iacute;as benignas que pueden, no afectar a la producci&oacute;n hormonal, o bien causar un exceso (hipertiroidismo) o un defecto (hipotiroidismo) hormonal.<span></span></p>
<p>La intervenciones que se realizan sobre el tiroides conllevan la resecci&oacute;n de una parte de la gl&aacute;ndula. En funci&oacute;n de la cantidad de tiroides que se reseque se denominan:<span>&nbsp;</span><b><span>tiroidectom&iacute;a total, hemitiroidectom&iacute;a o tiroidectom&iacute;a subtotal</span></b><span>&nbsp;</span>(Ver glosario t&eacute;rminos).</p>
<p>Las principales patolog&iacute;as benignas que afectan a la gl&aacute;ndula son:</p>
<p>&nbsp;</p>
<ul>
<li><b>BOCIO MULTINODULAR</b></li>
</ul>
<p>El bocio es un aumento de tama&ntilde;o de la gl&aacute;ndula tiroides.</p>
<p>Intervienen muchos factores en su desarrollo, aunque el m&aacute;s importante es la deficiencia de yodo en la alimentaci&oacute;n.</p>
<p>Inicialmente el tiroides s&oacute;lo aumenta de tama&ntilde;o y posteriormente pueden aparecer n&oacute;dulos de diferentes tama&ntilde;os.</p>
<p>La producci&oacute;n hormonal puede ser normal, estar aumentada o disminuida.</p>
<p>La<span>&nbsp;</span><b>indicaci&oacute;n</b><span>&nbsp;</span>de Cirug&iacute;a viene determinada por:</p>
<p>- S&iacute;ntomas compresivos derivados del crecimiento progresivo de la gl&aacute;ndula y que pueden ocasionar desviaci&oacute;n de la tr&aacute;quea, dificultad para tragar o respirar.</p>
<p>- Datos sospechosos en el an&aacute;lisis de las c&eacute;lulas extraidas de los n&oacute;dulos por PAAF.</p>
<p>Generalmente se realiza<span>&nbsp;</span><span>TIROIDECTOMIA TOTAL&nbsp;</span>y en algunos casos muy seleccionados<span>&nbsp;</span><span>TIROIDECTOMIA SUBTOTAL.</span></p>
<p>&nbsp;</p>
<ul>
<li><b>ENFERMEDAD DE GRAVES BASEDOW.</b></li>
</ul>
<p>La enfermedad de Graves Basedow es una<span>&nbsp;</span><span>causa de Hipertiroidismo</span>( producci&oacute;n aumentada de hormonas tiroideas) , que suele asociarse a patolog&iacute;a ocular en forma de ojos prominentes o exoftalmos.</p>
<p>Se debe a un problema autoinmune que mantiene el tiroides constantemente estimulado.</p>
<p>Puede ser tratado con f&aacute;rmacos antitiroideos, radioiodo y con cirug&iacute;a &ndash;<span>&nbsp;</span><b><span>Tiroidectom&iacute;a total.</span></b></p>
<p>La tiroidectom&iacute;a total es el tratamiento que tiene mayor porcentaje de &eacute;xitos (&gt;90%), controla r&aacute;pidamente los s&iacute;ntomas y , en manos expertas, tiene pocas complicaciones.</p>
<p>&nbsp;</p>
<ul>
<li><b>N&Oacute;DULO TIROIDEO</b></li>
</ul>
<p>La gl&aacute;ndula tiroides puede ser asiento de n&oacute;dulos tiroideos &uacute;nicos.</p>
<p>Estos n&oacute;dulos deben ser valorados por un especialista, sobre todo para excluir que puedan ser un tumor maligno.</p>
<p>En este contexto, la<b><span>&nbsp;cirug&iacute;a</span></b><span>&nbsp;</span>puede tener un papel:</p>
<p>-Para completar el dian&oacute;stico, cuando no se ha podido realizar con otrs estudios.</p>
<p>-Para tratar en n&oacute;dulo, pudiendo ser necesario un hemitiroidectom&iacute;a o una tiroidetom&iacute;a total.</p>
<p>&nbsp;</p>
<ul>
<li><b>QUISTES TIROIDEOS RECIDIVANTES</b></li>
</ul>
<p>Los quistes tiroideos se diagnostican de forma adecuada por ecograf&iacute;a, que adem&aacute;s ofrece la posibilidad de tratarlos mediante punci&oacute;n y vaciado del mismo.</p>
<p>En ocasiones puede ser necesario indicar tratamiento quir&uacute;rgico bien por recidiva del quiste , tras varios aspirados o bien porque el quiste tenga un polo s&oacute;lido con dato de sospecha.</p>
<p>&nbsp;</p>
<p><strong><span>1.2 PROCESOS MALIGNOS QUE AFECTAN AL TIROIDES</span></strong></p>
<p>&nbsp;</p>
<p>El C&aacute;ncer de tiroides es el tumor m&aacute;s com&uacute;n de las gl&aacute;ndulas endocrinas. Su incidencia ha ido creciendo en los &uacute;ltimos a&ntilde;os debido al aumento del n&uacute;mero de ecograf&iacute;as cervicales que se realizan por otros motivos, y que permiten diagnosticas tumores en estad&iacute;as muy iniciales. A pesar de este hecho, la tasa de mortalidad se ha mantenido estable, ya que en general se trata de un tumor de buen pron&oacute;stico.</p>
<p>La incidencia actual se sit&uacute;a entorno a unos 9 casos por 100000 habitantes y a&ntilde;o.</p>
<p>Existen distintas variedades de C&aacute;ncer de Tiroides &ndash; Ver referencia en blog pacientes &ndash;</p>
<p>C&aacute;ncer diferenciado &ndash; Papilar o folicular.</p>
<p>C&aacute;ncer medular.</p>
<p>C&aacute;ncer anapl&aacute;sico.</p>
<p>&nbsp;</p>
<p>En todos los casos realizamos TIROIDECTOMIA TOTAL EXTRACAPSULAR para tratar el tumor principal.</p>
<p>&nbsp;</p>
<p>Dependiendo de la variedad histol&oacute;gica y del tama&ntilde;o tumoral, asociaremos a procedimiento previo, una reacci&oacute;n de los ganglios linf&aacute;ticos regionales o LINFADENECTOM&Iacute;A.</p>
<p>La linfadenectom&iacute;a puede realizarse sobre los ganglios paratraqueales, paralar&iacute;ngeos y paraesof&aacute;gicos y se denominar&aacute; LINFADENECTOMIA CENTRAL o bien puede realizarse sobre los ganglios que rodean al paquete vasculo-nervioso del cuello, llam&aacute;ndose, entonces, linfadenetom&iacute;a lateral o VACIAMIENTO LATERAL FUNCIONAL.</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4052, 16, N'¿Sabes qué es el hiperparatiroidismo?', CAST(N'2020-05-14T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4052.png', N'<p><span>El hiperparatiroidismo es la principal patolog&iacute;a que afecta a las gl&aacute;ndulas paratiroides.</span></p>
<p><span>El<strong>&nbsp;HIPERPARATIROIDISMO PRIMARIO</strong>&nbsp;puede deberse a:</span></p>
<p><span>-Adenoma Paratiroideo &uacute;nico.</span></p>
<p><span>-Adenoma Paratiroideo doble.</span></p>
<p><span>-Hiperplasia paratiroidea.</span></p>
<p><span>-Carcinoma Paratiroideo.</span></p>
<p><span>En UCEME realizamos<strong>&nbsp;paratiroidectom&iacute;a m&iacute;nimamente invasiva</strong>&nbsp;apoy&aacute;ndonos en:</span></p>
<p><span>- Adecuado diagn&oacute;stico de localizaci&oacute;n preoperatorio.</span></p>
<p><span>- Determinaci&oacute;n intraoperatoria de la PTH: La vida media de la PTH es de 5 minutos. Tras la ex&eacute;resis de la gl&aacute;dula o gl&aacute;ndulas afectas, realizamo an&aacute;lisis a los 5 minutos y a los 10 minutos postex&eacute;resis. La caida de la cifra de PTH, siguiendo unos criterios, determina el exito de la intervenci&oacute;n.</span></p>
<p><span>-Cirug&iacute;a Radioguiada: Marcaje preoperatorio de las gl&aacute;ndulas paratiroides con Tecnecio y posteriormente busqueda de las mismoas en el campo operatorio empleando una sonda de detecci&oacute;n portatil.</span></p>
<p><span>&nbsp;</span></p>
<p><span>En el caso de&nbsp;<strong>HIPERPARATIROIDISMO SECUNDARIO,&nbsp;</strong>la t&eacute;cnica que realizamos consiste en una paratiroidectomia subtotal o total con reimplante de un peque&ntilde;o fragmento de una de las gl&aacute;ndulas en un m&uacute;sculo del antebrazo.</span></p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4054, 16, N'Obesidad mórbida/ Síndrome metabólico', CAST(N'2020-05-21T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4054.png', N'<p></p>
<p>En UCEME ofrecemos nuestra experiencia en cirug&iacute;a laparosc&oacute;pica de obesidad m&oacute;rbida, realizando diferentes t&eacute;cnicas individualizando su indicaci&oacute;n seg&uacute;n las caracter&iacute;sticas de cada paciente. En concreto, realizamos Bypass g&aacute;strico, Gastrectom&iacute;a tubular, Derivaci&oacute;n biliopancre&aacute;tica.</p>
<p>El sobrepeso y la obesidad es una enfermedad cada vez m&aacute;s frecuente en los pa&iacute;ses desarrollados, llegando a cifras epid&eacute;micas en pa&iacute;ses como Estados Unidos.</p>
<p>Aunque es una enfermedad en s&iacute; misma, favorece adem&aacute;s la aparici&oacute;n de muchas otras como la hipertensi&oacute;n arterial, la diabetes mellitus, la hipertrigliceridemia.</p>
<p>El s&iacute;ndrome metab&oacute;lico consiste en diabetes mellitus asociada a hipertensi&oacute;n arterial, dislipemia, u obesidad.</p>
<p>En los &uacute;ltimos a&ntilde;os se ha observado que el s&iacute;ndrome metab&oacute;lico se corrige total o casi totalmente en pacientes obesos sometidos a cirug&iacute;a bari&aacute;trica (o cirug&iacute;a de la obesidad). Por esta raz&oacute;n se han comenzado a aplicar estas t&eacute;cnicas en pacientes diab&eacute;ticos no obesos. Los resultados a corto y medio plazo han sido muy optimistas, con altos porcentajes de curaci&oacute;n de la diabetes.</p>
<p>Ofrecemos en nuestro grupo la realizaci&oacute;n de cirug&iacute;a metab&oacute;lica laparosc&oacute;pica destinada al tratamiento de diabetes mellitus tipo II de mal control m&eacute;dico.</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4055, 16, N'UCEME, único grupo en España en realizar tiroidectomía sin cicatriz', CAST(N'2020-06-02T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4055.png', N'<p>&iquest;Sabes que en UCEME somos pioneros en Espa&ntilde;a en tiroidectom&iacute;a por abordaje extracervical?&nbsp;<strong>Somos el &uacute;nico grupo en Espa&ntilde;a en realizar &eacute;sta t&eacute;cnica y uno de los de mayor experiencia en Europa</strong></p>
<p>En la &uacute;ltima d&eacute;cada, liderados por los cirujanos endocrinos asi&aacute;ticos, se han desarrollado varias t&eacute;cnicas con el fin de poder extraer la gl&aacute;ndula tiroides evitando dejar cicatrices en el cuello. Una cirug&iacute;a tan segura como la tiroidea, presenta, como secuela m&aacute;s importante, una herida en un lugar tan visible como es el centro del cuello. En nuestro grupo quir&uacute;rgico nos planteamos el aprender las t&eacute;cnicas m&aacute;s reproducibles y vers&aacute;tiles de las muchas que se han descrito, y dise&ntilde;amos un programa de aprendizaje hace m&aacute;s de un a&ntilde;o. La primera parte del mismo consist&iacute;a en formaci&oacute;n te&oacute;rica, y posterior asistencia&nbsp;<strong>durante tres semanas al Hospital Mount Sina&iacute; de Nueva York, con los profesores Inabnet y Suh</strong>, pioneros en estas t&eacute;cnicas en EEUU. Esta primera parte culmin&oacute; en junio de 2017 con la realizaci&oacute;n de los primeros casos de abordaje biaxilo-bimamario por nuestro grupo habiendo realizado un total de quince casos.</p>
<p>Los resultados de los primeros casos que intervinimos fueron excelentes y totalmente equiparables a los de la cirug&iacute;a abierta (sin lesi&oacute;n nerviosa ni de paratiroides) pero sin cicatriz visible en el cuello.</p>', 0)
INSERT [dbo].[Blog] ([idBlog], [idUsuario], [titulo], [fecha], [foto], [texto], [profesional]) VALUES (4057, 16, N'Cirugía de Glándulas Suprarrenales', CAST(N'2020-06-10T00:00:00.000' AS DateTime), N'~/uploads/fotos/Blog4057.png', N'<p>Las gl&aacute;ndulas suprarrenales son dos hormonas localizadas encima de sendos ri&ntilde;ones cuya funci&oacute;n es la de regular las respuestas al estr&eacute;s a trav&eacute;s de la s&iacute;ntesis de corticoesteroides y catecolaminas.</p>
<p>Algunas enfermedades de las gl&aacute;ndulas suprarrenales precisan de un tratamiento quir&uacute;rgico</p>
<p>- Adenoma. Tumor benigno unilateral&nbsp;</p>
<p>- Feocromocitoma.&nbsp;</p>
<p>- Incidentaloma</p>
<p>- Carcinoma</p>
<p>- Met&aacute;stasis</p>
<p>En todos los casos proponemos como T&Eacute;CNICA DE ELECCI&Oacute;N la<strong>&nbsp;EXERESIS DE LA GL&Aacute;NDULA POR V&Iacute;A LAPAROSC&Oacute;PICA.&nbsp;</strong>T&eacute;cnnica de elecci&oacute;n en la actualidad con excelente tolerancia postoperatoria</p>
<p>Le recomendamos que visite nuestra secci&oacute;n de v&iacute;deos en la que mostramos varios casos de suprarrenalectom&iacute;a laparosc&oacute;pica</p>', 0)
SET IDENTITY_INSERT [dbo].[Blog] OFF
GO
SET IDENTITY_INSERT [dbo].[Cita] ON 

INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1025, 20150130, CAST(16.50 AS Decimal(5, 2)), N'MONICA VIGIL', N'pintosvigil@someprivate.email', N'654238025', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1028, 20150320, CAST(16.83 AS Decimal(5, 2)), N'Petra Bragulla', N'petra.bragulla@someprivate.email', N'609512019', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1029, 20150325, CAST(15.00 AS Decimal(5, 2)), N'Luis Valenzuela', N'luis.valenzuela@someprivate.email', N'649490455', 6)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1030, 20150327, CAST(16.00 AS Decimal(5, 2)), N'Concepción  Gómez Robledo ', N'saravanto@someprivate.email ', N'655134265', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1031, 20150410, CAST(16.00 AS Decimal(5, 2)), N'Concepción Gomez Robledo', N'saravanto@someprivate.email', N'655134265', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1032, 20150417, CAST(16.16 AS Decimal(5, 2)), N'MAGDALENA LEAL GOMEZ', N'', N'699755165', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1033, 20150519, CAST(17.83 AS Decimal(5, 2)), N'Mª BELÉN JIMÉNEZ ROMÁN', N'mbjimenez19@gmaillcom', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1034, 20150519, CAST(17.83 AS Decimal(5, 2)), N'Mª BELÉN JIMÉNEZ ROMÁN', N'mbjimenez19@gmaillcom', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1035, 20150519, CAST(17.83 AS Decimal(5, 2)), N'Mª BELÉN JIMÉNEZ ROMÁN', N'mbjimenez19@gmaillcom', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1036, 20150519, CAST(16.00 AS Decimal(5, 2)), N'Antonio ', N'Muñoz-ant@hotmail', N'915863132', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1037, 20150519, CAST(16.16 AS Decimal(5, 2)), N'Josefa Pino Pachon', N'', N'685840201', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1038, 20150609, CAST(16.00 AS Decimal(5, 2)), N'Concepción Gómez Robledo', N'saravanto@someprivate.email', N'655134265', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1039, 20150609, CAST(16.00 AS Decimal(5, 2)), N'Concepción Gómez Robledo', N'saravanto@someprivate.email', N'655134265', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1040, 20150622, CAST(16.00 AS Decimal(5, 2)), N'Tania', N'tazevedo@someprivate.email', N'606040674', 4)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1041, 20150602, CAST(16.00 AS Decimal(5, 2)), N'Alejandra Garcia Fernandez', N'alejandra-64@someprivate.email', N'661544576', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1042, 20150630, CAST(16.00 AS Decimal(5, 2)), N'Alejandra Garcia Fernandez', N'alejandra-64@someprivate.email', N'661544576', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1043, 20150707, CAST(17.83 AS Decimal(5, 2)), N'MARIA BELEN JIMÉNEZ ROMÁN', N'mbjimenez19@someprivate.email', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1044, 20150707, CAST(17.83 AS Decimal(5, 2)), N'MARIA BELEN JIMÉNEZ ROMÁN', N'mbjimenez19@someprivate.email', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1045, 20150707, CAST(17.83 AS Decimal(5, 2)), N'MARIA BELEN JIMÉNEZ ROMÁN', N'mbjimenez19@someprivate.email', N'649286592', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1048, 20151009, CAST(16.00 AS Decimal(5, 2)), N'Emma del Rio Redondo', N'', N'654538700', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1049, 20151030, CAST(16.00 AS Decimal(5, 2)), N'Emma del Río Redondo', N'emma_delrio@someprivate.email', N'610935513', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1050, 20151201, CAST(16.00 AS Decimal(5, 2)), N'Jessica Fernanda Borges Siqueira ', N'', N'638559056', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1051, 20160119, CAST(17.50 AS Decimal(5, 2)), N'Jessica Fernanda', N'Borges Siqueira', N'638559056', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1052, 20160119, CAST(16.50 AS Decimal(5, 2)), N'Mª Teresa  Esquina Perea', N'mayteespe@someprivate.email', N'625134428', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1053, 20160119, CAST(16.50 AS Decimal(5, 2)), N'Mª Teresa  Esquina Perea', N'mayteespe@someprivate.email', N'625134428', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1054, 20160119, CAST(16.00 AS Decimal(5, 2)), N'Mª Teresa  Esquina Perea', N'mayteespe@someprivate.email', N'625134428', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1055, 20160219, CAST(16.83 AS Decimal(5, 2)), N'Jessica Fernanda', N'Jeessiborges2801@someprivate.email', N'638559056', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1056, 20160308, CAST(16.66 AS Decimal(5, 2)), N'BEATRIZ GONZALEZ', N'beatriz-197@someprivate.email', N'679521765', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1057, 20160419, CAST(18.16 AS Decimal(5, 2)), N'BEATRIZ GONZALEZ', N'beatriz-197@someprivate.email', N'679521765', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1058, 20160419, CAST(18.16 AS Decimal(5, 2)), N'BEATRIZ GONZALEZ', N'beatriz-197@someprivate.email', N'679521765', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1059, 20160510, CAST(17.00 AS Decimal(5, 2)), N'M. Angeles Fernández Rodríguez ', N'', N'625995010', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1060, 20160527, CAST(16.83 AS Decimal(5, 2)), N'maria camacho', N'cropero@someprivate.email', N'915700947', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1061, 20160518, CAST(15.50 AS Decimal(5, 2)), N'Esteban Salas Varona', N'', N'638628580', 6)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1062, 20160518, CAST(15.33 AS Decimal(5, 2)), N'Esteban Salas Varona', N'salasvarona@someprivate.email', N'638628580', 6)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1063, 20160603, CAST(16.00 AS Decimal(5, 2)), N'MARIA CAMACHO', N'cropero@someprivate.email', N'915700947', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1064, 20160610, CAST(16.83 AS Decimal(5, 2)), N'Amparo Casarrubios Hoyos', N'', N'650917231', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (1065, 20160614, CAST(18.50 AS Decimal(5, 2)), N'BEATRIZ GONZALEZ', N'beatriz-197@someprivate.email', N'679521765', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2059, 20161025, CAST(18.00 AS Decimal(5, 2)), N'M.de los Ángeles Hernández Parro ', N'manghern@someprivate.email', N'630533087', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2060, 20161117, CAST(16.33 AS Decimal(5, 2)), N'Juan Manuel Manzano Camacho', N'manzanocamacho@someprivate.email', N'680984502', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2061, 20170117, CAST(17.00 AS Decimal(5, 2)), N'Francisco Javier Sánchez Burgos', N'jsburgos23@someprivate.email', N'669745636', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2062, 20170117, CAST(17.16 AS Decimal(5, 2)), N'Francisco Javier Sanchez BUrgos', N'jsburgos23@someprivate.email', N'669745636', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2063, 20170119, CAST(17.00 AS Decimal(5, 2)), N'Francisco JAvier Sánchez Burgos', N'jsburgos23@someprivate.email', N'669745636', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2064, 20170124, CAST(17.50 AS Decimal(5, 2)), N'Mª ,Angeles hernández', N'manghern@someprivate.email', N'630533087', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2065, 20170202, CAST(16.50 AS Decimal(5, 2)), N'Mª Ángeles Hernández Parro', N'manghern@someprivate.email', N'630533087', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2066, 20170523, CAST(17.00 AS Decimal(5, 2)), N'Pilar García Sanz ', N'mpgsanz@someprivate.email ', N'626320110', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2067, 20170622, CAST(17.33 AS Decimal(5, 2)), N'Mercedes de Miguel Muñoz', N'mdemiguel@someprivate.email', N'699375892', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2068, 20170627, CAST(16.00 AS Decimal(5, 2)), N'Virginia Navarro Blanco', N'Virnanavarro@someprivate.email', N'649630420', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2069, 20170627, CAST(16.16 AS Decimal(5, 2)), N'Virginia Navarro Blanco', N'Virnanavarro@someprivate.email', N'649630420', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2070, 20170727, CAST(15.50 AS Decimal(5, 2)), N'Nathan Sowatskey', N'Nathan@someprivate.email', N'638083675', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2071, 20170801, CAST(15.00 AS Decimal(5, 2)), N'Nathan Sowatskey', N'nathan@someprivate.email', N'638083675', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2072, 20170801, CAST(16.50 AS Decimal(5, 2)), N'marta', N'martavillal@someprivate.email', N'696298191', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2073, 20170905, CAST(17.00 AS Decimal(5, 2)), N'RITA', N'rita.escudero@someprivate.email', N'661367615', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2074, 20171005, CAST(17.33 AS Decimal(5, 2)), N'Juan Ramón Ordóñez Becerra', N'jr.ordonezbecerra@someprivate.email', N'618717951', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2075, 20171005, CAST(17.16 AS Decimal(5, 2)), N'Juan Ramón Ordóñez Becerra', N'jr.ordonezbecerra@someprivate.email', N'618717951', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2081, 20181106, CAST(16.66 AS Decimal(5, 2)), N'Marta Caballero ', N'martacm36@someprivate.email', N'636090153', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2082, 20181206, CAST(15.50 AS Decimal(5, 2)), N'Amanda', N'amanda_garcia_g@someprivate.email', N'627764494', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2083, 20190124, CAST(15.50 AS Decimal(5, 2)), N'RAUL CEFERINO GIL MARTIN', N'raul.gil.martin@someprivate.email', N'655811023', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2084, 20190124, CAST(15.66 AS Decimal(5, 2)), N'RAÚL CEFERINO GIL MARTÍN', N'raul.gil.martin@someprivate.email', N'655811023', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2085, 20190131, CAST(17.16 AS Decimal(5, 2)), N'Vanessa Estefania Gonzalez', N'vanessa.gonzalez@someprivate.email', N'677849865', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2086, 20190131, CAST(17.16 AS Decimal(5, 2)), N'Vanessa Estefania Gonzalez', N'vanessa.gonzalez@someprivate.email', N'677849865', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2087, 20190311, CAST(15.16 AS Decimal(5, 2)), N'Maria Jose yela', N'mjyela@someprivate.email', N'654038319', 4)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2088, 20190326, CAST(18.66 AS Decimal(5, 2)), N'Carolina Calvo', N'calvocalle@someprivate.email', N'652314031', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2089, 20190404, CAST(15.50 AS Decimal(5, 2)), N'Montserrat Martin de Arriba', N'mmartinarriba@someprivate.email', N'619207807', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2090, 20190416, CAST(15.00 AS Decimal(5, 2)), N'CARMEN Cerrada', N'cerrada.carmen@someprivate.email', N'609063545', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2091, 20190506, CAST(15.83 AS Decimal(5, 2)), N'Nuria', N'ngrelag@someprivate.email', N'658515366', 4)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2092, 20190418, CAST(15.50 AS Decimal(5, 2)), N'bb', N'kakoap@someprivate.email', N'456543234', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2093, 20190418, CAST(15.66 AS Decimal(5, 2)), N'pp', N'kakoap@someprivate.email', N'679819173', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2094, 20190606, CAST(15.50 AS Decimal(5, 2)), N'Raúl Ceferino Gil Martín ', N'raul.gil.martin@someprivate.email', N'655811023', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2095, 20190516, CAST(15.50 AS Decimal(5, 2)), N'ij', N'iamunateguiprats@someprivate.email', N'679819173', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2096, 20190528, CAST(17.33 AS Decimal(5, 2)), N'Nuria', N'ngrelag@someprivate.email', N'658515366', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2097, 20190528, CAST(16.66 AS Decimal(5, 2)), N'Nuria', N'ngrelag@someprivate.email', N'658515366', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2098, 20190919, CAST(17.50 AS Decimal(5, 2)), N'Carolina Moreno', N'c.moreno@someprivate.email', N'661083060', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2099, 20190912, CAST(17.50 AS Decimal(5, 2)), N'Carolina Moreno', N'c.moreno@someprivate.email', N'661083060', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2100, 20190903, CAST(19.00 AS Decimal(5, 2)), N'Elisa Ramos', N'e.ramos@someprivate.email', N'607180784', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2101, 20190905, CAST(16.50 AS Decimal(5, 2)), N'Dorel Liviu Omania', N'pepedorel@someprivate.email', N'647887305', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2102, 20191015, CAST(16.33 AS Decimal(5, 2)), N'jose maria lopez', N'jlopezp@someprivate.email', N'630942555', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2103, 20190924, CAST(17.66 AS Decimal(5, 2)), N'Nuria', N'Nrincon70@someprivate.email', N'692103080', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2104, 20190926, CAST(17.50 AS Decimal(5, 2)), N'Nuria', N'nrincon70@someprivate.email', N'692103080', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2105, 20190926, CAST(17.16 AS Decimal(5, 2)), N'Nuria ', N'nrincon70@someprivate.email', N'692103080', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2106, 20191003, CAST(15.50 AS Decimal(5, 2)), N'Raúl Ceferino Gil Martín ', N'raul.gil.martin@someprivate.email', N'655811023', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2107, 20190924, CAST(16.33 AS Decimal(5, 2)), N'Susana Gil', N'sgilreg@someprivate.email', N'690031113', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2108, 20190924, CAST(16.33 AS Decimal(5, 2)), N'Susana Gil', N'sgilreg@someprivate.email', N'690031113', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2109, 20191016, CAST(15.83 AS Decimal(5, 2)), N'Arancha Sanchez Velasco', N'a.sanchezvel@someprivate.email', N'646817001', 6)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2110, 20191127, CAST(15.83 AS Decimal(5, 2)), N'Sonsoles Bienes', N'sonsobienes1214@someprivate.email', N'652205028', 6)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2111, 20200128, CAST(15.00 AS Decimal(5, 2)), N'Raúl Ceferino Gil Martín', N'raul.gil.martin@someprivate.email', N'655811023', 2)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2112, 20200220, CAST(17.50 AS Decimal(5, 2)), N'LOLA MARTINEZ', N'lolamartinezbernabeu@someprivate.email', N'629199152', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2113, 20200220, CAST(17.50 AS Decimal(5, 2)), N'Lola Martinez', N'lolamartinezbernabeu@someprivate.email', N'620199152', 7)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (2114, 20200217, CAST(11.50 AS Decimal(5, 2)), N'IRENE PARRA GOMIS', N'ireneparra25@someprivate.email', N'622411701', 12)
INSERT [dbo].[Cita] ([idCita], [dia], [hora], [nombre], [email], [telefono], [idTurno]) VALUES (3095, 20200409, CAST(15.50 AS Decimal(5, 2)), N'Lola Martinez', N'lolamartinezbernabeu@someprivate.email', N'620199152', 7)
SET IDENTITY_INSERT [dbo].[Cita] OFF
GO
SET IDENTITY_INSERT [dbo].[Companias] ON 

INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (1, N'Adeslas', N'~/uploads/fotos/comp1.GIF', N'http://www.adeslas.es')
INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (2, N'Mapfre', N'~/uploads/fotos/comp2.jpg', N'http://www.mapfre.es')
INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (3, N'Sanitas', N'~/uploads/fotos/comp3.gif', N'http://www.sanitas.es/')
INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (4, N'Asisa', N'~/uploads/fotos/comp4.jpeg', N'http://www.asisa.es/')
INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (1006, N'Colegio de Abogados', N'~/uploads/fotos/comp1006.gif', N'http://www.icam.es')
INSERT [dbo].[Companias] ([idCompanias], [nombre], [foto], [link]) VALUES (1011, N'HNA', N'~/uploads/fotos/comp1011.jpg', N'https://www.hna.es')
SET IDENTITY_INSERT [dbo].[Companias] OFF
GO
SET IDENTITY_INSERT [dbo].[Curriculum] ON 

INSERT [dbo].[Curriculum] ([idCurriculum], [Titulo], [Text]) VALUES (1, N'Cirujano General y del Aparato Digestivo', N'(Universidad Complutense de Madrid)')
INSERT [dbo].[Curriculum] ([idCurriculum], [Titulo], [Text]) VALUES (3, N'Cirujano General y del Aparato Digestivo.', N'(Universidad Complutense de Madrid)')
INSERT [dbo].[Curriculum] ([idCurriculum], [Titulo], [Text]) VALUES (7, N'Cirujano General y del Aparato Digestivo', NULL)
INSERT [dbo].[Curriculum] ([idCurriculum], [Titulo], [Text]) VALUES (8, N'Rellenar por el usuario', NULL)
INSERT [dbo].[Curriculum] ([idCurriculum], [Titulo], [Text]) VALUES (9, N'Rellenar por el usuario', NULL)
SET IDENTITY_INSERT [dbo].[Curriculum] OFF
GO
SET IDENTITY_INSERT [dbo].[DatosContacto] ON 

INSERT [dbo].[DatosContacto] ([idDatosContacto], [email], [telefono], [direccion]) VALUES (1, N'kakoap@someprivate.email', N'911111111', N'c/ Sanchez Barcaiztegui')
INSERT [dbo].[DatosContacto] ([idDatosContacto], [email], [telefono], [direccion]) VALUES (2, N'enriquemercader@someprivate.email', N'912222222', N'C/ Vinateros')
INSERT [dbo].[DatosContacto] ([idDatosContacto], [email], [telefono], [direccion]) VALUES (6, N'prueba2@someprivate.email', N'12345689', N'calle huerto')
INSERT [dbo].[DatosContacto] ([idDatosContacto], [email], [telefono], [direccion]) VALUES (7, N'juliocejudo@someprivate.email', N'6', N'6')
INSERT [dbo].[DatosContacto] ([idDatosContacto], [email], [telefono], [direccion]) VALUES (8, N'juliocejudo@someprivate.email', N'6', N'6')
SET IDENTITY_INSERT [dbo].[DatosContacto] OFF
GO
SET IDENTITY_INSERT [dbo].[DatosProfesionales] ON 

INSERT [dbo].[DatosProfesionales] ([idDatosPro], [nombre], [telefono], [email], [direccion], [texto], [foto], [activo]) VALUES (1, N'Hospital Beata María Ana', N'91.573.20.22', N'www.hospitalbeata.org', N'c/ Doctor Esquerdo, 83, Madrid', N'<iframe width="225" height="220" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://www.google.es/maps?f=q&amp;source=s_q&amp;hl=es&amp;geocode=&amp;q=beata+maria+ana&amp;aq=&amp;sll=40.418286,-3.669349&amp;sspn=0.019571,0.038581&amp;ie=UTF8&amp;hq=beata+maria+ana&amp;hnear=&amp;t=m&amp;cid=14732725101197434751&amp;ll=40.420554,-3.670979&amp;spn=0.028751,0.038624&amp;z=13&amp;iwloc=A&amp;output=embed"></iframe>             <br />             <small><a href="https://www.google.es/maps?f=q&amp;source=embed&amp;hl=es&amp;geocode=&amp;q=beata+maria+ana&amp;aq=&amp;sll=40.418286,-3.669349&amp;sspn=0.019571,0.038581&amp;ie=UTF8&amp;hq=beata+maria+ana&amp;hnear=&amp;t=m&amp;cid=14732725101197434751&amp;ll=40.420554,-3.670979&amp;spn=0.028751,0.038624&amp;z=13&amp;iwloc=A" style="color: #0000FF; text-align: left">Ver mapa más grande</a></small>', N'~/uploads/fotos/hosp1.jpg', 1)
INSERT [dbo].[DatosProfesionales] ([idDatosPro], [nombre], [telefono], [email], [direccion], [texto], [foto], [activo]) VALUES (2, N'Hospital Nuestra Señora del Rosario', N'91.435.91.00', N'www.hospitalrosario.com', N'c/ Principe de Vergara, 53, Madrid', N'<iframe width="225" height="220" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.es/maps?ie=UTF8&amp;q=Hospital+Nuestra+Se%C3%B1ora+del+Rosario&amp;fb=1&amp;gl=es&amp;hq=Hospital+Nuestra+Se%C3%B1ora+del+Rosario&amp;hnear=0xd422997800a3c81:0xc436dec1618c2269,Madrid&amp;cid=0,0,7442511807078869611&amp;t=m&amp;ll=40.43202,-3.679819&amp;spn=0.003593,0.004807&amp;z=16&amp;iwloc=A&amp;output=embed"></iframe><br /><small><a href="https://maps.google.es/maps?ie=UTF8&amp;q=Hospital+Nuestra+Se%C3%B1ora+del+Rosario&amp;fb=1&amp;gl=es&amp;hq=Hospital+Nuestra+Se%C3%B1ora+del+Rosario&amp;hnear=0xd422997800a3c81:0xc436dec1618c2269,Madrid&amp;cid=0,0,7442511807078869611&amp;t=m&amp;ll=40.43202,-3.679819&amp;spn=0.003593,0.004807&amp;z=16&amp;iwloc=A&amp;source=embed" style="color:#0000FF;text-align:left">Ver mapa más grande</a></small>', N'~/uploads/fotos/logorosario.png', 0)
SET IDENTITY_INSERT [dbo].[DatosProfesionales] OFF
GO
SET IDENTITY_INSERT [dbo].[Documento] ON 

INSERT [dbo].[Documento] ([idDocumento], [nombre], [link], [idUsuario]) VALUES (3026, N'Cirugía de la glándula tiroides', N'../Uploads/Documentos/doc3026.pdf', 4)
INSERT [dbo].[Documento] ([idDocumento], [nombre], [link], [idUsuario]) VALUES (3027, N'Cirugía de las glándulas paratiroides', N'../Uploads/Documentos/doc3027.pdf', 4)
INSERT [dbo].[Documento] ([idDocumento], [nombre], [link], [idUsuario]) VALUES (3028, N'Cirugía suprarrenal laparoscópica', N'../Uploads/Documentos/doc3028.pdf', 4)
INSERT [dbo].[Documento] ([idDocumento], [nombre], [link], [idUsuario]) VALUES (3029, N'Bypass Gástrico Laparoscópico', N'../Uploads/Documentos/doc3029.pdf', 4)
INSERT [dbo].[Documento] ([idDocumento], [nombre], [link], [idUsuario]) VALUES (3030, N'Cirugía bariátrica laparoscópica', N'../Uploads/Documentos/doc3030.pdf', 4)
SET IDENTITY_INSERT [dbo].[Documento] OFF
GO
SET IDENTITY_INSERT [dbo].[Faq] ON 

INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Qué es un nódulo en el Tiroides?', N'Los nódulos tiroideos son lesiones de aspecto esférico que pueden aparecer en cualquier parte del tiroides y a cualquier edad. Estas lesiones pueden diagnosticarse por la exploración física, al ser palpables en el cuello, o bien ser diagnosticadas por una ecografía. Las principales dudas que resolveremos con el estudio del nódulo son: (1) Valorar si produce síntomas, (2) Determinar si produce hormonas, (3) Excluir malignidad (15% de los nódulos tiroideos pueden ser un cáncer de tiroides). Todas estas dudas quedaran resueltas consultando con el especialista adecuado y realizando unos sencillos estudios.', 2006)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Debo operarme un nódulo en el Tiroides?', N'No, al menos de entrada, y mucho menos sin un estudio completo. Lo mas importante es diagnosticar de forma adecuada y precisa el origen del nódulo. Si tras el estudio pormenorizado existe sospecha de MALIGNIDAD, se debe indicar cirugía. De la igual forma si es un nódulo de GRAN TAMAÑO que produce síntomas por compresión de las vísceras del cuello (desplazamiento de la traquea, por ejemplo), también debe indicarse cirugía. En otras ocasiones en tiroides tiene múltiples nódulos benignos de pequeño tamaño (bocio multinodular) que inicialmente puede no requerir tratamiento quirúrgico. En cualquier caso las decisiones son casi siempre personalizadas. Hay que tener en cuenta diversos factores que pueden condicionar la decisión final de hacer o no cirugía. ', 2008)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Qué es un Bócio Multinodular?', N'El bócio es un aumento de tamaño del tiroides debido a la presencia de dos o más nódulos. Puede deberse a varias causas. El défict de yodo en la dieta es la más importante, si bien es cierto que la adición de yodo a la sal de mesa ha hecho casi desaparecer esta causa en nuestro medio. Otras causas implicadas en el bocio son los factores alimenticios. Algunos alimentos causan trastornos en el proceso de formación de las hormonas tiroideas provocando, a largo plazo, bocio. Inicialmente el bocio es difuso, pero tras un tiempo de estimulación prolongada, se produce un crecimiento desordenado, generándose nódulos. El bocio puede asociarse a una producción hormonal normal, en exceso o en defecto. ', 2009)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Qué es una PAAF?', N'Responde a Punción-Aspiración con Aguja Fina. Se trata de un método diagnóstico que consiste en pinchar el tiroides con una aguja muy fina y guiado por ecografía. Con este método obtenemos una pequeña muestra del tiroides, de  una forma completamente segura y con mínima molestia. Esta muestra se remite a un patólogo, el cual la examina al microscopio y nos proporciona una información muy útil para el diagnóstico adecuado.', 2010)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Cómo queda la cicatriz del cuello tras la cirugía del tiroides?', N'En la cirugía tiroidea se realiza una incisión de aproximadamente 5 centímetros. Cerramos la piel con un punto que va por dentro de la herida, de forma que la cicatriz es mínima. Siguiendo las indicaciones en el postoperatorio queda casi imperceptible. Recomendamos visitar nuestra galería de imágenes.', 2011)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿En qué consiste el ingreso para cirugía del tiroides?', N'La mayor parte de las cirugías del tiroides son mínimamente agresivas de forma que el tiempo de ingreso se estima entre unas pocas horas y un día. ', 2012)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Es necesario quitar siempre todo el tiroides?', N'No, no siempre quitamos la glándula al completo. Algunas patologías permiten conservar medio tiroides o una parte del tiroides, de esta forma evitamos tomar hormona tiroidea en el postoperatorio.', 2013)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Ganaré peso tras la cirugía del tiroides?', N'La glándula tiroides segrega una hormona que, cuando es deficiente, produce aumento de peso. Esta hormona se puede sustituir con un sencillo tratamiento oral, con una pastilla al día. Tras la extirpación del tiroides se pauta dicho tratamiento, sustituyendo de forma sencilla y segura la función que la glándula realizaba. Por tanto no, no se gana peso tras la cirugía del tiroides.', 2014)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Que es el Iodo Radioactivo?', N'El iodo radioactivo es fármaco que utiliza las propiedades radioactivas del Tecnecio con una finalidad diagnóstica o terapéutica. La administración de este producto se realiza, siempre, por un especialista en Medicina Nuclear. La cantidad de radiación que se suministra es muy escasa y no supone un riesgo para el paciente. Las principales áreas de aplicación son en el diagnóstico de funcionalidad de un nódulo del tiroides y en el tratamiento del hipertiroidismo o el cáncer de tiroides.', 2015)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Es grave un cáncer de Tiroides?', N'El Cáncer de Tiroides en un patología compleja que requiere un asesoramiento individualizado. En el diagnóstico, tratamiento y seguimiento de esta patología colaboran múltiples especialistas que conforman un grupo de trabajo multidisciplinar. Si usted padece esta patología le recomendamos solicite cita en nuestra unidad para evaluar en detalle su caso.', 2016)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿ Cómo es la pérdida de peso tras cirugía bariátrica?', N'Tras la cirugía bariátrica existen tres etapas bien definidas. Una primera de pérdida de mucho peso de forma rápida, que dura unos seis meses. Una segunda en la que la pérdida se hace más lenta, que continúa hasta los dos años. Una tercera que depende fundamentalmente de los  nuevos hábitos adquiridos por el paciente. El seguimiento por parte de un equipo especializado en dicha patología hace que las tres etapas consigan mejores resultados ', 2017)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Recuperaré peso tras la cirugía bariátrica?', N'Tras la cirugía bariátrica se pierde mucho peso, mejorando la calidad de vida del paciente de forma muy importante. Esto conlleva un cambio en los hábitos de vida que ayuda en el mantenimiento de la pérdida de peso. El mantenimiento de estos hábitos saludables es de gran importancia, por lo que recomendamos a nuestros pacientes seguir un control con los especialistas en nutrición de nuestro grupo', 2018)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Puedo comer de todo tras la cirugía bariátrica?', N'Al reducir la capacidad del estómago, existe un periodo tras la cirugía en el que se debe llevar un control estricto de lo que se come. Este control lo realiza el médico especialista en nutrición, y este período dura entre uno y tres meses, según cada caso. Posteriormente se puede comer de todo tratando de que sea lo más equilibrado posible.', 2019)
INSERT [dbo].[Faq] ([titulo], [texto], [idFaq]) VALUES (N'¿Cómo son las heridas tras la cirugía laparoscópica de obesidad?', N'Según la técnica realizada hacemos cuatro o cinco pequeñas incisiones de aproximadamente un centímetro y medio cada una. No precisan curas postoperatorias y los puntos los retiramos en 7 a 10 días. Para más información solicite cita en nuestra consulta', 2020)
SET IDENTITY_INSERT [dbo].[Faq] OFF
GO
SET IDENTITY_INSERT [dbo].[Fotos] ON 

INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (11, N'~/uploads/fotos/img11.jpg', N'Equipo quirúrgico', 1, 1)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (12, N'~/uploads/fotos/img12.jpg', N'Cirugía Laparoscópica de la Obesidad', 0, 6)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (13, N'~/uploads/fotos/img13.jpg', N'Cirugía Cervical empleando Gafas Lupa', 0, 7)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (21, N'~/uploads/fotos/img21.png', N'Hospital Beata Maria Ana', 0, 8)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1017, N'~/uploads/fotos/img1017.jpg', N'Incisión quirúrgica habitual', 0, 9)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1018, N'~/uploads/fotos/img1018.jpg', N'Doctor Amunategui operando', 0, 10)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1019, N'~/uploads/fotos/img1019.jpg', N'Aspecto tras operar', 0, 11)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1021, N'~/uploads/fotos/img1021.jpg', N'Tiroidectomía por abordaje biaxilo-biareolar', 0, 2)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1022, N'~/uploads/fotos/img1022.png', N'Con el Profesor Inabnet, nuestro mentor en Mount Sinaí, Nueva York', 0, 5)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1023, N'~/uploads/fotos/img1023.JPG', N'El Profesor Suh, de Nueva York, nos asistió en los primeros BABA', 0, 4)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1024, N'~/uploads/fotos/img1024.png', N'Publicamos nuestra experiencia inicial en tiroidectomía por abordaje extracervical', 0, 3)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1026, N'~/uploads/fotos/img1026.jpg', N'Pirámide huella de carbono (arc eurobanan)', 0, 12)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1027, N'~/uploads/fotos/img1027.jpg', N'Alimentos proteicos', 0, 13)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1028, N'~/uploads/fotos/img1028.jpg', N'Huella hídrica alimentos (FAO)', 0, 14)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1029, N'~/uploads/fotos/img1029.jpg', N'Huella ecológica alimentos', 0, 15)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1030, N'~/uploads/fotos/img1030.png', N'libros-nutrición', 0, 16)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1031, N'~/uploads/fotos/img1031.jpg', N'Dr Enrique Mercader', 0, 17)
INSERT [dbo].[Fotos] ([idFoto], [nombre], [texto], [destacada], [posicion]) VALUES (1033, N'~/uploads/fotos/img1033.jpg', N'Diagnostico ecográfico intraoperatorio', 0, 18)
SET IDENTITY_INSERT [dbo].[Fotos] OFF
GO
SET IDENTITY_INSERT [dbo].[ItemCurriculum] ON 

INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (1, N'Licenciado en Medicina por la Universidad Complutense de Madrid', NULL, N'', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (7, N'Titulado Superior Especialista en Cirugía General y del Aparato Digestivo', NULL, N'Formación MIR en Hospital General Universitario Gregorio Marañón, Madrid)', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (9, N'Cirujano Adjunto del Servicio de Cirugía General', N'2006 - Actualidad', N'Hospital General Universitario Gregorio Marañon de Madrid', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (10, N'Superespecialización en Cirugía Endocrina Cervical y Abdominal', N'Cirujano Adjunto de la Sección de Cirugía Endocrinometabólica ', N'Hospital General Universitario Gregorio Marañon de Madrid', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (11, N'Formación avanzada en Oncología Endocrina y Neuromonitorización intraoperatoria.', NULL, N'Coordinación y realización de diversos seminarios y cursos específicos anualmente', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (12, N'Especial interés y formación en Técnicas de Tiroidectomía y Paratiroidectomía minimamente invasivas', N'Desarrollo del abordaje Biaxilo-Biareolar en España', N'', 1)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3023, N'Licenciado en Medicina por la Universidad Complutense de Madrid', NULL, N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3025, N'Especialista en Cirugía General y del Aparato Digestivo.', N'Formado en el Hospital General Universitario Gregorio Marañón de Madrid.', N'Servicio de Cirugía General I.', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3027, N'Superespecialización en Cirugía Endocrino-Metabólica y Obesidad', N'Fellow European Board Surgical Qualification - Endocrine Surgery (Neck and Abdominal)', N'Titulo avalado por la Universidad de Especialidades Quirúrgicas (UEMS)', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3028, N'Cirujano adjunto en el Hospital Universitario Gergorio Marañón de Madrid', N'Unidad de Cirugía Endocrino-Metabólica del Servicio de Cirugía General ', N'Realización de unos 350 procedimientos anuales', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3029, N'Formación avanzada en el campo de la Oncología Endocrina.', N'Múltiples congresos,seminarios y cursos específicos sobre el tratamiento del Cáncer avanzado Tiroides.', N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3030, N'Especial interés en la aplicación y desarrollo de la neuromonitorizaciónen el campo de la Cirugía cervical endocrina.', NULL, N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3031, N'Formación especializada en Cirugía Radioguiada.', N'Herramienta de especial relevancia en el campo de la oncología ( Detección de ganglio céntinela en Cáncer de mama y melanoma cutaneo)', N'Herramienta de Guía y Seguridad en la Cirugía de las glándulas paratiroides.', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3032, N'Especial interés y formación en técnicas mínimamente invasivas para el tratamiento de la Obesidad.', NULL, N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3033, N'Formación Avanzada en el manejo terapéutico del Melanoma Cutaneo.', N'Formación en la detección del ganglio centinela empleando cirugía radioguiada.', N'Miembro del Grupo Multidisciplinario de tratamiento del Melanoma del Hospital General Universitario Gergorio Marañón.', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3034, N'Miembro del Grupo de Transplante Hepático (2005-2015)', N'Hospital General Universitario Gregorio Marañón de Madrid.', N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3035, N'Miembro de la Sección de Cirugía Endocrina de la Asociación Española de Cirujanos.', NULL, N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3036, N'Miembro de la Asociación Europea de Cirugía Endocrina', N'Principal órgano representativo de nuestra superespecilidad en Europoa', N'', 3)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3038, N'Licenciado en Medicina y Cirugía en 1993', N' Universidad Autónoma de Madrid', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3039, N'Formación como especialista en Cirugía General y Aparato Digestivo entre los años 1984-1988.', N'Hospital Gregorio Marañón de Madri', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3043, N'Médico Adjunto al Servicio de Cirugía General I desde 1989.', N'Hospital General Universitario Gregorio Marañón de Madrid.', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3044, N'Formación en Cirugía Oncológica en el Instituto Oncológico Príncipe de Asturias entre los años  1992-1995', N'Hospital General Universitario Gregorio Marañón', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3045, N'Cirujano responsable de la Cirugía Oncológica del Instituto Oncológico Príncipe de Asturias del  (1.995-2.002) ', N'Hospital General Universitario Gregorio Marañón', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3046, N'Pionero en la puesta en marcha de la técnica de ganglio centinela, implantándola y desarrollándola en la cirugía del melanoma y del cáncer de mama en el año 1997', N'Hospital General Universitario Gregorio Marañón.', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3047, N'Miembro de la Comisión de Tumores.', N'Hospital General Universitario Gregorio Marañón', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3049, N'Miembro del Comité Organizador del XXVII Congreso Nacional de Cirugía', NULL, N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3050, N'Coordinador de la Unidad de cáncer de mama  desde su creación hasta 2011. ', N'Hospital general Universitario Gregorio Marañón', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3051, N'Profesor asociado a la cátedra de Fisiopatología y Propedéutica Quirúrgica en Ciencias de la Salud desde 2012.', N' Universidad Complutense de Madrid', N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3052, N'Experto en Oncología Endocrina', N'Dedicación exclusiva en el Servicio de Cirugía General I desde 2008.', N'Hospital General Universitario Gregorio Marañón de Madrid.', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3053, N'Amplia experiencia Cirugía Radioguiada como instrumento para el tratamiento del Hiperparatiroidismo.', NULL, N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3054, N'Múltiples cursos, seminarios , congresos y reuniones nacionales e internacionales en patología endocrina.', NULL, N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3055, N'Miembro de la Sección de Cirugía endocrina de la Asociación española de Cirujanos.', NULL, N'', 7)
INSERT [dbo].[ItemCurriculum] ([idItem], [Titulo], [Fechas], [Texto], [idCurriculum]) VALUES (3056, N'Miembro activo de la Asociación Europea de Cirugía Endocrina.', NULL, N'', 7)
SET IDENTITY_INSERT [dbo].[ItemCurriculum] OFF
GO
SET IDENTITY_INSERT [dbo].[PaginaAmiga] ON 

INSERT [dbo].[PaginaAmiga] ([idPagina], [nombre], [descripcion], [link], [icono]) VALUES (1, N'Epinut', N'Grupo de investigación acreditado de la Universidad Complutense de Madrid', N'http://epinut.ucm.es', N'~/uploads/fotos/PagAm1.png')
INSERT [dbo].[PaginaAmiga] ([idPagina], [nombre], [descripcion], [link], [icono]) VALUES (3, N'Coloproctología Madrid', N'El Dr. Rodríguez Martín dirige un grupo de cirugía coloproctológica especializada. Le invitamos a que visite su web.', N'http://coloproctologiamadrid.com', N'~/uploads/fotos/PagAm3.jpg')
SET IDENTITY_INSERT [dbo].[PaginaAmiga] OFF
GO
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (1, 1)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (1, 2)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (1, 3)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (1, 4)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (1, 1006)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (2, 1)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (2, 2)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (2, 4)
INSERT [dbo].[ProfesionalesCompanias] ([idDatosPro], [idCompanias]) VALUES (2, 1006)
GO
INSERT [dbo].[Rol] ([idRol], [nombre]) VALUES (1, N'SuperAdmin')
INSERT [dbo].[Rol] ([idRol], [nombre]) VALUES (2, N'Admin')
INSERT [dbo].[Rol] ([idRol], [nombre]) VALUES (3, N'Usuario')
GO
SET IDENTITY_INSERT [dbo].[Servicio] ON 

INSERT [dbo].[Servicio] ([idServicio], [nombre], [text], [foto], [cabecera]) VALUES (1, N'Cirugía Tiroidea', N'<p>La gl&aacute;ndula tiroides es un &oacute;rgano impar cuya principal funci&oacute;n es producir las hormonas tiroideas, encargadas de regular nuestro metabolismo.</p>
<p>La gl&aacute;ndula tiroides tiene forma de mariposa y est&aacute; dividida en dos peque&ntilde;os l&oacute;bulos conectados por un istmo (los l&oacute;bulos ser&iacute;an las alas y el istmo ser&iacute;a el cuerpo de la mariposa).</p>
<p>Se encuentra situada en la parte anterior del cuello, justo por delante de la tr&aacute;quea (que es la v&iacute;a respiratoria principal).</p>
<p><span style="color: #ff0000;"><b>1.1 <span style="text-decoration: underline;">PROCESOS BENIGNOS QUE AFECTAN AL TIROIDES</span></b></span></p>
<p>El tiroides puede verse afectado por varias patolog&iacute;as benignas que pueden, no afectar a la producci&oacute;n hormonal, o bien causar un exceso (hipertiroidismo) o un defecto (hipotiroidismo) hormonal.<span style="text-decoration: underline;"> </span></p>
<p>La intervenciones que se realizan sobre el tiroides conllevan la resecci&oacute;n de una parte de la gl&aacute;ndula. En funci&oacute;n de la cantidad de tiroides que se reseque se denominan: <b><span style="text-decoration: underline;">tiroidectom&iacute;a total, hemitiroidectom&iacute;a o tiroidectom&iacute;a subtotal</span></b><span style="text-decoration: underline;"> </span>(Ver glosario t&eacute;rminos).</p>
<p>Las principales patolog&iacute;as benignas que afectan a la gl&aacute;ndula son:</p>
<p>&nbsp;</p>
<ul>
<li><b>BOCIO MULTINODULAR</b></li>
</ul>
<p>El bocio es un aumento de tama&ntilde;o de la gl&aacute;ndula tiroides.</p>
<p>Intervienen muchos factores en su desarrollo, aunque el m&aacute;s importante es la deficiencia de yodo en la alimentaci&oacute;n.</p>
<p>Inicialmente el tiroides s&oacute;lo aumenta de tama&ntilde;o y posteriormente pueden aparecer n&oacute;dulos de diferentes tama&ntilde;os.</p>
<p>La producci&oacute;n hormonal puede ser normal, estar aumentada o disminuida.</p>
<p>La <b>indicaci&oacute;n</b> de Cirug&iacute;a viene determinada por:</p>
<p>- S&iacute;ntomas compresivos derivados del crecimiento progresivo de la gl&aacute;ndula y que pueden ocasionar desviaci&oacute;n de la tr&aacute;quea, dificultad para tragar o respirar.</p>
<p>- Datos sospechosos en el an&aacute;lisis de las c&eacute;lulas extraidas de los n&oacute;dulos por PAAF.</p>
<p>Generalmente se realiza <span style="text-decoration: underline;">TIROIDECTOMIA TOTAL </span>y en algunos casos muy seleccionados <span style="text-decoration: underline;">TIROIDECTOMIA SUBTOTAL.</span></p>
<p>&nbsp;</p>
<ul>
<li><b>ENFERMEDAD DE GRAVES BASEDOW.</b></li>
</ul>
<p>La enfermedad de Graves Basedow es una <span style="text-decoration: underline;">causa de Hipertiroidismo</span>( producci&oacute;n aumentada de hormonas tiroideas) , que suele asociarse a patolog&iacute;a ocular en forma de ojos prominentes o exoftalmos.</p>
<p>Se debe a un problema autoinmune que mantiene el tiroides constantemente estimulado.</p>
<p>Puede ser tratado con f&aacute;rmacos antitiroideos, radioiodo y con cirug&iacute;a &ndash; <b><span style="text-decoration: underline;">Tiroidectom&iacute;a total.</span></b></p>
<p>La tiroidectom&iacute;a total es el tratamiento que tiene mayor porcentaje de &eacute;xitos (&gt;90%), controla r&aacute;pidamente los s&iacute;ntomas y , en manos expertas, tiene pocas complicaciones.</p>
<p>&nbsp;</p>
<ul>
<li><b>N&Oacute;DULO TIROIDEO</b></li>
</ul>
<p>La gl&aacute;ndula tiroides puede ser asiento de n&oacute;dulos tiroideos &uacute;nicos.</p>
<p>Estos n&oacute;dulos deben ser valorados por un especialista, sobre todo para excluir que puedan ser un tumor maligno.</p>
<p>En este contexto, la<b><span style="text-decoration: underline;"> cirug&iacute;a</span></b><span style="text-decoration: underline;"> </span>puede tener un papel:</p>
<p>-Para completar el dian&oacute;stico, cuando no se ha podido realizar con otrs estudios.</p>
<p>-Para tratar en n&oacute;dulo, pudiendo ser necesario un hemitiroidectom&iacute;a o una tiroidetom&iacute;a total.</p>
<p>&nbsp;</p>
<ul>
<li><b>QUISTES TIROIDEOS RECIDIVANTES</b></li>
</ul>
<p>Los quistes tiroideos se diagnostican de forma adecuada por ecograf&iacute;a, que adem&aacute;s ofrece la posibilidad de tratarlos mediante punci&oacute;n y vaciado del mismo.</p>
<p>En ocasiones puede ser necesario indicar tratamiento quir&uacute;rgico bien por recidiva del quiste , tras varios aspirados o bien porque el quiste tenga un polo s&oacute;lido con dato de sospecha.</p>
<p>&nbsp;</p>
<p><strong><span style="color: #ff0000;">1.2 PROCESOS MALIGNOS QUE AFECTAN AL TIROIDES</span></strong></p>
<p>&nbsp;</p>
<p>El C&aacute;ncer de tiroides es el tumor m&aacute;s com&uacute;n de las gl&aacute;ndulas endocrinas. Su incidencia ha ido creciendo en los &uacute;ltimos a&ntilde;os debido al aumento del n&uacute;mero de ecograf&iacute;as cervicales que se realizan por otros motivos, y que permiten diagnosticas tumores en estad&iacute;as muy iniciales. A pesar de este hecho, la tasa de mortalidad se ha mantenido estable, ya que en general se trata de un tumor de buen pron&oacute;stico.</p>
<p>La incidencia actual se sit&uacute;a entorno a unos 9 casos por 100000 habitantes y a&ntilde;o.</p>
<p>Existen distintas variedades de C&aacute;ncer de Tiroides &ndash; Ver referencia en blog pacientes &ndash;</p>
<p>C&aacute;ncer diferenciado &ndash; Papilar o folicular.</p>
<p>C&aacute;ncer medular.</p>
<p>C&aacute;ncer anapl&aacute;sico.</p>
<p>&nbsp;</p>
<p>En todos los casos realizamos TIROIDECTOMIA TOTAL EXTRACAPSULAR para tratar el tumor principal.</p>
<p>&nbsp;</p>
<p>Dependiendo de la variedad histol&oacute;gica y del tama&ntilde;o tumoral, asociaremos a procedimiento previo, una reacci&oacute;n de los ganglios linf&aacute;ticos regionales o LINFADENECTOM&Iacute;A.</p>
<p>La linfadenectom&iacute;a puede realizarse sobre los ganglios paratraqueales, paralar&iacute;ngeos y paraesof&aacute;gicos y se denominar&aacute; LINFADENECTOMIA CENTRAL o bien puede realizarse sobre los ganglios que rodean al paquete vasculo-nervioso del cuello, llam&aacute;ndose, entonces, linfadenetom&iacute;a lateral o VACIAMIENTO LATERAL FUNCIONAL.</p>', N'~/uploads/fotos/ser1.jpg', N'<p>La tiroidectom&iacute;a es su tratamiento en bocio multinodular, en c&aacute;ncer de tiroides y en hipertiroidismo. Nuestro grupo es pionero en Espa&ntilde;a en su extirpaci&oacute;n sin cicatriz en el cuello.</p>                                                                                                                                                                                                                                                                                            ')
INSERT [dbo].[Servicio] ([idServicio], [nombre], [text], [foto], [cabecera]) VALUES (2, N'Cirugía Paratiroidea', N'<p><span>El hiperparatiroidismo es la principal patolog&iacute;a que afecta a las gl&aacute;ndulas paratiroides.</span></p>
<p><span>El<strong> HIPERPARATIROIDISMO PRIMARIO</strong> puede deberse a:</span></p>
<p><span>-Adenoma Paratiroideo &uacute;nico.</span></p>
<p><span>-Adenoma Paratiroideo doble.</span></p>
<p><span>-Hiperplasia paratiroidea.</span></p>
<p><span>-Carcinoma Paratiroideo.</span></p>
<p><span>En UCEME realizamos<strong> paratiroidectom&iacute;a m&iacute;nimamente invasiva</strong> apoy&aacute;ndonos en:</span></p>
<p><span>- Adecuado diagn&oacute;stico de localizaci&oacute;n preoperatorio.</span></p>
<p><span>- Determinaci&oacute;n intraoperatoria de la PTH: La vida media de la PTH es de 5 minutos. Tras la ex&eacute;resis de la gl&aacute;dula o gl&aacute;ndulas afectas, realizamo an&aacute;lisis a los 5 minutos y a los 10 minutos postex&eacute;resis. La caida de la cifra de PTH, siguiendo unos criterios, determina el exito de la intervenci&oacute;n.</span></p>
<p><span>-Cirug&iacute;a Radioguiada: Marcaje preoperatorio de las gl&aacute;ndulas paratiroides con Tecnecio y posteriormente busqueda de las mismoas en el campo operatorio empleando una sonda de detecci&oacute;n portatil.</span></p>
<p><span>&nbsp;</span></p>
<p><span>En el caso de <strong>HIPERPARATIROIDISMO SECUNDARIO,&nbsp;</strong>la t&eacute;cnica que realizamos consiste en una paratiroidectomia subtotal o total con reimplante de un peque&ntilde;o fragmento de una de las gl&aacute;ndulas en un m&uacute;sculo del antebrazo.</span></p>
<p></p>', N'~/uploads/fotos/ser2.png', N'<p>Las enfermedades de las gl&aacute;ndulas paratiroides necesitan una valoraci&oacute;n por grupos de endocrin&oacute;logos (m&eacute;dicos y cirujanos) con gran experiencia</p>                                                                                                                                                                                                                                                                                                                                  ')
INSERT [dbo].[Servicio] ([idServicio], [nombre], [text], [foto], [cabecera]) VALUES (6, N'Obesidad mórbida/ Síndrome metabólico', N'<p>Ofrecemos nuestra experiencia en cirug&iacute;a laparosc&oacute;pica de obesidad m&oacute;rbida, realizando diferentes t&eacute;cnicas individualizando su indicaci&oacute;n seg&uacute;n las caracter&iacute;sticas de cada paciente.</p>
<p>Realizamos Bypass g&aacute;strico, Gastrectom&iacute;a tubular, Derivaci&oacute;n biiopancre&aacute;tica</p>
<p>El sobrepeso y la obesidad es una enfermedad cada vez m&aacute;s frecuente en los pa&iacute;ses desarrollados, llegando a cifras epid&eacute;micas en pa&iacute;ses como Estados Unidos.</p>
<p>Aunque es una enfermedad en s&iacute; misma, favorece adem&aacute;s la aparici&oacute;n de muchas otras como la hipertensi&oacute;n arterial, la diabetes mellitus, la hipertrigliceridemia.</p>
<p>El s&iacute;ndrome metab&oacute;lico consiste en diabetes mellitus asociada a hipertensi&oacute;n arterial, dislipemia, u obesidad.</p>
<p>En los &uacute;ltimos a&ntilde;os se ha observado que el s&iacute;ndrome metab&oacute;lico se corrige total o casi totalmente en pacientes obesos sometidos a cirug&iacute;a bari&aacute;trica (o cirug&iacute;a de la obesidad). Por esta raz&oacute;n se han comenzado a aplicar estas t&eacute;cnicas en pacientes diab&eacute;ticos no obesos. Los resultados a corto y medio plazo han sido muy optimistas, con altos porcentajes de curaci&oacute;n de la diabetes.</p>
<p>Ofrecemos en nuestro grupo la realizaci&oacute;n de cirug&iacute;a metab&oacute;lica laparosc&oacute;pica destinada al tratamiento de diabetes mellitus tipo II de mal control m&eacute;dico.</p>', N'~/uploads/fotos/ser7.jpg', N'<p>Nuestro equipo tiene una amplia experiencia en estas dos enfermedades cuyo tratamiento quir&uacute;rgico ha evolucionado de forma muy importante en los &uacute;ltimos a&ntilde;os</p>                                                                                                                                                                                                                                                                                                                           ')
INSERT [dbo].[Servicio] ([idServicio], [nombre], [text], [foto], [cabecera]) VALUES (8, N'Cirugía de Glándulas Suprarrenales', N'<p>Las gl&aacute;ndulas suprarrenales son dos hormonas localizadas encima de sendos ri&ntilde;ones cuya funci&oacute;n es la de regular las respuestas al estr&eacute;s a trav&eacute;s de la s&iacute;ntesis de corticoesteroides y catecolaminas.</p>
<p>Algunas enfermedades de las gl&aacute;ndulas suprarrenales precisan de un tratamiento quir&uacute;rgico</p>
<p>- Adenoma. Tumor benigno unilateral&nbsp;</p>
<p>- Feocromocitoma.&nbsp;</p>
<p>- Incidentaloma</p>
<p>- Carcinoma</p>
<p>- Met&aacute;stasis</p>
<p></p>
<p>En todos los casos proponemos como T&Eacute;CNICA DE ELECCI&Oacute;N la<strong> EXERESIS DE LA GL&Aacute;NDULA POR V&Iacute;A LAPAROSC&Oacute;PICA.&nbsp;</strong>T&eacute;cnnica de elecci&oacute;n en la actualidad con excelente tolerancia postoperatoria</p>
<p></p>
<p>Le recomendamos que visite nuestra secci&oacute;n de v&iacute;deos en la que mostramos varios casos de suprarrenalectom&iacute;a laparosc&oacute;pica</p>', N'~/uploads/fotos/ser8.jpeg', N'Ofrecemos un abordaje multidisciplinar de la patología suprarrenal con estudio
funcional completo y cirugía mínimamente invasiva                                                                                                                                                                                                                                                                                                                                                                                   ')
INSERT [dbo].[Servicio] ([idServicio], [nombre], [text], [foto], [cabecera]) VALUES (11, N'Tiroidectomía sin cicatriz', N'<div>La tiroidectom&iacute;a en manos expertas es una t&eacute;cnica muy segura de forma que la &uacute;nica secuela permanente suele ser la cicatriz en el cuello. Aunque las incisiones son habitualmente de peque&ntilde;o tama&ntilde;o, es una cicatriz que se ve bastante, por la localizaci&oacute;n que presenta en la parte inferior del cuello. Por ello se han descrito varias t&eacute;cnicas para extirpar el tiroides en las mismas condiciones de seguridad que habitualmente, pero desde incisiones lejos del cuello que no se ven. Nuestro grupo es pionero en Europa en abordaje biaxilo biareolar con excelentes resultados.</div>
<div></div>
<div>Consulte con nosotros y valoraremos su caso.</div>
<div class="yj6qo"></div>
<div class="adL"></div>', N'~/uploads/fotos/ser11.jpg', N'<p><span><strong>Nuestro grupo es pionero en Europa</strong> en la realizaci&oacute;n de tiroidectom&iacute;a por abordaje biaxilo biareolar con excelentes resultados, evitando la &uacute;nica secuela definitiva; la cicatriz en el cuello.</span></p>                                                                                                                                                                                                                                                           ')
SET IDENTITY_INSERT [dbo].[Servicio] OFF
GO
SET IDENTITY_INSERT [dbo].[Tecnica] ON 

INSERT [dbo].[Tecnica] ([idTecnica], [titulo], [fecha], [foto], [texto]) VALUES (1, N'OBESIDAD MÓRBIDA', CAST(N'2013-10-29T21:02:59.773' AS DateTime), N'~/uploads/fotos/Tecnica1.jpeg', N'<p>La obesidad es un problema creciente de salud en nuestra sociedad. La obesidad m&oacute;rbida se define seg&uacute;n una relaci&oacute;n entre la estatura y el peso del paciente, y en ella el adelgazamiento eficaz y duradero dif&iacute;cilmente se consigue sin ayuda de una intervenci&oacute;n quir&uacute;rgica.</p>
<p>La cirug&iacute;a de la obesidad m&oacute;rbida es una pieza m&aacute;s en el puzzle que supone el tratamiento de este tipo de pacientes; pero tan importante es una t&eacute;cnica bien realizada y ajustada a las caracter&iacute;sticas del paciente, como el seguimiento por un m&eacute;dico endocrino especialista en esta materia.</p>
<p>Nuestro grupo ofrece un tratamiento personalizado del obeso m&oacute;rbido; un estudio individual de los problemas que la han causado y un ajuste personal tanto de la t&eacute;cnica quir&uacute;rgica como del seguimiento antes y despu&eacute;s de la misma</p>
<p>Con una gran experiencia en cirug&iacute;a laparosc&oacute;pica avanzada, ofrecemos las distintas t&eacute;cnicas aprobadas por las sociedades m&eacute;dicas para el estudio y tratamiento de la obesidad entre las que destacan, por su frecuencia, <b>GASTROPLASTIA TUBULAR</b>, el <b>BYPASS G&Aacute;STRICO</b>, y la <b>DERIVACI&Oacute;N BILIOPANCRE&Aacute;TICA</b></p>
<p><b><br /></b></p>
<p><b>GASTROPLASTIA TUBULAR</b></p>
<p>Se trata de una t&eacute;cnica <b>restrictiva</b> que consiste en la extirpaci&oacute;n de tres cuartas partes del est&oacute;mago, quedando un est&oacute;mago residual de aproximadamente 100 mL de capacidad. Es una t&eacute;cnica ideal en obesidades m&aacute;s leves (IMC &lt;45) y en pacientes que presentan otras enfermedades asociadas que impiden la realizaci&oacute;n de t&eacute;cnicas m&aacute;s complejas</p>
<p><b>BYPASS G&Aacute;STRICO</b></p>
<p>T&eacute;cnica <b>mixta</b> (restrictiva y malabsortiva); en un primer paso se secciona el est&oacute;mago quedando un volumen residual de unos 50 mL; posteriormente se altera el orden del intestino delgado dejando un segmento del mismo de entre 150 y 200 cm en el que no se absorben alimentos. Es la t&eacute;cnica m&aacute;s utilizada a nivel mundial por sus excelentes resultados, y sus bajas cifras de complciaciones. Est&aacute; indicada en cifras de IMC entre 35 y 60</p>
<p><b>DERIVACI&Oacute;N BILIOPANCRE&Aacute;TICA</b></p>
<p>T&eacute;cnica mixta en el que se secciona (o extirpa) una parte del est&oacute;mago quedando entre 100 y 200 mL de volumen residual, y posteriormente se altera el orden del intestino delgado dejando un segmento del mismo de hasta 300 cm sin absorci&oacute;n. Es la t&eacute;cnica m&aacute;s compleja y est&aacute; indicada sobretodo en obesidades muy graves, con IMC &gt;60</p>
<p>En nuestro grupo practicamos estas t&eacute;cnicas por v&iacute;a laparosc&oacute;pica, ajustando la indicaci&oacute;n en cada paciente seg&uacute;n sus caracter&iacute;sticas personales, sus enfermedades asociadas, su grado de obesidad</p>
<p>&nbsp;</p>
<p>No dude en solicitar cita con nosotros para recibir una valoraci&oacute;n especializada de su caso.</p>
<p><strong></strong></p>')
INSERT [dbo].[Tecnica] ([idTecnica], [titulo], [fecha], [foto], [texto]) VALUES (2, N'NEUROMONITORIZACIÓN EN CIRUGÍA ENDOCRINA', CAST(N'2014-03-12T19:06:36.543' AS DateTime), N'~/uploads/fotos/Tecnica2.jpg', N'<p>Los principales nervios encargados de la movilidad de las cuerdas vocales se llaman NERVIOS RECURRENTES LAR&Iacute;NGEOS.</p>
<p>Se encuentran ubicados por detr&aacute;s de la gl&aacute;ndula tiroidea y en &iacute;ntimo contacto con las gl&aacute;ndulas paratiroides.</p>
<p>Su di&aacute;metro suele estar entorno a 1 mm y pueden presentarse en un tronco &uacute;nico o en varias ramas, y con una enorme variabilidad anat&oacute;mica.</p>
<p>Es ESENCIAL la identificaci&oacute;n de los nervios durante la cirug&iacute;a, para garantizar una adecuada calidad de voz tras la misma.</p>
<p>Son muchas las medidas empleadas para EVITAR&nbsp; causar da&ntilde;os a estas estructuras nerviosas: T&eacute;cnica quir&uacute;rgica delicada y meticulosa, empleo de gafas de aumento y en los &uacute;ltimos a&ntilde;os la evoluci&oacute;n tecnol&oacute;gica nos ha permitido introducir la <b>NEUROMONITORIZACI&Oacute;N</b>.</p>
<p>Son m&uacute;ltiples los trabajos en la literatura que avalan la utilidad de esta herramienta que, si bien, no es un elemento esencial para un cirujano experto que se enfrenta a una cirug&iacute;a est&aacute;ndar, puede ser CRUCIAL, en algunas situaciones, como reintervenciones, cirug&iacute;as oncol&oacute;gicas complejas o bocios de gran tama&ntilde;o entre otros. </p>
<p><b>&iquest;En qu&eacute; consiste?</b></p>
<p>La neuromonitorizaci&oacute;n nos permite verificar la <b>INTEGRIDAD EL&Eacute;CTRICA </b>de los nervios lar&iacute;ngeos recurrentes, es decir, al t&eacute;rmino de la intervenci&oacute;n, podemos estar seguros de que los nervios, adem&aacute;s de presentar un aspecto EXTERNO normal, tambi&eacute;n INTERNAMENTE, conducen de forma adecuada los impulsos nerviosos. </p>
<p>No es un procedimiento doloroso para el paciente.</p>
<p>Se realiza colocando unos sensores en el tubo endotraqueal y durante la intervenci&oacute;n se suministran est&iacute;mulos el&eacute;ctricos de muy baja intensidad a los tejidos mediante un terminal. Si este est&iacute;mulo es detectado por un monitor externo nos estar&aacute; identificando un nervio y nos permitir&aacute; diferenciarlo de un tejido cicatricial o de otras estructuras no nerviosas. </p>
<p>Una vez identificado el nervio, podemos monitorizar la corriente de forma continua, verificando que los impulsos nerviosos mantienen su amplitud y latencia dentro&nbsp; de unos margenes de seguridad. </p>
<p>Como mencionamos antes, no es un procedimiento esencial para un cirujano experto, pero existen algunas situaciones en que es IMPRESCINDIBLE para la seguridad del paciente: </p>
<p>-REINTERVENCIONES CERVICALES: Los tejidos cicatriciales pueden dificultar enormemente la identificaci&oacute;n del nervio. En este escenario la neuromonitorizaci&oacute;n nos permite verificar que los tejidos que vamos seccionando no son estructuras nerviosas. </p>
<p>-CIRUG&Iacute;A ONCOL&Oacute;GICA COMPLEJA: En ocasiones es preciso realizar ex&eacute;resis de los ganglios adyacentes a un tumor. &Eacute;stos, habitualmente, rodean a los nervios y gracias a la neuromonitorizaci&oacute;n podemos identificarlos precozmente y asegurar su funcionamiento durante la disecci&oacute;n.</p>
<p>-BOCIOS DE GRAN TAMA&Ntilde;O.</p>
<p>En UCEME empleamos esta herramienta con frecuencia.</p>
<p>Realizamos nuestra formaci&oacute;n en MONITORIZACI&Oacute;N NERVIOSA &nbsp;en los mejores centros de Espa&ntilde;a y nos mantenemos al d&iacute;a de sus nuevas aplicaciones en el campo de la cirug&iacute;a endocrina. </p>
<p>Pod&eacute;is ver un video en la galer&iacute;a de im&aacute;genes</p>')
INSERT [dbo].[Tecnica] ([idTecnica], [titulo], [fecha], [foto], [texto]) VALUES (3, N'PIONEROS EN ESPAÑA EN TIROIDECTOMÍA POR ABORDAJE EXTRACERVICAL', CAST(N'2020-03-04T19:34:18.310' AS DateTime), N'~/uploads/fotos/Tecnica3.jpg', N'<p>En la &uacute;ltima d&eacute;cada, liderados por los cirujanos endocrinos asi&aacute;ticos, se han desarrollado varias t&eacute;cnicas con el fin de poder extraer la gl&aacute;ndula tiroides evitando dejar cicatrices en el cuello. Una cirug&iacute;a tan segura como la tiroidea, presenta, como secuela m&aacute;s importante, una herida en un lugar tan visible como es el centro del cuello. En nuestro grupo quir&uacute;rgico nos planteamos el aprender las t&eacute;cnicas m&aacute;s reproducibles y vers&aacute;tiles de las muchas que se han descrito, y dise&ntilde;amos un programa de aprendizaje hace m&aacute;s de un a&ntilde;o. La primera parte del mismo consist&iacute;a en formaci&oacute;n te&oacute;rica, y posterior asistencia <strong>durante tres semanas al Hospital Mount Sina&iacute; de Nueva York, con los profesores Inabnet y Suh</strong>, pioneros en estas t&eacute;cnicas en EEUU. Esta primera parte culmin&oacute; el pasado junio (2017) con la realizaci&oacute;n de los primeros casos de abordaje biaxilo-bimamario por nuestro grupo habiendo realizado un total de quince casos.</p>
<p><strong>Somos el &uacute;nico grupo en Espa&ntilde;a en realizar &eacute;sta t&eacute;cnica, y uno de los de mayor experiencia en Europa.</strong> Hemos presentado en Diario M&eacute;dico (@diariomedico) los resultados de los primeros casos que han sido excelentes y totalmente equiparables a los de la cirug&iacute;a abierta (sin lesi&oacute;n nerviosa ni de paratiroides) pero sin cicatriz visible en el cuello. (para m&aacute;s informaci&oacute;n http://cirugia-general.diariomedico.com/2018/01/09/area-cientifica/especialidades/cirugia-general/la-via-axilo-mamaria-bilateral-elude-la-cicatriz-cervical-en-tiroidectomia).</p>
<p><strong>C&Iacute;TESE</strong> en nuestra secci&oacute;n de <strong>CITA ON LINE</strong> para recibir m&aacute;s informaci&oacute;n&nbsp;</p>')
SET IDENTITY_INSERT [dbo].[Tecnica] OFF
GO
SET IDENTITY_INSERT [dbo].[Termino] ON 

INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Apendicitis', N'La apendicitis es la inflación aguda del apéndice cecal, localizado en el ciego.
 
La clínica es de comienzo más o menos rápido y cursa con dolor alrededor del ombligo que posteriormente se localiza en la zona baja y derecha del abdomen. Suele acompañarse de fiebre y disminución del apetito.
 
En la exploración es muy característico el dolor en la zona baja y derecha del abdomen a la compresión que aumenta al soltar de forma brusca.
 
El tratamiento siempre es quirúrgico y urgente.
Esta patología se puede tratar con Cirugía laparoscópica', N'', NULL, 1)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Cirugía Laparoscópica', N'<p>La cirug&iacute;a laparosc&oacute;pica consiste en la realizaci&oacute;n de cirug&iacute;a abdominal a trav&eacute;s de peque&ntilde;os orificios realizados en el abdomen por donde se introducen la &oacute;ptica para la visi&oacute;n en un monitor y el instrumental necesario para llevar a cabo la cirug&iacute;a. Las ventajas son menor dolor postoperatorio y mayor rapidez en la recuperaci&oacute;n con menores defectos est&eacute;ticos.Este tipo de cirug&iacute;a requiere de una amplia experiencia para alcanzar buenos resultados. Nuestro grupo es pionero en el desarrollo de este abordaje en el tratamiento del c&aacute;ncer de colon, c&aacute;ncer de recto, cirug&iacute;a del prolapso rectal y tratamiento de la hernia periestomal. Otras patolog&iacute;as que pueden tratarse por v&iacute;a laparosc&oacute;pica son la Enfermedad de Crohn y la Colitis ulcerosa.</p>', NULL, NULL, 2)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Hernia Periestomal', N'La hernia periestomal es una patología frecuente en los pacientes ostomizados.Consiste en la herniación del contenido intestinal a través del orificio realizado para sacar el intestino al exterior para realizar la deposición (ostomía). También aparece en pacientes ostomizados después de una cirugía de vejiga (urostomía) La clínica suele consistir en la aparición de un bulto alrededor del estoma que crece progresivamente. Con frecuencia las bolsas se despegan debido a la irregularidad del estoma, produciéndose manchado. Otro síntoma menos frecuente y más grave es la obstrucción intestinal, que suele cursar con dolor local, ausencia de tránsito intestinal, distensión del abdomen y vómitos.', NULL, NULL, 3)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Diverticulosis', N'La diverticulosis es una patología frecuente y suele aparecer en la edad adulta.
 
Lo más frecuente es que sea asintomática.', NULL, NULL, 4)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Diverticulitis', N'La clínica más frecuente y grave de la diverticulosis es la diverticulitis: Inflamación del segmento de colon donde se localizan los divertículos. Suele cursar con dolor en la región baja del abdomen, que se irradia hacia el lado izquierdo, acompañado de fiebre y malestar general.
 
El tratamiento incial de la diverticulitis es antibiótico y dependiendo del grado de inflamación o la formación o no de abscesos o perforación del colon se opta por medidas más agresivas que van desde el drenaje de abscesos de forma percutánea a la cirugía.
 
La diverticulosis debe ser tratada quirúrgicamente cuando se repiten varios episodios de diverticulitis o si existen síntomas de estenosis del segmento de colon afectado.
 
la estenosis de manifiesta con síntomas de obstrucción y dolor entre los episodios de diverticulitis.
 
La diverticulosis puede ser tratada mediante Cirugía laparoscópica.', NULL, NULL, 5)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Obesidad Mórbida', N'Obesidad con IMC mayor de 40, en la que es necesaria realizar una cirugía para conseguir adelgazamiento mantenido en el tiempo', NULL, NULL, 1002)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'TIROIDECTOMIA TOTAL', N'Resección COMPLETA de la glándula tiroides', NULL, NULL, 1003)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'TIROIDECTOMIA SUBTOTAL', N'Resección CASI COMPLETA de la glándula. Suele conservarse una pequeña parte del tejido tiroideo que se encuantra en íntimo contacto con el nervio recurrente.', NULL, NULL, 1004)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'HEMITIROIDECTOMIA-HEMITIROIDECTOMIA AMPLIADA', N'Reseccción de SÓLO un lóbulo del tiroides -Hemitiroidectomía - o de un lóbulo del tiroides mas el istmo tiroideo - Hemitiroidectomía ampliada.', NULL, NULL, 1005)
INSERT [dbo].[Termino] ([nombre], [texto], [foto], [link], [idTermino]) VALUES (N'Glándula Paratiroides', N'En la cara posterior de la glándula tiroides se encuentran las glándulas paratiroides; habitualmente son cuatro y regulan el metabolismo del calcio en el organismo', NULL, NULL, 2003)
SET IDENTITY_INSERT [dbo].[Termino] OFF
GO
SET IDENTITY_INSERT [dbo].[Turno] ON 

INSERT [dbo].[Turno] ([idTurno], [dia], [inicio], [fin], [paralelas], [porhora], [idHospital]) VALUES (2, 2, CAST(15.00 AS Decimal(5, 2)), CAST(19.00 AS Decimal(5, 2)), 1, 3, 1)
INSERT [dbo].[Turno] ([idTurno], [dia], [inicio], [fin], [paralelas], [porhora], [idHospital]) VALUES (4, 1, CAST(15.00 AS Decimal(5, 2)), CAST(16.00 AS Decimal(5, 2)), 1, 6, 2)
INSERT [dbo].[Turno] ([idTurno], [dia], [inicio], [fin], [paralelas], [porhora], [idHospital]) VALUES (6, 3, CAST(15.00 AS Decimal(5, 2)), CAST(16.00 AS Decimal(5, 2)), 1, 6, 2)
INSERT [dbo].[Turno] ([idTurno], [dia], [inicio], [fin], [paralelas], [porhora], [idHospital]) VALUES (7, 4, CAST(15.50 AS Decimal(5, 2)), CAST(17.50 AS Decimal(5, 2)), 1, 3, 1)
INSERT [dbo].[Turno] ([idTurno], [dia], [inicio], [fin], [paralelas], [porhora], [idHospital]) VALUES (12, 1, CAST(9.50 AS Decimal(5, 2)), CAST(11.50 AS Decimal(5, 2)), 1, 3, 1)
SET IDENTITY_INSERT [dbo].[Turno] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (4, N'Iñaki', N'Amunategui Prats', N'IAP', N'inakiamunategui@someprivate.email', N'~/uploads/fotos/usu4.png', CAST(N'2012-12-12T00:00:00.000' AS DateTime), 2, 1, 1, N'9b947f06c8b3e4087ac70369a64d3b2d00522db4', 0, N'http://es.linkedin.com/pub/inaki-amunategui-prats/60/480/104', 2)
INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (5, N'Enrique', N'Mercader Cidoncha', N'EMC', N'enriquemercader@someprivate.email', N'~/uploads/fotos/usu5.jpg', CAST(N'2012-12-12T00:00:00.000' AS DateTime), 2, 3, 2, N'7c4a8d09ca3762af61e59520943dc26494f8941b', 0, N'http://www.linkedin.com/pub/enrique-mercader-cidoncha/68/283/216', 3)
INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (6, N'Super', N'Admin', N'SA', N'info@someprivate.email', N'~/uploads/fotos/usu6.png', CAST(N'2013-01-01T00:00:00.000' AS DateTime), 1, 1, 1, N'7c4a8d09ca3762af61e59520943dc26494f8941b', 0, NULL, NULL)
INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (9, N'Jose Luis', N'Escat', N'JLE', N'joseluisescat@someprivate.email', N'~/uploads/fotos/usu9.png', CAST(N'2013-07-02T17:52:37.047' AS DateTime), 1, 7, 6, N'7c4a8d09ca3762af61e59520943dc26494f8941b', 0, N'http://www.linkedin.com', 1)
INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (11, N'Julio', N'CT', N'JCT', N'juliocejudo@someprivate.email', N'~/uploads/fotos/usu11.jpg', CAST(N'2013-07-05T01:49:36.333' AS DateTime), 1, 9, 8, N'9b947f06c8b3e4087ac70369a64d3b2d00522db4', 0, N'http://www.linkedin.com', NULL)
INSERT [dbo].[Usuario] ([idUsuario], [nombre], [apellidos], [nick], [login], [foto], [ultimoupdate], [idRol], [idCurriculum], [idDatosContacto], [password], [newsletter], [linkedin], [display_order]) VALUES (16, N'Uceme', N' ', N'US', N'socialuceme@someprivate.email', N'none', NULL, 1, 9, 8, N'9b947f06c8b3e4087ac70369a64d3b2d00522db4', 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
SET IDENTITY_INSERT [dbo].[Video] ON 

INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Suprarrenalectomia Derecha', N'Vídeo en el que se realiza la extirpación por vía laparoscópica de la glándula suprarrenal derecha por feocromocitoma.', N'//www.youtube.com/embed/x29rADUCKNs', 1, 3)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Neuromonitorización del nervio recurrente laríngeo', N' Comprobación de la integridad externa e interna (transmisión eléctrica adecuada) del nervio recurrente laríngeo izquierdo tras tiroidectomía. Tras las comprobaciones de seguridad, se procede a la finalización de la intervención.', N'//www.youtube.com/embed/7kGmjISXZiM', 4, 5)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Suprarrenalectomia por Metástasis de cáncer pulmonar', N'Presentamos una suprarrenalectomía derecha laparoscópica por metástasis de cáncer pulmonar. El hallazgo de un trombo venoso maligno nos obligó a la sección de la vena suprarrenal a ras de la vena cava', N'//www.youtube.com/embed/JDAR46qqit4', 9, 6)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Suprarrenalectomía izquierda en Síndrome MEN-IIA', N'Mujer joven diagnosticada de feocromocitoma bilateral en el seno de un síndrome de MEN-IIA. Realizamos suprarrenalectomía bilateral laparoscópica por vía anterior. En este vídeo presentamos la suprarrenalectomía izquierda ', N'//www.youtube.com/embed/EVe8IZg-Hf0', 20, 2)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'NIM nervio laríngeo no recurrente', N'Paciente de 55 años con cáncer papilar de tiroides recidivado. Presenta arteria subclavia derecha aberrante, lo que se asocia a alteraciones en el trayecto del nervio recurrente. Presentamos la neuromonitorización (NIM) de dicho nervio laríngeo no recurrente', N'//www.youtube.com/embed/WDYLcn5hLS4', 21, 4)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Tiroidectomía Sin cicatriz cervical', N'Somos pioneros en España en la extirpación de la glándula tiroides sin cicatriz por la vía biaxilo-biareolar. Es una cirugía tan segura como la clásica pero con la ventaja de no la tener cicatriz en el cuello', N'//www.youtube.com/watch?v=Cf5FDdk3_-U', 25, 1)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Tiroidectomía Sin cicatriz cervical', N'Somos pioneros en España en la extirpación de la glándula tiroides sin cicatriz por la vía biaxilo-biareolar. Es una cirugía tan segura como la clásica pero con la ventaja de no la tener cicatriz en el cuello', N'www.youtube.com/watch?v=Cf5FDdk3_-U', 26, 7)
INSERT [dbo].[Video] ([titulo], [descripcion], [link], [idVideo], [posicion]) VALUES (N'Tiroidectomía Sin cicatriz cervical', N'Somos pioneros en España en la extirpación de la glándula tiroides sin cicatriz por la vía biaxilo-biareolar. Es una cirugía tan segura como la clásica pero con la ventaja de no la tener cicatriz en el cuello', N'//www.youtube.com/watch?v=Cf5FDdk3_-U', 27, 8)
SET IDENTITY_INSERT [dbo].[Video] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserProf__C9F28456B58BAC64]    Script Date: 15/05/2020 17:54:57 ******/
ALTER TABLE [dbo].[UserProfile] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__webpages__8A2B6160C0D8AEA4]    Script Date: 15/05/2020 17:54:57 ******/
ALTER TABLE [dbo].[webpages_Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Turno] ADD  CONSTRAINT [ColumnDefault_e5e2ea1f-333d-4e5d-81d7-a36a743d9ebc]  DEFAULT ((1)) FOR [paralelas]
GO
ALTER TABLE [dbo].[Turno] ADD  CONSTRAINT [ColumnDefault_83dd5ac8-b8d3-4581-a4c3-205e6c608615]  DEFAULT ((4)) FOR [porhora]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Usuario] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_Blog_Usuario]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_0] FOREIGN KEY([idTurno])
REFERENCES [dbo].[Turno] ([idTurno])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_0]
GO
ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [FK_Documento_Usuario] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [FK_Documento_Usuario]
GO
ALTER TABLE [dbo].[ItemCurriculum]  WITH CHECK ADD  CONSTRAINT [FK_ItemCurriculum_Curriculum] FOREIGN KEY([idCurriculum])
REFERENCES [dbo].[Curriculum] ([idCurriculum])
GO
ALTER TABLE [dbo].[ItemCurriculum] CHECK CONSTRAINT [FK_ItemCurriculum_Curriculum]
GO
ALTER TABLE [dbo].[ProfesionalesCompanias]  WITH CHECK ADD  CONSTRAINT [FK_ProfesionalesCompanias_Companias] FOREIGN KEY([idCompanias])
REFERENCES [dbo].[Companias] ([idCompanias])
GO
ALTER TABLE [dbo].[ProfesionalesCompanias] CHECK CONSTRAINT [FK_ProfesionalesCompanias_Companias]
GO
ALTER TABLE [dbo].[ProfesionalesCompanias]  WITH CHECK ADD  CONSTRAINT [FK_ProfesionalesCompanias_DatosProfesionales] FOREIGN KEY([idDatosPro])
REFERENCES [dbo].[DatosProfesionales] ([idDatosPro])
GO
ALTER TABLE [dbo].[ProfesionalesCompanias] CHECK CONSTRAINT [FK_ProfesionalesCompanias_DatosProfesionales]
GO
ALTER TABLE [dbo].[ProfesionalUsuario]  WITH CHECK ADD  CONSTRAINT [FK_ProfesionalUsuario_DatosProfesionales] FOREIGN KEY([idDatosPro])
REFERENCES [dbo].[DatosProfesionales] ([idDatosPro])
GO
ALTER TABLE [dbo].[ProfesionalUsuario] CHECK CONSTRAINT [FK_ProfesionalUsuario_DatosProfesionales]
GO
ALTER TABLE [dbo].[ProfesionalUsuario]  WITH CHECK ADD  CONSTRAINT [FK_ProfesionalUsuario_Usuario] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuario] ([idUsuario])
GO
ALTER TABLE [dbo].[ProfesionalUsuario] CHECK CONSTRAINT [FK_ProfesionalUsuario_Usuario]
GO
ALTER TABLE [dbo].[Turno]  WITH CHECK ADD  CONSTRAINT [FK_Turno_0] FOREIGN KEY([idHospital])
REFERENCES [dbo].[DatosProfesionales] ([idDatosPro])
GO
ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT [FK_Turno_0]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Curriculum] FOREIGN KEY([idCurriculum])
REFERENCES [dbo].[Curriculum] ([idCurriculum])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Curriculum]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_DatosContacto] FOREIGN KEY([idDatosContacto])
REFERENCES [dbo].[DatosContacto] ([idDatosContacto])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_DatosContacto]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([idRol])
REFERENCES [dbo].[Rol] ([idRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
