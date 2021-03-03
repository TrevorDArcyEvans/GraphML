// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GraphML.UI.Uno
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.InteropServices.WindowsRuntime;
  using Windows.Foundation;
  using Windows.Foundation.Collections;
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Controls.Primitives;
  using Windows.UI.Xaml.Data;
  using Windows.UI.Xaml.Input;
  using Windows.UI.Xaml.Media;
  using Windows.UI.Xaml.Navigation;
  
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
      
      dp.Date = new DateTime(2006, 2, 20);
    }

    private void OnClick(object sender, object args)
    {
      var dt = DateTime.Now.ToString();
      txt.Text = dt;
    }

    public void Menu_OnClick(object sender, object args) 
    { 
      txt.Text = "Reset";
    }
  }
}
