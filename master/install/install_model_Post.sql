IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts]') AND type in (N'U'))
DROP TABLE [dbo].[Posts]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[Posts]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[AppId] [int] NULL,
[Title] [nvarchar](150) NOT NULL,
[Description] [nvarchar](200) NULL,
[Content] [ntext] NOT NULL,
[PublishDate] [datetime] NULL,
[IsPublished] [bit] NULL,
[IsPublic] [bit] NULL,
[CategoryId] [int] NULL,
[Tags] [nvarchar](80) NULL,
[Slug] [nvarchar](150) NULL,
[RefKey] [nvarchar](20) NULL,
[IsFavorite] [bit] NULL,
[IsCommentEnabled] [bit] NULL,
[IsCommentModerated] [bit] NULL,
[IsRatable] [bit] NULL,
[Year] [int] NULL,
[Month] [int] NULL,
[CommentCount] [int] NULL,
[ViewCount] [int] NULL,
[AverageRating] [int] NULL,
[TotalLiked] [int] NULL,
[TotalDisLiked] [int] NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Posts_GetByFilter]
go

CREATE PROCEDURE [dbo].[Posts_GetByFilter]
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
	set @sql = 'SELECT Posts.id FROM Posts '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the Posts in the page requested
	SELECT    Posts.*
	FROM      Posts
	WHERE	  Posts.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Posts_GetRecent]
go


CREATE PROCEDURE [dbo].[Posts_GetRecent]
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
			FROM   Posts with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the Posts in the page requested
	SELECT    Posts.*
	FROM      Posts
	WHERE	  Posts.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
