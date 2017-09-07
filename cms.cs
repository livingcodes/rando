CMS
- Search 
	* Lucene auto index specified content
	* Tags
	* Implied search
- Edit page
	* Templates to generate edit pages
	* Edit controls (textbox, textarea, radiobox group, checkbox group, select (hard-coded, sproc, c# function), date, datetime)
	* Control validation (Required, Range, RegEx:{Email, Date})
	* Role based/claim based authentication/authorization
	* Instructions on how to enter content
- Inline Editing
- Versioning
	* Keep multiple versions
	* ModifiedBy
	* ModifiedDate
	* Version History listing
	* Roll forward 
	* Blame field (friendlier, Last modified by @user on @date)
Content Types
- Wiki
- CmsUser
- Analytics
	* What content (ex. article or video) is popular
	* Page views
	* What ads are clicked and on what pages
- HTML encoding (auto)
- Support on web server so searching and editing doesn't effect production site
- Support on separate database for reporting/analytics queries
	* Version history could be stored only on CMS database and not production

Property, Database Column (Name, Type), Control (Options), Validators

public class Content
{
	[Text, Hide] public int CmsId;
	[Overwrite, MaxLength(50)] public string CmsTitle;
	[TextArea(lines:2)] public string CmsDescription;
	[Tags] public string CmsTags;
	[Version, OnlyAdminEdit] public string CmsVersion;
	[DateTime] public DateTime CmsDateCreated;
	[User, Readonly] public int CmsCreatedByUser;
}

public class ContentControls : Controls(typeof(Content))
{
	public void CmsId(new TextBox(ReadOnly));
}

public class ContentControls : Controls<Content>
{
	public ContentControls(Content content) {
		Generate(content.CmsId, new TextBox(readonly:true));
		Generate(content.CmsDescription, new TextArea(lines:2));
		Generate(content.CmsVersion, new Version() { 
			EditPermission:new Permission(None,View,Edit)
		});
		Generate(content.CmsDateCreated, new DateTimePicker() {
			Readonly = true,
			Min = DateTime.Now,
			Max = DateTime.Now.AddYears(2)
		});
		Generate(content.CmsTags, new TagPicker(), customizedMarkup:true);
	}
}

typeof(string)
	new TextBox()
typeof(int)
	new TextBox()
typeof(DateTime)
	new DateTimePicker()


Winnercomm.Content
[CMS ID] CmsId
[CMS Title, Title only shown in CMS, validation => Required] CmsTitle
[CMS Description, Description only shown in CMS, 
	(control) => control.Type = TextArea, control.Lines:2] CmsDescription
[CMS Version, Version of content. Updated everytime content is modified.] CmsVersion
[CMS Date Created] CmsDateCreated
CmsCreatedByUser
[CMS Date Modified, Last date modified in CMS or application] CmsDateModified
CmsModifiedByUser
Clone()

Winnercomm.CmsUser : Winnercomm.Content
Email
PasswordHash
Authenticated

Aplication:User : Winnercomm.Content
Email
PasswordHash

// map controls (intellisense, options extensible depending on control)
Map(content, control) =>
	(content.Email) => 
		control.Label = "CMS Title <a>Only shown in CMS</a>",
		control.Validators = []{Validators.MaxLength = 255},
		control.Lines = 2,
		control.Order = 3
// instead just generate control
<div class="control">
	<label>{Email} <a title="[Only shown in CMS]">i</a></label>
	<input type="text" [max-length="255"]>
</div>
// templates
// radio
<div id="{PropertyName}" class="control radio [horizontal/vertical]">
	<div class="subcontrol">
		<input id="us" type="radio" name="country" value="US">
		<label for="us">US</label>
	</div>
	<div class="subcontrol">
		<input id="ca" type="radio" name="country" value="CA">
		<label for="ca">CA</label>
	</div>
</div>
// text
<div id="{PropertyName}" class="control text">
	<label for="{PropertyName}text">{PropertyName}</label>
	<input id="{PropertyName}text" type="text" placeholder="">
</div>
// checkbox
<div id="{PropertyName}" class="control checkbox">
	<div class="subcontrol">
		<input id="{PropertyName}text" type="checkbox" value="This" checked>
		<label for="{PropertyName}text">This</label>
	</div>
	<div class="subcontrol">
		<input id="{PropertyName}text" type="checkbox" value="That">
		<label for="{PropertyName}text">That</label>
	</div>
</div>
// datetime
// textarea
// select
// upload
// wysiwyg


Partial Application:User // Custom
Authenticated

[Can be relative path /images/image.png or absolute path http://domain.com/images/image.png <a href='image-tips'>Tips on choosing an image</a>] Url


var service = new Service(token)
var user = service.Query<User>(userId);
user.Email = "update@email.com";
service.Save(user);

var user = database.Get<User>(userId);
user.Email = "update@email.com";
database.Save(user);

var ajax = new Ajax(token)
var user = ajax.getJson('/api/cms/user/{userId}')
ajax.postJson('/api/cms/user', user)

// validate mappings in unit test