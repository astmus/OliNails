using System.Web.UI;

namespace MainSite
{
	public class HtmlCheckBox : System.Web.UI.HtmlControls.HtmlInputCheckBox
	{
		public string Text { get; set; }
		public override void RenderControl(HtmlTextWriter writer)
		{
			base.RenderControl(writer);
			writer.Write(this.Text);
		}
	}
}