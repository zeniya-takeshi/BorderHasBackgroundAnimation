namespace BorderHasBackgroundAnimation;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private bool isBorderOn = false;

    private void OnBorderChanged(object sender, EventArgs e)
    {
        if (this.isBorderOn)
        {
            this.TheBorder.IsVisible = true;
            //this.TheBorder.BackgroundColor = Colors.Red;

            //The same animation occurs even when changing the CornerRadius
            //this.TheBorder.StrokeShape = new Rectangle();

            //This animation dosen't occer because #18204 workaround works.
            //this.TheBorder.WidthRequest = 200.0;
        }
        else
        {
            this.TheBorder.IsVisible = false;
            //this.TheBorder.BackgroundColor = Colors.Transparent;

            //this.TheBorder.StrokeShape = new RoundRectangle { CornerRadius = new(100.0) };

            //this.TheBorder.WidthRequest = 100.0;
        }

        this.isBorderOn = !this.isBorderOn;
    }

    private bool isFrameOn = false;

    private void OnFrameChanged(object sender, EventArgs e)
    {
        if (this.isFrameOn)
        {
            this.TheFrame.BackgroundColor = Colors.Red;
            this.TheFrame.BorderColor = Colors.Red;

            //this.TheFrame.CornerRadius = 0f;

            this.TheFrame.WidthRequest = 200.0;
        }
        else
        {
            this.TheFrame.BackgroundColor = Colors.Transparent;
            this.TheFrame.BorderColor = Colors.Transparent;

            //this.TheFrame.CornerRadius = 100f;

            //this.TheFrame.WidthRequest = 100.0;
        }

        this.isFrameOn = !this.isFrameOn;
    }
}
