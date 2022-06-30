namespace WebPrintManager.Messages
{
    internal class GenericResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public void ThrowOnFail()
        {
            if (!this.Success)
            {
                throw new InvalidOperationException(this.Message);
            }
        }
    }
}
