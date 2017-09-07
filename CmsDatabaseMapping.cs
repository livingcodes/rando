void Configure() {
	var config = new Conman.Configuration();
	// regenerate during development
	config.RegenerateDatabase = true;
	config.RegenerateControls = true;
	// don't regenerate certain content types (in case there is a lot of custom control markup/javascript)
	config.DontRegenerateControls<User>();
	Conman.ApplyConfiguration(config);
}

CmsDatabaseMapping

public class VarCharColumn : Column
{
	public VarCharColumn() {
		Type = DatabaseType.VarChar;
		Length = 255;
	}
}

public class PropertyMapping
{
	Type Type;
	Property Property;
	public void UseDefaultColumn() {
		if (Type is string)
			return new VarCharColumn(Name:Property.GetName());
		if (Type is int)
			return new Column() {
				Name = Property.GetName(),
				Type = DatabaseType.Int
			};
	}
}

public class DatabaseMapping<T>
{
	virtual void OnMapped(T instance) {
		foreach (var property in instance.GetType().GetProperties()) {
			var propertyMapping = this.getMapping(property);
			if (propertyMapping.DidNotHaveColumnSet) {
				propertyMapping.UseDefaultColumn();
			}
		}
	}
}

public class PageDatabaseMapping : DatabaseMapping<Page>
{
	public PageDatabaseMapping() {}
	
	public void Map() {
		MapProperty(
			() => page.Title,
			new Column() {
				Type = new VarChar(150)
			}
		);
	}
}

// raise event on every connection open and close
	// log sql and parameters
// log warn long running connection (configurable duration. default 1sec)
// include sql and parameters in exceptions
// transactions, shared connections
// async
// multiple result sets
// paging
// top


// generic execute
var page = db
	.Parameter("@Title", title)
	.Execute("SELECT * FROM Pages WHERE Title = @Title")
// select top one
var page = db
	.Parameter("@Title", title)
	.SelectTop<Page>(1, "Title = @Title");
// select by id
var page = db.Select<Page>(id);
// get all
var pages = db
	.Parameter("@Title", title)
	.Select<Page>(where: "Title = @Title");
// get all, alternate syntax ???
var pages = db
	.Parameter("@Title", title)
	.Where<Page>("Title = @Title");
// get all
var pages = db.Select<Page>(where: "Title = @title AND PublishDate < GETDATE()", title)
// ??? using c#  methods to build sql
var pages = db.Where()
	.Equals("Title", title)
	.And()
	.LessThan("PublishDate", publishDate, "GETDATE()")
	.Select<Page>()
var pages = db.Select<Page>(#sql:
	SELECT * FROM Pages 
	WHERE Title = @title 
		AND PublishDate < GETDATE()#
// get all
var pages = db
// sproc parameters set by anonymous type
var pages = db
	.Sproc<Page>("SelectByTitle")
	.Select(new {Title = title});
// sproc parameters in order and generic on Sproc
var pages = db.Sproc<Page>("SelectByTitle", title);

// no generic on sproc so doesn't execute immediately, just returns sproc instance
var pages = db.Sproc("SelectByTitle")
	.Parameter("@Title", title)
	.Select<Page>()
	-or-
	.SelectFirst<Page>()
	-or-
	.SelectTop<Page(10)
	
db.Open();
	var pages = db.Sproc<Page>("SelectPageByTitle", title);
	foreach (var page in pages)
		page.Widgets.Add( db.Sproc<Widget>("SelectWidgetByPageTitle", title) );
db.Close();
	
// paging
var users, numPages, numUsers = db
	.Parameter("@Category", category)
	.Select<User>("Category = @Category)
	.Page(pageNumber:3, pageSize:10)
	//-or-
	.Page(start:21, count:10)
	
// transaction
using (db.StartTransaction()) {
	var order = db
		.Parameter("@CartId", cart.Id)
		.Sproc("Order")
		.Select<Order>();
	var succeeded = db
		.Sproc("UpdateInventory")
		.Parameter("@OrderId", order.Id)
		.Execute<bool>();
	if (!succeeded)
		db.Rollback();
	else
		db.Commit();
}

// shared
using (db.Open()) {
	var page = db.Select<Page>(pageId);
	var ads = db
		.Parameter("@PageId", pageId)
		.Select<Ad>("PageId = @PageId");
}

// shared (access outside open scope)
db.Open();
	var page = db.Select<Page>(pageId);
	var ads = db
		.Parameter("@PageId", pageId)
		.Select<Ad>("PageId = @PageId");
db.Close();

// async
var pagesResult = db
	.Parameter("@Category", category)
	.SelectAsync<Page>("Category = @Category");

// ... stuff

var pages = await pagesResult


