using System.ComponentModel;

public delegate void SROptionsPropertyChanged(object sender, string propertyName);

public partial class SROptions
{

	public static SROptions Current
	{
		get { return _current; }
	}

	private static readonly SROptions _current = new SROptions();

	public event SROptionsPropertyChanged PropertyChanged;

#if UNITY_EDITOR
	[JetBrains.Annotations.NotifyPropertyChangedInvocator]
#endif
	public void OnPropertyChanged(string propertyName)
	{

		if (PropertyChanged != null)
			PropertyChanged(this, propertyName);

	}

}
