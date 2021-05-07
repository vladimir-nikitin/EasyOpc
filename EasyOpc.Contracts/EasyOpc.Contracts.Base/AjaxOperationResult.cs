namespace EasyOpc.Contracts.Base
{
	/// <summary>
	/// Asynchronous operation result
	/// </summary>
	public class AjaxOperationResult
	{
		/// <summary>
		/// Returned data
		/// </summary>
		public object Data { get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Completed operation status
		/// </summary>
		public AjaxOperationResultStatus Status { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public AjaxOperationResult()
		{
			Status = AjaxOperationResultStatus.Ok;
		}

		/// <summary>
		/// Returns a new object of the result of the asynchronous request(Status=Error)
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="data">Returned data</param>
		/// <returns></returns>
		public static AjaxOperationResult Error(string message = null, object data = null)
		{
			return GetAjaxOperationResult(AjaxOperationResultStatus.Error, message, data);
		}

		/// <summary>
		/// Returns a new object of the result of the asynchronous request(Status=Ok)
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="data">Returned data</param>
		/// <returns></returns>
		public static AjaxOperationResult Ok(string message = null, object data = null)
		{
			return GetAjaxOperationResult(AjaxOperationResultStatus.Ok, message, data);
		}

		/// <summary>
		/// Returns a new object of the result of the asynchronous request
		/// </summary>
		/// <param name="status">Completed operation status</param>
		/// <param name="message">Error message</param>
		/// <param name="data">Returned data</param>
		/// <returns></returns>
		private static AjaxOperationResult GetAjaxOperationResult(AjaxOperationResultStatus status, string message, object data)
		{
			return new AjaxOperationResult
			{
				Status = status,
				Message = message,
				Data = data
			};
		}
	}
}
