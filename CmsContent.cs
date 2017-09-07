public class ContentBase
{
	public int Id { get; set; }
	public string CmsTitle { get; set; }
	public string CmsDescription { get; set; }
	public string CmsTags { get; set; }
	public string ModifiedBy { get; set; }
	public DateTime? ModifiedDate { get; set; }
	public string CreatedBy { get; set; }
	public DateTime CreatedDate { get; set; }
	
	public Version CmsVersion { get; set; }
}

public class PublishedContent : ContentBase
{
	public DateTime? PublishDate { get; set; }
	public DateTime? UnpublishDate { get; set; }
}

public class Content : ContentBase
{
	public string Body { get; set; }
}

public class Page : ContentBase
{
	public string Title { get; set; }
	public string Head { get; set; }
	public string Content { get; set; } // easy to html encode
	public string Javascript { get; set; } // easy to javascript encode
}

public class Subscription : ContentBase
{
	public int UserId { get; set; }
	public string ConvergeRecurringId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime ExpirationDate { get; set; }
	public string Status { get; set; }
}


public class User : ContentBase
{
	public string Email { get; set; }
	public string Password { get; set; }
}