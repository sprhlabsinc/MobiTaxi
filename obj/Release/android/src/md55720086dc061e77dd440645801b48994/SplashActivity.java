package md55720086dc061e77dd440645801b48994;


public class SplashActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;Landroid/os/PersistableBundle;)V:GetOnCreate_Landroid_os_Bundle_Landroid_os_PersistableBundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("MOB1TAXI.SplashActivity, MOB1TAXI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SplashActivity.class, __md_methods);
	}


	public SplashActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SplashActivity.class)
			mono.android.TypeManager.Activate ("MOB1TAXI.SplashActivity, MOB1TAXI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0, android.os.PersistableBundle p1)
	{
		n_onCreate (p0, p1);
	}

	private native void n_onCreate (android.os.Bundle p0, android.os.PersistableBundle p1);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
