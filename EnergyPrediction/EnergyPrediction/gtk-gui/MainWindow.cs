
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow {
	private global::Gtk.UIManager UIManager;

	private global::Gtk.Action HelpNotSureIfThisIsRightAction;

	protected virtual void Build () {
		global::Stetic.Gui.Initialize ( this );
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ( "Default" );
		this.HelpNotSureIfThisIsRightAction = new global::Gtk.Action ( "HelpNotSureIfThisIsRightAction", global::Mono.Unix.Catalog.GetString ( "Help Not sure if this is right" ), null, null );
		this.HelpNotSureIfThisIsRightAction.ShortLabel = global::Mono.Unix.Catalog.GetString ( "Help Not sure if this is right" );
		w1.Add ( this.HelpNotSureIfThisIsRightAction, null );
		this.UIManager.InsertActionGroup ( w1, 0 );
		this.AddAccelGroup ( this.UIManager.AccelGroup );
		this.WidthRequest = 0;
		this.HeightRequest = 500;
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ( "MainWindow" );
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.BorderWidth = ((uint)(3));
		this.DefaultWidth = 400;
		this.DefaultHeight = 600;
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler ( this.OnDeleteEvent );
	}
}
