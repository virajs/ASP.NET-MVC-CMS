IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets]') AND type in (N'U'))
DROP TABLE [dbo].[Widgets]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[Widgets]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[Name] [nvarchar](150) NULL,
[Description] [ntext] NULL,
[FullTypeName] [nvarchar](150) NULL,
[Path] [nvarchar](150) NULL,
[Version] [nvarchar](20) NULL,
[Author] [nvarchar](20) NULL,
[AuthorUrl] [nvarchar](150) NULL,
[Email] [nvarchar](30) NULL,
[Url] [nvarchar](150) NULL,
[IncludeProperties] [ntext] NOT NULL,
[ExcludeProperties] [ntext] NOT NULL,
[StringClobProperties] [ntext] NOT NULL,
[PathToEditor] [nvarchar](150) NULL,
[DeclaringType] [nvarchar](150) NULL,
[DeclaringAssembly] [nvarchar](150) NULL,
[SortIndex] [int] NULL,
[IsCacheable] [bit] NULL,
[IsEditable] [bit] NULL,
 CONSTRAINT [PK_Widgets] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Widgets_GetByFilter]
go

CREATE PROCEDURE [dbo].[Widgets_GetByFilter]
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
	set @sql = 'SELECT Widgets.id FROM Widgets '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the Widgets in the page requested
	SELECT    Widgets.*
	FROM      Widgets
	WHERE	  Widgets.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Widgets_GetRecent]
go


CREATE PROCEDURE [dbo].[Widgets_GetRecent]
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
			FROM   Widgets with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the Widgets in the page requested
	SELECT    Widgets.*
	FROM      Widgets
	WHERE	  Widgets.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
