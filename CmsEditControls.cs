public interface IControl<T>
{
	T Value { get; }
}

public interface IControlValidator
{
	bool IsValid { get; }
}

public class TextBox : IControl<string>, IControlValidator
{
	public TextBox(TextBox control) {
		this.control = control;
	}
	
	public string Value { get {
		return control.Text;
	} }
	
	public bool IsValid { get {
		if (Required && Value.NotSet())
			Message = "Required"
			return false;
		if (MaxLength > 0 && MaxLength < Value.Length)
			Message = "Max length is " + MaxLength;
			return false;
		return true;
	} }
	
	public string Message { get; set; }
	public bool Required { get; set; }
	public int MaxLength { get; set; }
}

public class ContentEditControls<T> : BaseEditControls, T as ContentBase
{
	public ContentEditControl(T content) {
		this.content = content;
	}
	
	public override void Setup() {
		base.Setup();
		Generate(
			() => content.Body, 
			new HtmlEditor()
		);
	}
}

public class BaseEditControl
{
	public BaseEditControl() {
		
	}
	
	virtual void Setup() {
		Generate(
			() => content.Id,
			new TextBox() { Readonly = true }
		);
		Generate(
			() => content.CmsTitle,
			() => {
				var textbox = new TextBox() { MaxLength = 100 };
				textbox.Validators.Add(new MaxLength(100));
				textbox.Validators.Add(new Required());
				return textbox;
			}
		);
		Generate(
			() => content.CmsDescription,
			new TextArea() { Rows = 2 }
		);
		Generate(
			() => content.ModifiedDate,
			new TextBox() { Readonly = true }
		);
		Generate(
			() => content.ModifiedBy,
			new UserTextBox()
		);
		Generate(
			() => content.CreatedDate,
			new TextBox() { Readonly = true }
		);
		Generate(
			() => content.CreatedBy,
			new UserTextBox()
		);
	}
}


public class EditControls<Subscription>
{
	public EditControls<Subscription>(Subscription content, User user) {
		
	}
	public void Setup() {
		base.Setup();
		Generate(
			() => content.UserId,
			new TextBox() { Readonly = true }
		);
		Generate(
			() => content.ExpirationDate,
			new DatePicker() { 
				Readonly = user.Role != Role.Admin
			}
		);
		Generate(
			() => content.AlwaysActive,
			() =>  {
				var checkbox = new CheckBox();
				Visible = user.Role == Role.Admin;
				return checkbox;
			}
		);
	}
}


public class EditControls<Settings>
{
	public void Setup() {
		base.Setup();
		Generate(
			() => content.PasswordAttempts,
			() => {
				var textbox = new TextBox();
				textbox.Validators.Add(new Range(1, 50, "Range 1-50"));
				return textbox;
			}
		);
		Generate(
			() => content.MarketingLandingPageUrl,
			() => {
				var textbox = new TextBox();
				text.Validators.Add(new UrlIsUnique());
				return textbox;
			}
		);
	}
}