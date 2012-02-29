IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles]') AND type in (N'U'))
DROP TABLE [dbo].[Profiles]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[Profiles]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[UserId] [int] NULL,
[UserName] [nvarchar](20) NULL,
[About] [ntext] NOT NULL,
[FirstName] [nvarchar](20) NULL,
[LastName] [nvarchar](20) NULL,
[Alias] [nvarchar](50) NULL,
[IsFeatured] [bit] NULL,
[Email] [nvarchar](50) NULL,
[WebSite] [nvarchar](150) NULL,
[ImageUrl] [nvarchar](150) NULL,
[AddressDisplayLevel] [nvarchar](10) NULL,
[ImageRefId] [int] NULL,
[EnableDisplayOfName] [bit] NULL,
[IsGravatarEnabled] [bit] NULL,
[IsAddressEnabled] [bit] NULL,
[EnableMessages] [bit] NULL,
[Street] [nvarchar](60) NULL,
[City] [nvarchar](30) NULL,
[State] [nvarchar](20) NULL,
[Country] [nvarchar](20) NULL,
[Zip] [nvarchar](10) NULL,
[CityId] [int] NULL,
[StateId] [int] NULL,
[CountryId] [int] NULL,
[IsOnline] [bit] NULL,
 CONSTRAINT [PK_Profiles] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Profiles_GetByFilter]
go

CREATE PROCEDURE [dbo].[Profiles_GetByFilter]
(
	@Filter varchar(250),
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )
	declare @sql as nvarchar(4000)
	
	-- create dynamic sql
	set @sql = 'SELECT Profiles.id FROM Profiles '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the Profiles in the page requested
	SELECT    Profiles.*
	FROM      Profiles
	WHERE	  Profiles.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Profiles_GetRecent]
go


CREATE PROCEDURE [dbo].[Profiles_GetRecent]
(
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )

	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts (id)
			SELECT Id 
			FROM   Profiles with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the Profiles in the page requested
	SELECT    Profiles.*
	FROM      Profiles
	WHERE	  Profiles.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
