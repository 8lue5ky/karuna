namespace Frontend
{
    public class FooterService
    {
        public bool Visible { get; private set; } = true;

        public event Action? OnChange;

        public void Show()
        {
            Visible = true;
            OnChange?.Invoke();
        }

        public void Hide()
        {
            Visible = false;
            OnChange?.Invoke();
        }
    }
}
