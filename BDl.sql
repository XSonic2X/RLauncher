USE [master]
GO
/****** Object:  Database [RShopProgram]    Script Date: 13.06.2023 6:41:02 ******/
CREATE DATABASE [RShopProgram]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RShopProgram', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RShopProgram.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RShopProgram_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RShopProgram_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RShopProgram] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RShopProgram].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RShopProgram] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RShopProgram] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RShopProgram] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RShopProgram] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RShopProgram] SET ARITHABORT OFF 
GO
ALTER DATABASE [RShopProgram] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [RShopProgram] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RShopProgram] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RShopProgram] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RShopProgram] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RShopProgram] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RShopProgram] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RShopProgram] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RShopProgram] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RShopProgram] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RShopProgram] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RShopProgram] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RShopProgram] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RShopProgram] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RShopProgram] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RShopProgram] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RShopProgram] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RShopProgram] SET RECOVERY FULL 
GO
ALTER DATABASE [RShopProgram] SET  MULTI_USER 
GO
ALTER DATABASE [RShopProgram] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RShopProgram] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RShopProgram] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RShopProgram] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RShopProgram] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RShopProgram] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RShopProgram', N'ON'
GO
ALTER DATABASE [RShopProgram] SET QUERY_STORE = OFF
GO
USE [RShopProgram]
GO
/****** Object:  Table [dbo].[ListProducts]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListProducts](
	[LoginUser] [nvarchar](18) NULL,
	[ProductName] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Name] [nvarchar](50) NULL,
	[Price] [money] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Name] [nvarchar](15) NULL,
	[Login] [nvarchar](18) NULL,
	[Pass] [nvarchar](18) NULL,
	[Key] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[addProduct]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[addProduct]
  @Name nvarchar(50),
  @Price money
  as
  begin
 Insert into Products ([Name],Price)
 values (@Name,@Price)
  end
GO
/****** Object:  StoredProcedure [dbo].[addToListProduct]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[addToListProduct]
 @LoginUser nvarchar(18),
 @ProdName nvarchar(50)
 as
 begin
 if EXISTS(select * from ListProducts where [LoginUser] =  @LoginUser and [ProductName] =  @ProdName)
 return
 else
 Insert into ListProducts(LoginUser,ProductName)
 values ( @LoginUser,@ProdName)
 end
GO
/****** Object:  StoredProcedure [dbo].[ChangeKey]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[ChangeKey] 
@Login nvarchar(18),
@NewKey nvarchar(50)
as
begin
Update [dbo].[User] set [Key] = @NewKey Where [Login] = @Login
end
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create procedure [dbo].[CreateUser]
@Name nvarchar(15),
@Pass nvarchar(18),
@Login nvarchar(18)
as
begin
if EXISTS(select * from [User] where @Login = [Login])
return
else
insert into [User] ([Name], [Login], Pass, [Key])
values (@Name, @Login, @Pass, '0')
Select * from [dbo].[User] where [Login] = @Login
end
GO
/****** Object:  StoredProcedure [dbo].[Log_In]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[Log_In]
@Login nvarchar(15),
@Password nvarchar(18)
as
begin
Select * from [dbo].[User] where [Login] = @Login and Pass = @Password
end
GO
/****** Object:  StoredProcedure [dbo].[Log_In_key]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[Log_In_key]
@Key nvarchar(50)
as
begin
if @Key = ''
return
else
Select * from [dbo].[User] where [Key] = @Key
 end
GO
/****** Object:  StoredProcedure [dbo].[PassHelp]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create procedure [dbo].[PassHelp]
@login nvarchar(18)
as
begin
if EXISTS(select * from [User] where @Login = [Login])
select * from [User] where @Login = [Login]
else return
end
GO
/****** Object:  StoredProcedure [dbo].[ViewListProducts]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[ViewListProducts]
@loginUser nvarchar(18)
 as
 begin
 Select * from [dbo].[Products]  where [Name] in (Select ProductName from ListProducts where [LoginUser] = @loginUser)
 end
GO
/****** Object:  StoredProcedure [dbo].[ViewProducts]    Script Date: 13.06.2023 6:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create procedure [dbo].[ViewProducts]
 as
 begin
Select * from Products
 end
GO
USE [master]
GO
ALTER DATABASE [RShopProgram] SET  READ_WRITE 
GO
