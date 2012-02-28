IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys]') AND type in (N'U'))
DROP TABLE [dbo].[Countrys]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[Countrys]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[CountryCode] [nvarchar](10) NULL,
[Name] [nvarchar](100) NULL,
[Abbreviation] [nvarchar](30) NULL,
[AliasRefName] [nvarchar](100) NULL,
[IsActive] [bit] NULL,
[IsAlias] [bit] NULL,
[AliasRefId] [int] NULL,
 CONSTRAINT [PK_Countrys] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] 

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Countrys_GetByFilter]
go

CREATE PROCEDURE [dbo].[Countrys_GetByFilter]
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
	set @sql = 'SELECT Countrys.id FROM Countrys '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the Countrys in the page requested
	SELECT    Countrys.*
	FROM      Countrys
	WHERE	  Countrys.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Countrys_GetRecent]
go


CREATE PROCEDURE [dbo].[Countrys_GetRecent]
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
			FROM   Countrys with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the Countrys in the page requested
	SELECT    Countrys.*
	FROM      Countrys
	WHERE	  Countrys.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
